using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	class CheckpointMonthlyIncomeEnoughForRepaymentTests
	{
		private const string TestMask = "test:MonthlyIncomeEnoughForRepayment";

		[Test]
		public void CheckpointMonthlyIncomeEnoughForRepaymentAccept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			ApplicationBuilder.New(customer)
				.WithLoanAmount(GetLoanThresholdForCustomer(customer) - 1)
				.Build();
		}

		[Test]
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
