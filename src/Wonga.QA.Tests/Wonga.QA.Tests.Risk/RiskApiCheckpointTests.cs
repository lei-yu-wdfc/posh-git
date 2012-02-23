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
    class RiskApiCheckpointTests
    {
        private Boolean SingleCheckPointVerification(Application application, CheckpointStatus expectedStatus, CheckpointDefinitionEnum checkpoint)
        {
            var db = new DbDriver();

            int riskApp = db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id).RiskApplicationId;

            var dbCheckpoint = db.Risk.WorkflowCheckpoints.Single(r => r.RiskApplicationId == riskApp);

            return Data.EnumToString(checkpoint) ==
                   db.Risk.CheckpointDefinitions.Single(r => r.CheckpointDefinitionId == dbCheckpoint.CheckpointDefinitionId).Name &&
                   dbCheckpoint.CheckpointStatus == Convert.ToByte(expectedStatus);
        }
        private void AssertCreditBureauUsed(Application application, CreditBureauEnum creditBureauUsed)
        {
            var db = new DbDriver();
            var riskApplicationEntity = db.Risk.RiskApplications.SingleOrDefault(r => r.ApplicationId == application.Id);
            Assert.IsNotNull(riskApplicationEntity, "The risk application should exist");
            Assert.IsNotNull(riskApplicationEntity.CreditBureauUsed, "One credit bureau should have been used");
            Assert.AreEqual((CreditBureauEnum)riskApplicationEntity.CreditBureauUsed, creditBureauUsed, "The credit bureau used should be the same"); // <- Lol
        }

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

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-575"),
        Description("CallReport -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint - ")]
        public void TestCallReportUnknownApplicant_LoanIsApproved()
        {
            const string maskName = "ApplicantIsNotDeceased";
            const String forename = "unknown";
            const String surname = "customer";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-575"),
        Description("CallReport -> This test creates a loan for the Kathleen customer that is alive according to call report, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsNotDeceased_LoanIsApproved()
        {
            const String maskName = "ApplicantIsNotDeceased";
            const String forename = "kathleen";
            const String surname = "bridson";


            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-575"),
        Description("CallReport -> This test creates a loan for the customer that is dead according to call report, then checks the risk checkpoint"),
        Pending("Deadguy case not working properly - needs investigating what breaks it and how to fix - it times out at creating application")]
        public void TestCallReportApplicantIsDeceased_LoanIsDeclined()
        {
            const string maskName = "ApplicantIsNotDeceased";
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Failed, CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-584"),
        Description("CallReport -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const string maskName = "ApplicationElementNotCIFASFlagged";
            const String forename = "kathleen";
            const String surname = "nicole";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Verified, CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-584"),
        Description("CallReport -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint"),
        Pending("Laura Insolvent case not working properly - it times out at creating application")]
        public void TestCallReportApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const string maskName = "ApplicationElementNotCIFASFlagged";
            const String forename = "laura";
            const String surname = "insolvent";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Failed, CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-638"),
        Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsSolvent_LoanIsApproved()
        {
            const string maskName = "ApplicantIsSolvent";
            const String forename = "kathleen";
            const String surname = "nicole";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Verified, CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-638"), 
        Description("CallReport -> This test creates a loan for the insolvent customer, then checks the risk checkpoint"),
        Pending("Laura Insolvent case not working properly - it times out at creating application")]
        public void TestCallReportApplicantIsInsolvent_LoanIsDeclined()
        {
            const string maskName = "ApplicantIsSolvent";
            const String forename = "laura";
            const String surname = "insolvent";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Failed, CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        #endregion

        #region Experian
        [Test,
        AUT(AUT.Wb),
        JIRA("SME-575"),
        Description("Experian -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianUnknownApplicant_LoanIsApproved()
        {
            const string maskName = "ExperianApplicantIsNotDeceased";
            const String forename = "Unknown";
            const String surname = "Customer";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-575"),
        Description("Experian -> This test creates a loan for the Kathleen customer that is alive, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsNotDeceased_LoanIsApproved()
        {
            const String maskName = "ExperianApplicantIsNotDeceased";
            const String forename = "kathleen";
            const String surname = "bridson";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-575"),
        Description("Experian -> This test creates a loan for the customer that is dead, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsDeceased_LoanIsDeclined()
        {
            const string maskName = "ExperianApplicantIsNotDeceased";
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Failed, CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-584"),
        Description("Experian -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const string maskName = "ExperianApplicationElementNotCIFASFlagged";
            const String forename = "kathleen";
            const String surname = "nicole";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Verified, CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-584"),
        Description("Experian -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const string maskName = "ExperianApplicationElementNotCIFASFlagged";
            const String forename = "laura";
            const String surname = "insolvent";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Failed, CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, 
        AUT(AUT.Wb), 
        JIRA("SME-638"),
        Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsSolvent_LoanIsApproved()
        {
            const string maskName = "ExperianApplicantIsSolvent";
            const String forename = "kathleen";
            const String surname = "nicole";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Verified, CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test,
        AUT(AUT.Wb),
        JIRA("SME-638"),
        Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint"),
        Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsInsolvent_LoanIsDeclined()
        {
            const string maskName = "ExperianApplicantIsSolvent";
            const String forename = "laura";
            const String surname = "insolvent";

            var customer = CustomerBuilder.New().WithMiddleName("TEST" + maskName).WithForename(forename).WithSurname(surname).Build();
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

            var checkpointDefinitionEntity = riskDb.CheckpointDefinitions.SingleOrDefault(p => p.Name == Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.IsNotNull(checkpointDefinitionEntity, "Risk checkpoint should exist");
            Assert.IsTrue(SingleCheckPointVerification(application, CheckpointStatus.Failed, CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        #endregion

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("CA-1735")]
        public void IovationBlackBoxDecline()
        {
            Customer cust = CustomerBuilder.New()
                .WithEmployer("test:DeviceNotOnBlacklist").Build();

            Application app = ApplicationBuilder.New(cust).WithIovationBlackBox("Deny")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Failed, CheckpointDefinitionEnum.Applicationdatablacklistcheck));
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("CA-1735")]
        public void IovationBlackBoxAllow()
        {
            Customer cust = CustomerBuilder.New()
                .WithEmployer("test:DeviceNotOnBlacklist").Build();

            Application app = ApplicationBuilder.New(cust).WithIovationBlackBox("Allow")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Assert.IsTrue(SingleCheckPointVerification(app, CheckpointStatus.Verified, CheckpointDefinitionEnum.Applicationdatablacklistcheck));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1743")]
        public void LnShouldPassEidCheck()
        {
            Customer cust = CustomerBuilder.New()
                .WithEmployer("test:DeviceNotOnBlacklist").Build();

            Application l0App = ApplicationBuilder.New(cust).WithIovationBlackBox("Allow")
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Assert.IsTrue(SingleCheckPointVerification(l0App, CheckpointStatus.Verified, CheckpointDefinitionEnum.Applicationdatablacklistcheck));

            l0App.Repay();

            EmploymentDetailEntity employmentDetails = Driver.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == cust.Id);
            employmentDetails.EmployerName = "test:DirectFraud";
            employmentDetails.Submit();

            Application lNApp = ApplicationBuilder.New(cust).WithIovationBlackBox("Allow")
               .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Assert.IsTrue(SingleCheckPointVerification(lNApp, CheckpointStatus.Verified, CheckpointDefinitionEnum.UserAssistedFraudCheck));
        }

    }
}