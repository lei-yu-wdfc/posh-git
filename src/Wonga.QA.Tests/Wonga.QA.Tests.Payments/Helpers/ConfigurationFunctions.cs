using System;
using System.Linq;
using Gallio.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;

namespace Wonga.QA.Tests.Payments.Helpers
{
	public static class ConfigurationFunctions
	{
		public const string FixedTermLoanOfferHandler = "Payments.FixedTermLoanOfferHandler.DateTime.UtcNow";
        private const string DelayBeforeApplicationClosedInMinutes = "Payments.DelayBeforeApplicationClosedInMinutes";
        private const string VariableInterestRateEnabled = "Payments.VariableInterestRateEnabled";
        public const string BankGateWayIsTestMode = "BankGateway.IsTestMode";

		public static void SetupQaUtcNowOverride(DateTime now)
		{
			var driver = new DbDriver();
			var scEntry = driver.Ops.ServiceConfigurations.SingleOrDefault(x => x.Key == FixedTermLoanOfferHandler);
			if (scEntry == null)
			{
				driver.Ops.ServiceConfigurations.InsertOnSubmit(new ServiceConfigurationEntity()
				{
					Key = FixedTermLoanOfferHandler,
					Value = now.Date.ToString("yyyy-MM-dd")
				});
			}
			else
			{
				scEntry.Value = now.Date.ToString("yyyy-MM-dd");
			}

			driver.Ops.SubmitChanges();
		}

		public static void ResetQaUtcNowOverride()
		{
			var driver = new DbDriver();
			var scEntry = driver.Ops.ServiceConfigurations.SingleOrDefault(x => x.Key == FixedTermLoanOfferHandler);
			if (scEntry != null)
			{
				driver.Ops.ServiceConfigurations.DeleteOnSubmit(scEntry);
				driver.Ops.SubmitChanges();
			}

		}

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

        // Move to configurations functions
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

        // Move to configurations functions
        public static int GetDelayBeforeApplicationClosed()
        {
            return int.Parse(Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == DelayBeforeApplicationClosedInMinutes).Value);
        }


        public static void SetDelayBeforeApplicationClosed(int delayInMinutes)
        {
            var db = new DbDriver();
            var delayInMinutesString = delayInMinutes.ToString();
            var row = db.Ops.ServiceConfigurations.Single(v => v.Key == DelayBeforeApplicationClosedInMinutes);
            row.Value = delayInMinutesString;
            db.Ops.SubmitChanges();
        }

	}
}