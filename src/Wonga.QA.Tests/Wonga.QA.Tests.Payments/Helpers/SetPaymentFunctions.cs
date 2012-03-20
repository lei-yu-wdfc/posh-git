using System;
using System.Linq;
using Gallio.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class SetPaymentFunctions
    {
        public static void IncrementVariableInterestRatesMonthlyInterestRate(decimal increment)
        {
            var db = new DbDriver();
            
            foreach (var interestRate in db.Payments.VariableInterestRateDetails.Where(r => r.VariableInterestRateId == 1))
            {
                interestRate.MonthlyInterestRate = interestRate.MonthlyInterestRate + increment;
            }

            db.Payments.SubmitChanges();
        }
    }
}
