using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk), Pending("ZA-2565")]
	public class CheckpointCustomerIsEmployedTests
	{
        private const RiskMask TestMask = RiskMask.TESTCustomerIsEmployed;
		private Application l0Application;

		[Test, JIRA("UK-1566")]
		public void L0_CustomerIsEmployed_LoanIsAccepted()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithEmployerStatus(EmploymentStatusEnum.EmployedFullTime.ToString()).Build();
			l0Application =  ApplicationBuilder.New(customer).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(l0Application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsEmployed));
		}

		[Test, JIRA("UK-1566")]
		public void L0_CustomerIsUnemployed_LoanIsDeclined()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsEmployed));
		}

		[Test, DependsOn("L0_CustomerIsEmployed_LoanIsAccepted"), JIRA("UK-1566")]
        public void Ln_CustomerIsEmployed_LoanIsAccepted()
		{
			l0Application.RepayOnDueDate();

            var lnApplication = ApplicationBuilder.New(l0Application.GetCustomer()).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsEmployed));
        }
	}
}
