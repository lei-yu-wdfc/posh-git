using System;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;

namespace Wonga.QA.Framework
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTerm { get; set; }

        public Application()
        {
        }

        public Application(Guid id)
        {
            Id = id;
        }

        public Customer GetCustomer()
        {
            return new Customer(Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).AccountId);
        }

        public Application Repay()
        {
        	var transactionAmount = GetBalance();

            ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);

            MakeDueToday(application);

            ServiceConfigurationEntity testmode = Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");
            if (testmode == null || !Boolean.Parse(testmode.Value))
            {
                var utcNow = DateTime.UtcNow;

                ScheduledPaymentSagaEntity sp = Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
                Driver.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow, TransactionAmount = transactionAmount, ApplicationId = Id});
                Do.While(sp.Refresh);
            }

            TransactionEntity transaction = Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Single(t => (PaymentTransactionScopeEnum)t.Scope == PaymentTransactionScopeEnum.Credit && t.Amount == -transactionAmount));

            CloseApplicationSagaEntity ca = Do.Until(() => Driver.Db.OpsSagas.CloseApplicationSagaEntities.Single(s => s.TransactionId == transaction.ExternalId));
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ca.Id});
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

        private void MakeDueToday(ApplicationEntity application)
        {
            TimeSpan span = application.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today;

            RewindApplicationDates(application, span);

            FixedTermLoanSagaEntity ftl = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
            Driver.Msmq.Payments.Send(new TimeoutMessage {SagaId = ftl.Id});
            Do.While(ftl.Refresh);
        }

        private static void RewindApplicationDates(ApplicationEntity application, TimeSpan span)
        {
            application.ApplicationDate -= span;
            application.SignedOn -= span;
            application.CreatedOn -= span;
            application.AcceptedOn -= span;
            application.FixedTermLoanApplicationEntity.PromiseDate -= span;
            application.FixedTermLoanApplicationEntity.NextDueDate -= span;
            application.Transactions.ForEach(t => t.PostedOn -= span);
            application.Submit();
        }

        public Application PutApplicationIntoArrears()
		{
			ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);

			TimeSpan span = application.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today;
			application.FixedTermLoanApplicationEntity.PromiseDate -= span;
			application.FixedTermLoanApplicationEntity.NextDueDate -= span;
			application.Submit();

            if (Config.AUT == AUT.Uk)
			{
				Driver.Msmq.Payments.Send(new ProcessScheduledPaymentCommand { ApplicationId = application.ApplicationId });
			}
			else
			{
				FixedTermLoanSagaEntity ftl = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
				Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
				Do.While(ftl.Refresh);
			}

			ScheduledPaymentSagaEntity sp =
				Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
			Driver.Msmq.Payments.Send(new TakePaymentFailedCommand { SagaId = sp.Id });
			Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = sp.Id });

			Do.Until(() => Driver.Db.Payments.Arrears.Single(s => s.ApplicationId == application.ApplicationId));

			return this;
		}

		public Decimal GetBalance()
		{
			return
				new DbDriver().Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Where(
					a => a.Scope != (decimal) PaymentTransactionScopeEnum.Other).Sum(a => a.Amount);
		}

        public Application RepayEarly(decimal amount, int dayOfLoanToMakeRepayment)
        {
            int daysUntilStartOfLoan = 0;

            if (Config.AUT == AUT.Ca)
            {
                daysUntilStartOfLoan = DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
            }

            int daysToRewind = daysUntilStartOfLoan + dayOfLoanToMakeRepayment - 1;

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

        private void Rewind(int absoluteDays)
        {
            // Rewinds a Loans Dates
            ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);

            if (application.FixedTermLoanApplicationEntity.NextDueDate == null)
            {
                throw new Exception("Rewind: FixedTermLoanApplication.NextDueDate is null");
            }

            var duration = new TimeSpan(absoluteDays, 0, 0, 0);

            RewindApplicationDates(application, duration);
        }
    }
}
