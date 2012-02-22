using System;
using System.Linq;
using Gallio.Framework;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class SetPaymentFunctions
    {
        public const string BankGateWayIsTestMode = "BankGateway.IsTestMode";
        private const string DelayBeforeApplicationClosedInMinutes = "Payments.DelayBeforeApplicationClosedInMinutes";
        private const string VariableInterestRateEnabled = "Payments.VariableInterestRateEnabled";

        public static void SetBankGatewayTestMode(Boolean value)
        {
            var db = new DbDriver();
            var bgw = db.Ops.ServiceConfigurations.Single(bg => bg.Key == BankGateWayIsTestMode);
            var dbValue = bgw.Value;
            var wantedValue = value.ToString().ToLower();
            if (dbValue != wantedValue)
            {
                bgw.Value = wantedValue;
                db.Ops.SubmitChanges();
            }
        }

        public static void SetVariableInterestRateEnabled(bool value)
        {
            var db = new DbDriver();
            var row = db.Ops.ServiceConfigurations.Single(v => v.Key == VariableInterestRateEnabled);
            row.Value = value.ToString();
            db.Ops.SubmitChanges();
            TestLog.DebugTrace.WriteLine("SetVariableInterestRateEnabled -> {0}",
                                         db.Ops.ServiceConfigurations.Single(
                                             v => v.Key == VariableInterestRateEnabled.ToString()).
                                             Value);
        }

        public static void SetDelayBeforeApplicationClosed(int delayInMinutes)
        {
            var db = new DbDriver();
            var delayInMinutesString = delayInMinutes.ToString();
            var row = db.Ops.ServiceConfigurations.Single(v => v.Key == DelayBeforeApplicationClosedInMinutes);
            row.Value = delayInMinutesString;
            db.Ops.SubmitChanges();
        }

        public static void SetVariableInterestRates(decimal numberOfPoints)
        {
            var db = new DbDriver();
            for (var i = 1; i < db.Payments.VariableInterestRateDetails.Count(v => v.VariableInterestRateId == 1); i++)
            {
                var row = db.Payments.VariableInterestRateDetails.Single(v => v.VariableInterestRateId == 1 && v.VariableInterestRateDetailId == i);
                row.MonthlyInterestRate = row.MonthlyInterestRate + numberOfPoints;
            }

            db.Payments.SubmitChanges();
        }
    }
}
