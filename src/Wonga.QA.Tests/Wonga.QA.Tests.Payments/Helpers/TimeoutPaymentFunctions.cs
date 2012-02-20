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
        public static void TimeoutFixedTermLoanAndSchedInterest(Guid applicationGuid)
        {
            //Timeout ScheduledPostAccruedInterestSaga to calculate the interest
            var accruedInterest = Driver.Db.OpsSagas.ScheduledPostAccruedInterestSagaEntities.Single(a => a.ApplicationGuid == applicationGuid).Id;
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = accruedInterest });

            var app = Driver.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ApplicationId;
            Do.Until(() => Driver.Db.Payments.Transactions.Single(a => (a.Type == PaymentTransactionType.Interest.ToString() && a.ApplicationId == app)), new TimeSpan(0, 0, 10, 0));

            // Timout FixedTerm Loan Saga
            var sagaId = Driver.Db.OpsSagas.FixedTermLoanSagaEntities.Single(a => a.ApplicationGuid == applicationGuid).Id;
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId, Expires = DateTime.Today });

        }

        public static Guid TimeoutScheduledPayment(Guid applicationGuid)
        {
            var sagaId = Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == applicationGuid);
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId.Id });
            return Guid.Parse(sagaId.ToString());
        }

        public static void TimeoutPaymentsInArrearsSagaEntity(Guid applicationGuid)
        {
            var sagaId = Driver.Db.OpsSagas.PaymentsInArrearsSagaEntities.Single(a => a.ApplicationId == applicationGuid);
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = sagaId.Id });
        }
    }
}
