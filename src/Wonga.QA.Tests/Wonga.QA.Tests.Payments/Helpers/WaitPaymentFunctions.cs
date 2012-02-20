using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class WaitPaymentFunctions
    {
        public static void WaitForApplicationToEnterIntoArrears(Guid applicationGuid)
        {
            var waiting = Timeout.ThreeMinutes;
            var interval = new TimeSpan(0, 0, 0, 10);

            Do.Until(
                () => Driver.Db.OpsSagas.PaymentsInArrearsSagaEntities.Single(a => (a.ApplicationId == applicationGuid)),
                waiting, interval);
        }

        public static void WaitForTransactionTypeOfDirectBankPayment(Guid applicationGuid, decimal amount)
        {
            var waiting = Timeout.ThreeMinutes;
            var interval = new TimeSpan(0, 0, 0, 10);

            var applicationid = GetPaymentFunctions.GetApplicationId(applicationGuid);

            Do.Until(
                () =>
                Driver.Db.Payments.Transactions.Single(
                    a =>
                    a.ApplicationId == applicationid & a.Type == PaymentTransactionType.DirectBankPayment.ToString() &
                    a.Amount == amount), waiting, interval);
        }
    }
}
