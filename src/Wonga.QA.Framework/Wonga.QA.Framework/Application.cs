using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework
{
	public class Application
	{
		public Guid Id { get; set; }
		public Guid BankAccountId { get; set; }
		public decimal LoanAmount { get; set; }
		public int LoanTerm { get; set; }
		public string FailedCheckpoint { get; private set; }

		public Application()
		{
		}

		public Application(Guid id)
		{
			Id = id;
		}

		public Application(Guid id, string failedCheckpoint)
		{
			Id = id;
			FailedCheckpoint = failedCheckpoint;
		}

		public bool IsClosed
		{
			get { return Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).ClosedOn.HasValue; }
		}

		public Guid AccountId
		{
			get { return Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).AccountId; }
		}

		public Customer GetCustomer()
		{
			//avoid going to the DB twice
			Guid currentAccountId = AccountId;
			return new Customer(
				Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).AccountId,
				Driver.Db.Comms.CustomerDetails.Single(cd => cd.AccountId == currentAccountId).Email,
				Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == currentAccountId).BankAccountsBaseEntity.ExternalId);
		}

		public Application RepayOnDueDate()
		{
			ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);

			MakeDueToday(application);

			ServiceConfigurationEntity testmode = Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");
			if (Config.AUT != AUT.Uk && (testmode == null || !Boolean.Parse(testmode.Value)))
			{
				var utcNow = DateTime.UtcNow;

				ScheduledPaymentSagaEntity sp = Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
                Driver.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow,  ApplicationId = Id, TransactionAmount = GetBalance()});
				Do.While(sp.Refresh);
			}

			TransactionEntity transaction = Do.Until(() => Driver.Db.Payments.Applications.Single(
                a => a.ExternalId == Id).Transactions.Single(t => (PaymentTransactionScopeEnum)t.Scope == PaymentTransactionScopeEnum.Credit && t.Type == Data.EnumToString(
                    Config.AUT == AUT.Uk ? PaymentTransactionEnum.CardPayment : PaymentTransactionEnum.DirectBankPayment)));

			CloseApplicationSagaEntity ca = Do.Until(() => Driver.Db.OpsSagas.CloseApplicationSagaEntities.Single(s => s.TransactionId == transaction.ExternalId));
			Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ca.Id });
			Do.While(ca.Refresh);

			Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).ClosedOn);

			return this;
		}

		public Application MakeDueToday()
		{
			ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);

			MakeDueToday(application);

			return this;
		}

		public void MakeDueToday(ApplicationEntity application)
		{
			TimeSpan span = application.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today;
            RiskApplicationEntity riskApplication = Driver.Db.Risk.RiskApplications.Single(r => r.ApplicationId == Id);

			RewindApplicationDates(application, riskApplication, span);

			FixedTermLoanSagaEntity ftl = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
			Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
			Do.While(ftl.Refresh);
		}

        public virtual Application PutApplicationIntoArrears(int daysInArrears)
		{
            PutApplicationIntoArrears();

            Rewind(daysInArrears);

            return this;
		}

        public virtual Application PutApplicationIntoArrears()
		{
            ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);
            DateTime dueDate = application.FixedTermLoanApplicationEntity.NextDueDate ??
                              application.FixedTermLoanApplicationEntity.PromiseDate;

            TimeSpan span = dueDate - DateTime.Today;

            RewindApplicationDates(application, span);

            ScheduledPostAccruedInterestSagaEntity entity = Driver.Db.OpsSagas.ScheduledPostAccruedInterestSagaEntities.Single(a => a.ApplicationGuid == Id);
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });

            FixedTermLoanSagaEntity ftl = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
            Do.While(ftl.Refresh);

            ScheduledPaymentSagaEntity sp = Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
            Driver.Msmq.Payments.Send(new TakePaymentFailedCommand { SagaId = sp.Id, CreatedOn = DateTime.UtcNow, ValueDate = DateTime.UtcNow });
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = sp.Id });

            Do.Until(() => Driver.Db.Payments.Arrears.Single(s => s.ApplicationId == application.ApplicationId));

            return this;
		}

		public Application CreateRepaymentArrangement()
		{
			var cmd = new Api.CreateRepaymentArrangementCommand()
			{
				ApplicationId = Id,
				Frequency = Api.PaymentFrequencyEnum.Every4Weeks,
				RepaymentDates = new[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1) },
				NumberOfMonths = 2
			};
			Driver.Api.Commands.Post(cmd);

			var dbApplication = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);
			Do.Until(() => Driver.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));

			return this;
		}

		public Decimal GetBalance()
		{
			return
				new DbDriver().Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Where(
					a => a.Scope != (decimal)PaymentTransactionScopeEnum.Other).Sum(a => a.Amount);
		}

		public Application RepayEarly(decimal amount, int dayOfLoanToMakeRepayment)
		{
            var daysToRewind = GetAbsoluteDaysToRewind(dayOfLoanToMakeRepayment);

            Rewind(daysToRewind);

			Guid repaymentRequestId = Guid.NewGuid();

			Driver.Msmq.Payments.Send(new RepayLoanInternalViaBankCommand
			{
				Amount = amount,
				ApplicationId = Id,
				CashEntityId = BankAccountId,
				RepaymentRequestId = repaymentRequestId
			});

			ServiceConfigurationEntity testmode = Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");

			if (testmode == null || !Boolean.Parse(testmode.Value))
			{
				var utcNow = DateTime.UtcNow;

				Int32 applicationid =
					Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).ApplicationId;

				RepaymentSagaEntity sp = Do.Until(() => Driver.Db.OpsSagas.RepaymentSagaEntities.Single(s => s.ApplicationId == applicationid));
				Driver.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow });
				Do.While(sp.Refresh);
			}

			return this;
		}

        /// <summary>
        /// This metod ASSUMES that there is only one workflow for an application and returns a list of executed checkpoints for it
        /// </summary>
        /// <param name="applicationId">The GUID of the application</param>
        /// <param name="expectedStatus">Optional:The expected status</param>
        /// <returns>Returns a list of CheckpointDefinitions.Name</returns>
        public static List<String> GetExecutedCheckpointsDefinitionsForApplicationId(Guid applicationId, params CheckpointStatus[] expectedStatus)
        {
            var db = new DbDriver();
            var riskWorkflowEntity = db.Risk.RiskWorkflows.SingleOrDefault(r => r.ApplicationId == applicationId);
            var executedCheckpoints = new List<string>();

            if (riskWorkflowEntity != null)
            {
                var executedCheckpointIds = expectedStatus.Any()
                                                 ? db.Risk.WorkflowCheckpoints.Where(
                                                     p =>
                                                     p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId &&
                                                     expectedStatus.Contains((CheckpointStatus)p.CheckpointStatus)).Select(
                                                         p => p.CheckpointDefinitionId).ToList()
                                                 : db.Risk.WorkflowCheckpoints.Where(
                                                     p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId).
                                                       Select(p => p.CheckpointDefinitionId).ToList();
                executedCheckpoints.AddRange(db.Risk.CheckpointDefinitions.Where(p => executedCheckpointIds.Contains(p.CheckpointDefinitionId)).Select(p => p.Name));
                return executedCheckpoints;
            }
            return executedCheckpoints;
        }

		/// <summary>
		/// This method returns a list of checkpoints that were executed for a given Risk Workflow id and an optional array of statuses.
		/// </summary>
		/// <param name="workflowId">The GUID of the workflow</param>
		/// <param name="expectedStatus">Optional:The expected status</param>
		/// <returns>Returns a list of CheckpointDefinitions.Name</returns>
		public static List<String> GetExecutedCheckpointDefinitionsForRiskWorkflow(Guid workflowId, params CheckpointStatus[] expectedStatus)
		{
			var db = new DbDriver();
		    var riskWorkflowEntity = db.Risk.RiskWorkflows.SingleOrDefault(r => r.WorkflowId == workflowId);
			var executedCheckpoints = new List<string>();

            if (riskWorkflowEntity != null)
            {
                var executedCheckpointIds = expectedStatus.Any()
                                                 ? db.Risk.WorkflowCheckpoints.Where(
                                                     p =>
                                                     p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId &&
                                                     expectedStatus.Contains((CheckpointStatus)p.CheckpointStatus)).Select(
                                                         p => p.CheckpointDefinitionId).ToList()
                                                 : db.Risk.WorkflowCheckpoints.Where(
                                                     p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId).
                                                       Select(p => p.CheckpointDefinitionId).ToList();
                executedCheckpoints.AddRange(db.Risk.CheckpointDefinitions.Where(p => executedCheckpointIds.Contains(p.CheckpointDefinitionId)).Select(p => p.Name));
                return executedCheckpoints;
            }
			return executedCheckpoints;
		}

        /// <summary>
        /// This method returns a list of verifications that were executed for a given Risk Workflow id and an optional array of statuses.
        /// </summary>
        /// <param name="workflowId">The GUID of the workflow</param>
        /// <returns>Returns a list of CheckpointDefinitions.Name</returns>
        public static List<String> GetExecutedVerificationDefinitionsForRiskWorkflow(Guid workflowId)
        {
            var db = new DbDriver();
            var riskWorkflowEntity = db.Risk.RiskWorkflows.SingleOrDefault(r => r.WorkflowId == workflowId);
            var executedVerifications = new List<string>();

            if (riskWorkflowEntity != null)
            {
                var executedVerificationsIds =
                    db.Risk.WorkflowVerifications.Where(p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId).
                        Select(p => p.VerificationDefinitionId).ToList();

                executedVerifications.AddRange(db.Risk.VerificationDefinitions.Where(p=>executedVerificationsIds.Contains(p.VerificationDefinitionId)).Select(p=>p.Name));
                return executedVerifications;
            }
            return executedVerifications;
        }

        /// <summary>
        /// This function returns a list of Workflow entities for a given ApplicationId
        /// </summary>
        /// <param name="applicationId">The GUID of the application</param>
        /// <returns></returns>
        public static  List<RiskWorkflowEntity> GetWorkflowsForApplication(Guid applicationId)
        {
            var db = new DbDriver();
            return db.Risk.RiskWorkflows.Where(p => p.ApplicationId == applicationId).ToList();
        }



		private void Rewind(int absoluteDays)
		{
			// Rewinds a Loans Dates
			ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);
		    RiskApplicationEntity riskApplication = Driver.Db.Risk.RiskApplications.Single(r => r.ApplicationId == Id);

			if (application.FixedTermLoanApplicationEntity.NextDueDate == null)
			{
				throw new Exception("Rewind: FixedTermLoanApplication.NextDueDate is null");
			}

			var duration = new TimeSpan(absoluteDays, 0, 0, 0);

			RewindApplicationDates(application, riskApplication, duration);
        }

		private static void RewindApplicationDates(ApplicationEntity application, RiskApplicationEntity riskApp, TimeSpan span)
		{
			application.ApplicationDate -= span;
			application.SignedOn -= span;
			application.CreatedOn -= span;
			application.AcceptedOn -= span;
			application.FixedTermLoanApplicationEntity.PromiseDate -= span;
			application.FixedTermLoanApplicationEntity.NextDueDate -= span;
			application.Transactions.ForEach(t => t.PostedOn -= span);
            if (application.ClosedOn != null)
                application.ClosedOn -= span;


		    riskApp.ApplicationDate -= span;
            riskApp.PromiseDate -= span;
            if (riskApp.ClosedOn != null)
                riskApp.ClosedOn -= span;
		    riskApp.Submit();
		    
        }

        public void RewindToDayOfLoanTerm(int dayOfLoanTerm)
        {
            var daysToRewind = GetAbsoluteDaysToRewind(dayOfLoanTerm);

            Rewind(daysToRewind);

        }

        private static int GetAbsoluteDaysToRewind(int dayOfLoanToMakeRepayment)
        {
            int daysUntilStartOfLoan = 0;

            if (Config.AUT == AUT.Ca)
            {
                daysUntilStartOfLoan = DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
            }

            int daysToRewind = daysUntilStartOfLoan + dayOfLoanToMakeRepayment - 1;
            return daysToRewind;
        }
	}
}
