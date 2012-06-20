using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Sagas
{
	[TestFixture]
	[Parallelizable(TestScope.Self)]
	public class ScheduledServiceFeeTests
	{
		private const string Key_UtcNow =
			"Wonga.Payments.Validators.Za.CreateFixedTermLoanApplicationValidator.DateTime.UtcNow";

		private const string Key_Saga_UtcNow =
			@"Wonga.Payments.Handlers.FixedLoanOperations.Za.ScheduledServiceFeeSaga.DateTime.UtcNow";

		private const string Key_RepaymentArrangementEnabled =
			"Payments.RepaymentArrangementEnabled";

		private bool _repaymentArrangementEnabled;
		private Guid _applicationId;
		private int _applicationIntId;

		[FixtureSetUp]
		public void FixtureSetup()
		{
			if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
				Assert.Inconclusive("Bankgateway is in test mode");

			Drive.Data.Ops.SetServiceConfiguration(Key_UtcNow, "2012/06/16");

			_repaymentArrangementEnabled = Drive.Data.Ops.GetServiceConfiguration<bool>(Key_RepaymentArrangementEnabled);
			Drive.Data.Ops.SetServiceConfiguration(Key_RepaymentArrangementEnabled, "true");
		}

		[FixtureTearDown]
		public void FixtureTeardown()
		{
			Drive.Data.Ops.Db.ServiceConfigurations.Delete(Key: Key_UtcNow);
			Drive.Data.Ops.SetServiceConfiguration(Key_RepaymentArrangementEnabled, _repaymentArrangementEnabled);
		}

		[Test]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		[Parallelizable]
		public void ServiceFeePostedUpTo90DaysTest()
		{
			var postedOnDates = new List<DateTime>();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer)
				.WithLoanTerm(35)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();

			int applicationId = GetApplicationId(application.Id);
			postedOnDates.Add(DateTime.UtcNow);
			AssertForServiceFees(postedOnDates, applicationId);

			var sagaId = Drive.Data.OpsSagas.Db.ApplicationSagaEntity.FindByApplicationId(applicationId).Id;

			AssertServiceFeePostedOn30thDay(postedOnDates, sagaId, applicationId);

			AssertServiceFeePostedOn30thDay(postedOnDates, sagaId, applicationId);

			AssertServiceFeePostedOn30thDay(postedOnDates, sagaId, applicationId);

			//saga is completed after 90 days.
			Do.Until(() => Drive.Data.OpsSagas.Db.ApplicationSagaEntity.FindByApplicationId(applicationId) == null);
		}

		[Test]
		[Parallelizable]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		public void ServiceFeeIsNotPostedAfterApplicationIsClosed()
		{
			var postedOnDates = new List<DateTime>();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer)
				.WithLoanTerm(35)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();

			int applicationId = GetApplicationId(application.Id);
			postedOnDates.Add(DateTime.UtcNow);
			AssertForServiceFees(postedOnDates, applicationId);

			Drive.Msmq.Payments.Send(new IApplicationClosedEvent
			{
				ApplicationId = application.Id
			});

			//saga is completed cause applcation is closed.
			Do.Until(() => Drive.Data.OpsSagas.Db.ApplicationSagaEntity.FindByApplicationId(applicationId) == null);
		}

		[Test]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		public void ServiceFeeIsNotPostedHavingRepaymentArrangement()
		{
			var postedOnDates = new List<DateTime>();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer)
				.WithLoanTerm(35)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();

			_applicationId = application.Id;
			int applicationId = GetApplicationId(application.Id);
			_applicationIntId = applicationId;
			AssertForServiceFees(postedOnDates, applicationId);

			application.PutIntoArrears(5);
			var signOnDate = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).SignedOn;
			postedOnDates.Add(signOnDate);
			application.CreateRepaymentArrangement();

			var sagaId = Drive.Data.OpsSagas.Db.ApplicationSagaEntity.FindByApplicationId(applicationId).Id;

			AssertServiceFeesOnDate(postedOnDates, sagaId, applicationId, DateTime.UtcNow);
		}

		[Test]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		[DependsOn("ServiceFeeIsRetroPostedAfterRepaymentArrangementIsClosed")]
		public void ServiceFeeIsNotPostedWhenInDca()
		{
			var postedOnDates = new List<DateTime>();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer)
				.WithLoanTerm(35)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();

			_applicationId = application.Id;
			int applicationId = GetApplicationId(application.Id);
			_applicationIntId = applicationId;
			AssertForServiceFees(postedOnDates, applicationId);

			Drive.Msmq.Payments.Send(new IApplicationMovedToDcaEvent()
			{
				ApplicationId = application.Id
			});
			postedOnDates.Add(DateTime.UtcNow);

			var sagaId = Drive.Data.OpsSagas.Db.ApplicationSagaEntity.FindByApplicationId(applicationId).Id;
			AssertServiceFeesOnDate(postedOnDates, sagaId, applicationId, DateTime.UtcNow);
		}

		[Test]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		[DependsOn("ServiceFeeIsNotPostedHavingRepaymentArrangement")]
		public void ServiceFeeIsRetroPostedAfterRepaymentArrangementIsClosed()
		{
			Drive.Msmq.Payments.Send(new IRepaymentArrangementClosedEvent()
			{
				ApplicationId = _applicationId
			});
			var postedOnDates = new List<DateTime>()
                                    {
                                        Drive.Data.Payments.Db.Applications.FindByExternalId(_applicationId).SignedOn,
                                        DateTime.UtcNow
                                    };
			AssertForServiceFees(postedOnDates, _applicationIntId);
		}

		[Test]
		[Parallelizable]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		public void ServiceFeeIsRetroPostedAfterRepaymentArrangementIsCanceled()
		{
			var postedOnDates = new List<DateTime>();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer)
				.WithLoanTerm(35)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();

			_applicationId = application.Id;
			int applicationId = GetApplicationId(application.Id);
			_applicationIntId = applicationId;
			AssertForServiceFees(postedOnDates, applicationId);

			application.PutIntoArrears(5);
			var signOnDate = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).SignedOn;
			postedOnDates.Add(signOnDate);
			application.CreateRepaymentArrangement();

			var sagaId = Drive.Data.OpsSagas.Db.ApplicationSagaEntity.FindByApplicationId(applicationId).Id;

			AssertServiceFeesOnDate(postedOnDates, sagaId, applicationId, DateTime.UtcNow);

			Drive.Msmq.Payments.Send(new IRepaymentArrangementCancelledEvent()
			{
				ApplicationId = _applicationId
			});
			postedOnDates = new List<DateTime>()
                                    {
                                        Drive.Data.Payments.Db.Applications.FindByExternalId(_applicationId).SignedOn,
                                        DateTime.UtcNow
                                    };
			AssertForServiceFees(postedOnDates, _applicationIntId);
		}

		[Test]
		[AUT(AUT.Za), JIRA("ZA-2659")]
		[DependsOn("ServiceFeeIsNotPostedWhenInDca")]
		public void ServiceFeeIsRetroPostedAfterOutOfDca()
		{
			Drive.Msmq.Payments.Send(new IApplicationRevokedFromDcaEvent()
			{
				ApplicationId = _applicationId
			});
			var postedOnDates = new List<DateTime>()
                                    {
                                        DateTime.UtcNow,
                                        DateTime.UtcNow
                                    };
			AssertForServiceFees(postedOnDates, _applicationIntId);
		}

		private void AssertServiceFeePostedOn30thDay(IList<DateTime> postedOnDates, Guid sagaId, int applicationId)
		{
			var day30s = postedOnDates.Last().AddDays(30);
			postedOnDates.Add(day30s);
			AssertServiceFeesOnDate(postedOnDates, sagaId, applicationId, day30s);
		}

		private void AssertServiceFeesOnDate(IList<DateTime> postedOnDates, Guid sagaId, int applicationId, DateTime today)
		{
			Drive.Data.Ops.SetServiceConfiguration(Key_Saga_UtcNow, today.ToString(@"yyyy/MM/dd"));
			TimeoutServiceFeeSaga(sagaId);
			AssertForServiceFees(postedOnDates, applicationId);
			Drive.Data.Ops.Db.ServiceConfigurations.Delete(Key: Key_Saga_UtcNow);
		}

		private void TimeoutServiceFeeSaga(Guid sagaId)
		{
			Drive.Msmq.Payments.Send(new TimeoutMessage()
			{
				SagaId = sagaId
			});
		}

		private int GetApplicationId(Guid externalId)
		{
			var apps = Drive.Data.Payments.Db.Applications;
			var app = apps.FindByExternalId(externalId);
			return app.ApplicationId;
		}

		private void AssertForServiceFees(IList<DateTime> postedOnDates, int applicationId)
		{
			var trs = Drive.Data.Payments.Db.Transactions;

			var sfTrs = trs.FindAllBy(Type: "ServiceFee", ApplicationId: applicationId);
			Stopwatch timeoutWatch = Stopwatch.StartNew();
			while (sfTrs.Count() != postedOnDates.Count && timeoutWatch.Elapsed < TimeSpan.FromSeconds(120))
			{
				Console.WriteLine("Waiting for transaction to be posted...");
				Thread.Sleep(500);
			}
			/*            Do.Until(sfTrs.Count() == postedOnDates.Count);*/

			var trsPostedOnDates = new List<DateTime>();
			foreach (var t in sfTrs)
			{
				trsPostedOnDates.Add(t.PostedOn);
			}

			foreach (var pd in postedOnDates)
			{
				Assert.Exists(trsPostedOnDates, d => d.Date == pd.Date);
			}
		}
	}
}