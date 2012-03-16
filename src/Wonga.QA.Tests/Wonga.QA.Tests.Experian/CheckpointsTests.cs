using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Experian
{
    [Parallelizable(TestScope.All)]
    public class CheckpointsTests
    {
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianUnknownApplicant_LoanIsApproved()
        {
            const String forename = "Unknown";
            const String surname = "Customer";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicantIsNotDeceased, forename,surname, Data.GetDoB(),ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count,1,"There should be 1 workflow");

            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the Kathleen customer that is alive, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicantIsNotDeceased, forename, surname, Data.GetDoB(), ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the customer that is dead, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicantIsNotDeceased, forename, surname, Data.GetDoB(), ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicationElementNotCIFASFlagged, forename, surname, Data.GetDoB(), ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicationElementNotCIFASFlagged, forename, surname, Data.GetDoB(), ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicantIsSolvent, forename,surname, Data.GetDoB(),ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianApplicantIsSolvent, forename, surname, Data.GetDoB(), ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianCustomerDateOfBirthIsCorrect, forename, surname, dateOfBirth, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianCustomerDateOfBirthIsCorrect, forename, surname, dateOfBirth, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianDateOfBirthIsNotProvided_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMask.TESTExperianCustomerDateOfBirthIsCorrect, forename, surname, dateOfBirth, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.Experian);
        }
        private static Application CreateApplicationWithAsserts(RiskMask middlenameMask, String forename, String surname, Date dateOfBirth, ApplicationDecisionStatusEnum applicationDecision)
        {
            var customer = CustomerBuilder.New().WithMiddleName(middlenameMask.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(applicationDecision).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Risk Social details should exist");

            return application;
        }
    }
}
