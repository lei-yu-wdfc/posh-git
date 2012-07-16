﻿using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Risk.UI;
﻿﻿using Wonga.QA.Tests.Core;
using System;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za, AUT.Ca)]
	class CheckpointApplicationElementNotOnCSBlacklistTests
	{
		private const RiskMask TestMask = RiskMask.TESTApplicationElementNotOnCSBlacklist;

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("CA-1974")]
		public void L0_CheckpointApplicationElementNotOnCSBlacklist_Accept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			var riskAccount = Do.With.Timeout(1).Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id));
			Assert.IsFalse(riskAccount.DoNotRelend);

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("CA-1974")]
		public void L0_CheckpointApplicationElementNotOnCSBlacklist_Decline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			Drive.Msmq.Risk.Send(new RegisterDoNotRelendMessage { AccountId = customer.Id, DoNotRelend = true });
			Do.Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id).DoNotRelend);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void LN_CheckpointApplicationElementNotOnCSBlacklist_Accept()
		{
			var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();

			var l0Application = ApplicationBuilder.New(customer).Build();
			l0Application.RepayOnDueDate();
			CustomerOperations.UpdateEmployerNameInRisk(customer.Id, TestMask.ToString());

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void LN_CheckpointApplicationElementNotOnCSBlacklist_Decline()
		{
			var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();
			var l0Application = ApplicationBuilder.New(customer).Build();
			l0Application.RepayOnDueDate();
			CustomerOperations.UpdateEmployerNameInRisk(customer.Id, TestMask.ToString());

			Drive.Msmq.Risk.Send(new RegisterDoNotRelendMessage { AccountId = customer.Id, DoNotRelend = true });
			Do.Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id).DoNotRelend);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}
	}
}