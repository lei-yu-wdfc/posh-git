using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    public class CheckpointApplicationDeviceNotOnBlacklistTests
    {
    	private const string TestMask = "test:ApplicationDeviceNotOnBlacklist";

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("CA-1735")]
		public void CheckpointApplicationDeviceNotOnBlacklistDecline()
        {
            Customer cust = CustomerBuilder.New()
				.WithEmployer(TestMask).Build();

            Application app = ApplicationBuilder.New(cust).WithIovationBlackBox("Deny")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(RiskApiCheckpointTests.SingleCheckPointVerification(app, CheckpointStatus.Failed, CheckpointDefinitionEnum.Applicationdatablacklistcheck));
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("CA-1735")]
		public void CheckpointApplicationDeviceNotOnBlacklistAllow()
        {
            Customer cust = CustomerBuilder.New()
				.WithEmployer(TestMask).Build();

            Application app = ApplicationBuilder.New(cust).WithIovationBlackBox("Allow")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Assert.IsTrue(RiskApiCheckpointTests.SingleCheckPointVerification(app, CheckpointStatus.Verified, CheckpointDefinitionEnum.Applicationdatablacklistcheck));
        }

    }
}