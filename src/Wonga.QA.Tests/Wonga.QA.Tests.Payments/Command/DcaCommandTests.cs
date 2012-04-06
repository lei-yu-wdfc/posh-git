using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Payments.Command
{
	public class DcaCommandTests
	{
		private const string BankGatewayIsTestModeKey = "BankGateway.IsTestMode";
		private const string FeatureSwitchMoveLoanToDca = "FeatureSwitch.MoveLoanToDca";
		private string _bankGatewayIsTestMode;
		private string _featureSwitchMoveLoanToDcaMode;
	    private dynamic _debtCollections = Drive.Data.Payments.Db.DebtCollection;
	    private dynamic _fixedTermLoanSagas = Drive.Data.OpsSagas.Db.FixedTermLoanSagaEntity;
	    private dynamic _externalDebtCollectionSagas = Drive.Data.OpsSagas.Db.ExternalDebtCollectionSagaEntity;
	    private dynamic _schedulePaymentSagas = Drive.Data.OpsSagas.Db.ScheduledPaymentSagaEntity;
        
		[SetUp]
		public void SetUp()
		{
			ServiceConfigurationEntity entity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == BankGatewayIsTestModeKey);
			_bankGatewayIsTestMode = entity.Value;

			entity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == FeatureSwitchMoveLoanToDca);
			_featureSwitchMoveLoanToDcaMode = entity.Value;

		    Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: BankGatewayIsTestModeKey, Value: "false");
            Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: FeatureSwitchMoveLoanToDca, Value: "true");
		}


		[TearDown]
		public void TearDown()
		{
            Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: BankGatewayIsTestModeKey, Value: _bankGatewayIsTestMode);
            Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: FeatureSwitchMoveLoanToDca, Value: _featureSwitchMoveLoanToDcaMode);
		}


		[Test, AUT(AUT.Za), JIRA("ZA-2147")]
		public void FlagApplicationToDca_ShouldMoveApplicationToDCA()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var flagToDcaCommand = new Framework.Cs.FlagApplicationToDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(flagToDcaCommand);

			//Assert
            var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
                                                    .OrderByDescending(_debtCollections.CreatedOn)
                                                    .FirstOrDefault());
			Assert.IsNotNull(debtCollection);
			Assert.AreEqual(true, debtCollection.MovedToAgency);

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147")]
		public void RevokeApplicationFromDca_ShouldMoveApplicationFromDCA()
		{
			//Arrange
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

			Thread.Sleep(5000);

			var revokeFromDcaCommand = new Framework.Cs.RevokeApplicationFromDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);

			Thread.Sleep(5000);

			var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
													.OrderByDescending(_debtCollections.CreatedOn)
													.FirstOrDefault());

			//Assert
			Assert.AreEqual(false, debtCollection.MovedToAgency);
		}

	}
}