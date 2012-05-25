using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.AddPaymentCardCommand;
using CreateFixedTermLoanExtensionCommand = Wonga.QA.Framework.Api.CreateFixedTermLoanExtensionCommand;

namespace Wonga.QA.Tests.Comms.Email
{

    [TestFixture, Parallelizable(TestScope.All)]
    public class ExtensionLoanEmailTests
    {
        private LoanExtensionEntity _extension;
        private readonly dynamic _applications = Drive.Data.Payments.Db.Applications;
        private Guid _accountId;
        private Guid _applicationId;

        public ExtensionLoanEmailTests()
        {
            _extension = CreateLoanAndExtend(); //run once for all tests.
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1281")]
        [Row(2, "Extension Secci")]
        [Row(3, "Extension Agreement")]
        [Row(20, "Extension AE Document")]
        public void CreateLoanExtensionDocumentsTest(int documentType)
        {
            Guid extensionId = _extension.ExternalId;

            dynamic documents = null;

            Assert.DoesNotThrow(() => documents = Do.Until(() =>
                            Drive.Data.Comms.Db.ExtensionDocuments.FindAllBy(ExtensionId: extensionId, DocumentType: documentType)
                            )
                , "ExtensionDocument not found for extension Id: {0} and docuemnt type:{1}", extensionId, documentType);


            Assert.AreEqual(1, documents.ToList().Count, "Document exist exactly once:{0}", documentType);

            Guid fileId = (Guid)documents.ToList()[0].ExternalId;
            Assert.DoesNotThrow(() => Do.Until(() => Drive.Data.FileStorage.Db.Files.FindByFileId(fileId))
                , "FileStorage record not found for FileId:{0}.", fileId);

        }

        [Test, AUT(AUT.Uk), JIRA("UK-1281")]
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

            Drive.Msmq.Comms.Send(new IExtensionSignedEvent
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

            Drive.Msmq.Comms.Send(new IExtensionCancelledEvent
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

            var app = Do.With.Interval(1).Until(() => Drive.Db.Payments.Applications.Single(x => x.ExternalId == _applicationId));
            var fixedTermApp =
                Do.With.Interval(1).Until(
                    () => Drive.Db.Payments.FixedTermLoanApplications.Single(x => x.ApplicationId == app.ApplicationId));

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

            Do.With.Interval(1).Until(
                () => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId && x.AuthorizedOn != null));

            Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
            {
                ApplicationId = _applicationId,
                ExtendDate = new Date(fixedTermApp.NextDueDate.Value.AddDays(2), DateFormat.Date),
                ExtensionId = extensionId,
                PartPaymentAmount = 20M,
                PaymentCardCv2 = "000",
                PaymentCardId = paymentCardId
            });

            var loanExtension =
                Do.With.Interval(1).Until(
                    () =>
                    Drive.Db.Payments.LoanExtensions.Single(x => x.ExternalId == extensionId && x.ApplicationId == app.ApplicationId
                        && x.PartPaymentTakenOn != null));

            Assert.IsNotNull(loanExtension, "A loan extension should be created");

            return loanExtension;
        }

        private static void CreateCommsData(Guid clientId, Guid accountId)
        {
            const string homePhone = "02071111111";
            Drive.Msmq.Comms.Send(new
                                      SaveCustomerDetailsCommand
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
            Drive.Msmq.Comms.Send(new IAccountCreatedEvent { AccountId = accountId });


            Assert.DoesNotThrow(() =>
                                Do.With.Interval(1).Until(() => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(accountId))
                                , "CustomerDetails was not created."
                );


            Drive.Msmq.Comms.Send(new
                                      SaveCustomerAddressCommand
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
