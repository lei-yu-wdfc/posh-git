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

        private void TestSingleBusinessCheckpoint(RiskMiddlenameMask riskMask, CheckpointStatus expectedResult, CheckpointDefinitionEnum checkpoint, int? orgNo = null)
        {
            ApplicationDecisionStatusEnum appOutcome = expectedResult == CheckpointStatus.Verified
                                                           ? ApplicationDecisionStatusEnum.Accepted
                                                           : ApplicationDecisionStatusEnum.Declined;
            
            Customer cust = CustomerBuilder.New()
                .WithMiddleName(Data.EnumToString(riskMask)).Build();

            var orgB = OrganisationBuilder.New().WithPrimaryApplicant(cust);

            Organisation org = orgNo == null ? orgB.Build() : orgB.WithOrganisationNumber((int)orgNo).Build();
            
            Application app = ApplicationBuilder.New(cust, org)
                .WithExpectedDecision(appOutcome).Build();

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(app.Id, expectedResult), Data.EnumToString(checkpoint));
        }
        
        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void BusinessDataFound()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessBureauDataIsAvailable, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessBureauDataIsAvailable);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void BusinessDataNotFound()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessBureauDataIsAvailable, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessBureauDataIsAvailable, 99999999);
        }


        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        public void BusinessPerformanceScoreIsAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPerformanceScoreIsAcceptaple , CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        public void BusinessPerformanceScoreIsNotAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPerformanceScoreIsAcceptaple, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple, 99999902);
        }


        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void BusinessIsCurrentlyTrading()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessIsCurrentlyTrading, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessIsCurrentlyTrading);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void BusinessIsCurrentlyNotTrading()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessIsCurrentlyTrading, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessIsCurrentlyTrading, 90000001);
        }


        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        public void BusinessPaymentScoreIsAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        public void BusinessPaymentScoreNotFound()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable, 90000004);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162"), ExpectedException(typeof(TimeoutException))]
        public void BusinessPaymentScoreIsNotAcceptable()
        {
            TestSingleBusinessCheckpoint(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable, 90000003);
        }

        #endregion
    }
}