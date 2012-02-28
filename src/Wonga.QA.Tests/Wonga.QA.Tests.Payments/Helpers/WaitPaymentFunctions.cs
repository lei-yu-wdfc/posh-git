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
            Do.With().Timeout(3).Interval(10).Until(
                () => Driver.Db.OpsSagas.PaymentsInArrearsSagaEntities.Single(a => (a.ApplicationId == applicationGuid)));
        }

        public static void WaitForTransactionTypeOfDirectBankPayment(Guid applicationGuid, decimal amount)
        {
            var applicationid = GetPaymentFunctions.GetApplicationId(applicationGuid);

            Do.With().Timeout(3).Interval(10).Until(
                () =>
                Driver.Db.Payments.Transactions.Single(
                    a =>
                    a.ApplicationId == applicationid & a.Type == PaymentTransactionType.DirectBankPayment.ToString() &
                    a.Amount == amount));
        }
    }
}
