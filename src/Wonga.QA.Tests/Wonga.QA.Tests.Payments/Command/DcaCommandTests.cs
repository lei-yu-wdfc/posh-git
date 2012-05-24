using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
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
	[TestFixture, Parallelizable(TestScope.All), Pending("ZA-2565")]
	public class DcaCommandTests
	{
		private const string BankGatewayIsTestModeKey = "BankGateway.IsTestMode";
		private const string FeatureSwitchMoveLoanToDca = "FeatureSwitch.MoveLoanToDca";
		private bool _bankGatewayIsTestMode;
        //private string _featureSwitchMoveLoanToDcaMode;
	    private dynamic _debtCollections = Drive.Data.Payments.Db.DebtCollection;
	    private dynamic _fixedTermLoanSagas = Drive.Data.OpsSagas.Db.FixedTermLoanSagaEntity;
	    private dynamic _externalDebtCollectionSagas = Drive.Data.OpsSagas.Db.ExternalDebtCollectionSagaEntity;
	    private dynamic _schedulePaymentSagas = Drive.Data.OpsSagas.Db.ScheduledPaymentSagaEntity;
		private const int delay = 15000;
        
		[FixtureSetUp]
		public void FixtureSetUp()
		{
			_bankGatewayIsTestMode = Drive.Data.Ops.SetServiceConfiguration(BankGatewayIsTestModeKey, false);
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			Drive.Data.Ops.SetServiceConfiguration(BankGatewayIsTestModeKey, _bankGatewayIsTestMode);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		public void FlagApplicationToDca_ShouldMoveApplicationToDCA()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			Thread.Sleep(delay);

			var flagToDcaCommand = new Framework.Cs.FlagApplicationToDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(flagToDcaCommand);

			Thread.Sleep(delay);
			//Assert
            var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
                                                    .OrderByDescending(_debtCollections.CreatedOn)
                                                    .FirstOrDefault());
			Assert.IsNotNull(debtCollection);
			Assert.AreEqual(true, debtCollection.MovedToAgency);

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		[ExpectedException(typeof(ValidatorException), "Payments_FlagToDca_ApplicationDoesNotExist")]
		public void FlagApplicationToDca_ForNonExitingApplication_ExpectValidationException()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			Thread.Sleep(delay);

			var flagToDcaCommand = new Framework.Cs.FlagApplicationToDcaCommand
			{
				ApplicationId = Guid.NewGuid()
			};

			//Act
			Drive.Cs.Commands.Post(flagToDcaCommand);

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		[ExpectedException(typeof(ValidatorException), "FlagApplicationToDca_ApplicationNotOpen")]
		public void FlagApplicationToDca_ForClosedApplication_ExpectValidationException()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			//Close the Application
			var loanApp = Do.Until(() => Drive.Data.Payments.Db.Applications.FindAllByExternalId(app.Id).Single());
			loanApp.ClosedOn = DateTime.Now;
			Drive.Data.Payments.Db.Applications.Update(loanApp);

			Thread.Sleep(delay);

			var flagToDcaCommand = new Framework.Cs.FlagApplicationToDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(flagToDcaCommand);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
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

			Thread.Sleep(delay);

			var revokeFromDcaCommand = new Framework.Cs.RevokeApplicationFromDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);

			Thread.Sleep(delay);

			var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
													.OrderByDescending(_debtCollections.CreatedOn)
													.FirstOrDefault());

			//Assert
			Assert.AreEqual(false, debtCollection.MovedToAgency);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		[ExpectedException(typeof(ValidatorException), "Payments_RevokeFromDca_ApplicationDoesNotExist")]
		public void RevokeApplicationFromDca_ForNonExitingApplication_ExpectValidationException()
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

			Thread.Sleep(delay);

			var revokeFromDcaCommand = new Framework.Cs.RevokeApplicationFromDcaCommand
			{
				ApplicationId = Guid.NewGuid()
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		[ExpectedException(typeof(ValidatorException), "RevokeApplicationFromDca_ApplicationNotOpen")]
		public void RevokeApplicationFromDca_ForClosedApplication_ExpectValidationException()
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

			Thread.Sleep(delay);

			//Close the Application
			var loanApp = Do.Until(() => Drive.Data.Payments.Db.Applications.FindAllByExternalId(app.Id).Single());
			loanApp.ClosedOn = DateTime.Now;
			Drive.Data.Payments.Db.Applications.Update(loanApp);


			var revokeFromDcaCommand = new Framework.Cs.RevokeApplicationFromDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);
		}
	}
}