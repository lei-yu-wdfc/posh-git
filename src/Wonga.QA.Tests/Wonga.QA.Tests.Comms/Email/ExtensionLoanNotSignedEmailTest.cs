
using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;
using Wonga.QA.Framework.Msmq.Messages.Comms.Commands;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Comms.Email
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class ExtensionLoanNotSignedEmailTest
    {
        // Payments DB, FileStorage and Email must be deployed

        private Guid _accountId;
        private Guid _appId;
        private Guid _paymentCardId;
        private Guid _bankAccountId;
        private Guid _extId;
        private string _emailAddress;

        [SetUp]
        private void Setup()
        {

            _accountId = Guid.NewGuid();
            _appId = Guid.NewGuid();
            _paymentCardId = Guid.NewGuid();
            _bankAccountId = Guid.NewGuid();
            _extId = Guid.NewGuid();
            _emailAddress = Get.RandomAlphaNumeric(9, 9) + Get.GetEmail();
            const decimal trustRating = 400.00M;

            // In Payments create application
            var setupData = new ExtendLoanFunctions();
            setupData.TenDayLoanQuoteOnDayFiveSetup(_appId, _paymentCardId, _bankAccountId, _accountId, trustRating);


            // In Comms CreateCustomer 
            Drive.Msmq.Comms.Send(new SaveCustomerDetails()
                                      {
                                           AccountId = _accountId,
                                           Email = _emailAddress,
                                           Forename = Get.GetName(),
                                           Surname =  Get.GetName() ,
                                           Gender = GenderEnum.Male,
                                           HomePhone = Get.GetPhone(),
                                           DateOfBirth = Get.GetDoB(),

                                      });

            // Simulate Payment publishing IExtensionCancelled
            Drive.Msmq.Comms.Send(new IExtensionCancelled()
                                      {
                                          AccountId = _accountId,
                                          ApplicationId = _appId,
                                          CreatedOn = DateTime.UtcNow,
                                          ExtensionId = _extId
                                      });
        }

        [Test, AUT(AUT.Uk), Owner(Owner.CharlieBarker), JIRA("UKWEB-252")]
        public void TriggerExtensionNotSignedEmailTest()
        {
            const int loanExtensionCancelledEmailTemplate = 34121;
            //Time out extension reminder saga in comms
            var saga = Do.With.Interval(1).Until(() => Drive.Data.OpsSagas.Db.ExtensionCancelledEmailData.FindByAccountId(_accountId));
            Drive.Msmq.Comms.Send(new TimeoutMessage { ClearTimeout = true, Expires = DateTime.UtcNow, SagaId = saga.Id, State = null });

            // Check the QA database to see if an email was sent
            var email = Do.With.Interval(1).Until(() => Drive.Data.QaData.Db.Email.FindByEmailAddressAndTemplateName(_emailAddress, loanExtensionCancelledEmailTemplate));
            Assert.IsNotNull(email);
            var emailData = Drive.Data.QaData.Db.EmailTokens.FindByEmailIdAndKey(email.EmailId, "Html_body");
            Assert.Contains(emailData.Value, "still need to repay <b>&pound;110.70");
        }
    }
}
