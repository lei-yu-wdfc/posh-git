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
	class CheckpointBankAccountIsValidTests
	{
        private const RiskMask TestMask = RiskMask.TESTBankAccountIsValid;

		[Test, AUT(AUT.Za), JIRA("ZA-1910")]
		public void CheckpointBankAccountIsValidShouldReturnReadyToSignStatus()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.ReadyToSign).Build();
		}
	}
}
