using System;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Framework
{
    public class Application
    {
        public Guid Id { get; set; }

        public Application(Guid id)
        {
            Id = id;
        }

        public Application Repay()
        {
            FixedTermLoanSagaEntity ftl = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(s => s.ApplicationGuid == Id);
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
            //Do.While(() => );
            //Driver.Db.Payments.Applications.Single(a => a.ExternalId == Id);
            Driver.Db.OpsSagas.FixedTermLoanInitialAdvanceSagaEntities.First();
            return this;
        }
    }
}