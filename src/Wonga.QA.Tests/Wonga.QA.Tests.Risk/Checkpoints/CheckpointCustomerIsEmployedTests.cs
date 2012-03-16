using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	public class CheckpointCustomerIsEmployedTests
	{
        private const RiskMask TestMask = RiskMask.TESTCustomerIsEmployed;

		[Test]
		public void CustomerIsEmployedAccept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithEmployerStatus(EmploymentStatusEnum.EmployedFullTime.ToString()).Build();
			ApplicationBuilder.New(customer).Build();
		}

		[Test]
		public void CustomerIsEmployedDecline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).Build();
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}
	}
}
