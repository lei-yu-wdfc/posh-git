using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
    public class CheckpointApplicationDeviceNotOnBlacklistTests
    {
        private readonly string _testMask = RiskMask.TESTDeviceNotOnBlacklist.ToString();

        [Test, AUT(AUT.Ca, AUT.Uk), JIRA("CA-1735")]
		public void CheckpointApplicationDeviceNotOnBlacklistDecline()
        {
            var customer = CustomerBuilder.New().WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithIovationBlackBox("Deny").WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Ca, AUT.Uk), JIRA("CA-1735")]
		public void CheckpointApplicationDeviceNotOnBlacklistAccept()
        {
            Customer customer = CustomerBuilder.New().WithEmployer(_testMask).Build();
            Application application = ApplicationBuilder.New(customer).WithIovationBlackBox("Allow").WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

    }
}