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
    	private const string MultipleRepresentmentsInArrearsKeyName = "FeatureSwitch.MultipleRepresentmentsInArrears";

    	[Test, AUT(AUT.Ca), JIRA("CA-913")]
		public void When31DaysPassedAndArrearsCollectionSucceededThenShouldNotMoveApplicationToDca()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears();

			RepayLoanInArrears(application);

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

		[Test, AUT(AUT.Ca), JIRA("CA-1862")]
		public void RevokeApplicationFromDcaShouldAddARevokeRecordToDebtCollections()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var command = new RevokeApplicationFromDcaCommand
			{
				ApplicationId = application.Id
			};

			var db = new DbDriver();
			
			var applicationEntity = db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			var debtCollection = new DebtCollectionEntity
				                     	{
				                     		ApplicationEntity = applicationEntity,
				                     		CreatedOn = DateTime.UtcNow,
				                     		MovedToAgency = true,
				                     	};
			db.Payments.DebtCollections.Insert(debtCollection);
			db.Payments.SubmitChanges();
			
			Drive.Cs.Commands.Post(command);

			Do.Until( () => Drive.Db.Payments.DebtCollections.Single(a => a.ApplicationEntity.ExternalId == application.Id && !a.MovedToAgency));
		}

		private static void RepayLoanInArrears(Application application)
		{
			WaitForArrearsSaga(application.Id, 1);

			Guid sagaId = GetArrearsSagaId(application.Id);

			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId });

			SendPaymentTakenCommand(application, sagaId);

			WaitForArrearsSaga(application.Id, 2);

			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId });

			SendPaymentTakenCommand(application, sagaId);

			if (Drive.Data.Ops.GetServiceConfiguration<bool>(MultipleRepresentmentsInArrearsKeyName))
			{
				WaitForArrearsSaga(application.Id, 3);

				Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId });

				SendPaymentTakenCommand(application, sagaId);
			}

		}

    	private static void SendPaymentTakenCommand(Application application, Guid sagaId)
    	{
    		Decimal? amount = GetArrearsAmountSent(application.Id);
    		var firstPaymentTaken = new PaymentTakenCommand
    		                        	{
    		                        		ApplicationId = application.Id,
    		                        		SagaId = sagaId,
    		                        		TransactionAmount = amount.GetValueOrDefault(),
    		                        		ValueDate = DateTime.UtcNow,
    		                        	};

    		Drive.Msmq.Payments.Send(firstPaymentTaken);
    	}

    	private static Guid GetArrearsSagaId(Guid applicationId)
		{
			if (Drive.Data.Ops.GetServiceConfiguration<bool>(MultipleRepresentmentsInArrearsKeyName))
			{
				return Drive.Data.OpsSagas.Db.MultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationId).Id;
			}
			
			return Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(e => e.ApplicationId == applicationId).Id;
			
		}

		private static decimal ? GetArrearsAmountSent(Guid applicationId)
		{
			if (Drive.Data.Ops.GetServiceConfiguration<bool>(MultipleRepresentmentsInArrearsKeyName))
			{
				return Drive.Data.OpsSagas.Db.MultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationId).LastRepresentmentAmount;
			}

			return Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(e => e.ApplicationId == applicationId).Amount;

		}

		private static void WaitForArrearsSaga(Guid applicationId, int attempt)
		{
			if (Drive.Data.Ops.GetServiceConfiguration<bool>(MultipleRepresentmentsInArrearsKeyName))
			{
				//representments sent has 0 value initially
				int arrearsSent = attempt - 1;
				Do.Until(() => Drive.Data.OpsSagas.Db.MultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(ApplicationId: applicationId, RepresentmentsSent: arrearsSent));
			}
			else
			{
				string arrearsState = GetArrearsStateFromAttempt(attempt);
				Do.Until(() => Drive.Db.OpsSagasCa.PaymentsInArrearsSagaEntities.Single(
					e => e.ApplicationId == applicationId && e.InArrearsState == arrearsState));
			}
		}

		private static string GetArrearsStateFromAttempt(int attempt)
		{
			switch (attempt)
			{
				case 1:
					return "FirstAttempt";

				case 2:
					return "SecondAttempt";

				default:
					throw new NotImplementedException();
			}
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

            var applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(e => e.ExternalId == command.ApplicationId));
            var debtCollectionEntity = Do.Until(() => Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationEntity.ApplicationId));
            Assert.IsNotNull(debtCollectionEntity);
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

            var applicationEntity = Drive.Db.Payments.Applications.Single(e => e.ExternalId == command.ApplicationId);
            var debtCollectionEntity = Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationEntity.ApplicationId);
            Assert.IsNull(debtCollectionEntity);
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

            var applicationEntity = Drive.Db.Payments.Applications.Single(e => e.ExternalId == command.ApplicationId);
            var debtCollectionEntity = Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationEntity.ApplicationId);
            Assert.IsNull(debtCollectionEntity);
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

            var applicationEntity = Drive.Db.Payments.Applications.Single(e => e.ExternalId == command.ApplicationId);
            var debtCollectionEntity = Drive.Db.Payments.DebtCollections.SingleOrDefault(e => e.ApplicationId == applicationEntity.ApplicationId);
            Assert.IsNull(debtCollectionEntity);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1862"), Ignore("SF takes too long to respond and therefore test keeps timing out")]
        public void WhenApplicationMovedToDcaSalesForceStatusShouldBeUpdated()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Do.With.Timeout(2).Until(() =>
            {
                var salesForceLoan = Drive.ThirdParties.Salesforce.GetApplicationById(application.Id);
                return (int)Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus.DCA == (int)salesForceLoan.Status_ID__c.Value;
            });
        }
	}
}