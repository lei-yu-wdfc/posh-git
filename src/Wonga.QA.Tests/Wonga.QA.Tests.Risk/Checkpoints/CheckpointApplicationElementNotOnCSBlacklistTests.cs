using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Risk;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Enums;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CheckpointApplicationElementNotOnCSBlacklistTests
	{
		private const string TestMask = "test:ApplicationElementNotOnCSBlacklist";

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnCSBlacklistAccept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			var riskAccount = Do.Until(() => Driver.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id));
			Assert.IsFalse(riskAccount.DoNotRelend);

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnCSBlacklistDecline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			Driver.Msmq.Risk.Send(new RegisterDoNotRelendCommand{AccountId =  customer.Id, DoNotRelend =  true});
			Do.Until(() => Driver.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id).DoNotRelend);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}
	}
}
