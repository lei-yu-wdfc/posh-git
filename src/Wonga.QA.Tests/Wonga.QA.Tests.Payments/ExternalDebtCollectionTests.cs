using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db.OpsSagasCa;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using CloseApplicationSagaEntity = Wonga.QA.Framework.Db.OpsSagas.CloseApplicationSagaEntity;
using System.Threading;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class ExternalDebtCollectionTests
    {
        private static bool _bankGatewayIsinTestMode;

        [FixtureSetUp]
        public static void FixtureSetUp()
        {
            _bankGatewayIsinTestMode = ConfigurationFunctions.GetBankGatewayTestMode();

            ConfigurationFunctions.SetBankGatewayTestMode(false);
        }

        [FixtureTearDown]
        public static void FixtureTearDown()
        {
            ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayIsinTestMode);
        }

    	[Test, AUT(AUT.Ca), JIRA("CA-913")]
		public void When31DaysPassedAndArrearsCollectionSucceededThenShouldNotMoveApplicationToDca()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears();

			RepayLoanInArrears(application);

    		var transaction = WaitForCreditTransaction(application);
    		TimeoutCloseApplicationSaga(transaction);

    		// Validate that external debt collection finished
			Do.Until(() => Drive.Db.OpsSagasCa.ExternalDebtCollectionSagaEntities.SingleOrDefault(e => e.ApplicationId == application.Id) == null);

			// And no debt collection record was created
			Assert.AreEqual(0, Drive.Db.Payments.DebtCollections.Count(d => d.ApplicationEntity.ExternalId == application.Id));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-913")]
		public void When31DaysPassedThenShouldMoveApplicationToDca()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears();

			// Trigger 31 days timeout of debt collection agency
			var debtCollectionSaga = Do.Until( () => Drive.Db.OpsSagasCa.ExternalDebtCollectionSagaEntities.SingleOrDefault(e => e.ApplicationId == application.Id));
			Drive.Msmq.Payments.Send(new TimeoutMessage{ SagaId = debtCollectionSaga.Id});

			// Wait for debt collection to be created
			Do.Until( () => Drive.Db.Payments.DebtCollections.Single(d => d.ApplicationEntity.ExternalId == application.Id && d.MovedToAgency ));
		}

    	[Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void WhenApplicationMovedToDcaInterestAcrualShouldBeSuppressed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Assert.IsTrue(VerifyPaymentFunctions.VerifyInterestSuspended(application, DateTime.UtcNow.Date));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void WhenApplicationRevokedFromDcaInterestAcrualShouldBeRessumed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Drive.Cs.Commands.Post(new RevokeApplicationFromDcaCommand
                                        {
                                            ApplicationId = application.Id
                                        });

            Assert.IsTrue(VerifyPaymentFunctions.VerifyInterestResumed(application, DateTime.UtcNow.Date));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationHasNotBeenMovedToDca()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var command = new RevokeApplicationFromDcaCommand
            {
                ApplicationId = application.Id
            };

            try
            {
                Drive.Cs.Commands.Post(command);

                Assert.Fail("Exception expected.");
            }
            catch (ValidatorException exception)
            {
                Assert.AreEqual("Payments_RevokeFromDca_ApplicationHasNotBeenMovedToDca", exception.Errors.Single());
            }
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationWasNotSpecified()
        {
            var command = new RevokeApplicationFromDcaCommand
            {
                ApplicationId = Guid.Empty
            };

            try
            {
                Drive.Cs.Commands.Post(command);

                Assert.Fail("Exception expected.");
            }
            catch (ValidatorException exception)
            {
                Assert.AreEqual("Payments_RevokeFromDca_MissingApplicationId", exception.Errors.Single());
            }
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldRejectCommandBecauseApplicationDoesNotExist()
        {
            var command = new RevokeApplicationFromDcaCommand
            {
                ApplicationId = Guid.NewGuid()
            };

            try
            {
                Drive.Cs.Commands.Post(command);

                Assert.Fail("Exception expected.");
            }
            catch (ValidatorException exception)
            {
                Assert.AreEqual("Payments_RevokeFromDca_ApplicationDoesNotExist", exception.Errors.Single());
            }
        }

		//[Test, AUT(AUT.Ca), JIRA("CA-1862")]
		//public void RevokeApplicationFromDcaShouldAddARevokeRecordToDebtCollections()
		//{
		//    var customer = CustomerBuilder.New().Build();
		//    var application = ApplicationBuilder.New(customer).Build();

		//    var command = new Wonga.QA.Framework.Cs.RevokeApplicationFromDcaCommand
		//    {
		//        ApplicationId = application.Id
		//    };

		//    // TODO: Add debt collection record

		//    Drive.Cs.Commands.Post(command);

		//    // TODO: Need to query for a further debt collection record that indicates the revoke
		//}

		private static void RepayLoanInArrears(Application application)
		{
			Do.Until(() => Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(
				e => e.ApplicationId == application.Id && e.InArrearsState == "FirstAttempt"));

			PaymentsInArrearsSagaEntity entity =
				Do.Until(() => Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(e => e.ApplicationId == application.Id));
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });

			var firstPaymentTaken = new PaymentTakenCommand
			{
				ApplicationId = application.Id,
				SagaId = entity.Id,
				TransactionAmount = entity.Amount.GetValueOrDefault(),
				ValueDate = DateTime.UtcNow,
			};

			Drive.Msmq.Payments.Send(firstPaymentTaken);

			Do.Until(() => Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(
				e => e.ApplicationId == application.Id && e.InArrearsState == "SecondAttempt"));

			entity =
				Do.Until(() => Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(e => e.ApplicationId == application.Id));
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });

			var arrears = new PaymentTakenCommand
			{
				ApplicationId = application.Id,
				SagaId = entity.Id,
				TransactionAmount = entity.Amount.GetValueOrDefault(),
				ValueDate = DateTime.UtcNow,
			};

			Drive.Msmq.Payments.Send(arrears);
		}

		private TransactionEntity WaitForCreditTransaction(Application application)
		{
			return Do.Until(() => Drive.Db.Payments.Applications.Single(
				a => a.ExternalId == application.Id).Transactions.OrderByDescending(o => o.CreatedOn).
				Single(t =>(PaymentTransactionScopeEnum)t.Scope == PaymentTransactionScopeEnum.Credit));
		}

		private void TimeoutCloseApplicationSaga(TransactionEntity transaction)
		{
			CloseApplicationSagaEntity ca =
				Do.Until(() => Drive.Db.OpsSagas.CloseApplicationSagaEntities.Single(s => s.TransactionId == transaction.ExternalId));
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ca.Id });
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldCreateADebtCollectionRecord()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var command = new MoveApplicationToDcaCommand
            {
                ApplicationId = application.Id,
                AccountId = customer.Id,
                ChargeBackOccured = false
            };

            Drive.Msmq.Payments.Send(command);

            var applicationRow = Do.Until(() => Drive.Db.Payments.Applications.SingleOrDefault(e => e.ExternalId == command.ApplicationId));
            var debtCollectionRow = Do.Until(() => Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationRow.ApplicationId));
            Assert.IsNotNull(debtCollectionRow);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldNotCreateADebtCollectionRecordIfInDispute()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var db = new DbDriver();
            var customerDetailsRow = db.Payments.AccountPreferences.Single(cd => cd.AccountId == customer.Id);
            customerDetailsRow.IsDispute = true;
            db.Payments.SubmitChanges();

            var command = new MoveApplicationToDcaCommand
            {
                ApplicationId = application.Id,
                AccountId = customer.Id,
                ChargeBackOccured = false
            };

            Drive.Msmq.Payments.Send(command);

            Thread.Sleep(10000);

            var applicationRow = Drive.Db.Payments.Applications.SingleOrDefault(e => e.ExternalId == command.ApplicationId);
            var debtCollectionRow = Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationRow.ApplicationId);
            Assert.IsNull(debtCollectionRow);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldNotCreateADebtCollectionRecordIfInHardship()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var db = new DbDriver();
            var customerDetailsRow = db.Payments.AccountPreferences.Single(cd => cd.AccountId == customer.Id);
            customerDetailsRow.IsHardship = true;
            db.Payments.SubmitChanges();

            var command = new MoveApplicationToDcaCommand
            {
                ApplicationId = application.Id,
                AccountId = customer.Id,
                ChargeBackOccured = false
            };

            Drive.Msmq.Payments.Send(command);

            Thread.Sleep(10000);

            var applicationRow = Drive.Db.Payments.Applications.SingleOrDefault(e => e.ExternalId == command.ApplicationId);
            var debtCollectionRow = Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationRow.ApplicationId);
            Assert.IsNull(debtCollectionRow);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862")]
        public void ShouldNotCreateADebtCollectionRecordIfAChargeBackOccurred()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var command = new MoveApplicationToDcaCommand
            {
                ApplicationId = application.Id,
                AccountId = customer.Id,
                ChargeBackOccured = true
            };

            Drive.Msmq.Payments.Send(command);

            Thread.Sleep(10000);

            var applicationRow = Drive.Db.Payments.Applications.SingleOrDefault(e => e.ExternalId == command.ApplicationId);
            var debtCollectionRow = Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationRow.ApplicationId);
            Assert.IsNull(debtCollectionRow);
        }
	}
}