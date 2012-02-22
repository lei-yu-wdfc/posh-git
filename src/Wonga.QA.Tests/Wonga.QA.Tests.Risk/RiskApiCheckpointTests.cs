using System;
using System.Linq;
using System.Reflection;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
    [Parallelizable(TestScope.All)]
    class RiskApiCheckpointTests
    {
        [SetUp, AUT]
        public void Setup(){}
        
        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void BusinessIsCurrentlyTrading()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("TESTBusinessIsCurrentlyTrading").Build();

            Organisation org = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            var app = ApplicationBuilder.New(cust, org).Build();
            
            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessIsCurrentlyTrading));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        public void BusinessIsCurrentlyNotTrading()
        {
            Customer cust = CustomerBuilder.New()
                .WithMiddleName("TESTBusinessIsCurrentlyTrading").Build();

            Organisation org = OrganisationBuilder.New()
                .WithOrganisationNumber(90000001)
                .WithPrimaryApplicant(cust).Build();

            Application app = ApplicationBuilder.New(cust, org)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessIsCurrentlyTrading));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void BusinessDataFound()
        {
            Customer cust = CustomerBuilder.New().WithMiddleName("TESTBusinessBureauDataIsAvailable").Build();


            Organisation org = OrganisationBuilder.New().WithPrimaryApplicant(cust).Build();

            Application app = ApplicationBuilder.New(cust, org).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Verified, CheckpointDefinitionEnum.BusinessBureauDataIsAvailable));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        public void BusinessDataNotFound()
        {
            Customer cust = CustomerBuilder.New()
                .WithMiddleName("TESTBusinessBureauDataIsAvailable").Build();

            Organisation org = OrganisationBuilder.New()
                .WithOrganisationNumber(99999999)
                .WithPrimaryApplicant(cust).Build();

            Application app = ApplicationBuilder.New(cust, org)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Failed, CheckpointDefinitionEnum.BusinessBureauDataIsAvailable));
        }

        #region CallReport

        [Test, JIRA("SME-575"), AUT(AUT.Wb)]
        public void TestApplicantIsNotDeceased_LoanIsApproved()
        {
            const string maskName = "ApplicantIsNotDeceased";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");
            Assert.AreEqual(1, riskApplicationEntity.WorkflowCheckpoints.Count, "1 Checkpoint should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Verified, CheckpointDefinitionEnum.ApplicantIsAlive));
        }

        [Test, JIRA("SME-575"), AUT(AUT.Wb)]
        public void TestUnknownApplicant_LoanIsApproved()
        {
            const string maskName = "ApplicantIsNotDeceased";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");
            Assert.AreEqual(1, riskApplicationEntity.WorkflowCheckpoints.Count, "1 Checkpoint should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault( p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.IsNotNull(checkpointDefinitionEntity,"Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Verified, CheckpointDefinitionEnum.ApplicantIsAlive));
        }

        #endregion

        private Boolean SingleCheckPointVerification(Application application, CheckpointStatus expectedStatus, CheckpointDefinitionEnum checkpoint)
        {
            var db = new DbDriver();

            int riskApp = db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id).RiskApplicationId;

            var dbCheckpoint = db.Risk.WorkflowCheckpoints.Single(r => r.RiskApplicationId == riskApp);

            return Data.EnumToString(checkpoint) ==
                   db.Risk.CheckpointDefinitions.Single(r => r.CheckpointDefinitionId == dbCheckpoint.CheckpointDefinitionId).Name &&
                   dbCheckpoint.CheckpointStatus == Convert.ToByte(expectedStatus);
        }
    }
}