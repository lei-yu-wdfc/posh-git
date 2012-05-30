using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class TimeoutPaymentFunctions
    {
        public static Guid TimeoutScheduledPayment(Guid applicationGuid)
        {
            var sagaId = Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == applicationGuid);
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId.Id });
            return Guid.Parse(sagaId.ToString());
        }

        public static void TimeoutCloseApplicationSaga(TransactionEntity transaction)
        {
            CloseApplicationSagaEntity ca =
                Do.Until(() => Drive.Db.OpsSagas.CloseApplicationSagaEntities.Single(s => s.TransactionId == transaction.ExternalId));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ca.Id });
        }
    }
}
