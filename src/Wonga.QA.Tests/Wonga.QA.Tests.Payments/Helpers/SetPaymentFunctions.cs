﻿using System;
using System.Linq;
using Gallio.Framework;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class SetPaymentFunctions
    {
        public const string BankGateWayIsTestMode = "BankGateway.IsTestMode";
        private const string DelayBeforeApplicationClosedInMinutes = "Payments.DelayBeforeApplicationClosedInMinutes";
        private const string VariableInterestRateEnabled = "Payments.VariableInterestRateEnabled";

        public static void SetBankGatewayTestMode(Boolean value)
        {
            var bgw = Driver.Db.Ops.ServiceConfigurations.Single(bg => bg.Key == BankGateWayIsTestMode);
            var dbValue = bgw.Value;
            var wantedValue = value.ToString().ToLower();
            if (dbValue != wantedValue)
            {
                bgw.Value = wantedValue;
                Driver.Db.Ops.SubmitChanges();
            }
        }

        public static void SetVariableInterestRateEnabled(bool value)
        {
            var row = Driver.Db.Ops.ServiceConfigurations.Single(v => v.Key == VariableInterestRateEnabled);
            row.Value = value.ToString();
            Driver.Db.Ops.SubmitChanges();
            TestLog.DebugTrace.WriteLine("SetVariableInterestRateEnabled -> {0}",
                                         Driver.Db.Ops.ServiceConfigurations.Single(
                                             v => v.Key == VariableInterestRateEnabled.ToString()).
                                             Value);
        }

        public static void SetDelayBeforeApplicationClosed(int delayInMinutes)
        {
            var delayInMinutesString = delayInMinutes.ToString();
            var row = Driver.Db.Ops.ServiceConfigurations.Single(v => v.Key == DelayBeforeApplicationClosedInMinutes);
            row.Value = delayInMinutesString;
            Driver.Db.Ops.SubmitChanges();
        }

        public static void SetVariableInterestRates(decimal numberOfPoints)
        {
            for (var i = 1; i < Driver.Db.Payments.VariableInterestRateDetails.Count(v => v.VariableInterestRateId == 1); i++)
            {
                var row = Driver.Db.Payments.VariableInterestRateDetails.Single(v => v.VariableInterestRateId == 1 && v.VariableInterestRateDetailId == i);
                row.MonthlyInterestRate = row.MonthlyInterestRate + numberOfPoints;
            }

            Driver.Db.Payments.SubmitChanges();
        }
    }
}
