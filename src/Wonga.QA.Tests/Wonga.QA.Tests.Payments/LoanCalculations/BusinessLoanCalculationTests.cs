using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Payments.LoanCalculations
{
    [TestFixture]
    [Parallelizable(TestScope.All), Category(TestCategories.CoreTest)]
    public class BusinessLoanCalculationTests
    {
        [Test, AUT(AUT.Wb)]
        [Row(9850/*loanAmount*/, 3/*Term*/, 1.75 /*interestRate*/, 517.12 /*totalInterest*/, 492.50 /*arrangementFee*/ , 3619.88/*weeklyRepaymentAmount*/, 10859.62 /*totalRepay*/, 3619.86/*finalRepaymentAmount*/)]
        [Row(5000, 10, 1.75, 875, 250, 612.5, 6125, 0)]
        [Row(3150, 11, 1.75, 606.38, 157.5, 355.81, 3913.88, 0)]
        [Row(3300, 13, 1.75, 750.75, 165, 324.29, 4215.75, 0)]
        [Row(3050, 51, 1.75, 2722.12, 152.50, 116.17, 5924.62, 0)]
        [Row(3000, 52, 1.75, 2730, 150, 113.08, 5880.00, 0)]
        [Row(7750, 27, 1.75, 3661.88, 387.5, 437.02, 11799, 0)]
        [Row(5550, 22, 1.75, 2136.75, 277.5, 362.02, 7964, 0)]
        [Row(3000, 1, 1.75, 52.50, 150, 3202.5, 3202.5, 0)]
        [Row(9950, 5, 1.75, 870.62, 497.5, 2263.63, 11318.12, 0)]
        public void BusinessLoanCalculations_PriceTier5(decimal loanAmount, int term, decimal interestRate, decimal totalInterest, decimal arrangementFee, decimal weeklyRepaymentAmount, decimal totalRepay, decimal finalRepaymentAmount)
        {
            const String tier = "5";

            /*************************************************************************************************************************************************
             * This test will check the calculations on the payments side of things or PANNI`s Nightmare                                                     *
             * 1 -Check the price tier from -> [RiskApplications]                                                                                            *
             * 2 -Check the LoanAmount / Interest Rate / Application Fee / Term -> from [BusinessFixedInstallmentLoanApplications]                           * 
             * 3 -Check the ArragementFee / Total Interest -> from [Payments].[payment].[Transactions] - the initial 3 transactions                          *
             * 4 -Check the WeeklyRepaymentAmount / RemainingNumberOfPayments / TotalOutstandingAmount (total?) using GetBusinessAccountSummaryWbUkQuery     *
             * ***********************************************************************************************************************************************/
            var riskDb = Drive.Data.Risk.Db;
            var paymentsDb = Drive.Data.Payments.Db;

            var mainApplicant = CustomerBuilder.New().WithSpecificAge(37).WithNumberOfDependants(0).WithGender(GenderEnum.Female).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).WithOrganisationNumber("00000086").Build();
            var application = (ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder).WithLoanAmount(loanAmount).WithLoanTerm(term).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            /******************** RISK **********************************/
            var riskApplication = riskDb.RiskApplications.FindBy(ApplicationId : application.Id);
            Assert.IsNotNull(riskApplication,"Risk.RiskApplication should exist");
            Assert.AreEqual(tier, riskApplication.PriceTier.ToString(), "The price tier should be " + tier);

            /******************** Payments.Application **********************************/
            var paymentsApplication = paymentsDb.Applications.FindBy(ExternalID : application.Id);
            Assert.IsNotNull(paymentsApplication, "Payments.Application should exist");

            /******************** Payments.BusinessFixedInstallmentLoanApplications **********************************/
            var businessFixedTermLoan =
                paymentsDb.BusinessFixedInstallmentLoanApplications.FindBy(
                    ApplicationId: paymentsApplication.ApplicationId);
            Assert.IsNotNull(businessFixedTermLoan, "Payments.BusinessFixedInstallmentLoanApplications should exist");

            Assert.AreEqual(loanAmount, decimal.Parse(businessFixedTermLoan.LoanAmount.ToString()), "The loan ammount should be the same");
            Assert.AreEqual(interestRate, decimal.Parse(businessFixedTermLoan.InterestRate.ToString()), "The interest rate should be the same");
            Assert.AreEqual(term,businessFixedTermLoan.Term,"The term should be the same");

            /******************** Payments.Applications.Transactions **********************************/

            var initialPaymentsTransactions = paymentsDb.Transactions.FindAllBy(ApplicationId : paymentsApplication.ApplicationId);
            Assert.AreEqual(3, initialPaymentsTransactions.Count(), "There should be 3 initial transactions");

            var cashAdvanceTransaction = initialPaymentsTransactions.FindBy(Type: "CashAdvance");
            var feeTransaction = initialPaymentsTransactions.FindBy(Type: "Fee");
            var interestTransacation = initialPaymentsTransactions.FindBy(Type: "Interest");

            Assert.IsNotNull(cashAdvanceTransaction, "Cash advance transaction should be there");
            Assert.AreEqual(loanAmount, decimal.Parse(cashAdvanceTransaction.Amount.ToString()), "The loan amount should be the same");

            Assert.IsNotNull(feeTransaction, "Fee transaction should be there");
            Assert.AreEqual(arrangementFee, decimal.Parse(feeTransaction.Amount.ToString()), "The fee should be the same");

            Assert.IsNotNull(interestTransacation, "Interest transaction should be there");
            Assert.AreEqual(totalInterest, decimal.Parse(interestTransacation.Amount.ToString()), "The interest amount should be the same");

            /******************** GetBusinessAccountSummaryWbUkQuery **********************************/

            var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
            {
                AccountId = paymentsApplication.AccountId
            });

            Assert.AreEqual(weeklyRepaymentAmount, decimal.Parse(response.Values["WeeklyRepaymentAmount"].SingleOrDefault()), "The weekly amount should be equal");
            Assert.AreEqual(term.ToString(), response.Values["RemainingNumberOfPayments"].SingleOrDefault(),"The number of repayments should be equal");
            Assert.AreEqual((loanAmount + totalInterest + arrangementFee),  decimal.Parse(response.Values["TotalOutstandingAmount"].SingleOrDefault()), "The total amount should be equal");
        }
    }
}
