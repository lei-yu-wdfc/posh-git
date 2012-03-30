using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.CallReport
{
    public class CheckpointsTests
    {
        private const String GoodCompanyRegNumber = "00000086";

        #region Main Applicant
        
        /* Main Appplicant Is Alive */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        public void TestCallReportUnknownMainApplicant_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTApplicantIsNotDeceased, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id,RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkFlows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(mainApplicantRiskWorkFlows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkFlows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the Kathleen customer that is alive according to call report, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTApplicantIsNotDeceased, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the customer that is dead according to call report, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTApplicantIsNotDeceased, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        /* Main Applicant CIFAS check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("CallReport -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTApplicationElementNotCIFASFlagged,ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("CallReport -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTApplicationElementNotCIFASFlagged,ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        /* Main Applicant Data is Available */

        [Test, AUT(AUT.Wb)]
        [Description("Callreport -> This test creates a loan and checks if the main applicant has data available")]
        public void TestCallReportMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename,surname,Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTCreditBureauDataIsAvailable, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var mainApplicantRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        [Test, AUT(AUT.Wb),]
        [Description("Callreport -> This test creates a loan and checks if the main applicant has data available")]
        public void TestCallReportMainApplicantIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname,Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTCreditBureauDataIsAvailable, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        /* Main Applicant is Insolvent */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTApplicantIsSolvent, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("CallReport -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTApplicantIsSolvent,ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        /* Main applicant DOB is correct */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTCustomerDateOfBirthIsCorrectSME, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTCustomerDateOfBirthIsCorrectSME, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantDateOfBirthNotProvided_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var wrongDateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, wrongDateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTCustomerDateOfBirthIsCorrectSME, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.DateOfBirthIsCorrectVerification));
        }

        #endregion

        #region Guarantor

        /* Guarantor is Alive */
        
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("CallReport -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestCallReportUnknownGuarantor_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTApplicantIsNotDeceased.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTNoCheck,ApplicationDecisionStatus.Accepted,guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() =>Drive.Db.Risk.WorkflowCheckpoints.Any(p=>p.RiskWorkflowId == guarantorRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("CallReport -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsAlive_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTApplicantIsNotDeceased.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() =>Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("CallReport -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTApplicantIsNotDeceased.ToString(),
                                            }
                                    };

            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflows.Count, 1, "There should be 1 risk workflow");
            Do.Until(() =>Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicantIsAlive));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification));
        }

        /* Guarantor CIFAS check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("CallReport -> This test creates a loan for a guarantor that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());
            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTApplicationElementNotCIFASFlagged.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() =>Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("CallReport -> This test creates a loan for a guarantor that is CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone());
            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = RiskMask.TESTApplicationElementNotCIFASFlagged.ToString(),
                                            }
                                    };

            //THIS IS A BIG PROBLEM AS EXPECTED DECISION IS BASED ON CONSUMER LOGIC!!! - I actually send preaccepted and get declined at the end
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(
                () =>
                Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CIFASFraudCheck));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification));
        }

        /* Guarantor Data is available */

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors has data available")]
        public void TestCallReportGuarantorDataIsAvailable_LoanIsApproved()
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
                                                MiddleName = RiskMask.TESTCreditBureauDataIsAvailable.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            Do.Until(() => Drive.Db.Risk.RiskWorkflows.Any(p => p.ApplicationId == application.Id));
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors has data available")]
        public void TestCallReportGuarantorDataIsNotAvailable_LoanIsDeclined()
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
                                                MiddleName = RiskMask.TESTCreditBureauDataIsAvailable.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() =>Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification));
        }

        /* Guarantor is solvent */

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors is solvent")]
        public void TestCallReportGuarantorIsSolvent_LoanIsApproved()
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
                                                MiddleName = RiskMask.TESTApplicantIsSolvent.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.Accepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors is solvent")]
        public void TestCallReportGuarantorIsInsolvent_LoanIsDeclined()
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
                                                MiddleName = RiskMask.TESTApplicantIsSolvent.ToString(),
                                            }
                                    };


            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTNoCheck, ApplicationDecisionStatus.PreAccepted, guarantorList);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.CustomerIsSolvent));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification));
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

        private void WaitForRiskWorkflowData(Guid applicationId , RiskWorkflowTypes riskWorkflowType)
        {
            Do.Until(
                () =>
                Drive.Db.Risk.RiskWorkflows.Any(
                    p => p.ApplicationId == applicationId && (RiskWorkflowTypes) p.WorkflowType == riskWorkflowType));
        }
    }
}
