using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
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

        public static void TimeoutPaymentsInArrearsSagaEntity(Guid applicationGuid)
        {
            var sagaId = Drive.Db.OpsSagas.PaymentsInArrearsSagaEntities.Single(a => a.ApplicationId == applicationGuid);
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId.Id });
        }
    }
}
