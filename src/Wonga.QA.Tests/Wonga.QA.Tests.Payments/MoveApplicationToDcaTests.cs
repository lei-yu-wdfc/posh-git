using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using CreateFixedTermLoanApplicationCommand = Wonga.QA.Framework.Api.CreateFixedTermLoanApplicationCommand;
using SignApplicationCommand = Wonga.QA.Framework.Api.SignApplicationCommand;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, Parallelizable(TestScope.All)]
	public class MoveApplicationToDcaTests
	{
        //private const string BankGatewayIsTestModeKey = "BankGateway.IsTestMode";
        //private const string FeatureSwitchMoveLoanToDca = "FeatureSwitch.MoveLoanToDca";
        //private string _bankGatewayIsTestMode;
        //private string _featureSwitchMoveLoanToDcaMode;
        private dynamic _debtCollections = Drive.Data.Payments.Db.DebtCollection;
        private dynamic _fixedTermLoanSagas = Drive.Data.OpsSagas.Db.FixedTermLoanSagaEntity;
        private dynamic _externalDebtCollectionSagas = Drive.Data.OpsSagas.Db.ExternalDebtCollectionSagaEntity;
        private dynamic _schedulePaymentSagas = Drive.Data.OpsSagas.Db.ScheduledPaymentSagaEntity;
	    private dynamic _transactions = Drive.Data.Payments.Db.Transactions;

		[SetUp]
		public void SetUp()
		{
			//ServiceConfigurationEntity entity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == BankGatewayIsTestModeKey);
			//_bankGatewayIsTestMode = entity.Value;

			//entity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == FeatureSwitchMoveLoanToDca);
			//_featureSwitchMoveLoanToDcaMode = entity.Value;

			//Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: BankGatewayIsTestModeKey, Value: "false");
			//Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: FeatureSwitchMoveLoanToDca, Value: "true");
		}


		[TearDown]
		public void TearDown()
		{
			//Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: BankGatewayIsTestModeKey, Value: _bankGatewayIsTestMode);
			//Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: FeatureSwitchMoveLoanToDca, Value: _featureSwitchMoveLoanToDcaMode);
		}


		[Test, AUT(AUT.Za), JIRA("ZA-2256")]
		public void MoveApplicationToDca_AfterDcaDelayPeriod()
		{
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
				Build();

			var fixedTermLoanSagaEntity = Do.Until(() => _fixedTermLoanSagas.FindAllByApplicationGuid(app.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = fixedTermLoanSagaEntity.Id,
			});

			var scheduledPaymentSagaEntity = Do.Until(() => _schedulePaymentSagas.FindAllByApplicationGuid(app.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = scheduledPaymentSagaEntity.Id,
			});

			//Force dca to timeout immmediately
			var externalDebtCollectionSagaEntities = Do.Until(() => _externalDebtCollectionSagas.FindAllByApplicationId(app.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = externalDebtCollectionSagaEntities.Id,
			});

			var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id).Single());

			Assert.AreEqual(debtCollection.MovedToAgency, true);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2256")]
		public void MoveApplicationToDca_AfterDcaDelayPeriod_SuspendInterstTransactionIsPosted()
		{
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
				Build();

			var fixedTermLoanSagaEntity = Do.Until(() => _fixedTermLoanSagas.FindAllByApplicationGuid(app.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = fixedTermLoanSagaEntity.Id,
			});

			var scheduledPaymentSagaEntity = Do.Until(() => _schedulePaymentSagas.FindAllByApplicationGuid(app.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = scheduledPaymentSagaEntity.Id,
			});

			//Force dca to timeout immmediately
			var externalDebtCollectionSagaEntities = Do.Until(() => _externalDebtCollectionSagas.FindAllByApplicationId(app.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = externalDebtCollectionSagaEntities.Id,
			});


			var loanApp = Do.Until(() => Drive.Data.Payments.Db.Applications.FindAllByExternalId(app.Id).Single());
			var suspendInterstTransaction = Do.Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId && 
                                                           _transactions.Type == PaymentTransactionEnum.SuspendInterestAccrual.ToString()).FirstOrDefault());

			Assert.IsNotNull(suspendInterstTransaction);

		}


	}
}