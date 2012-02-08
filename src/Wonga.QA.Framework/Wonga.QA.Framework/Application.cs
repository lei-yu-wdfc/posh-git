using System;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
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

            ScheduledPaymentSagaEntity sp = Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationGuid == Id));
            Driver.Msmq.Payments.Send(new PaymentTakenCommand { SagaId = sp.Id });
            Do.While(sp.Refresh);

            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id).ClosedOn);

            return this;
        }
    }
}