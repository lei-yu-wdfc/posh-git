using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using ApplicationBuilder = Wonga.QA.Framework.Builders.ApplicationBuilder;

namespace Wonga.QA.Tests.PayLater.RiskDecision.Checkpoints
{
    [TestFixture]
    public class MonthlyIncomeEnoughForRepaymentTests
    {
        private const RiskMask TestMask = RiskMask.TESTMonthlyIncomeEnoughForRepayment;

        [Test, AUT(AUT.Uk), JIRA("PAYLATER-743"), Ignore("blocked - checkpoints not loading")]
        public void WhenACustomerWithAWongaConsumerLoanHasAnIncomeGreaterThanTheRequiredThenThisShouldResultInRiskAccepted()
        {
            var consumerAccount = AccountBuilder.Consumer.New().WithNetMonthlyIncome(GetDefaultCreditLimit()+1).Build();
            ApplicationBuilder.Consumer.New(consumerAccount).Build();
            ApplicationBuilder.PayLater.New(consumerAccount).Build();

            Assert.IsTrue(VerifyThresholdForAccount(consumerAccount)); 
        }

        [Test, AUT(AUT.Uk), JIRA("PAYLATER-743"), Ignore("blocked - checkpoints not loading")]
        public void WhenACustomerWithAWongaConsumerLoanHasAnIncomeLessThanTheRequiredThenThisShouldResultInRiskDeclined()
        {
            var consumerAccount = AccountBuilder.Consumer.New().WithNetMonthlyIncome(GetDefaultCreditLimit() - 1).Build();
            ApplicationBuilder.Consumer.New(consumerAccount).Build();
            ApplicationBuilder.PayLater.New(consumerAccount).Build();

            Assert.IsFalse(VerifyThresholdForAccount(consumerAccount)); 
        }

        [Test, AUT(AUT.Uk), JIRA("PAYLATER-743"), Ignore("blocked - checkpoints not loading")]
        public void WhenACustomerWithoutAWongaConsumerLoanHasAnIncomeGreaterThanTheRequiredThenThisShouldResultInRiskAccepted()
        {
            var payLaterAccount = AccountBuilder.PayLater.New().WithRiskTestMask(TestMask).Build();
            ApplicationBuilder.PayLater.New(payLaterAccount).WithoutSigning().Build();

            Assert.IsTrue(VerifyThresholdForAccount(payLaterAccount));
        }

        [Test, AUT(AUT.Uk), JIRA("PAYLATER-743"), Ignore("blocked - checkpoints not loading")]
        public void WhenACustomerWithoutAWongaConsumerLoanHasAnIncomeLessThanTheRequiredThenThisShouldResultInRiskDeclined()
        {
            var payLaterAccount = AccountBuilder.PayLater.New().Build();
            ApplicationBuilder.PayLater.New(payLaterAccount).Build();

            Assert.IsFalse(VerifyThresholdForAccount(payLaterAccount));
        }

        #region PrivateFunctions

        private bool VerifyThresholdForAccount(PayLaterAccount payLaterAccount)
        {
            decimal netMonthlyIncome = Drive.Db.Risk.EmploymentDetails.Single(a => a.AccountId == payLaterAccount.Id).NetMonthlyIncome;
            decimal transactionFee = Drive.Db.PayLater.Applications.Single(a => a.AccountId == payLaterAccount.Id).TransactionFee;
            decimal creditInUse = Drive.Db.PayLater.Applications.Single(a => a.AccountId == payLaterAccount.Id).TotalAmount;

            int applicationId = Drive.Db.PayLater.Applications.First(a => a.AccountId == payLaterAccount.Id).ApplicationId;
            decimal firstInstallmentAmount = Drive.Db.PayLater.Installments.Single(a => a.ApplicationId == applicationId).Amount;

            return CalculateThresholdForPayLaterAppOnly(netMonthlyIncome, transactionFee, firstInstallmentAmount, creditInUse);
        }

        private bool CalculateThresholdForPayLaterAppOnly(decimal netMonthlyIncome, decimal transactionFee, decimal firstInstallmentAmount, decimal creditInUse)
        {
            //X == TransactionFee + firstInstallmentForCurrentApp + PayLater (credit in use) / 3
            //X < X%

            decimal allowedIncomeLimitPercentage = GetAllowedIncomeLimitPercent();
            int divideByNumber = 3;

            return (((transactionFee + firstInstallmentAmount + creditInUse)/divideByNumber) <
                    (netMonthlyIncome*allowedIncomeLimitPercentage));
        }

        private bool VerifyThresholdForAccount(ConsumerAccount payLaterAccount)
        {
            decimal wongaLiveBalanceLoanPlusInsterestAndFees = GetwongaLiveBalanceLoanPlusInsterestAndFeesBalance();

            decimal netMonthlyIncome = Drive.Db.Risk.EmploymentDetails.Single(a => a.AccountId == payLaterAccount.Id).NetMonthlyIncome;
            decimal transactionFee = Drive.Db.PayLater.Applications.Single(a => a.AccountId == payLaterAccount.Id).TransactionFee;
            decimal creditInUse = Drive.Db.PayLater.Applications.Single(a => a.AccountId == payLaterAccount.Id).TotalAmount;

            int applicationId = Drive.Db.PayLater.Applications.First(a => a.AccountId == payLaterAccount.Id).ApplicationId;
            decimal firstInstallmentAmount = Drive.Db.PayLater.Installments.Single(a => a.ApplicationId == applicationId).Amount;

            return CalculateThresholdForPayLaterAppWithConsumerLoan(netMonthlyIncome, transactionFee, firstInstallmentAmount, 
                creditInUse, wongaLiveBalanceLoanPlusInsterestAndFees);
        }

        private decimal GetwongaLiveBalanceLoanPlusInsterestAndFeesBalance()
        {
            throw new NotImplementedException();
        }

        private bool CalculateThresholdForPayLaterAppWithConsumerLoan(decimal netMonthlyIncome, decimal transactionFee, decimal firstInstallmentAmount, 
            decimal creditInUse, decimal wongaLiveBalanceLoanPlusInsterestAndFees)
        {
            //X == TransactionFee + firstInstallmentForCurrentApp + WongaLiveLoanPlusInsterestAndFees + PayLater (credit in use) / 3
            //X < X%

            decimal allowedIncomeLimitPercentage = GetAllowedIncomeLimitPercent();
            int divideByNumber = 3;

            return (((transactionFee + firstInstallmentAmount + wongaLiveBalanceLoanPlusInsterestAndFees + creditInUse) / divideByNumber) <
                    (netMonthlyIncome * allowedIncomeLimitPercentage));
        }

        private static decimal GetAllowedIncomeLimitPercent()
        {
            return Decimal.Parse(Drive.Db.GetServiceConfiguration("Risk.AllowedIncomeLimitPercent").Value);
        }

        private static decimal GetDefaultCreditLimit()
        {
            return Decimal.Parse(Drive.Db.GetServiceConfiguration("Risk.DefaultCreditLimit").Value); 
        }

        #endregion
    }
}
