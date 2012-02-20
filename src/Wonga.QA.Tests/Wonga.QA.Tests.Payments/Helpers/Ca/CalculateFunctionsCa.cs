using System;

namespace Wonga.QA.Tests.Payments.Helpers.Ca
{
    public static class CalculateFunctionsCa
    {
        public static decimal CalculateDailyInterestRateCa(int day, bool loanInArrears = false)
        {
            return (GetPaymentFunctions.GetMonthlyInterestRateForGivenDay(day, loanInArrears)*12)/36500;
        }

        private static decimal CalculateTotalDailyInterestRateCa(int loanTerm, int startDay = 1, bool loanInArrears = false)
        {
            decimal totalDailyInterestRate = 0;

            for (var i = startDay; i < loanTerm + 1; i++)
            {
                totalDailyInterestRate = totalDailyInterestRate + CalculateDailyInterestRateCa(i, loanInArrears);
            }

            return totalDailyInterestRate;
        }

        public static decimal CalculateExpectedVariableInterestAmountAppliedCa(decimal loanAmount, int loanTerm)
        {
            return Decimal.Round(loanAmount*CalculateTotalDailyInterestRateCa(loanTerm), 2,
                                 MidpointRounding.AwayFromZero);
        }

        public static decimal CalculateExpectedEarlyRepaymentVariableInterestAmountAppliedCa(decimal loanAmount, int loanTerm, int earlyRepaymentTerm, int earlyRepaymentAmount)
        {
            var dailyInterestRateOne = CalculateTotalDailyInterestRateCa(earlyRepaymentTerm);
            var dailyInterestRateTwo = CalculateTotalDailyInterestRateCa(loanTerm, earlyRepaymentTerm + 1);

            var interestAmountOne = dailyInterestRateOne * loanAmount;
            var interestAmountTwo = dailyInterestRateTwo * (loanAmount - earlyRepaymentAmount);

            return Decimal.Round(interestAmountOne + interestAmountTwo, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculateExpectedArrearsInterestAmountAppliedCa(decimal loanAmount, int loanTerm)
        {

            return Decimal.Round(loanAmount*CalculateTotalDailyInterestRateCa(loanTerm, 1, true), 2,
                                 MidpointRounding.AwayFromZero);

        }
    }
}
