using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Core.Helpers
{
    public static class RiskBasedPricingHeplers
    {
        public static void SwithcRiskBasedPricing(bool isEnabled)
        {
            Drive.Data.Ops.Db.ServiceConfigurations.UpdateByKey(Key: "Payments.Wb.RiskBasedPricingEnabled", Value: isEnabled);  
        }

        public static bool GetIsRisBasedPricingEnabled()
        {
            var result = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.Wb.RiskBasedPricingEnabled").Value;
            return Boolean.Parse(result);
        }
    }
}
