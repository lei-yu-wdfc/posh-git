using System;
using System.Linq;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;

namespace Wonga.QA.Tests.Payments.Helpers
{
	public static class ConfigurationFunctions
	{
		public const string FixedTermLoanOfferHandler = "Payments.FixedTermLoanOfferHandler.DateTime.UtcNow";


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

	}
}