using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Comms.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
	public class CollectionsChaseSmsTests
	{
		#region constants

		private const string A2Text =
			"We have just tried to collect your Wonga repayment but it was declined by your bank. " +
			"Please pay today to avoid further charges. Call us on 0861966421";

		private const string A3Text =
			"We still haven't received full payment for your Wonga.com loan and it is now overdue. " +
			"Please pay your overdue account or call us to discuss on 08619664221";

		private const string A4Text =
			"Despite our efforts to contact you, your Wonga.com loan remains unpaid. " +
			"Act now to avoid a possible impact on your credit rating. Call us on 0861966421";

		private const string A5Text =
			"Please contact Wonga urgently as your account is overdue and interest is accruing. " +
			"Call us on 0861966421 - Our agents are waiting to help you resolve this";

		private const string A6Text =
			"Your Wonga.com loan remains in arrears. Please don't ignore this message. " +
			"Contact us urgently on 0861966421 to avoid us taking further steps against you.";

		private const string A7Text =
			"Your Wonga.com loan remains in arrears. Please don't ignore this message. " +
			"Contact us urgently on 0861966421 to avoid us taking further steps against you.";

		#endregion

		private bool _bankGatewayTestModeOriginal;
		private static readonly dynamic AccountPreferences = Drive.Data.Payments.Db.AccountPreferences;
		private static readonly dynamic SmsMessages = Drive.Data.Sms.Db.SmsMessages;
		private static readonly dynamic InArrearsNoticeSagaEntities = Drive.Data.OpsSagas.Db.InArrearsNoticeSagaEntity;

		[FixtureSetUp]
		public void FixtureSetup()
		{
			_bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
			ConfigurationFunctions.SetBankGatewayTestMode(false);
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za)]
		public void A2SmsIsSentTest()
		{
			VerifySmsIsSentAfterDaysInArrears(0, A2Text);
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za)]
		public void A3SmsIsSentTest()
		{
			VerifySmsIsSentAfterDaysInArrears(5, A3Text);
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za)]
		public void A4SmsIsSentTest()
		{
			VerifySmsIsSentAfterDaysInArrears(15, A4Text);
		}

		[Test, JIRA("ZA-2233", "ZA-1676"), AUT(AUT.Za)]
		public void A5SmsIsSentTest()
		{
			VerifySmsIsSentAfterDaysInArrears(20, A5Text);
		}

		[Test, JIRA("ZA-2233", "ZA-1676"), AUT(AUT.Za)]
		public void A6SmsIsSentTest()
		{
			VerifySmsIsSentAfterDaysInArrears(30, A6Text);
		}

		[Test, JIRA("ZA-2233", "ZA-1676"), AUT(AUT.Za)]
		public void A7SmsIsSentTest()
		{
			VerifySmsIsSentAfterDaysInArrears(40, A7Text);
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za)]
		public void WhenInHardshipSmsIsNotSent()
		{
			VerifyA2SentA3SuppressedA4Sent(
				a => AccountPreferences.UpdateByAccountId(AccountId: a.AccountId, IsHardship: true),
				a => AccountPreferences.UpdateByAccountId(AccountId: a.AccountId, IsHardship: false));
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za)]
		public void WhenInDisputeSmsIsNotSent()
		{
			VerifyA2SentA3SuppressedA4Sent(
				a => AccountPreferences.UpdateByAccountId(AccountId: a.AccountId, IsDispute: true),
				a => AccountPreferences.UpdateByAccountId(AccountId: a.AccountId, IsDispute: false));
		}

		#region helpers

		private static void VerifySmsIsSentAfterDaysInArrears(int daysInArrears, string smsText)
		{
			DateTime atTheBeginningOfThisTest = DateTime.Now;
			string phoneNumberChunk = GetPhoneNumberChunk();
			string formattedPhoneNumber = GetFormattedPhoneNumber(phoneNumberChunk);
			Application application = ArrangeApplicationInArrears(phoneNumberChunk);

			TimeoutNotificationSagaForDays(application, daysInArrears);

			AssertSmsIsSent(formattedPhoneNumber, smsText, atTheBeginningOfThisTest);
		}

		private static void VerifyA2SentA3SuppressedA4Sent(
			Action<Application> suppressAction, Action<Application> resumeAction)
		{
			DateTime atTheBeginningOfThisTest = DateTime.Now;
			string phoneNumberChunk = GetPhoneNumberChunk();
			string formattedPhoneNumber = GetFormattedPhoneNumber(phoneNumberChunk);
			Application application = ArrangeApplicationInArrears(phoneNumberChunk);

			// wait unit A2 is sent
			AssertSmsIsSent(formattedPhoneNumber, A2Text, atTheBeginningOfThisTest);

			// set suppress
			suppressAction(application);

			// time out to after the A3 point
			TimeoutNotificationSagaForDays(application, 7);

			// verify A3 is not sent
			AssertSmsIsNotSent(formattedPhoneNumber, A3Text, atTheBeginningOfThisTest);

			// resume
			resumeAction(application);

			// time out to the A4 point
			TimeoutNotificationSagaForDays(application, 15);

			// wait unit A4 is sent
			AssertSmsIsSent(formattedPhoneNumber, A4Text, atTheBeginningOfThisTest);

			// verify A3 is still not sent
			AssertSmsIsNotSent(formattedPhoneNumber, A3Text, atTheBeginningOfThisTest);
		}

		private static string GetPhoneNumberChunk()
		{
			return Get.RandomLong(100000000, 1000000000).ToString(CultureInfo.InvariantCulture);
		}

		private static string GetFormattedPhoneNumber(string phoneNumberChunk)
		{
			return string.Format("27{0}", phoneNumberChunk);
		}

		private static Application ArrangeApplicationInArrears(string phoneNumberChunk)
		{
			Customer customer = CustomerBuilder.New()
				.WithMobileNumber(string.Format("0{0}", phoneNumberChunk))
				.Build();
			Do.Until(customer.GetBankAccount);
			return ApplicationBuilder.New(customer).Build().PutApplicationIntoArrears(20);
		}

		private static void TimeoutNotificationSagaForDays(Application application, int days)
		{
			var saga =
				Do.Until(() =>
				         InArrearsNoticeSagaEntities.FindByAccountId(application.AccountId));
			Assert.IsNotNull(saga);

			for (int i = 0; i < days - saga.DaysInArrears; i++)
			{
				Drive.Msmq.Payments.Send(new TimeoutMessage {Expires = DateTime.UtcNow, SagaId = saga.Id});
			}
			Assert.IsNotNull(Do.Until(
				() =>
				InArrearsNoticeSagaEntities.FindBy(AccountId: application.AccountId, DaysInArrears: days)));
		}

		private static void AssertSmsIsSent(string formattedPhoneNumber, string text, DateTime createdAfter)
		{
			Assert.IsNotNull(
				Do.Until(() =>
				         SmsMessages.Find(
				         	SmsMessages.CreatedOn >= createdAfter &&
				         	SmsMessages.MobilePhoneNumber == formattedPhoneNumber &&
				         	SmsMessages.MessageText == text)));
		}

		private static void AssertSmsIsNotSent(string formattedPhoneNumber, string text, DateTime createdAfter)
		{
			Assert.IsTrue(
				Do.Watch(() =>
				         (bool) (
				                	SmsMessages.Find(
				                		SmsMessages.CreatedOn >= createdAfter &&
				                		SmsMessages.MobilePhoneNumber == formattedPhoneNumber &&
				                		SmsMessages.MessageText == text &&
				                		(SmsMessages.Status != 3 ||
				                		 SmsMessages.ErrorMessage != null || // TODO: error message is set to null by mistake in sms
				                		 SmsMessages.ServiceMsgId != null))
				                	== null)));
		}

		#endregion
	}
}
