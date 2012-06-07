using System;
using System.Linq;
using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, AUT(AUT.Za), Pending("ZA-2565")]
	class CollectionsTests
	{
		private const int TrackingDayThreshold = 19;
		private const int MaximumRetries = 4;
		private const string NowServiceConfigKey = "Payments.ProcessScheduledPaymentSaga.DateTime.UtcNow";
		private const int InArrearsMaxDays = 90; //Hardcoded in payments
		private readonly int[] NaedoTrackingDays = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 14, 21, 32 }; //Hardcoded in payments

		private readonly string PrevBankGatewayTestMode = Drive.Db.GetServiceConfiguration("BankGateway.IsTestMode").Value;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			Drive.Db.SetServiceConfiguration("BankGateway.IsTestMode", "false");
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			Drive.Db.SetServiceConfiguration("BankGateway.IsTestMode", PrevBankGatewayTestMode);
		}

		[SetUp]
		public void SetUp()
		{
			SetPaymentsUtcNow(DateTime.UtcNow);
		}

		[TearDown]
		public void TearDown()
		{
			var db = new DbDriver();
			var paymentsNowDb = db.Ops.ServiceConfigurations.Single(a => a.Key == NowServiceConfigKey);
			db.Ops.ServiceConfigurations.DeleteOnSubmit(paymentsNowDb);
			db.Ops.SubmitChanges();
		}

		[Test, AUT(AUT.Za), Pending("ZA-2565")]
		public void CollectionsNaedoEntireWorkflowTest()
		{
			const int term = 25;
			var promiseDate = new Date(DateTime.UtcNow.AddDays(term));
			const int loanAmount = 500;

			var nextPayDate = new Date(DateTime.UtcNow.AddDays(term / 2));
			nextPayDate.DateTime = Drive.Db.GetNextWorkingDay(nextPayDate);

			var customer = CustomerBuilder.New()
				.WithNextPayDate(nextPayDate)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithPromiseDate(promiseDate)
				.Build();

			application.PutApplicationIntoArrears();

			AttemptNaedoCollection(application, 0);
			FailNaedoCollection(application, 0);

			AttemptNaedoCollection(application, 1);
			FailNaedoCollection(application, 1);

			AttemptNaedoCollection(application, 2);
			FailNaedoCollection(application, 2);

			AttemptNaedoCollection(application, 3);
			FailNaedoCollection(application, 3);
		}

		[Test, AUT(AUT.Za), Pending("ZA-2565")]
		public void CollectionsNaedoFullPaymentAfterTrackingEndsClosesLoanTest()
		{
			const int term = 25;
			var promiseDate = new Date(DateTime.UtcNow.AddDays(term));
			const int loanAmount = 500;

			var nextPayDate = new Date(DateTime.UtcNow.AddDays(term / 2));
			nextPayDate.DateTime = Drive.Db.GetNextWorkingDay(nextPayDate);

			var customer = CustomerBuilder.New()
				.WithNextPayDate(nextPayDate)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithPromiseDate(promiseDate)
				.Build();

			application.PutApplicationIntoArrears();

			AttemptNaedoCollection(application, 0);
			FailNaedoCollection(application, 0);

			SendPaymentTaken(application, application.GetBalance());

			Do.With.Timeout(1).Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).ClosedOn != null);
			Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id) == false);
			Do.Until(() => Drive.Db.OpsSagas.PendingScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id) == false);
		}

		[Test, AUT(AUT.Za), Pending("ZA-2565")]
		public void CollectionsNaedoPartialPaymentAfterTrackingEndsContinuesTrackingTest()
		{
			const int term = 25;
			var promiseDate = new Date(DateTime.UtcNow.AddDays(term));
			const int loanAmount = 500;

			var nextPayDate = new Date(DateTime.UtcNow.AddDays(term / 2));
			nextPayDate.DateTime = Drive.Db.GetNextWorkingDay(nextPayDate);

			var customer = CustomerBuilder.New()
				.WithNextPayDate(nextPayDate)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithPromiseDate(promiseDate)
				.Build();

			application.PutApplicationIntoArrears();

			AttemptNaedoCollection(application, 0);
			FailNaedoCollection(application, 0);

			SendPaymentTaken(application, application.GetBalance() / 2);

			Assert.IsNull(Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).ClosedOn);
			Assert.IsTrue(new DbDriver().OpsSagas.PendingScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id));

			//Test a collection attempt is created for the remaining balance. But need to put a delay for the saga
			//To be created here.
			var followupCollectionExisted = Do.Until(() => new DbDriver().OpsSagas.ScheduledPaymentSagaEntities.Any(a => a.ApplicationGuid == application.Id));
			Assert.IsTrue(followupCollectionExisted);
		}


		#region Helpers

		private void AttemptNaedoCollection(Application application, uint attempt)
		{
			TestLog.WriteLine("Attempting Collection " + attempt);

			var fixedTermLoanApplication = GetFixedTermLoanApplicationEntity(application);
			DateTime now;

			if (attempt == 0)
			{
				now = fixedTermLoanApplication.NextDueDate.Value;
				SetPaymentsUtcNow(now);

				Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id));
			}

			else
			{
				var pendingScheduledPayment = Drive.Db.OpsSagas.PendingScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id);
				var scheduledPaymentDate = Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id).PaymentRequestDate;

				now = pendingScheduledPayment.PaymentRequestDate.Value;
				SetPaymentsUtcNow(now);

				new MsmqDriver().Payments.Send(new TimeoutMessage { SagaId = pendingScheduledPayment.Id });
				Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id).PaymentRequestDate != scheduledPaymentDate);

				now = (DateTime)Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).ScheduledPayments.OrderBy(a => a.CreatedOn).ToArray()[attempt - 1].ToBeRetriedOnDate;
			}

			var scheduledPayment = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).ScheduledPayments.OrderBy(a => a.CreatedOn).ToArray()[attempt];

			if (IsTrackingForMoreThanMaxDays(application, now))
				return;

			var expectedPaymentRequestDate = GetExpectedPaymentRequestDate(application, attempt, now);
			var expectedTrackingDays = GetExpectedTrackingDays(expectedPaymentRequestDate, attempt);

			Assert.AreEqual(expectedPaymentRequestDate, scheduledPayment.PaymentDate);
			Assert.AreEqual(expectedTrackingDays, scheduledPayment.TrackingDays);
		}

		private void FailNaedoCollection(Application application, uint attempt)
		{
			TestLog.WriteLine("Failing Collection " + attempt);

			var db = new DbDriver();

			var scheduledPaymentSaga = db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id);

			var now = scheduledPaymentSaga.PaymentRequestDate.Value.AddDays(scheduledPaymentSaga.TrackingDays.Value);
			SetPaymentsUtcNow(now);

			new MsmqDriver().Payments.Send(new TimeoutMessage { SagaId = scheduledPaymentSaga.Id });

			if (attempt < MaximumRetries - 1)
			{
				Do.Until(() => db.OpsSagas.PendingScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id));
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
			DateTime paymentRequestDate;

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
						var month = selfReportedPayDay > now.Day ? now.Month : now.Month + 1;
						var validPayDay = Drive.Db.GetPreviousWorkingDay(new Date(new DateTime(now.Year, month, selfReportedPayDay))).DateTime.Day;

						paymentRequestDate = new Date(new DateTime(now.Year, month, validPayDay - 1));
					}
					break;
				default:
					{
						paymentRequestDate = new Date(new DateTime(now.Year, now.Month, GetDefaultPayDaysOfMonth()[now.Month - 1]).AddDays(-1));
					}
					break;
			}

			return paymentRequestDate;
		}

		private int GetExpectedTrackingDays(DateTime paymentRequestDate, uint attempt)
		{
			var dateTrackingBegins = attempt == 0
										? paymentRequestDate
										: new Date(paymentRequestDate).DateTime;


			int trackingDays = 0;

			if (Drive.Db.GetPreviousWorkingDay(new Date(dateTrackingBegins.AddDays(-1))).DateTime.Day <= TrackingDayThreshold || attempt == 0)
				trackingDays = 3;
			else
				trackingDays = (DateTime.DaysInMonth(dateTrackingBegins.Year, dateTrackingBegins.Month)) - dateTrackingBegins.Day + 3; //3 = actionDate + til 2nd of month 

			trackingDays = NaedoTrackingDays.Where(a => trackingDays >= a).Max();

			return trackingDays;
		}

		private bool IsTrackingForMoreThanMaxDays(Application application, DateTime now)
		{
			return GetDaysTracking(application, now) > InArrearsMaxDays;
		}

		private int GetDaysTracking(Application application, DateTime now)
		{
			var promiseDate = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).FixedTermLoanApplicationEntity.PromiseDate;
			return (now - promiseDate).Days;
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
												  (ra, ed) => new { ra, ed }).Where(@t => @t.ra.ApplicationId == application.Id).
				Select(@t => @t.ed.NextPayDate)).Single().Value.Day;

		}

		private void SetPaymentsUtcNow(DateTime dateTime)
		{
			Drive.Db.SetServiceConfiguration(NowServiceConfigKey, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
		}

		public int[] GetDefaultPayDaysOfMonth()
		{
			var value = new DbDriver().Ops.ServiceConfigurations.Single(a => a.Key == "Payments.PayDayPerMonth").Value;
			return value.Split(',').Select(Int32.Parse).ToArray();
		}

		#endregion
	}
}