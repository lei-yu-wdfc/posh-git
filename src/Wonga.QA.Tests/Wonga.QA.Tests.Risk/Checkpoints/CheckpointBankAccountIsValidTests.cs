using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture, Parallelizable(TestScope.All)]
	class CheckpointBankAccountIsValidTests
	{
        private const RiskMask TestMask = RiskMask.TESTBankAccountIsValid;

		[Test, AUT(AUT.Za), JIRA("ZA-1910")]
		public void CheckpointShouldReturnReadyToSignStatus()
		{
			var bankAccountNumber = Get.GetBankAccountNumber().ToString();

			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithBankAccountNumber(bankAccountNumber).Build();
            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.ReadyToSign).Build();
		}
	}
}
