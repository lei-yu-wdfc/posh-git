using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class GetPaymentFunctions
    {
        private const string VariableInterestRateEnabled = "Payments.VariableInterestRateEnabled";

        public static decimal GetMonthlyInterestRateForGivenDay(int day, bool loanInArrears = false)
        {
            if (loanInArrears)
            {
                return
                    Drive.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 2).
                        MonthlyInterestRate;
            }

            if (day > 30 || day < 1)
            {
                return -1;
            }

            if (!GetVariableInterestRateEnabledValue())
            {
                return
                    Drive.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 1).
                        MonthlyInterestRate;
            }

            return day < 11
                       ? Drive.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 1).
                             MonthlyInterestRate
                       : Drive.Db.Payments.VariableInterestRateDetails.Single(v => v.Day == day).MonthlyInterestRate;
        }

        public static bool GetVariableInterestRateEnabledValue()
        {
            return
                Convert.ToBoolean(
                    Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == VariableInterestRateEnabled).Value);
        }

        public static List<VariableInterestRateDetailEntity> GetVariableRatesFromApiResponse(ApiResponse response)
        {
            var actualVariableRates = new List<VariableInterestRateDetailEntity>();

            List<string> day = response.Values["Day"].ToList();
            List<string> rate = response.Values["Rate"].ToList();

            for (int counter = 0; counter < day.Count; counter++)
            {
                actualVariableRates.Add(CreatePaymentFunctions.CreateRowOfTypeVariableInterestRateDetail());
                actualVariableRates.Last().Day = Convert.ToByte(day[counter]);
                actualVariableRates.Last().MonthlyInterestRate = Convert.ToDecimal(rate[counter]);
            }

            return actualVariableRates;
        }

        public static List<VariableInterestRateDetailEntity> GetCurrentVariableInterestRates()
        {
            return Drive.Db.Payments.VariableInterestRateDetails.Where(v => v.VariableInterestRateId == 1).ToList();
        }

        public static decimal GetCurrentArrearsInterestRate()
        {
            return Drive.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 2).MonthlyInterestRate;
        }

        public static List<TransactionEntity> GetInterestRatesForApplication(Guid applicationGuid)
        {
            var applicationId = GetApplicationId(applicationGuid);
            return
                Drive.Db.Payments.Transactions.Where(
                    a => a.ApplicationId == applicationId && a.Type == PaymentTransactionType.InterestRate.ToString()).ToList();
        }

        public static int GetApplicationId(Guid applicationGuid)
        {
            return Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ApplicationId);
        }

        public static decimal GetInterestAmountApplied(Guid applicationGuid)
        {
            var applicationid = GetApplicationId(applicationGuid);
            return
                Do.Until(() => Drive.Db.Payments.Transactions.Single(
                    a => a.ApplicationId == applicationid && a.Type == PaymentTransactionType.Interest.ToString()).Amount);
        }

        public static decimal GetArrearsInterestAmountApplied(Guid applicationGuid)
        {
            var applicationid = GetApplicationId(applicationGuid);

            var allInterestAmountsPosted = Drive.Db.Payments.Transactions.Where(
                a => a.ApplicationId == applicationid && a.Type == PaymentTransactionType.Interest.ToString()).ToList();

            return allInterestAmountsPosted.Last().Amount;
        }

        public static ApiResponse GetFixedTermLoanOfferCaQuery()
        {
            return Drive.Api.Queries.Post(new GetFixedTermLoanOfferCaQuery());
        } 

        public static List<TransactionEntity> GetCurrentVariableInterestRates(int loanTerm)
        {
            var expectedRates = new List<TransactionEntity>();
            var counter = 1;
            var loanCreatedDate = DateTime.Today;
            int numberOfDaysUntilStartOfLoan = DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
            var loanStartDate = DateTime.Today.AddDays(numberOfDaysUntilStartOfLoan);

            expectedRates.Add(CreatePaymentFunctions.CreateRowOfTypeTransaction());
            expectedRates.Last().PostedOn = loanCreatedDate;
            expectedRates.Last().Mir = GetMonthlyInterestRateForGivenDay(counter);

            if (!GetVariableInterestRateEnabledValue()) return expectedRates;

            counter = counter + 10;

            while (counter <= loanTerm)
            {
                expectedRates.Add(CreatePaymentFunctions.CreateRowOfTypeTransaction());
                expectedRates.Last().PostedOn = loanStartDate.AddDays(counter - 1);
                expectedRates.Last().Mir = GetMonthlyInterestRateForGivenDay(counter);
                counter++;
            }

            return expectedRates;
        }

        public static decimal GetActualDefaultChargeAmount(Guid applicationGuid)
        {
            var applicationid = GetApplicationId(applicationGuid);
            return
                Do.Until(() => Drive.Db.Payments.Transactions.Single(
                    a => a.ApplicationId == applicationid && a.Type == PaymentTransactionType.DefaultCharge.ToString()).Amount);
        }
    }
}
