using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading;
using Gallio.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class VerifyPaymentFunctions
    {
        public static bool VerifyFixedTermLoanOfferQueryRates(List<VariableInterestRateDetailEntity> actualVariableRates, List<VariableInterestRateDetailEntity> expectedVariableRates)
        {
            if (actualVariableRates.Count != expectedVariableRates.Count)
            {
                TestLog.DebugTrace.WriteLine("VerifyVariableInterestRatesApplied -> actual.Count != expected.Count\n");
                return false;
            }

            var numOfTransactions = expectedVariableRates.Count;
            var counter = 0;

            while (counter < numOfTransactions)
            {
                TestLog.DebugTrace.WriteLine("VerifyFixedTermLoanOfferQueryRates: PostedOn: actual -> expected: {0} -> {1}", actualVariableRates[counter].Day, expectedVariableRates[counter].Day);
                TestLog.DebugTrace.WriteLine("VerifyFixedTermLoanOfferQueryRates: Mir: actual -> expected: {0} -> {1}\n", actualVariableRates[counter].MonthlyInterestRate, expectedVariableRates[counter].MonthlyInterestRate);

                if (actualVariableRates[counter].Day != expectedVariableRates[counter].Day)
                {
                    return false;
                }
                if (actualVariableRates[counter].MonthlyInterestRate != expectedVariableRates[counter].MonthlyInterestRate)
                {
                    return false;
                }
                counter++;
            }

            return true;
        }

        public static bool VerifyVariableInterestCharged(decimal actualInterestAmountApplied, decimal expectedInterestAmountApplied)
        {
            TestLog.DebugTrace.WriteLine(
                "VerifyVariableInterestCharged: actualInterestAmountApplied -> expectedInterestAmountApplied   {0} -> {1}\n", actualInterestAmountApplied,
                expectedInterestAmountApplied);

            return actualInterestAmountApplied == expectedInterestAmountApplied;
        }

        public static bool VerifyApplicationNotClosed(Guid applicationGuid)
        {
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ClosedOn == null);

            return true;
        }

        public static bool VerifyApplicationNotClosedAfterCashIn(Guid applicationGuid)
        {
            Do.Until(() => Drive.Db.Payments.Applications.Single(
                a => a.ExternalId == applicationGuid).Transactions.Single(
                    t =>
                    (PaymentTransactionScopeEnum)t.Scope == PaymentTransactionScopeEnum.Credit && t.Type == Get.EnumToString(
                        Config.AUT == AUT.Uk ? PaymentTransactionEnum.CardPayment : PaymentTransactionEnum.DirectBankPayment)));

            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ClosedOn == null);

            return true;
        }

        public static bool VerifyApplicationInArrears(Guid applicationGuid)
        {
            return
                Do.With.Timeout(2).Interval(10).Until(
                    () => Drive.Db.OpsSagas.PaymentsInArrearsSagaEntities.Single(s => s.ApplicationId == applicationGuid)) != null;
        }

        public static bool VerifyVariableInterestRatesApplied(List<TransactionEntity> actual, List<TransactionEntity> expected)
        {
            if (actual.Count != expected.Count)
            {
                TestLog.DebugTrace.WriteLine("VerifyVariableInterestRatesApplied -> actual.Count != expected.Count\n");
                return false;
            }

            var numOfTransactions = expected.Count;
            var counter = 0;

            while (counter < numOfTransactions)
            {
                TestLog.DebugTrace.WriteLine("VerifyVariableInterestRatesApplied: PostedOn: actual -> expected: {0} -> {1}", actual[counter].PostedOn.Date, expected[counter].PostedOn.Date);
                TestLog.DebugTrace.WriteLine("VerifyVariableInterestRatesApplied: Mir: actual -> expected: {0} -> {1}\n", actual[counter].Mir, expected[counter].Mir);

                if (actual[counter].PostedOn.Date != expected[counter].PostedOn.Date)
                {
                    return false;
                }
                if (actual[counter].Mir != expected[counter].Mir)
                {
                    return false;
                }
                counter++;
            }

            return true;
        }

        public static bool VerifyDirectBankPaymentOfAmount(Guid applicationGuid, decimal amount)
        {
            int appId = Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ApplicationId;

            Do.With.Timeout(3).Interval(10).Until(() => Drive.Db.Payments.Transactions.Single(a => a.ApplicationId == appId & a.Type == PaymentTransactionType.DirectBankPayment.ToString() & a.Amount == amount));
            return true;
        }

        public static bool VerifyNoDefaultChargeApplied(Guid applicationGuid)
        {
            var appId = Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ApplicationId;

            return Drive.Db.Payments.Transactions.SingleOrDefault((s => s.ApplicationId == appId & s.Type == PaymentTransactionType.DefaultCharge.ToString())) == null;
        }

        public static bool VerifyOnlineBillPaymentRecordForCcin(String ccin)
        {
            return Do.With.Timeout(2).Interval(10).Until(() => Drive.Db.Payments.OnlineBillPayments.Single(c => c.Ccin == ccin)) != null;
        }

        internal static bool VerifyInterestSuspended(Application application, DateTime suspensionDate)
        {
            return
                Do.Until(
                    () =>
                    Drive.Db.Payments.Transactions.Any(
                        t =>
                        t.ApplicationEntity.ExternalId == application.Id &&
                        t.CreatedOn.Date == suspensionDate.Date &&
                        t.Type == PaymentTransactionType.SuspendInterestAccrual.ToString()));
        }

        internal static bool VerifyInterestResumed(Application application, DateTime resumptionDate)
        {
            return
                Do.Until(
                    () =>
                    Drive.Db.Payments.Transactions.Any(
                        t =>
                        t.ApplicationEntity.ExternalId == application.Id &&
                        t.CreatedOn.Date == resumptionDate.Date &&
                        t.Type == PaymentTransactionType.SuspendInterestAccrual.ToString()));
        }
    }
}
