using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    public class CheckpointApplicationElementNotOnCsBlacklist
    {
        /* The Story is : We need to create a customer and then send SuspectFraud command and see the failure */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-849")]
        public void DoNotRelendIsOff_L0ApplicationAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTDoNotRelend).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.FraudListCheck));

        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-849")]
        public void DoNotRelendIsOff_LNApplicationAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).Build();
            var l0Application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            l0Application.RepayOnDueDate();
            Drive.Db.UpdateEmployerName(customer.Id, RiskMask.TESTDoNotRelend.ToString());

            var lnApplication = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.FraudListCheck));
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-849")]
        public void DoNotRelendIsOn_L0ApplicationDeclined()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTDoNotRelend).Build();
            Drive.Cs.Commands.Post(new SuspectFraudCommand() {AccountId = customer.Id, CaseId = Get.RandomString(5)});

            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.FraudListCheck));
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-849")]
        public void DoNotRelendIsOn_LNApplicationDeclined()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).Build();
            var l0Application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            l0Application.RepayOnDueDate();
            Drive.Db.UpdateEmployerName(customer.Id, RiskMask.TESTDoNotRelend.ToString());

            Drive.Cs.Commands.Post(new SuspectFraudCommand() { AccountId = customer.Id, CaseId = Get.RandomString(5) });
            var lnApplication = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lnApplication.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.FraudListCheck));
        }

    }
}
