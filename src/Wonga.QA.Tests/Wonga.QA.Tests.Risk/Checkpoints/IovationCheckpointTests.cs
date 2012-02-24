using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    public class IovationCheckpointTests
    {
        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("CA-1735")]
        public void IovationBlackBoxDecline()
        {
            Customer cust = CustomerBuilder.New()
                .WithEmployer("test:DeviceNotOnBlacklist").Build();

            Application app = ApplicationBuilder.New(cust).WithIovationBlackBox("Deny")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(RiskApiCheckpointTests.SingleCheckPointVerification(app, CheckpointStatus.Failed, CheckpointDefinitionEnum.Applicationdatablacklistcheck));
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("CA-1735")]
        public void IovationBlackBoxAllow()
        {
            Customer cust = CustomerBuilder.New()
                .WithEmployer("test:DeviceNotOnBlacklist").Build();

            Application app = ApplicationBuilder.New(cust).WithIovationBlackBox("Allow")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Assert.IsTrue(RiskApiCheckpointTests.SingleCheckPointVerification(app, CheckpointStatus.Verified, CheckpointDefinitionEnum.Applicationdatablacklistcheck));
        }

    }
}