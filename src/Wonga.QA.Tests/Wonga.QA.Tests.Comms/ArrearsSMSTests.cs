using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
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
        private readonly string[] _smsTexts = new string[] { "We still haven't received full payment for your recent Wonga loan and it is now overdue. Go to 'My Account' to make a payment",
                                                             "Your Wonga repayment is still due and interest is being added. Please go to 'My Account' now to pay in full or set up a repayment plan.",
                                                             "We haven't received full payment for your Wonga loan and it’s now overdue. Please pay using 'My Account' at Wonga.com  or call us to discuss on  0207 138 8333",
                                                             "We are concerned that you still haven't repaid your Wonga loan. We urge you to get in touch so we can resolve this situation. Call us on 0207 138 8333",
                                                             "Please contact Wonga urgently as your account is overdue and interest is accruing. Call us on 0207 138 8333",
                                                             "Despite our messages and offers to help, your Wonga loan remains unpaid. Act now to avoid a possible impact on your credit rating. Call us on 0207 138 8333",
                                                             "Your Wonga loan is overdue in spite of our numerous offers to help you. Call us on 0207 138 8333 to avoid an impact on your credit rating."};

        private readonly Dictionary<uint, int> _dayToSMSMap = new Dictionary<uint, int>
                                                              	{
																	{1 , 0},
																	{5 , 0},
																	{29, 0},
																	{3 , 1},
																	{27, 1},
																	{31, 1},
																	{10, 2},
																	{24, 2},
																	{17, 3},
																	{38, 3},
																	{56, 3},
																	{45, 4},
																	{52, 5},
																	{58, 5},
																	{60, 6}
																};

		private static readonly dynamic SmsMessages = Drive.Data.Sms.Db.SmsMessages;
        private static readonly dynamic InArrearsNoticeSagaEntities = Drive.Data.OpsSagas.Db.InArrearsNoticeSagaEntity;

        [FixtureSetUp]
        public void FixtureSetup()
        {
			if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
				Assert.Inconclusive("Bankgateway is in test mode");
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1555"), Owner(Owner.SeamusHoban)]
        [Row(60)]
        public void VerifySMSSentOnDaysUpTo(uint days)
        {
            var phoneNumber = GetPhoneNumber();

        	var customer = BuildCustomerWithMobilePhoneNumber(phoneNumber);
            var application = ApplicationBuilder.New(customer).Build();

            //Make sure the payment attempt fails by changing the expiry date of the card.
			SetPaymentCardExpiryDate(customer, new DateTime(DateTime.Now.Year - 1, 1, 31));
           
            //Put the application nto arrears.
			var timePutIntoArrears = DateTime.Now;
            application.PutIntoArrears(days);
            TimeoutArrearsNotificationSagaForDays(application, days);

        	var phoneNumberAsAppearsInCommsDb = FormatPhoneNumber(phoneNumber);
        	var sms = GetSmsMessagesAfterTime(phoneNumberAsAppearsInCommsDb, timePutIntoArrears);

        	//var daysInQuestion = from d in _dayToSMSMap.Keys where d <= days select d;


        	//foreach (uint day in daysInQuestion)
        	//{
        	//    AssertSmsIsSent(FormatPhoneNumber(phoneNumber), _smsTexts[_dayToSMSMap[day]], timePutIntoArrears);
        	//}
        }

        protected string GetPhoneNumber()
        {
            //return an unallocated number
            return "700900112";
        }

		private Customer BuildCustomerWithMobilePhoneNumber(string mobilePhoneNumber)
		{
			string phoneNumber = "07" + mobilePhoneNumber;

			Customer customer = CustomerBuilder.New()
											   .WithMobileNumber(phoneNumber)
											   .Build();

			var mobileVerificationEntity = Do.Until(() => Drive.Data.Comms.Db.MobilePhoneVerifications.FindBy(MobilePhone: phoneNumber, AccountId: customer.Id));

			Assert.IsNotNull(mobileVerificationEntity);
			Assert.IsNotNull(mobileVerificationEntity.Pin);

			//Force the mobile phone number to be verified successfully..
			Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new CompleteMobilePhoneVerificationCommand
			{
				Pin = mobileVerificationEntity.Pin,
				VerificationId = mobileVerificationEntity.VerificationId
			}));

			return customer;
		}

		private void SetPaymentCardExpiryDate(Customer customer, DateTime dateTime)
		{
			Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: customer.GetPaymentCard(), ExpiryDate: dateTime);
		}

        protected string FormatPhoneNumber(string unformatted)
        {
            return "447" + unformatted;
        }

        private void TimeoutArrearsNotificationSagaForDays(Application application, uint days)
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

		private dynamic GetSmsMessagesAfterTime(string formattedPhoneNumber, DateTime dateTime)
		{
			return SmsMessages.FindAll(SmsMessages.mobilePhoneNumber &&
			                    SmsMessages.CreatedOn > dateTime);
		}
    }
}
