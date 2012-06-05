using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Sms
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class A1SmsTests
	{
		private const string _smsText = @"We know life is hectic, so just a reminder that we'll collect " +
		                                @"your repayment from your bank account tomorrow - from the Wonga Team";

		private static readonly dynamic _smsTable = Drive.Data.Sms.Db.SmsMessages;
		private static readonly dynamic _scheduleA1SmsSagaEntityTable = Drive.Data.OpsSagas.Db.ScheduleA1SmsSagaEntity;

		[Test, AUT(AUT.Za), JIRA("WIN-886", "WIN-1127")]
		public void SendsA1Sms()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();
			DateTime utcNow = DateTime.UtcNow;

			TimeoutScheduleSaga(application);

			AssertSmsIsSent(GetFormattedMobilePhoneNumber(customer), _smsText, utcNow);
		}

		[Test, AUT(AUT.Za), JIRA("WIN-886", "WIN-1127")]
		public void DoesNotSendSmsWhenApplicationIsClosed()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();
			DateTime utcNow = DateTime.UtcNow;
			var scheduleSaga = GetScheduleA1SmsSagaEntity(application.Id);

			application.RepayEarly(application.GetBalanceToday() * 2, 1);

			WaitUntilScheduleSagaCompletes(scheduleSaga.Id);

			AssertSmsIsNotSent(GetFormattedMobilePhoneNumber(customer), _smsText, utcNow);
		}

		private static void TimeoutScheduleSaga(Application application)
		{
			var scheduleSagaEntity = GetScheduleA1SmsSagaEntity(application.Id);
			Drive.Msmq.Comms.Send(new TimeoutMessage
			                      	{
			                      		SagaId = scheduleSagaEntity.Id,
			                      		Expires = DateTime.UtcNow.AddSeconds(-2)
			                      	});
		}

		private static dynamic GetScheduleA1SmsSagaEntity(Guid applicationId)
		{
			return Do.Until(() => _scheduleA1SmsSagaEntityTable.FindByApplicationId(applicationId));
		}

		private static string GetFormattedMobilePhoneNumber(Customer customer)
		{
			return string.Format("27{0}", customer.GetCustomerMobileNumber().Substring(1));
		}

		private static void AssertSmsIsSent(string formattedPhoneNumber, string text, DateTime createdAfter)
		{
			Assert.IsNotNull(Do.Until(() => _smsTable.Find(_smsTable.MobilePhoneNumber == formattedPhoneNumber
			                                               && _smsTable.MessageText == text
			                                               && _smsTable.CreatedOn > createdAfter)));
		}

		private static void WaitUntilScheduleSagaCompletes(Guid id)
		{
			Do.Until(() => _scheduleA1SmsSagaEntityTable.FindById(id) == null);
		}

		private static void AssertSmsIsNotSent(string formattedPhoneNumber, string text, DateTime createdAfter)
		{
			Assert.IsTrue(Do.Watch(() => (bool)
			                             (null == _smsTable.Find(_smsTable.MobilePhoneNumber == formattedPhoneNumber
			                                                     && _smsTable.MessageText == text
			                                                     && _smsTable.CreatedOn > createdAfter))));
		}
	}
}
