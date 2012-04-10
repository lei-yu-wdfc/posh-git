using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
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
			get { return Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).ClosedOn.HasValue; }
		}

		public Guid AccountId
		{
			get { return Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).AccountId; }
		}

		public Customer GetCustomer()
		{
			//avoid going to the DB twice
			Guid currentAccountId = AccountId;
			return new Customer(
				Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).AccountId,
				Drive.Db.Comms.CustomerDetails.Single(cd => cd.AccountId == currentAccountId).Email,
				Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == currentAccountId).BankAccountsBaseEntity.ExternalId);
		}

		public Application RepayOnDueDate()
		{
			ApplicationEntity application = Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id);

			MakeDueToday(application);

			ServiceConfigurationEntity testmode = Drive.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");
			if (Config.AUT != AUT.Uk && (testmode == null || !Boolean.Parse(testmode.Value)))
			{
				var utcNow = DateTime.UtcNow;

				ScheduledPaymentSagaEntity sp = Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
				Drive.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow, ApplicationId = Id, TransactionAmount = GetBalance() });
				Do.While(sp.Refresh);
			}

			var transaction = WaitForDirectBankPaymentCreditTransaction();

			TimeoutCloseApplicationSaga(transaction);

		    Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).ClosedOn);

			return this;
		}

	    private static void TimeoutCloseApplicationSaga(TransactionEntity transaction)
	    {
	        CloseApplicationSagaEntity ca =
	            Do.Until(
	                () => Drive.Db.OpsSagas.CloseApplicationSagaEntities.Single(s => s.TransactionId == transaction.ExternalId));
	        Drive.Msmq.Payments.Send(new TimeoutMessage {SagaId = ca.Id});
	        Do.While(ca.Refresh);
	    }

	    private TransactionEntity WaitForDirectBankPaymentCreditTransaction()
	    {
	        return Do.Until(() => Drive.Db.Payments.Applications.Single(
	            a => a.ExternalId == Id).Transactions.Single(
	                t =>
	                (PaymentTransactionScopeEnum) t.Scope == PaymentTransactionScopeEnum.Credit && t.Type == Get.EnumToString(
	                    Config.AUT == AUT.Uk ? PaymentTransactionEnum.CardPayment : PaymentTransactionEnum.DirectBankPayment)));
	    }

	    public Application MakeDueToday()
		{
			ApplicationEntity application = Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id);

			MakeDueToday(application);

			return this;
		}

        private void RewindAppDates(ApplicationEntity application)
        {
            TimeSpan span = application.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today;
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == Id);

            Drive.Db.RewindApplicationDates(application, riskApplication, span);
        }

		public void MakeDueToday(ApplicationEntity application)
		{
			RewindAppDates(application);

			FixedTermLoanSagaEntity ftl = Drive.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
			Do.With.While(ftl.Refresh);
		}

		public virtual Application PutApplicationIntoArrears(int daysInArrears)
		{
			PutApplicationIntoArrears();

			Drive.Db.Rewind(Id, daysInArrears);

			return this;
		}

		public virtual Application PutApplicationIntoArrears()
		{
			ApplicationEntity application = Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id);
			DateTime dueDate = application.FixedTermLoanApplicationEntity.NextDueDate ??
							  application.FixedTermLoanApplicationEntity.PromiseDate;
			RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == Id);

			TimeSpan span = dueDate - DateTime.Today;

			Drive.Db.RewindApplicationDates(application, riskApplication, span);

			ScheduledPostAccruedInterestSagaEntity entity = Drive.Db.OpsSagas.ScheduledPostAccruedInterestSagaEntities.Single(a => a.ApplicationGuid == Id);
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });

			FixedTermLoanSagaEntity ftl = Drive.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
			Do.While(ftl.Refresh);

			ScheduledPaymentSagaEntity sp = Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
			Drive.Msmq.Payments.Send(new TakePaymentFailedCommand { SagaId = sp.Id, CreatedOn = DateTime.UtcNow, ValueDate = DateTime.UtcNow });
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sp.Id });

			Do.Until(() => Drive.Db.Payments.Arrears.Single(s => s.ApplicationId == application.ApplicationId));

			return this;
		}

		public Application CreateRepaymentArrangement()
		{
			var cmd = new Api.CreateRepaymentArrangementCommand
			{
				ApplicationId = Id,
				Frequency = Api.PaymentFrequencyEnum.Every4Weeks,
				RepaymentDates = new[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1) },
				NumberOfMonths = 2
			};
			Drive.Api.Commands.Post(cmd);

			var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id);
			Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));

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
            Drive.Db.RewindToDayOfLoanTerm(Id, dayOfLoanToMakeRepayment);

			Guid repaymentRequestId = Guid.NewGuid();

			Drive.Msmq.Payments.Send(new RepayLoanInternalViaBankCommand
			{
				Amount = amount,
				ApplicationId = Id,
				CashEntityId = BankAccountId,
				RepaymentRequestId = repaymentRequestId
			});

			ServiceConfigurationEntity testmode = Drive.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");

			if (testmode == null || !Boolean.Parse(testmode.Value))
			{
				var utcNow = DateTime.UtcNow;

				Int32 applicationid =
					Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).ApplicationId;

				RepaymentSagaEntity sp = Do.Until(() => Drive.Db.OpsSagas.RepaymentSagaEntities.Single(s => s.ApplicationId == applicationid));
                Drive.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow, ApplicationId = Id });
				Do.While(sp.Refresh);
			}

            var transaction = WaitForDirectBankPaymentCreditTransaction();

            // Not sure this should be a part of this method 
            TimeoutCloseApplicationSaga(transaction);

			return this;
		}

        public Application MoveToDebtCollectionAgency()
        {
            ServiceConfigurationEntity serviceConfiguration =  Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Payments.MoveToDcaDelayInMinutes");

            int daysToRewind = int.Parse(serviceConfiguration.Value)/60/24;

            Drive.Db.Rewind(Id, daysToRewind);

            var entity = Do.Until(() => Drive.Db.OpsSagasCa.ExternalDebtCollectionSagaEntities.Single(e => e.ApplicationId == Id));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });

            Do.Until(() => Drive.Db.Payments.DebtCollections.Single(d => d.ApplicationEntity.ExternalId == Id && d.MovedToAgency));

            return this;
        }
	}
}