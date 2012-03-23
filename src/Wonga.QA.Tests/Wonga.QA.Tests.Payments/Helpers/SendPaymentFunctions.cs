using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class SendPaymentFunctions
    {
        public static void SendPaymentTaken(Guid applicationGuid)
        {
            var applicationid = GetPaymentFunctions.GetApplicationId(applicationGuid);
            var scheduledpaymentsaga =
                Do.With.Timeout(5).Interval(10).Until(
                    () =>
                    Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationId == applicationid).Id);
            Drive.Msmq.Payments.Send(new PaymentTakenCommand
                                          {
                                              SagaId = scheduledpaymentsaga,
                                              ValueDate = DateTime.UtcNow,
                                              CreatedOn = DateTime.UtcNow
                                          });


        }

        public static void SendPaymentTakenForPaymentsInArrears(Guid applicationGuid)
        {
            var repayLoan =
                Do.Until(
                    () =>
                    Drive.Db.OpsSagas.PaymentsInArrearsSagaEntities.Single(s => s.ApplicationId == applicationGuid).Id);
            Drive.Msmq.Payments.Send(new PaymentTakenCommand {SagaId = repayLoan, ValueDate = DateTime.UtcNow});
        }

        public static void SendPaymentTakenForRepayLoan(Guid applicationGuid)
        {
            var applicationid = GetPaymentFunctions.GetApplicationId(applicationGuid);
            var repayLoan =
                Do.Until(() => Drive.Db.OpsSagas.RepaymentSagaEntities.Single(s => s.ApplicationId == applicationid).Id);

            Drive.Msmq.Payments.Send(new PaymentTakenCommand {SagaId = repayLoan, ValueDate = DateTime.UtcNow});
        }

        public static void SendTakePaymentFailed(Guid applicationGuid)
        {
            var application = Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid);
            var applicationId = application.ApplicationId;
            var accountId = application.ExternalId;

            var scheduledpaymentsaga =
                Do.Until(
                    () =>
                    Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationId == applicationId).Id);

            Drive.Msmq.Payments.Send(new TakePaymentFailedCommand
                                          {
                                              AccountId = accountId,
                                              SagaId = scheduledpaymentsaga,
                                              ValueDate = DateTime.UtcNow
                                          });
        }

        public static void SendRepayLoanInternalViaBank(Guid applicationGuid, decimal amount, Guid bankAccountGuid)
        {
            var repaymentRequestId = Guid.NewGuid();
            Drive.Msmq.Payments.Send(new RepayLoanInternalViaBankCommand
                                          {
                                              Amount = amount,
                                              ApplicationId = applicationGuid,
                                              CashEntityId = bankAccountGuid,
                                              RepaymentRequestId = repaymentRequestId
                                          });
        }
    }
}
