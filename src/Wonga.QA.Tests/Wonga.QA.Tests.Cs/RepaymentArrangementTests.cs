using System;
using System.Diagnostics;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;


namespace Wonga.QA.Tests.Cs
{
	[TestFixture, Parallelizable(TestScope.All), Pending("ZA-2565")]
    public class RepaymentArrangementTests
    {
        private bool _bankGatewayTestModeOriginal;

    	private const uint MinDaysInArrears = 3;
    	private const uint MaxDaysInArrears = 90;

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

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		[Column(2, 20, 91)]
		public void RepaymentPlanIsAllowedTest(uint daysInArrears)
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();

			application.PutIntoArrears(daysInArrears);

			var expectedPlanIsAllowed = !(daysInArrears < MinDaysInArrears || daysInArrears > MaxDaysInArrears);
			var actualPlanIsAllowed = PlanIsAllowed(application);

			Assert.AreEqual(expectedPlanIsAllowed, actualPlanIsAllowed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void RepaymentPlanNotAllowedWhenInDisputeTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(35);

			Drive.Msmq.Payments.Send(new IDisputeStatusChangedEvent { AccountId = customer.Id, HasDispute = true });
			Do.With.Timeout(2).Until(() => (bool)Drive.Data.Payments.Db.AccountPreferences.FindByAccountId(customer.Id).IsDispute == true);

			var planIsAllowed = PlanIsAllowed(application);
			Assert.IsFalse(planIsAllowed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void RepaymentPlanIsAllowedWhenPreviousPlanWasCancelledTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);
			var repaymentArrangement = GetRepaymentArrangement(application);

			CancelRepaymentArrangement(repaymentArrangement);

			Do.Until(() => PlanIsAllowed(application) == true);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void FailureToPayBreaksRepaymentArrangement()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);
			FailToMakeRepayment(customer, application);

			Assert.IsTrue((bool)GetRepaymentArrangement(application).IsBroken);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void RepaymentPlanNotAllowedWhenPreviousPlanWasBrokenTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);
			FailToMakeRepayment(customer, application);

			var planIsAllowed = PlanIsAllowed(application);
			Assert.IsFalse(planIsAllowed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void RepaymentPlanNotAllowedWhenPlanAlreadyExistsTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);
			GetRepaymentArrangement(application);

			var planIsAllowed = PlanIsAllowed(application);
			Assert.IsFalse(planIsAllowed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void CreateRepaymentPlanTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);

			var repaymentArrangement = GetRepaymentArrangement(application);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);

			var paymentsApplicationId = (int)Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;
			Assert.IsNotNull(Drive.Db.Payments.Transactions.Where(x => x.ApplicationId == paymentsApplicationId && x.Type == "SuspendInterestAccrual"));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void RepayAllInstallmentsClosesPlanTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);
			RepayAllInstallments(customer, application);

			Trace.WriteLine(DateTime.UtcNow);

			Do.Until(() => GetRepaymentArrangement(application).ClosedOn != null);

			Trace.WriteLine(DateTime.UtcNow);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void GetRepaymentPlanTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();

			application.PutIntoArrears(20);
			CreatePlan(application, customer);

			var response = Do.Until(() => Drive.Cs.Queries.Post(new GetRepaymentArrangementsQuery() { ApplicationId = application.Id }));
			Assert.IsNotNull(response);

			Assert.AreEqual(application.Id, Guid.Parse(response.Values["ApplicationId"].Single()));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void GetParametersTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();
			application.PutIntoArrears(20);

			var response = Drive.Cs.Queries.Post(new GetRepaymentArrangementParametersQuery() { AccountId = customer.Id });
			Assert.IsNotNull(response);
			Assert.AreEqual(3, int.Parse(response.Values["MaxLengthMonths"].Single()));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void GetCalculationTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();
			application.PutIntoArrears(20);

			var response = Drive.Cs.Queries.Post(new GetRepaymentArrangementCalculationQuery
			{
				AccountId = customer.Id,
				RepaymentFrequency = PaymentFrequency.Monthly,
				NumberOfMonths = 4,
				FirstRepaymentDate = DateTime.Today.AddDays(7)
			});
			Assert.IsNotNull(response);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1864"), Pending("ZA-2565")]
		public void CancelTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			CreatePlan(application, customer);
			var repaymentArrangement = GetRepaymentArrangement(application);

			CancelRepaymentArrangement(repaymentArrangement);
		}

        public enum PaymentFrequency
        {
            Every2Weeks,
            Every4Weeks,
            Monthly,
            Weekly
        }

        //Needed for serialization in CreateExtendedRepaymentArrangementCommand
        public class RepaymentArrangementDetailCsapi
        {
            public decimal Amount { get; set; }
            public CurrencyCodeIso4217Enum Currency { get; set; }
            public DateTime DueDate { get; set; }
        }

        private static void CreatePlan(Application application, Customer customer)
        {
            Drive.Cs.Commands.Post(new CreateRepaymentArrangementCommand()
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                EffectiveBalance = 0,
                RepaymentAmount = 100,
                ArrangementDetails = new[]
                                                                    {
                                                                        new RepaymentArrangementDetailCsapi
                                                                            {
                                                                                Amount = 49,
                                                                                Currency = CurrencyCodeIso4217Enum.ZAR,
                                                                                DueDate = DateTime.Today.AddDays(7)
                                                                            },
                                                                        new RepaymentArrangementDetailCsapi
                                                                            {
                                                                                Amount = 51,
                                                                                Currency = CurrencyCodeIso4217Enum.ZAR,
                                                                                DueDate = DateTime.Today.AddDays(15)
                                                                            }
                                                                    }
            });
        }

		private static RepaymentArrangementEntity GetRepaymentArrangement(Application application)
		{
			var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			var repaymentArrangement = Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));

			return repaymentArrangement;
		}

		private static void CancelRepaymentArrangement(RepaymentArrangementEntity repaymentArrangement)
		{
			Drive.Cs.Commands.Post(new CancelRepaymentArrangementCommand()
			{
				RepaymentArrangementId = repaymentArrangement.ExternalId
			});

			Do.Until(() => Drive.Data.Payments.Db.RepaymentArrangements.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).CanceledOn != null);
		}

		private static void RepayAllInstallments(Customer customer, Application application)
		{
			var repaymentArrangement = GetRepaymentArrangement(application);
			var count = repaymentArrangement.RepaymentArrangementDetails.Count;
			
			for( int i = 0; i < count; i++)
			{
				MakeNextRepayment(application);
			}
		}

		private static void MakeNextRepayment(Application application)
		{
			var repaymentArrangement = GetRepaymentArrangement(application);
			var repaymentArrangementDetailId = repaymentArrangement.RepaymentArrangementDetails.First(a => a.AmountPaid <= 0).RepaymentArrangementDetailId;

			var repaymentArrangementSagaId = (Guid)Do.Until(() => Drive.Data.OpsSagas.Db.RepaymentArrangementSagaEntity.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).Id);

			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = repaymentArrangementSagaId });

			var scheduledRepaymentSagaId = (Guid)Do.Until(() => Drive.Data.OpsSagas.Db.ScheduledRepaymentSagaEntity.FindByRepaymemtArrangementGuid(repaymentArrangement.ExternalId).ScheduledPaymentSagaEntity_id);

			var bankGuid = (Guid) Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).BankAccountGuid;
			var bankDetails = (BankAccountsBaseEntity)Drive.Data.Payments.Db.BankAccountsBase.FindByExternalId(bankGuid);
			var amount = repaymentArrangement.RepaymentArrangementDetails.First(a => a.AmountPaid == 0).Amount;

			Drive.Msmq.Payments.Send(new PaymentTakenCommand
			{
				SagaId = scheduledRepaymentSagaId,
				ApplicationId = application.Id,
				TransactionAmount = amount,
				BankAccountNumber = bankDetails.AccountNumber,
				BankCode = bankDetails.BankCode,
				EffectiveDate = DateTime.UtcNow,
				BatchSendTime = DateTime.UtcNow,
				CreatedOn = DateTime.UtcNow,
				ValueDate = DateTime.UtcNow,
			});

			Do.Until(() => Drive.Data.OpsSagas.Db.ScheduledRepaymentSagaEntity.FindByScheduledPaymentSagaEntity_id(scheduledRepaymentSagaId) == null);

			Do.Until(() => GetRepaymentArrangement(application)
			               	.RepaymentArrangementDetails
			               	.Single(a => a.RepaymentArrangementDetailId == repaymentArrangementDetailId)
			               	.AmountPaid > 0);
		}

		private static void FailToMakeRepayment(Customer customer, Application application)
		{
			var repaymentArrangement = GetRepaymentArrangement(application);
			var repaymentArrangementSagaId = (Guid) Do.Until(() => Drive.Data.OpsSagas.Db.RepaymentArrangementSagaEntity.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).Id);

			Drive.Msmq.Payments.Send(new TimeoutMessage{SagaId = repaymentArrangementSagaId});

			var scheduledRepaymentSagaId = (Guid)Do.Until(() => Drive.Data.OpsSagas.Db.ScheduledRepaymentSagaEntity.FindByRepaymemtArrangementGuid(repaymentArrangement.ExternalId).ScheduledPaymentSagaEntity_id);

			Drive.Msmq.Payments.Send(new TakePaymentFailedCommand{AccountId = customer.Id, SagaId = scheduledRepaymentSagaId, CreatedOn =  DateTime.UtcNow});

			Do.Until(() => GetRepaymentArrangement(application).CanceledOn != null);
		}

		private static bool PlanIsAllowed(Application application)
		{
			var response = Drive.Cs.Queries.Post(new CsGetRepaymentArrangementAllowedQuery()
			{
				ApplicationId = application.Id
			});

			Assert.IsNotNull(response);

			var allowed = bool.Parse(response.Values["IsAllowed"].Single());
			return allowed;
		}
    }
}
