using System;
using System.Collections.Generic;
using System.Linq;
using Gallio.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Payments;

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

        public static void SetCurrentVariableInterestRates(List<VariableInterestRateDetailEntity> rates)
        {
            var db = Drive.Db.Payments;

            var currentRates = db.VariableInterestRateDetails.Where(v => v.VariableInterestRateId == 1);

            foreach (var currentRate in currentRates)
            {
                var rate = rates.Single(r => r.Day == currentRate.Day);

                if (rate != null)
                {
                    currentRate.MonthlyInterestRate = rate.MonthlyInterestRate;
                }
            }

            db.SubmitChanges();
        }
    }
}
