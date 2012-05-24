using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Comms.Helpers;
using Wonga.QA.Tests.Core;
using Api = Wonga.QA.Framework.Api;
using Msmq = Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    [Parallelizable(TestScope.Descendants)]
    public class ArrearsSMSTests
    {
        #region private members
        private readonly string[] _smsTexts = new string[] { "We still haven't received full payment for your recent Wonga loan and it is now overdue. Go to 'My Account' to make a payment",
                                                             "Your Wonga repayment is still due and interest is being added. Please go to 'My Account' now to pay in full or set up a repayment plan.",
                                                             "We haven't received full payment for your Wonga loan and it’s now overdue. Please pay using 'My Account' at Wonga.com  or call us to discuss on  0207 138 8333",
                                                             "We are concerned that you still haven't repaid your Wonga loan. We urge you to get in touch so we can resolve this situation. Call us on 0207 138 8333",
                                                             "Please contact Wonga urgently as your account is overdue and interest is accruing. Call us on 0207 138 8333",
                                                             "Despite our messages and offers to help, your Wonga loan remains unpaid. Act now to avoid a possible impact on your credit rating. Call us on 0207 138 8333",
                                                             "Your Wonga loan is overdue in spite of our numerous offers to help you. Call us on 0207 138 8333 to avoid an impact on your credit rating."};

        private readonly Dictionary<uint, int> _dayToSMSMap = new Dictionary<uint, int>();
        private bool _originalBankGatewayMode;
        private static readonly dynamic SmsMessages = Drive.Data.Sms.Db.SmsMessages;
        private static readonly dynamic InArrearsNoticeSagaEntities = Drive.Data.OpsSagas.Db.InArrearsNoticeSagaEntity;
        #endregion

        #region Constructor
        public ArrearsSMSTests()
        {
            _dayToSMSMap.Add(1, 0);
            _dayToSMSMap.Add(5, 0);
            _dayToSMSMap.Add(29, 0);
            _dayToSMSMap.Add(3, 1);
            _dayToSMSMap.Add(27, 1);
            _dayToSMSMap.Add(31, 1);
            _dayToSMSMap.Add(10, 2);
            _dayToSMSMap.Add(24, 2);
            _dayToSMSMap.Add(17, 3);
            _dayToSMSMap.Add(38, 3);
            _dayToSMSMap.Add(56, 3);
            _dayToSMSMap.Add(45, 4);
            _dayToSMSMap.Add(52, 5);
            _dayToSMSMap.Add(58, 5);
            _dayToSMSMap.Add(60, 6);
        }
        #endregion

        #region Setup & teardown
        [FixtureSetUp]
        public void FixtureSetup()
        {
            _originalBankGatewayMode = ConfigurationFunctions.GetBankGatewayTestMode();
            ConfigurationFunctions.SetBankGatewayTestMode(false);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            ConfigurationFunctions.SetBankGatewayTestMode(_originalBankGatewayMode);
        }
        #endregion
        [Test]
        [AUT(AUT.Uk), JIRA("UK-1555")]
        [Row(60)]
        public void VerifySMSSentOnDaysUpTo(uint days)
        {
            DateTime startTime = DateTime.Now;
            string phonePart = GetPhoneNumber();
            string phoneNumber = "07" + phonePart;

            Customer customer = CustomerBuilder.New()
                                               .WithMobileNumber(phoneNumber)
                                               .Build();

            var mobileVerificationEntity = Do.Until(() => Drive.Data.Comms.Db.MobilePhoneVerifications.FindBy(MobilePhone :  phoneNumber, AccountId: customer.Id));

            Assert.IsNotNull(mobileVerificationEntity);
            Assert.IsNotNull(mobileVerificationEntity.Pin);

            //Force the mobile phone number to be verified successfully..
            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new Api.CompleteMobilePhoneVerificationCommand { Pin = mobileVerificationEntity.Pin, 
                                                                                                               VerificationId = mobileVerificationEntity.VerificationId }));

            Application loan = ApplicationBuilder.New(customer).Build();

            //Make sure the payment attempt fails by changing the expiry date of the card.
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId : customer.GetPaymentCard(), ExpiryDate : new DateTime(DateTime.Now.Year -1, 1, 31));
            
            //Put the application nto arrears.
            loan.PutApplicationIntoArrears(days);

            TimeoutNotificationSagaForDays(loan, days);
            
            //Changed this to optimize the number of applicants created.
            var daysInQuestion = from d in _dayToSMSMap.Keys where d <= days select d;

            foreach (uint day in daysInQuestion)
            {
                AssertSmsIsSent(FormatPhoneNumber(phonePart), _smsTexts[_dayToSMSMap[day]], startTime);
            }
        }

        //[Test]
        // This is useful for demos.
        //[AUT(AUT.Uk)]
        //public void PushSpecificLoanIntoArrears()
        //{
        //    Guid applicationGuid = Guid.Parse("4fbde69e-98fc-41e9-840a-7f3cc0a819f3");
        //    Guid cardId = Guid.Parse("4FBDE6C1-34E4-4134-96AC-7F3CC0A819F3");
        //    const uint days = 60;

        //    Application application = new Application(applicationGuid);

        //    //Make sure the payment attempt fails by changing the expiry date of the card.
        //    Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId : cardId, ExpiryDate : new DateTime(DateTime.Now.Year -1, 1, 31));

        //    application.PutApplicationIntoArrears(days);

        //    TimeoutNotificationSagaForDays(application, days);

        //}
        protected string GetPhoneNumber()
        {
            //return an unallocated number
            return "700900112"; //Get.RandomLong(100000000, 1000000000).ToString(CultureInfo.InvariantCulture);
                    
        }

        protected string FormatPhoneNumber(string unformatted)
        {
            return "447" + unformatted;
        }

        private void TimeoutNotificationSagaForDays(Application application, uint days)
        {
            var saga = Do.With.Timeout(2).Interval(10).Until(() => InArrearsNoticeSagaEntities.FindByAccountId(application.AccountId));

            Assert.IsNotNull(saga);

            for (int i = 0; i < days; i++)
            {
                Drive.Msmq.Payments.Send(new Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = saga.Id });
            }

            Assert.IsNotNull(Do.With.Timeout(5).Interval(10).Until(() => InArrearsNoticeSagaEntities.FindBy(AccountId: application.AccountId, DaysInArrears: days)));
        }

        private void AssertSmsIsSent(string formattedPhoneNumber, string text, DateTime createdAfter)
        {
            Assert.IsNotNull(Do.Until(() => SmsMessages.Find(SmsMessages.MobilePhoneNumber == formattedPhoneNumber &&
                                                             SmsMessages.MessageText == text &&
                                                             SmsMessages.CreatedOn > createdAfter)));
        }
    }
}
