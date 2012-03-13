using System;
using System.Linq;
using System.Reflection;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
    [Parallelizable(TestScope.All)]
    public class RiskApiCheckpointTests
    {
        #region BusinessLoanNewCompanyCheckpoints

        private void TestSingleBusinessCheckpoint(RiskMiddlenameMask riskMask, CheckpointStatus expectedResult, CheckpointDefinitionEnum checkpoint, String orgNo = null)
        {
            ApplicationDecisionStatusEnum appOutcome = expectedResult == CheckpointStatus.Verified
                                                           ? ApplicationDecisionStatusEnum.Accepted
                                                           : ApplicationDecisionStatusEnum.Declined;
            
            Customer customer = CustomerBuilder.New().WithMiddleName(Data.EnumToString(riskMask)).Build();
            var orgB = OrganisationBuilder.New(customer);
            Organisation organization = orgNo == null ? orgB.Build() : orgB.WithOrganisationNumber(orgNo).Build();
            Application application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(appOutcome).Build();
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, expectedResult), Data.EnumToString(checkpoint));
        }
        
        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void BusinessDataFound()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessBureauDataIsAvailable, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessBureauDataIsAvailable);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void BusinessDataNotFound()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessBureauDataIsAvailable, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessBureauDataIsAvailable, "99999999");
        }


        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        public void BusinessPerformanceScoreIsAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPerformanceScoreIsAcceptaple , CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        public void BusinessPerformanceScoreIsNotAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPerformanceScoreIsAcceptaple, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple,"99999902");
        }


        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void BusinessIsCurrentlyTrading()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessIsCurrentlyTrading, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessIsCurrentlyTrading);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void BusinessIsCurrentlyNotTrading()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessIsCurrentlyTrading, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessIsCurrentlyTrading, "90000001");
        }


        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        public void BusinessPaymentScoreIsAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        public void BusinessPaymentScoreNotFound()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable, "99999904");
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        public void BusinessPaymentScoreIsNotAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable, "99999903");
        }

        #endregion
    }
}