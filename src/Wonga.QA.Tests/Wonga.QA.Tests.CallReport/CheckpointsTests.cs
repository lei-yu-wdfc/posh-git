﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CallReport
{
    public class CheckpointsTests
    {
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        public void TestCallReportUnknownApplicant_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicantIsNotDeceased, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id,RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count,1,"There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.AreEqual((CreditBureauEnum) riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the Kathleen customer that is alive according to call report, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicantIsNotDeceased, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the customer that is dead according to call report, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicantIsNotDeceased, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("CallReport -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicationElementNotCIFASFlagged, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("CallReport -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicationElementNotCIFASFlagged, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicantIsSolvent, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("CallReport -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var application = CreateApplicationWithAsserts(RiskMask.TESTApplicantIsSolvent, forename, surname, Get.GetDoB(), ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestCallReportDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMask.TESTCustomerDateOfBirthIsCorrectSME, forename, surname, dateOfBirth, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        public void TestCallReportDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTCustomerDateOfBirthIsCorrectSME, forename, surname, dateOfBirth, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        public void TestCallReportDateOfBirthNotProvided_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMask.TESTCustomerDateOfBirthIsCorrectSME, forename, surname, dateOfBirth, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.AreEqual((CreditBureauEnum)riskWorkflows[0].CreditBureauUsed, CreditBureauEnum.CallReport);
        }

        private static Application CreateApplicationWithAsserts(RiskMask middlenameMask, String forename, String surname, Date dateOfBirth, ApplicationDecisionStatus applicationDecision)
        {
            var customerBuilder = CustomerBuilder.New();
            customerBuilder.ScrubForename(forename);
            customerBuilder.ScrubSurname(surname);
            var customer = customerBuilder.WithMiddleName(middlenameMask.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(applicationDecision).Build();
            Assert.IsNotNull(application);

            var riskDb = Drive.Db.Risk;
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
