using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CheckpointApplicationElementNotOnCSBlacklistTests
	{
        private const RiskMask TestMask = RiskMask.TESTApplicationElementNotOnCSBlacklist;

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnCSBlacklistAccept()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			var riskAccount = Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id));
			Assert.IsFalse(riskAccount.DoNotRelend);

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnCSBlacklistDecline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			Drive.Msmq.Risk.Send(new RegisterDoNotRelendCommand{AccountId =  customer.Id, DoNotRelend =  true});
			Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id).DoNotRelend);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}
	}
}
