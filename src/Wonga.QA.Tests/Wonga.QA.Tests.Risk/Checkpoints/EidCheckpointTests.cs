﻿using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    public class EidCheckpointTests
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1743")]
        public void LnShouldPassEidCheck()
        {
            var customer = CustomerBuilder.New().Build();
            var l0app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            l0app.RepayOnDueDate();

            EmploymentDetailEntity employmentDetails = Driver.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == customer.Id);
            employmentDetails.EmployerName = "test:DirectFraud";
            employmentDetails.Submit();

            Application lNApp = ApplicationBuilder.New(customer).Build();
            var riskWorkflows = Application.GetWorkflowsForApplication(lNApp.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.UserAssistedFraudCheck));
            //Assert.Contains(Application.GetExecutedCheckpointDefinitions(lNApp.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.UserAssistedFraudCheck));
        }
    }
}