using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.Self)]
    public class CheckpointApplicationDeviceNotOnBlacklistTests
    {
        private const string IsManualVerificationEnabledKey = "Risk.IsManualVerificationEnabled";
        private const string IsIovationReviewAcceptedKey = "Risk.IsIovationReviewAccepted";

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1567")]
		public void L0_CheckpointApplicationFailsIovation_LoanIsDeclined()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();
            
            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(IovationMockResponse.Deny).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Deny).WithExpectedDecision(
                        ApplicationDecisionStatus.Declined).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id,RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1567")]
		public void L0_CheckpointApplicationPassesIovation_LoanIsAccepted()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(IovationMockResponse.Allow).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Allow).WithExpectedDecision(
                        ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id,RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1567")]
        public void L0_CheckpointApplicationGetsIovationReviewWithManualVerificationWithIovationReviewAccepted_LoanIsAccepted()
        {
            var isManualVerificationEnabled =
                    Drive.Data.Ops.GetServiceConfiguration<bool>(IsManualVerificationEnabledKey);
            var isIovationReviewAccepted =
                    Drive.Data.Ops.GetServiceConfiguration<bool>(IsIovationReviewAcceptedKey);
            try
            {
                var customerBuilder = CustomerBuilder.New();
                Application application;

                var customer = Config.AUT == AUT.Wb
                                   ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                                   : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

                Drive.Data.Ops.SetServiceConfiguration<bool>(IsManualVerificationEnabledKey, true);
                Drive.Data.Ops.SetServiceConfiguration<bool>(IsIovationReviewAcceptedKey, true);

                if (Config.AUT == AUT.Wb)
                {
                    var organisation = OrganisationBuilder.New(customer).Build();
                    application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
                }
                else
                    application =
                        ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(
                            ApplicationDecisionStatus.Accepted).Build();

                var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Drive.Data.Ops.SetServiceConfiguration(IsManualVerificationEnabledKey, isManualVerificationEnabled);
                Drive.Data.Ops.SetServiceConfiguration(IsIovationReviewAcceptedKey, isIovationReviewAccepted);
            }
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1567")]
        public void L0_CheckpointApplicationGetsIovationReviewWithManualVerificationWithoutIovationReviewAccepted_LoanIsDeclined()
        {
            var isManualVerificationEnabled =
                    Drive.Data.Ops.GetServiceConfiguration<bool>(IsManualVerificationEnabledKey);
            var isIovationReviewAccepted =
                    Drive.Data.Ops.GetServiceConfiguration<bool>(IsIovationReviewAcceptedKey);
            try
            {
                var customerBuilder = CustomerBuilder.New();
                Application application;

                var customer = Config.AUT == AUT.Wb
                                   ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                                   : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

                Drive.Data.Ops.SetServiceConfiguration<bool>(IsManualVerificationEnabledKey, true);
                Drive.Data.Ops.SetServiceConfiguration<bool>(IsIovationReviewAcceptedKey, false);

                if (Config.AUT == AUT.Wb)
                {
                    var organisation = OrganisationBuilder.New(customer).Build();
                    application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
                }
                else
                    application =
                        ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(
                            ApplicationDecisionStatus.Declined).Build();

                var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Drive.Data.Ops.SetServiceConfiguration(IsManualVerificationEnabledKey, isManualVerificationEnabled);
                Drive.Data.Ops.SetServiceConfiguration(IsIovationReviewAcceptedKey, isIovationReviewAccepted);
            }
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735","UK-1567")]
        public void L0_CheckpointApplicationGetsIovationReviewWithNoManualVerification_LoanIsDeclined()
        {
            var isManualVerificationEnabled =
                    Drive.Data.Ops.GetServiceConfiguration<bool>(IsManualVerificationEnabledKey);

			var isIovationReviewAccepted =
				Drive.Data.Ops.GetServiceConfiguration<bool>(IsIovationReviewAcceptedKey);
				
            try
            {
                var customerBuilder = CustomerBuilder.New();
                Application application;

                var customer = Config.AUT == AUT.Wb
                                   ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                                   : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

                Drive.Data.Ops.SetServiceConfiguration<bool>(IsManualVerificationEnabledKey, true);
				Drive.Data.Ops.SetServiceConfiguration<bool>(IsIovationReviewAcceptedKey, false);

                if (Config.AUT == AUT.Wb)
                {
                    var organisation = OrganisationBuilder.New(customer).Build();
                    application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
                }
                else
                    application =
                        ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(
                            ApplicationDecisionStatus.Declined).Build();

                var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
            }
            finally
            {
                Drive.Data.Ops.SetServiceConfiguration(IsManualVerificationEnabledKey, isManualVerificationEnabled);
            	Drive.Data.Ops.SetServiceConfiguration(IsIovationReviewAcceptedKey, isIovationReviewAccepted);
            }
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb), JIRA("CA-1735", "SME-130", "UK-1567")]
        public void L0_CheckpointApplicationGetsIovationUnknown_LoanIsAccepted()
        {
            var customerBuilder = CustomerBuilder.New();
            Application application;

            var customer = Config.AUT == AUT.Wb
                               ? customerBuilder.WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
                               : customerBuilder.WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();

            if (Config.AUT == AUT.Wb)
            {
                var organisation = OrganisationBuilder.New(customer).Build();
                application = ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(IovationMockResponse.Unknown).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            }
            else
                application =
                    ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Unknown).WithExpectedDecision(
                        ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1567")]
        public void Ln_CheckpointApplicationFailsIovation_LoanIsDeclined()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).Build();
            var l0Application = ApplicationBuilder.New(customer).Build();
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(customer.Id, RiskMask.TESTDeviceNotOnBlacklist.ToString());

            var lnApplication = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Deny).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1567")]
        public void Ln_CheckpointApplicationPassesIovation_LoanIsAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();
            var l0Application = ApplicationBuilder.New(customer).Build();
            l0Application.RepayOnDueDate();

            var lnApplication = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Allow).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1567")]
        [Ignore("AFAIK The review will trigger manual verification")]
        public void Ln_CheckpointApplicationGetsIovationReview_LoanIsAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();
            var l0Application = ApplicationBuilder.New(customer).Build();
            l0Application.RepayOnDueDate();

            var lnApplication = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Review).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1567")]
        public void Ln_CheckpointApplicationGetsIovationUnknown_LoanIsAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();
            var l0Application = ApplicationBuilder.New(customer).Build();
            l0Application.RepayOnDueDate();

            var lnApplication = ApplicationBuilder.New(customer).WithIovationBlackBox(IovationMockResponse.Unknown).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
        }
    }
}