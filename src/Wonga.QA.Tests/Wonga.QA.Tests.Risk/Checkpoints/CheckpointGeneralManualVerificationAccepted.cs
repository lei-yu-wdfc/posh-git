using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointGeneralManualVerificationAccepted
	{
		 [Test, JIRA("SME-188"), AUT(AUT.Wb)]
		public void ApplicantPassesManualVerification_LoanIsApproved()
		{
			var customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTGeneralManualVerification).Build();
		 	var organisation = OrganisationBuilder.New(customer).Build();
		
			var application = ApplicationBuilder.New(customer,organisation).WithExpectedDecision(ApplicationDecisionStatus.PreAccepted).Build();

			Do.Until(() => Drive.Db.Risk.RiskWorkflows.Single(w => w.ApplicationId == application.Id && (RiskWorkflowTypes)w.WorkflowType == RiskWorkflowTypes.ManualVerification));

			var verifyManualVerificationCommand = new ConfirmManualVerificationAcceptedCommand
													{
														ApplicationId = application.Id
													};

			Drive.Cs.Commands.Post(verifyManualVerificationCommand);

		 	Do.Until(
		 		() =>
		 		Drive.Db.Risk.RiskApplications.Single(
		 			a => a.ApplicationId == application.Id && a.Decision == 1));
		}

		 [Test, JIRA("SME-188"), AUT(AUT.Wb)]
		 public void ApplicantFailsManualVerification_LoanIsDeclined()
		 {
			 var customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTGeneralManualVerification).Build();
			 var organisation = OrganisationBuilder.New(customer).Build();

			 var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.PreAccepted).Build();

			 Do.Until(() => Drive.Db.Risk.RiskWorkflows.Single(w => w.ApplicationId == application.Id && (RiskWorkflowTypes)w.WorkflowType == RiskWorkflowTypes.ManualVerification));

			 var verifyManualVerificationCommand = new ConfirmManualVerificationDeclinedCommand()
			 {
				 ApplicationId = application.Id
			 };

			 Drive.Cs.Commands.Post(verifyManualVerificationCommand);

			 Do.Until(
				 () =>
				 Drive.Db.Risk.RiskApplications.Single(
					 a => a.ApplicationId == application.Id && a.Decision == 2));
		 }
	}
}
