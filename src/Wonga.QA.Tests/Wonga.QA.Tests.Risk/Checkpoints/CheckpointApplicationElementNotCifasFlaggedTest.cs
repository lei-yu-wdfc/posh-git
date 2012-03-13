using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	class CheckpointApplicationElementNotCifasFlaggedTest
	{
		private const string TestMask = "test:ApplicationElementNotCIFASFlagged";

		[Test, AUT(AUT.Uk), JIRA("UK-852")]
		public void Accepted()
		{
			RunTest(CreditBureauCustomerUk.ForeName, CheckpointStatus.Verified);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-852")]
		public void Declined()
		{
			RunTest(string.Format("{0}CIFAS", CreditBureauCustomerUk.ForeName), CheckpointStatus.Failed);
		}

		#region Implementation
		private void RunTest(string customerName, CheckpointStatus expectedStatus)
		{
			var customer =
				CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(customerName)
				.WithSurname(CreditBureauCustomerUk.SurName)
				.WithDateOfBirth(CreditBureauCustomerUk.DateOfBirth)
				.Build();

			var application = ApplicationBuilder.New(customer).Build();

			// wait until workflow will be created and started
			Do.Until(() => Driver.Db.Risk.RiskWorkflows.Single(x => x.ApplicationId == application.Id));

			var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
			Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");

			// wait until decision made
			Do.Until(() => Driver.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id && x.Decision != 0));

			Assert.Contains(
				Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, expectedStatus),
				Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
		}

		#endregion
	}
}
