using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointSuspiciousActivity
	{
		[Test, AUT(AUT.Uk), JIRA("UK-436"), Description("Scenario 1: Declined"), Owner(Owner.RiskTeam)]
		public void LnSuspiciousActivitySumOfLoanAmmountIsGreaterThatCreditLimitApplicationDeclined()
		{
            var customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).WithEmployer(Get.RandomString(3)).Build();

			var l0Application = ApplicationBuilder
				.New(customer)
				.WithLoanAmount(200)
				.Build();

			l0Application.RepayOnDueDate();

			CustomerOperations.UpdateMiddleNameInRisk(customer.Id, RiskMask.TESTNoSuspiciousApplicationActivity.ToString());
			CustomerOperations.UpdateCreditLimitInRisk(customer.Id, 200);

			ApplicationBuilder.New(customer)
				.WithLoanAmount(300)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();
		}

		[Test, AUT(AUT.Uk), JIRA("UK-436"), Description("Scenario 2: Accepted"), Owner(Owner.RiskTeam)]
		public void LnSuspiciousActivitySumOfLoanAmmountIsSmallerThatCreditLimitApplicationAccepted()
		{
            var customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).WithEmployer(Get.RandomString(3)).Build();

			var l0Application = ApplicationBuilder
				.New(customer)
				.WithLoanAmount(200)
				.Build();

			l0Application.RepayOnDueDate();

			CustomerOperations.UpdateMiddleNameInRisk(customer.Id, RiskMask.TESTNoSuspiciousApplicationActivity.ToString());
			CustomerOperations.UpdateCreditLimitInRisk(customer.Id, 200);

			ApplicationBuilder.New(customer)
				.WithLoanAmount(200)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.Build();
		}
	}
}
