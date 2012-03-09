using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	class CollectionsTests
	{
		private const int TrackingDayThreshold = 19;
		private const int MaximumRetries = 4;
		private const string NowServiceConfigKey = "Payments.ProcessScheduledPaymentSaga.DateTime.UtcNow";

		[SetUp]
		public void FixtureSetUp()
		{
				SetPaymentsUtcNow(DateTime.UtcNow);
		}

		[TearDown]
		public void FixtureTearDown()
		{
			var db = new DbDriver();
			var paymentsNowDb = db.Ops.ServiceConfigurations.Single(a => a.Key == NowServiceConfigKey);
			db.Ops.ServiceConfigurations.DeleteOnSubmit(paymentsNowDb);
			db.Ops.SubmitChanges();
		}

		[Test, AUT(AUT.Za)]
		public void CollectionsNaedoEntireWorkflowTest()
		{
			const int term = 25;
			var promiseDate = new Date(DateTime.UtcNow.AddDays(term));
			const int loanAmount = 500;

			var nextPayDate = new Date(DateTime.UtcNow.AddDays(term / 2));
			nextPayDate.DateTime = Driver.Db.GetNextWorkingDay(nextPayDate);

			var customer = CustomerBuilder.New()
				.WithNextPayDate(nextPayDate)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithPromiseDate(promiseDate)
				.Build();

			AttemptNaedoCollection(application, 0);
			FailNaedoCollection(application, 0);

			AttemptNaedoCollection(application, 1);
			FailNaedoCollection(application, 1);

			AttemptNaedoCollection(application, 2);
			FailNaedoCollection(application, 2);

			AttemptNaedoCollection(application, 3);
			FailNaedoCollection(application, 3);
		}

		[Test, AUT(AUT.Za)]
		public void CollectionsNaedoFullPaymentAfterTrackingEndsClosesLoanTest()
		{
			const int term = 25;
			var promiseDate = new Date(DateTime.UtcNow.AddDays(term));
			const int loanAmount = 500;

			var nextPayDate = new Date(DateTime.UtcNow.AddDays(term / 2));
			nextPayDate.DateTime = Driver.Db.GetNextWorkingDay(nextPayDate);

			var customer = CustomerBuilder.New()
				.WithNextPayDate(nextPayDate)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithPromiseDate(promiseDate)
				.Build();

			AttemptNaedoCollection(application, 0);
			FailNaedoCollection(application, 0);

			SendPaymentTaken(application, application.GetBalance());

			Do.With().Timeout(1).Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).ClosedOn != null);
			Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id) == false);
			Do.Until(() => new DbDriver().OpsSagas.PendingScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id) == false);
		}

		[Test, AUT(AUT.Za)]
		public void CollectionsNaedoPartialPaymentAfterTrackingEndsContinuesTrackingTest()
		{
			const int term = 25;
			var promiseDate = new Date(DateTime.UtcNow.AddDays(term));
			const int loanAmount = 500;

			var nextPayDate = new Date(DateTime.UtcNow.AddDays(term / 2));
			nextPayDate.DateTime = Driver.Db.GetNextWorkingDay(nextPayDate);

			var customer = CustomerBuilder.New()
				.WithNextPayDate(nextPayDate)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithPromiseDate(promiseDate)
				.Build();

			AttemptNaedoCollection(application, 0);
			FailNaedoCollection(application, 0);

			SendPaymentTaken(application, application.GetBalance() / 2);

			Assert.IsNull(Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).ClosedOn);
			Assert.IsTrue(new DbDriver().OpsSagas.PendingScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id));
			Assert.IsFalse(new DbDriver().OpsSagas.ScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id));
		}

		#region Helpers

		private void AttemptNaedoCollection(Application application, uint attempt)
		{
			var db = new DbDriver();
			var fixedTermLoanApplication = GetFixedTermLoanApplicationEntity(application);
			DateTime now;

			if (attempt == 0)
			{
				now = fixedTermLoanApplication.NextDueDate.Value;
				SetPaymentsUtcNow(now);

				new MsmqDriver().Payments.Send(new ProcessScheduledPaymentCommand { ApplicationId = fixedTermLoanApplication.ApplicationId });
				Do.Until(() => db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id));
			}

			else
			{
				var pendingScheduledPayment = db.OpsSagas.PendingScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id);
				var scheduledPaymentDate = db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id).PaymentRequestDate;

				now = pendingScheduledPayment.PaymentRequestDate.Value;
				SetPaymentsUtcNow(now);
				new MsmqDriver().Payments.Send(new TimeoutMessage { SagaId = pendingScheduledPayment.Id });
				Do.Until(() => db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id).PaymentRequestDate != scheduledPaymentDate);
			}

			
			var scheduledPaymentSaga = db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id);

			var expectedPaymentRequestDate = GetExpectedPaymentRequestDate(application, attempt, now);
			var expectedTrackingDays = GetExpectedTrackingDays(application, expectedPaymentRequestDate, now);

			Assert.AreEqual(expectedPaymentRequestDate, scheduledPaymentSaga.PaymentRequestDate);
			Assert.AreEqual(expectedTrackingDays, scheduledPaymentSaga.TrackingDays);
		}

		private void FailNaedoCollection(Application application, uint attempt)
		{
			var db = new DbDriver();

			var scheduledPaymentSaga = db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id);

			SetPaymentsUtcNow(scheduledPaymentSaga.PaymentRequestDate.Value.AddDays(scheduledPaymentSaga.TrackingDays.Value));

			new MsmqDriver().Payments.Send(new TimeoutMessage { SagaId = scheduledPaymentSaga.Id });

			if (attempt < MaximumRetries - 1)
			{
				Do.Until(() => db.OpsSagas.PendingScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id));
			}
			else
			{
				Do.Until(() => !db.OpsSagas.PendingScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id));
			}
		}

		private void SendPaymentTaken(Application application, decimal amount)
		{
			var db = new DbDriver();

			var fixedTermLoanApplication = GetFixedTermLoanApplicationEntity(application);

			var bankDetails = (from app in db.Payments.Applications
							   join bank in db.Payments.BankAccountsBases
								on app.BankAccountGuid equals bank.ExternalId
							   where app.ExternalId == application.Id
							   select bank).Single();

			var sagaId = db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id).Id;

			new MsmqDriver().Payments.Send(new PaymentTakenCommand
			{
				SagaId = sagaId,
				ApplicationId = application.Id,
				TransactionAmount = amount,
				BankAccountNumber = bankDetails.AccountNumber,
				BankCode = bankDetails.BankCode,
				EffectiveDate = DateTime.UtcNow,
				BatchSendTime = DateTime.UtcNow,
				CreatedOn = DateTime.UtcNow,
				ValueDate = DateTime.UtcNow,
			});

			Do.Until(() => db.Payments.Transactions.Single(a => a.Amount == -amount && a.Scope == 2 && a.ApplicationId == fixedTermLoanApplication.ApplicationId));
		}

		private DateTime GetExpectedPaymentRequestDate(Application application, uint attempt, DateTime now)
		{
			switch (attempt)
			{
				case 0:
					{
						return (DateTime)GetFixedTermLoanApplicationEntity(application).NextDueDate;
					}
				case 1:
					{
						//Payday of month - 1
						var selfReportedPayDay = GetSelfReportedPayDayForApplication(application);
						var validPayDay = Driver.Db.GetPreviousWorkingDay(new Date(new DateTime(now.Year, now.Month,  selfReportedPayDay))).DateTime.Day;

						return Driver.Db.GetPreviousWorkingDay(new Date(new DateTime(now.Year, now.Month,  validPayDay - 1)));
					}
				case 2:
					{
						//Default payday - 1
						return Driver.Db.GetPreviousWorkingDay(new Date(new DateTime(now.Year, now.Month, GetDefaultPayDaysOfMonth()[now.Month - 1] - 1)));
					}
				case 3:
					{
						//Default payday - 1
						return Driver.Db.GetPreviousWorkingDay(new Date(new DateTime(now.Year, now.Month, GetDefaultPayDaysOfMonth()[now.Month - 1] - 1)));
					}
				default:
					{
						throw new Exception(String.Format("We don't Naedo {0} times.", attempt));
					}
			}
		}

		private int GetExpectedTrackingDays(Application application, DateTime paymentRequestDate, DateTime now)
		{
			int trackingDays = 0;

			if( paymentRequestDate.Day > TrackingDayThreshold)
				trackingDays = (DateTime.DaysInMonth(paymentRequestDate.Year, paymentRequestDate.Month) + 1) - paymentRequestDate.Day;

			else
				trackingDays = 3;

			//Facilitates an odd edge case where the self reported payday is on a sunday or holiday
			var payDay = GetSelfReportedPayDayForApplication(application);
			if (!Driver.Db.IsWorkingDay(new Date(new DateTime(now.Year, now.Month, payDay))) && paymentRequestDate.Day > TrackingDayThreshold)
			{
				trackingDays -= 1;
			}

			return trackingDays;
		}

		private FixedTermLoanApplicationEntity GetFixedTermLoanApplicationEntity(Application application)
		{
			var db = new DbDriver();

			var fixedTermLoanApplication = (from app in db.Payments.Applications
											join fa in db.Payments.FixedTermLoanApplications
												on app.ApplicationId equals fa.ApplicationId
											where app.ExternalId == application.Id
											select fa).Single();

			return fixedTermLoanApplication;
		}

		private int GetSelfReportedPayDayForApplication(Application application)
		{
			var db = new DbDriver();
			return (db.Risk.RiskApplications.Join(db.Risk.EmploymentDetails, ra => ra.AccountId, ed => ed.AccountId,
			                                      (ra, ed) => new {ra, ed}).Where(@t => @t.ra.ApplicationId == application.Id).
				Select(@t => @t.ed.NextPayDate)).Single().Value.Day;

		}

		private void SetPaymentsUtcNow(DateTime dateTime)
		{
			Driver.Db.SetServiceConfiguration(NowServiceConfigKey, dateTime.ToString("yyyy-MM-dd hh:mm:ss"));
		}

		public int[] GetDefaultPayDaysOfMonth()
		{
			var value = new DbDriver().Ops.ServiceConfigurations.Single(a => a.Key == "Payments.PayDayPerMonth").Value;
			return value.Split(',').Select(Int32.Parse).ToArray();
		}

		#endregion
	}
}
