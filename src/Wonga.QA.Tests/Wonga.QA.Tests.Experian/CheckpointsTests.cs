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
    public class CheckpointsTests
    {
        private const String GoodCompanyRegNumber = "00000086";

        #region MainApplicant

        /* Main Applicant is Alive */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        public void TestExperianUnknownMainApplicant_LoanIsApproved()
        {
            const String forename = "Unknown";
            const String surname = "Customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsNotDeceased,ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the Kathleen customer that is alive, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsNotDeceased,ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the customer that is dead, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsNotDeceased,ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        /* Main Applicant CIFAS */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicationElementNotCIFASFlagged,ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicationElementNotCIFASFlagged, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        /* Main Applicant Data is Available */

        [Test, AUT(AUT.Wb)]
        [Description("Experian -> This test creates a loan and checks if the main applicant has data available")]
        public void TestExperianMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTExperianCreditBureauDataIsAvailable, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var mainApplicantRiskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        [Test, AUT(AUT.Wb),]
        [Description("Experian -> This test creates a loan and checks if the main applicant has data available")]
        public void TestExperianMainApplicantIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTExperianCreditBureauDataIsAvailable, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        /* Main Applciant Solvent */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsSolvent,ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsSolvent, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        /* Main applicant DOB check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianCustomerDateOfBirthIsCorrect, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianCustomerDateOfBirthIsCorrect,ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsNotProvided_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianCustomerDateOfBirthIsCorrect, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
        }

        #endregion

        #region Guarantor

        /* Guarantor Is Alive */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestExperianUnknownGuarantor_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicantIsNotDeceased.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestExperianGuarantorIsAlive_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicantIsNotDeceased.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestExperianGuarantorIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicantIsNotDeceased.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        /* Guarantor CIFAS check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("Experian -> This test creates a loan for a guarantor that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestExperianGuarantorIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());
            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicationElementNotCIFASFlagged.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(
                () =>
                Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("Experian -> This test creates a loan for a guarantor that is  CIFAS flagged, then checks the risk checkpoint")]
        public void TestExperianGuarantorIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());
            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicationElementNotCIFASFlagged.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() =>Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        /* Guarantor Data is available */

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Experian -> This test creates a loan and checks if the guarantors has data available")]
        public void TestExperianGuarantorDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianCreditBureauDataIsAvailable.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Experian -> This test creates a loan and checks if the guarantors has data available")]
        public void TestExperianGuarantorDataIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianCreditBureauDataIsAvailable.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        /* Guarantor is solvent */

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Experian -> This test creates a loan and checks if the guarantor is solvent")]
        public void TestExperianGuarantorIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicantIsSolvent.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Experian -> This test creates a loan and checks if the guarantor is solvent")]
        public void TestExperianGuarantorIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTExperianApplicantIsSolvent.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        #endregion

        private static Application CreateApplicationWithAsserts(Customer mainApplicant, String companyRegisteredNumber, RiskMask middlenameMask, ApplicationDecisionStatus applicationDecision, List<Customer> guarantors = null)
        {
            var customerBuilder = CustomerBuilder.New(mainApplicant.Id);
            customerBuilder.ScrubForename(mainApplicant.Forename);
            customerBuilder.ScrubSurname(mainApplicant.Surname);
            //STEP 1 - Create the main director
            var mainDirector = customerBuilder.WithMiddleName(middlenameMask.ToString()).WithForename(mainApplicant.Forename).WithSurname(mainApplicant.Surname).WithDateOfBirth(mainApplicant.DateOfBirth).WithMobileNumber(mainApplicant.MobilePhoneNumber).Build();

            //STEP2 - Create the company
            var organisationBuilder = OrganisationBuilder.New(mainDirector).WithOrganisationNumber(companyRegisteredNumber);
            var organisation = organisationBuilder.Build();


            //STEP3 - Create the application
            var applicationBuilder = ApplicationBuilder.New(mainDirector, organisation).WithExpectedDecision(applicationDecision) as BusinessApplicationBuilder;

            //STEP4 - Create the guarantors list + send it to the application
            if (guarantors != null)
            {
                applicationBuilder.WithGuarantors(guarantors);
            }

            //STEP5 - Build the application + send the list of guarantors
            var application = applicationBuilder.Build();

            if (applicationDecision != ApplicationDecisionStatus.Declined)
            {
                //STE6 - Build the extra guarantors + sign
                if (guarantors != null)
                {
                    applicationBuilder.BuildGuarantors();
                    applicationBuilder.SignApplicationForSecondaryDirectors();
                }
            }

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

        private void WaitForRiskWorkflowData(Guid applicationId, RiskWorkflowTypes riskWorkflowType)
        {
            Do.Until(
                () =>
                Drive.Db.Risk.RiskWorkflows.Any(
                    p => p.ApplicationId == applicationId && (RiskWorkflowTypes)p.WorkflowType == riskWorkflowType));
        }
    }
}
