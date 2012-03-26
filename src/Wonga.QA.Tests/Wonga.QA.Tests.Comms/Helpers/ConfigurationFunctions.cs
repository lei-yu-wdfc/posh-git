using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Comms.Helpers
{
	public static class ConfigurationFunctions
	{
		public const string BankGateWayIsTestMode = "BankGateway.IsTestMode";

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

		public static Boolean GetBankGatewayTestMode()
		{
			var db = new DbDriver();
			var bgw = db.Ops.ServiceConfigurations.Single(bg => bg.Key == BankGateWayIsTestMode);

			return bool.Parse(bgw.Value);
		}
	}
}
