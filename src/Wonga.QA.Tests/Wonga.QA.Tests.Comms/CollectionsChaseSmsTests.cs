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
		private const string A2Text =
			"We have just tried to collect your Wonga repayment but it was declined by your bank. Please pay today to avoid further charges.  Call us on 0861966421";

		private const string A3Text =
			"We still haven't received full payment for your Wonga.com loan and it is now overdue. Please pay your overdue account or call us to discuss on 08619664221";

		private const string A4Text =
			"Despite our efforts to contact you, your Wonga.com loan remains unpaid. Act now to avoid a possible impact on your credit rating. Call us on 0861966421";

		private bool _bankGatewayTestModeOriginal;
		private DateTime _atTheBeginningOfThisTest;

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

		[SetUp]
		public void Setup()
		{
			_atTheBeginningOfThisTest = DateTime.Now;
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za), Parallelizable]
		public void A2SmsIsSentTest()
		{
			string phoneNumberChunk = GetPhoneNumberChunk();
			string formattedPhoneNumber = GetFormattedPhoneNumber(phoneNumberChunk);
			ArrangeApplication(phoneNumberChunk);

			WaitForSms(formattedPhoneNumber, A2Text);
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za), Parallelizable]
		public void A3SmsIsSentTest()
		{
			string phoneNumberChunk = GetPhoneNumberChunk();
			string formattedPhoneNumber = GetFormattedPhoneNumber(phoneNumberChunk);
			Application application = ArrangeApplication(phoneNumberChunk);

			TimeoutNotificationSagaForDays(application, 5);

			WaitForSms(formattedPhoneNumber, A3Text);
		}

		[Test, JIRA("ZA-1676"), AUT(AUT.Za), Parallelizable]
		public void A4SmsIsSentTest()
		{
			string phoneNumberChunk = GetPhoneNumberChunk();
			string formattedPhoneNumber = GetFormattedPhoneNumber(phoneNumberChunk);
			Application application = ArrangeApplication(phoneNumberChunk);

			TimeoutNotificationSagaForDays(application, 15);

			WaitForSms(formattedPhoneNumber, A4Text);
		}

		private static string GetPhoneNumberChunk()
		{
			return Get.RandomLong(100000000, 1000000000).ToString(CultureInfo.InvariantCulture);
		}

		private static string GetFormattedPhoneNumber(string phoneNumberChunk)
		{
			return string.Format("27{0}", phoneNumberChunk);
		}

		private static Application ArrangeApplication(string phoneNumberChunk)
		{
			Customer customer = CustomerBuilder.New()
				.WithMobileNumber(string.Format("0{0}", phoneNumberChunk))
				.Build();
			Do.Until(customer.GetBankAccount);
			return ApplicationBuilder.New(customer).Build().PutApplicationIntoArrears(20);
		}

		private void WaitForSms(string formattedPhoneNumber, string text)
		{
			Do.Until(() => Drive.Db.Sms.SmsMessages.Where(
				m =>
				m.CreatedOn >= _atTheBeginningOfThisTest &&
				m.MobilePhoneNumber == formattedPhoneNumber &&
				m.MessageText == text));
		}

		private static void TimeoutNotificationSagaForDays(Application application, int days)
		{
			var saga =
				Do.Until(() => Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Single(e => e.AccountId == application.AccountId));
			Assert.AreEqual(0, saga.DaysInArrears);

			for (int i = 0; i < days; i++)
			{
				Drive.Msmq.Payments.Send(new TimeoutMessage {Expires = DateTime.UtcNow, SagaId = saga.Id});
			}
			Do.Until(
				() =>
				Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Single(
					e => e.AccountId == application.AccountId && e.DaysInArrears == days));
		}
	}
}
