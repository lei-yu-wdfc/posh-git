﻿using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [Parallelizable(TestScope.All)]
    public class CheckpointDirectFraudTests
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1743")]
        public void LnShouldPassEidCheck()
        {
            var customer = CustomerBuilder.New().Build();
            var l0app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            l0app.RepayOnDueDate();

            EmploymentDetailEntity employmentDetails = Drive.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == customer.Id);
            employmentDetails.EmployerName = Get.EnumToString(RiskMask.TESTDirectFraud);
            employmentDetails.Submit();

            Application lNApp = ApplicationBuilder.New(customer).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(lNApp.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.UserAssistedFraudCheck));
        }
    }
}