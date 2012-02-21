using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
    [Parallelizable(TestScope.All)]
    class RiskApiTests
    {
        private bool SingleCheckPointVerification(Application application, CheckpointStatus expectedStatus, string checkpointName)
        {
            var db = new DbDriver();

            int riskApp = db.Risk.RiskApplications.Single(r=>r.ApplicationId == application.Id).RiskApplicationId;

            var a = db.Risk.WorkflowCheckpoints.Single(r => r.RiskApplicationId == riskApp );

            return checkpointName ==
                   db.Risk.CheckpointDefinitions.Single(r => r.CheckpointDefinitionId == a.CheckpointDefinitionId).Name &&
                   a.CheckpointStatus == Convert.ToByte(expectedStatus);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void Risk_BusinessIsCurrentlyTrading()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("TESTBusinessIsCurrentlyTrading").Build();

            Organisation org = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            var app = ApplicationBuilder.New(cust, org).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Verified, "BusinessIsCurrentlyTrading"));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void Risk_BusinessIsCurrentlyNotTrading()
        {
            Customer cust = CustomerBuilder.New()
                .WithMiddleName("TESTBusinessIsCurrentlyTrading").Build();

            Organisation org = OrganisationBuilder.New()
                .WithOrganisationNumber(90000001)
                .WithPrimaryApplicant(cust).Build();

            Application app = ApplicationBuilder.New(cust, org)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Failed, "BusinessIsCurrentlyTrading"));
        }


        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void Risk_BusinessDataFound()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("TESTBusinessBureauDataIsAvailable").Build();

            Organisation org = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            Application app = ApplicationBuilder.New(cust, org).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Verified, "Business Bureau Data Is Available"));
        }


        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void Risk_BusinessDataNotFound()
        {
            Customer cust = CustomerBuilder.New()
                .WithMiddleName("TESTBusinessBureauDataIsAvailable").Build();

            Organisation org = OrganisationBuilder.New()
                .WithOrganisationNumber(99999999)
                .WithPrimaryApplicant(cust).Build();

            Application app = ApplicationBuilder.New(cust, org)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Failed, "Business Bureau Data Is Available"));
        }
    }
}