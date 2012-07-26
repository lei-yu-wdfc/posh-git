using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;
using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Messages.Comms.Commands;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.AddPaymentCardCommand;
using CreateFixedTermLoanExtensionCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateFixedTermLoanExtensionCommand;

namespace Wonga.QA.Tests.Comms.Email
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class ExtensionLoanEmailTests
    {
        private LoanExtensionEntity _extension;
		private readonly dynamic _applications = Drive.Data.Payments.Db.Applications;
    	private Guid _accountId;
    	private Guid _applicationId;
		
		[FixtureSetUp]
		public void FixtureSetUp()
		{
            _extension = CreateLoanAndExtend(); //run once for all tests.
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1281")]
        [Row(2, "Extension Secci")]
        [Row(3, "Extension Agreement")]
        public void CreateLoanExtensionDocumentsTest(int documentType)
        {
            Guid extensionId = _extension.ExternalId;


            Assert.DoesNotThrow(() => Do.Until(() =>
                            Drive.Data.Comms.Db.ExtensionDocuments.FindBy(ExtensionId: extensionId, DocumentType: documentType)
                            )
                , "ExtensionDocument not found for extension Id: {0} and docuemnt type:{1}", extensionId, documentType);


            dynamic documents = Drive.Data.Comms.Db.ExtensionDocuments.FindAllBy(ExtensionId: extensionId, DocumentType: documentType);

            Assert.AreEqual(1, documents.ToList().Count, "Document exist exactly once:{0}", documentType);

            Guid fileId = (Guid)documents.ToList()[0].ExternalId;
            Assert.DoesNotThrow(() => Do.Until(() => Drive.Data.FileStorage.Db.Files.FindByFileId(fileId))
                , "FileStorage record not found for FileId:{0}.", fileId);

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1281"), Owner(Owner.MichaelDoyle)]
        public void EmailExtensionAgreementTest()
        {

            Guid extensionId = _extension.ExternalId;

            Do.With.Interval(1).Until(() => _applications.Single(_applications.ApplicationId == _extension.ApplicationId));

            Assert.DoesNotThrow(() => Do.Until(() => Drive.Data.Comms.Db.ExtensionDocuments.FindAllBy(ExtensionId: extensionId, DocumentType: 3))
                , "ExtensionDocument not found for extension Id: {0} and docuemnt type: Loan extension agreement", extensionId);


            Assert.DoesNotThrow(() =>
                    Do.Until(() => Drive.Data.OpsSagas.Db.EmailExtensionAgreementEntity.FindByExtensionId(extensionId))
                , "Email Extension Agreement Saga is NOT in progress: {0}", extensionId
            );

            Drive.Msmq.Comms.Send(new IExtensionSigned
                                      {
                                          ApplicationId = _applicationId,
                                          ExtensionId = extensionId,
                                          CreatedOn = DateTime.UtcNow,
                                      });

            Assert.DoesNotThrow(() =>
                    Do.Until(() => (bool)(Drive.Data.OpsSagas.Db.EmailExtensionAgreementEntity.FindByExtensionId(extensionId) == null))
                , "Email Extension Agreement Saga has not completed: {0}", extensionId
            );

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1332")]
        public void EmailExtensionCancelledTest()
        {
            var extensionId = _extension.ExternalId;

            Do.With.Interval(1).Until(() => _applications.Single(_applications.ApplicationId == _extension.ApplicationId));

            Drive.Msmq.Comms.Send(new IExtensionCancelled
                                    {
                                        AccountId = _accountId,
                                        ApplicationId = _applicationId,
                                        ExtensionId = extensionId,
                                        CreatedOn = DateTime.UtcNow
                                    });

            Assert.DoesNotThrow(() =>
                                Do.Until(
                                    () =>
                                    (bool)
                                    (Drive.Data.OpsSagas.Db.ExtensionCancelledEmailData.FindByExtensionId(extensionId) == null))
                                , "Email Extension Agreement Saga has not completed: {0}", extensionId
                );
        }

        private LoanExtensionEntity CreateLoanAndExtend()
        {
           const decimal trustRating = 400.00M;
            _accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            _applicationId = Guid.NewGuid();
            var extensionId = Guid.NewGuid();

            var setupData = new AccountSummarySetupFunctions();
            var clientId = Guid.NewGuid();

            CreateCommsData(clientId, _accountId);

            setupData.Scenario03Setup(_applicationId, paymentCardId, bankAccountId, _accountId, trustRating);

            var applicationTab = Drive.Data.Payments.Db.Applications;
            var app = Do.With.Interval(1).Until(() => applicationTab.FindAll(applicationTab.ExternalId == _applicationId).Single());
            var fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
            var fixedTermApp =
                Do.With.Interval(1).Until(
                    () => fixedTermLoanAppTab.FindAll(fixedTermLoanAppTab.ApplicationId == app.ApplicationId).Single());

            Drive.Api.Commands.Post(new AddPaymentCardCommand
            {
                AccountId = _accountId,
                PaymentCardId = paymentCardId,
                CardType = "VISA",
                Number = "4444333322221111",
                HolderName = "Test Holder",
                StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
                ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
                IssueNo = "000",
                SecurityCode = "666",
                IsCreditCard = false,
                IsPrimary = true,
            });

            var paymentCardsBaseTab = Drive.Data.Payments.Db.PaymentCardsBase;
            Do.With.Interval(1).Until(
                () => paymentCardsBaseTab.FindAll(paymentCardsBaseTab.ExternalId == paymentCardId && paymentCardsBaseTab.AuthorizedOn != null).Single());

            Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
            {
                ApplicationId = _applicationId,
                ExtendDate = new Date(fixedTermApp.NextDueDate.Date.AddDays(2), DateFormat.Date),
                ExtensionId = extensionId,
                PartPaymentAmount = 20M,
                PaymentCardCv2 = "000",
                PaymentCardId = paymentCardId
            });

            var loanExtTab = Drive.Data.Payments.Db.LoanExtensions;
            var loanExtension =
                Do.With.Interval(1).Until(
                    () =>
                    loanExtTab.FindAll(loanExtTab.ExternalId == extensionId &&
                                       loanExtTab.ApplicationId == app.ApplicationId &&
                                       loanExtTab.PartPaymentTakenOn != null).Single());

            Assert.IsNotNull(loanExtension, "A loan extension should be created");

            return loanExtension;
        }

        private static void CreateCommsData(Guid clientId, Guid accountId)
        {
            const string homePhone = "02071111111";
            Drive.Msmq.Comms.Send(new
                                      SaveCustomerDetails
            {
                AccountId = accountId,
                ClientId = clientId,
                CreatedOn = DateTime.UtcNow,
                DateOfBirth = Get.GetDoB(),
                Email = Get.RandomEmail(),
                Forename = Get.GetName(),
                Gender = GenderEnum.Male,
                HomePhone = homePhone,
                MiddleName = Get.GetMiddleName(),
                MobilePhone = Get.GetMobilePhone(),//string.Format("07{0}", DateTime.UtcNow.Ticks.ToString().Substring(0, 8)),
                Surname = Get.GetName(),
                Title = TitleEnum.Dr,
                WorkPhone = homePhone,
            });
            Drive.Msmq.Comms.Send(new IAccountCreated { AccountId = accountId });


            Assert.DoesNotThrow(() =>
                                Do.With.Interval(1).Until(() => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(accountId))
                                , "CustomerDetails was not created."
                );


            Drive.Msmq.Comms.Send(new
                                      SaveCustomerAddress
            {
                CreatedOn = DateTime.UtcNow,
                AccountId = accountId,
                AddressId = Guid.NewGuid(),
                AtAddressFrom = new DateTime(2000, 1, 1),
                ClientId = clientId,
                CountryCode = CountryCodeEnum.Uk,
                Flat = "22",
                HouseName = "7",
                Postcode = "W7 3BX",
                Town = "London",
                Street = "Church Road",
                District = "East",
            });

            Assert.DoesNotThrow(() =>
                                Do.With.Interval(1).Until(() => Drive.Data.Comms.Db.Addresses.FindByAccountId(accountId))
                                , "Address was not created."
                );
        }
    }
}
