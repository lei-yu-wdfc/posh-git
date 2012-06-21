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
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Payments.Command
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class DcaCommandTests
	{
	    private dynamic _debtCollections = Drive.Data.Payments.Db.DebtCollection;
	    private dynamic _fixedTermLoanSagas = Drive.Data.OpsSagas.Db.FixedTermLoanSagaEntity;
	    private dynamic _externalDebtCollectionSagas = Drive.Data.OpsSagas.Db.ExternalDebtCollectionSagaEntity;
	    private dynamic _schedulePaymentSagas = Drive.Data.OpsSagas.Db.ScheduledPaymentSagaEntity;
		private const int delay = 15000;
        
		[FixtureSetUp]
		public void FixtureSetUp()
		{
			if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
				Assert.Inconclusive("Bankgateway is in test mode");
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		public void FlagApplicationToDca_ShouldMoveApplicationToDCA()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			FlagDca(app.Id);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		[ExpectedException(typeof(ValidatorException), "Payments_FlagToDca_ApplicationDoesNotExist")]
		public void FlagApplicationToDca_ForNonExistingApplication_ExpectValidationException()
		{
			FlagDca(Guid.NewGuid());
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2147"), Pending("ZA-2565")]
		[ExpectedException(typeof(ValidatorException), "FlagApplicationToDca_ApplicationNotOpen")]
		public void FlagApplicationToDca_ForClosedApplication_ExpectValidationException()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().RepayOnDueDate();

			FlagDca(app.Id);
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

			//Wait for dca creation
			Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
							.OrderByDescending(_debtCollections.CreatedOn)
							.FirstOrDefault());

			var revokeFromDcaCommand = new RevokeApplicationFromDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);

			//Thread.Sleep(delay);

			Do.Until(() => (bool)(_debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
									.OrderByDescending(_debtCollections.CreatedOn)
									.FirstOrDefault()).MovedToAgency == false);

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

			//Wait for dca creation
			Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == app.Id)
							.OrderByDescending(_debtCollections.CreatedOn)
							.FirstOrDefault());

			var revokeFromDcaCommand = new RevokeApplicationFromDcaCommand
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

			FlagDca(app.Id);
			app.RepayOnDueDate();

			var revokeFromDcaCommand = new RevokeApplicationFromDcaCommand
			{
				ApplicationId = app.Id
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);
		}

		#region Helpers

		private void FlagDca(Guid applicationId)
		{
			var flagToDcaCommand = new FlagApplicationToDcaCommand
			{
				ApplicationId = applicationId
			};

			Drive.Cs.Commands.Post(flagToDcaCommand);

			var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == applicationId)
													.OrderByDescending(_debtCollections.CreatedOn)
													.FirstOrDefault());
			Assert.IsNotNull(debtCollection);
			Assert.AreEqual(true, debtCollection.MovedToAgency);
		}

		#endregion
	}
}