using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CheckpointMonthlyIncomeEnoughForRepaymentTests
	{
        private const RiskMask TestMask = RiskMask.TESTMonthlyIncomeEnoughForRepayment;

		[Test, AUT(AUT.Za)]
		public void CheckpointMonthlyIncomeEnoughForRepaymentAccept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			ApplicationBuilder.New(customer)
				.WithLoanAmount(GetLoanThresholdForCustomer(customer) - 1)
				.Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointMonthlyIncomeEnoughForRepaymentDecline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			ApplicationBuilder.New(customer)
				.WithLoanAmount(GetLoanThresholdForCustomer(customer) + 1)
				.WithExpectedDecision(ApplicationDecisionStatusEnum.Declined)
				.Build();
		}

		#region Helpers

		private decimal GetLoanThresholdForCustomer(Customer customer)
		{
			var allowedIncomeLimit = GetAllowedIncomeLimitPercent();
			var netMonthlyIncome = Driver.Db.Risk.EmploymentDetails.Single(a => a.AccountId == customer.Id).NetMonthlyIncome;
			return ((allowedIncomeLimit / 100.0m) * netMonthlyIncome);
		}

		private decimal GetAllowedIncomeLimitPercent()
		{
			return Decimal.Parse(Driver.Db.GetServiceConfiguration("Risk.AllowedIncomeLimitPercent").Value);
		}

		#endregion
	}
}
