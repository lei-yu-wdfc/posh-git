using System;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;

namespace Wonga.QA.Framework
{
    public class Application
    {
        public Guid Id { get; set; }

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
            ApplicationEntity application = Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);

            TimeSpan span = application.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today;
            application.FixedTermLoanApplicationEntity.PromiseDate -= span;
            application.FixedTermLoanApplicationEntity.NextDueDate -= span;
            application.Transactions.ForEach(t => t.PostedOn -= span);
            application.Submit();

            FixedTermLoanSagaEntity ftl = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
            Do.While(ftl.Refresh);

            ServiceConfigurationEntity testmode = Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");
            if (testmode == null || !Boolean.Parse(testmode.Value))
            {
                ScheduledPaymentSagaEntity sp = Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
                Driver.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id });
                Do.While(sp.Refresh);
            }

            TransactionEntity transaction = Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Single(t => (PaymentTransactionScopeEnum)t.Scope == PaymentTransactionScopeEnum.Credit));

            CloseApplicationSagaEntity ca = Do.Until(() => Driver.Db.OpsSagas.CloseApplicationSagaEntities.Single(s => s.TransactionId == transaction.ExternalId));
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ca.Id });
            Do.While(ca.Refresh);

            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).ClosedOn);

            return this;
        }
    }
}