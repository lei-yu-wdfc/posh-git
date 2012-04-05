using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Payments.LoanCalculations
{
    [TestFixture]
    public class BusinessLoanCalculationTests
    {
        [Test, AUT(AUT.Wb)]
        [Ignore("WIP")]
        [Row(9850, 3, 517.12, 492.50, 3619.88, 3619.86, 10859.62)]
        public void BusinessLoanCalculations_PriceTier5(decimal loanAmount, int term, decimal expectedInterest, decimal expectedFee, decimal weeklyRepaymentAmount, decimal finalRepaymentAmount, decimal totalRepay)
        {
            const decimal interestRate = 1.5m;
            const String tier = "4";

            /*************************************************************************************************************************************************
             * This test will check the calculations on the payments side of things                                                                          *
             * 1 -Check the price tier from -> [RiskApplications]                                                                                            *
             * 2 -Check the LoanAmount / Interest Rate / Application Fee / Term -> from [BusinessFixedInstallmentLoanApplications]                           * 
             * 3 -Check the ArragementFee / Total Interest -> from [Payments].[payment].[Transactions] - the initial 3 transactions                          *
             * 4 -Check the WeeklyRepaymentAmount / RemainingNumberOfPayments / TotalOutstandingAmount (total?) using GetBusinessAccountSummaryWbUkQuery     *
             * ***********************************************************************************************************************************************/
            var riskDb = Drive.Data.Risk.Db;
            var paymentsDb = Drive.Data.Payments.Db;

            var mainApplicant = CustomerBuilder.New().WithMiddleName(RiskMask.TESTBusinessBureauDataIsAvailable).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();
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

            Assert.AreEqual(loanAmount,businessFixedTermLoan.LoanAmount,"The loan ammount should be the same");
            Assert.AreEqual(interestRate,businessFixedTermLoan.InterestRate,"The interest rate should be the same");
            Assert.AreEqual(term,businessFixedTermLoan.Term,"The term should be the same");

            /******************** Payments.Applications.Transactions **********************************/

            var initialPaymentsTransactions = paymentsDb.Transactions.FindAllBy(ApplicationId : paymentsApplication.ApplicationId);
            Assert.AreEqual(3, initialPaymentsTransactions.Count(), "There should be 3 initial transactions");

            var cashAdvanceTransaction = initialPaymentsTransactions.FindBy(Type: "CashAdvance");
            var feeTransaction = initialPaymentsTransactions.FindBy(Type: "Fee");
            var interestTransacation = initialPaymentsTransactions.FindBy(Type: "Interest");

            Assert.IsNotNull(cashAdvanceTransaction, "Cash advance transaction should be there");
            Assert.AreEqual(cashAdvanceTransaction.Amount, loanAmount, "The loan amount should be the same");

            Assert.IsNotNull(feeTransaction, "Fee transaction should be there");
            Assert.AreEqual(feeTransaction.Amount, expectedFee, "The fee should be the same");

            Assert.IsNotNull(interestTransacation, "Interest transaction should be there");
            Assert.AreEqual(interestTransacation.Amount, expectedInterest, "The interest amount should be the same");

            /******************** GetBusinessAccountSummaryWbUkQuery **********************************/

            var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
            {
                AccountId = paymentsApplication.AccountId
            });

            Assert.AreEqual(weeklyRepaymentAmount.ToString(), response.Values["WeeklyRepaymentAmount"].SingleOrDefault());
        }
    }
}
