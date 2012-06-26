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
	[TestFixture, Parallelizable(TestScope.All)]
	public class CheckpointApplicationDeviceNotOnBlacklistTests
	{
		private const string IsManualVerificationEnabledKey = "Risk.IsManualVerificationEnabled";
		private const string IsIovationReviewAcceptedKey = "Risk.IsIovationReviewAccepted";

		private Customer _customer;

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb, AUT.Za), JIRA("CA-1735", "SME-130", "UK-1567"),]
		public void L0IovationDenyIsDeclined()
		{
			var application = BuildApplication(ApplicationDecisionStatus.Declined, IovationMockResponse.Deny);
			VerifyRiskApplication(application, RiskCheckpointStatus.Failed);
		}

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb, AUT.Za), JIRA("CA-1735", "SME-130", "UK-1567")]
		public void L0IovationAllowIsAccepted()
		{
			_customer = BuildCustomer();
			var application = BuildApplication(ApplicationDecisionStatus.Accepted, IovationMockResponse.Allow);
			VerifyRiskApplication(application, RiskCheckpointStatus.Verified);
			application.RepayOnDueDate();
		}

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb, AUT.Za), JIRA("CA-1735", "SME-130", "UK-1567")]
		public void L0IovationUnknownIsAccepted()
		{
			var application = BuildApplication(ApplicationDecisionStatus.Accepted, IovationMockResponse.Unknown);
			VerifyRiskApplication(application, RiskCheckpointStatus.Verified);
		}

		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Wb, AUT.Za), JIRA("CA-1735", "SME-130", "UK-1567"), Pending("Modifies svcconfig")]
		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		public void L0IovationReview(bool manualVerificationEnabled, bool iovationReviewAccepted)
		{
			var isManualVerificationEnabled =
					Drive.Data.Ops.GetServiceConfiguration<bool>(IsManualVerificationEnabledKey);
			var isIovationReviewAccepted =
					Drive.Data.Ops.GetServiceConfiguration<bool>(IsIovationReviewAcceptedKey);

			Drive.Data.Ops.SetServiceConfiguration<bool>(IsManualVerificationEnabledKey, manualVerificationEnabled);
			Drive.Data.Ops.SetServiceConfiguration<bool>(IsIovationReviewAcceptedKey, iovationReviewAccepted);

			var expectedDecision = manualVerificationEnabled && iovationReviewAccepted ? ApplicationDecisionStatus.Accepted : ApplicationDecisionStatus.Declined;
			var expectedCheckpointStatus = manualVerificationEnabled && iovationReviewAccepted ? RiskCheckpointStatus.Verified : RiskCheckpointStatus.Failed;

			try
			{
				var application = BuildApplication(expectedDecision, IovationMockResponse.Review);
				VerifyRiskApplication(application, expectedCheckpointStatus);
			}

			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(IsManualVerificationEnabledKey, isManualVerificationEnabled);
				Drive.Data.Ops.SetServiceConfiguration(IsIovationReviewAcceptedKey, isIovationReviewAccepted);
			}
		}

		[Test, AUT(AUT.Uk, AUT.Za), JIRA("UK-1567"), DependsOn("L0IovationAllowIsAccepted")]
		public void LnIovationDenyIsDeclined()
		{
			var application = BuildApplication(_customer, ApplicationDecisionStatus.Declined, IovationMockResponse.Deny);
			VerifyRiskApplication(application, RiskCheckpointStatus.Failed);
		}

		[Test, AUT(AUT.Uk, AUT.Za), JIRA("UK-1567"), DependsOn("LnIovationDenyIsDeclined")]
		public void LnIovationAllowIsAccepted()
		{
			var application = BuildApplication(_customer, ApplicationDecisionStatus.Accepted, IovationMockResponse.Allow);
			VerifyRiskApplication(application, RiskCheckpointStatus.Verified);
			application.RepayOnDueDate();
		}

		[Test, AUT(AUT.Uk, AUT.Za), JIRA("UK-1567"), DependsOn("LnIovationAllowIsAccepted")]
        [Pending("We dont know yet how review is going to work.")]
		public void LnIovationReviewIsAccepted()
		{
			var application = BuildApplication(_customer, ApplicationDecisionStatus.Accepted, IovationMockResponse.Review);
			VerifyRiskApplication(application, RiskCheckpointStatus.Verified);
			application.RepayOnDueDate();
		}

		[Test, AUT(AUT.Uk, AUT.Za), JIRA("UK-1567"), DependsOn("LnIovationReviewIsAccepted")]
		public void LnIovationUnknownIsAccepted()
		{
			var application = BuildApplication(_customer, ApplicationDecisionStatus.Accepted, IovationMockResponse.Unknown);
			VerifyRiskApplication(application, RiskCheckpointStatus.Verified);
		}

		#region Helpers

		private Customer BuildCustomer()
		{
			return Config.AUT == AUT.Wb
					? CustomerBuilder.New().WithMiddleName(RiskMask.TESTApplicationDeviceNotOnBlacklist).Build()
					: CustomerBuilder.New().WithEmployer(RiskMask.TESTDeviceNotOnBlacklist).Build();
		}

		private Application BuildApplication(Customer customer, ApplicationDecisionStatus expectedDecision, IovationMockResponse iovationMockResponse)
		{
			if (Config.AUT == AUT.Wb)
			{
				var organisation = OrganisationBuilder.New(customer).Build();
				return ApplicationBuilder.New(customer, organisation).WithIovationBlackBox(iovationMockResponse).WithExpectedDecision(expectedDecision).Build();
			}

			return ApplicationBuilder.New(customer).WithIovationBlackBox(iovationMockResponse).WithExpectedDecision(
				expectedDecision).Build();
		}

		private Application BuildApplication(ApplicationDecisionStatus expectedDecision, IovationMockResponse iovationMockResponse)
		{
			var customer = BuildCustomer();
			return BuildApplication(customer, expectedDecision, iovationMockResponse);
		}

		private void VerifyRiskApplication(Application application, RiskCheckpointStatus expectedCheckpointStatus)
		{
			var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
			Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, expectedCheckpointStatus), Get.EnumToString(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck));
		}

		#endregion

	}
}