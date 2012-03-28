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
				[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1046")]
		public void CheckpointApplicationFailsIovation_LoanIsDeclined()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();
            
            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application =ApplicationBuilder.New(customer,organisation).WithIovationBlackBox("Deny").WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox("Deny").WithExpectedDecision(
                        ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id,RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

				[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1046")]
		public void CheckpointApplicationPassesIovation_LoanIsAccepted()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox("Allow").WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox("Allow").WithExpectedDecision(
                        ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id,RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130")]
        public void CheckpointApplicationGetsIovationReview_LoanIsAccepted()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox("Review").WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox("Review").WithExpectedDecision(
                        ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130")]
        public void CheckpointApplicationGetsIovationUnknown_LoanIsAccepted()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox("Unknown").WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox("Unknown").WithExpectedDecision(
                        ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }
    }
}