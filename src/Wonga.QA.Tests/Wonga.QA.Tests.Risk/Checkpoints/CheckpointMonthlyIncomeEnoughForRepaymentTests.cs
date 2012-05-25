using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	class CheckpointMonthlyIncomeEnoughForRepaymentTests
	{
        private const RiskMask TestMask = RiskMask.TESTMonthlyIncomeEnoughForRepayment;
		private static readonly decimal NetMonthlyIncome = GetDefaultCreditLimit()*4;

		[Test, AUT(AUT.Uk), JIRA("SME-866","UK-866"), Description("Scenario 1: Accepted")]
		public void CheckpointMonthlyIncomeEnoughForRepaymentAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithNetMonthlyIncome(NetMonthlyIncome)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(GetLoanThresholdForCustomer(customer) - 1)
				.Build();
		}

        [Test, AUT(AUT.Uk), JIRA("SME-866", "UK-866"), Description("Scenario 1: Declined")]
		public void CheckpointMonthlyIncomeEnoughForRepaymentDecline()
		{
			var customer = CustomerBuilder.New()
                .WithEmployer(TestMask)
				.WithNetMonthlyIncome(NetMonthlyIncome)
                .Build();

			ApplicationBuilder.New(customer)
				.WithLoanAmount(GetLoanThresholdForCustomer(customer) + 1)
                .WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();
		}

		#region Helpers

		private decimal GetLoanThresholdForCustomer(Customer customer)
		{
			var allowedIncomeLimit = GetAllowedIncomeLimitPercent();
			var netMonthlyIncome = Drive.Db.Risk.EmploymentDetails.Single(a => a.AccountId == customer.Id).NetMonthlyIncome;
			return ((allowedIncomeLimit / 100.0m) * netMonthlyIncome);
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
