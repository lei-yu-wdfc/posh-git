﻿using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public static class GetPaymentFunctions
    {

        public static decimal GetMonthlyInterestRateForGivenDay(int day, bool loanInArrears = false)
        {
            if (loanInArrears)
            {
                return
                    Driver.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 2).
                        MonthlyInterestRate;
            }

            if (day > 30 || day < 1)
            {
                return -1;
            }

            if (!GetVariableInterestRateEnabledValue())
            {
                return
                    Driver.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 1).
                        MonthlyInterestRate;
            }

            return day < 11
                       ? Driver.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 1).
                             MonthlyInterestRate
                       : Driver.Db.Payments.VariableInterestRateDetails.Single(v => v.Day == day).MonthlyInterestRate;
        }

        public static bool GetVariableInterestRateEnabledValue()
        {
            return
                Convert.ToBoolean(
                    Driver.Db.Ops.ServiceConfigurations.Single(v => v.Key == "VariableInterestRateEnabled").Value);
        }

        public static List<VariableInterestRateDetailEntity> GetVariableRatesFromApiResponse(ApiResponse response)
        {
            var actualVariableRates = new List<VariableInterestRateDetailEntity>();

            //List<string> day = response.GetValues("Day").ToList();
            //List<string> rate = response.GetValues("Rate").ToList();

            //for (int counter = 0; counter < day.Count; counter++)
            //{
            //    actualVariableRates.Add(CreatePaymentFunctions.CreateRowOfTypeVariableInterestRateDetail());
            //    actualVariableRates.Last().Day = Convert.ToByte(day[counter]);
            //    actualVariableRates.Last().MonthlyInterestRate = Convert.ToDecimal(rate[counter]);
            //}

            return actualVariableRates;
        }

        public static List<VariableInterestRateDetailEntity> GetCurrentVariableInterestRates()
        {
            return Driver.Db.Payments.VariableInterestRateDetails.Where(v => v.VariableInterestRateId == 1).ToList();
        }

        public static decimal GetCurrentArrearsInterestRate()
        {
            return Driver.Db.Payments.ProductInterestRates.Single(v => v.ProductInterestRateId == 2).MonthlyInterestRate;
        }

        public static List<TransactionEntity> GetInterestRatesForApplication(Guid applicationGuid)
        {
            var applicationId = GetApplicationId(applicationGuid);
            return
                Driver.Db.Payments.Transactions.Where(
                    a => a.ApplicationId == applicationId && a.Type == PaymentTransactionType.Interest.ToString()).ToList();
        }

        public static int GetApplicationId(Guid applicationGuid)
        {
            return Driver.Db.Payments.Applications.Single(a => a.ExternalId == applicationGuid).ApplicationId;
        }

        public static decimal GetInterestAmountApplied(Guid applicationGuid)
        {
            var applicationid = GetApplicationId(applicationGuid);
            return
                Driver.Db.Payments.Transactions.Single(
                    a => a.ApplicationId == applicationid && a.Type == PaymentTransactionType.Interest.ToString()).Amount;
        }

        public static decimal GetArrearsInterestAmountApplied(Guid applicationGuid)
        {
            var applicationid = GetApplicationId(applicationGuid);

            var allInterestAmountsPosted = Driver.Db.Payments.Transactions.Where(
                a => a.ApplicationId == applicationid && a.Type == PaymentTransactionType.Interest.ToString()).ToList();

            return allInterestAmountsPosted.Last().Amount;
        }

        public static ApiResponse GetFixedTermLoanOfferQuery()
        {
            return Driver.Api.Queries.Post(new GetFixedTermLoanApplicationQuery());
        }

        private static bool GetIsBankGatewayTestMode()
        {
            return Convert.ToBoolean(Driver.Db.Ops.ServiceConfigurations.Single(bg => bg.Key == SetPaymentFunctions.BankGateWayIsTestMode).Value);
        }

        public static List<TransactionEntity> GetCurrentVariableInterestRates(int loanTerm)
        {
            var expectedRates = new List<TransactionEntity>();
            var counter = 1;
            var startDate = DateTime.Today;

            expectedRates.Add(CreatePaymentFunctions.CreateRowOfTypeTransaction());
            expectedRates.Last().PostedOn = startDate;
            expectedRates.Last().Mir = GetMonthlyInterestRateForGivenDay(counter);

            if (!GetVariableInterestRateEnabledValue()) return expectedRates;

            counter = counter + 10;

            while (counter <= loanTerm)
            {
                expectedRates.Add(CreatePaymentFunctions.CreateRowOfTypeTransaction());
                //expectedRates.Last().PostedOn = GetNextWorkingDate(DateTime.Today).AddDays(counter - 1);
                expectedRates.Last().Mir = GetMonthlyInterestRateForGivenDay(counter);
                counter++;
            }

            return expectedRates;
        }

        public static decimal GetActualDefaultChargeAmount(Guid applicationGuid)
        {
            var applicationid = GetApplicationId(applicationGuid);
            return
                Driver.Db.Payments.Transactions.Single(
                    a => a.ApplicationId == applicationid && a.Type == PaymentTransactionType.DefaultCharge.ToString()).
                    Amount;
        }
    }
}
