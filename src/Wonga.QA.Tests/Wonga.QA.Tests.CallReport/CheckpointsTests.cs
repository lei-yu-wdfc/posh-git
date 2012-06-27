
using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CallReport
{
    [Parallelizable(TestScope.All)]
    [AUT(AUT.Wb,AUT.Uk)]
    public class CheckpointsTests
    {
        private const String GoodCompanyRegNumber = "00000086";

        /********************************************************************************************************
        * This should be used for all consumer for all regions too (UK,CA,etc)                                  *
        * Please check CreateApplicationWithAsserts for splitting the code between Consumer and Business        *
        * Please create all CallReport tests in this solution                                                   *
        * ******************************************************************************************************/

        #region Main Applicant

        /* Main Appplicant Is Alive L0  */

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-575", "UK-853"), Description("CallReport -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        public void TestCallReportUnknownMainApplicant_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsNotDeceased);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-575", "UK-853"), Description("CallReport -> This test creates a loan for the Kathleen customer that is alive according to call report, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsNotDeceased);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-575", "UK-853"), Description("CallReport -> This test creates a loan for the customer that is dead according to call report, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsNotDeceased);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        /* Main Appplicant Is Alive LN  */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-853")]
        public void Ln_TestCallReportMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsNotDeceased);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Accepted);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-853")]
        public void Ln_TestCallReportMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTApplicantIsNotDeceased.ToString());
            //Drive.Db.UpdateEmployerName(mainApplicant.Id, RiskMask.TESTApplicantIsNotDeceased.ToString());

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        /* Main Applicant CIFAS check L0 */

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-584", "UK-852"), Description("CallReport -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicationElementNotCIFASFlagged);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);

        }

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-584", "UK-852"), Description("CallReport -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicationElementNotCIFASFlagged);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        /* Main Applicant CIFAS check LN */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-852")]
        public void Ln_TestCallReportMainApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicationElementNotCIFASFlagged);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Accepted);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-852")]
        public void Ln_TestCallReportMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "Laura";
            const String surname = "Insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTApplicationElementNotCIFASFlagged.ToString());

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        /* Main Applicant Data is Available - L0 */

        [Test, AUT(AUT.Wb, AUT.Uk), JIRA("UK-851")]
        [Description("Callreport -> This test creates a loan and checks if the main applicant has data available")]
        public void TestCallReportMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTCreditBureauDataIsAvailable);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Wb, AUT.Uk), JIRA("UK-851")]
        [Description("Callreport -> This test creates a loan and checks if the main applicant has data available")]
        public void TestCallReportMainApplicantDataIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTCreditBureauDataIsAvailable);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        /* Main Applicant Data is Available - LN */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-851"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint for LN Jurney")]
        public void Ln_TestCallReportMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTCreditBureauDataIsAvailable);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Accepted);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-851"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint for LN Jurney")]
        public void Ln_TestCallReportMainApplicantDataIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTCreditBureauDataIsAvailable.ToString());

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }


        /* Main Applicant is Insolvent L0 */

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-638", "UK-854"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsSolvent);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Wb, AUT.Uk)]
        [JIRA("SME-638", "UK-854"), Description("CallReport -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsSolvent);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);

        }


        /* Main Applicant is Insolvent LN */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-854"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint for LN Jurney")]
        public void Ln_TestCallReportMainApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTApplicantIsSolvent);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Accepted);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-854"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint for LN Jurney")]
        public void Ln_TestCallReportMainApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTApplicantIsSolvent.ToString());

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }


        /* Main applicant DOB is correct */

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-644", "UKRISK-71"), Description("CallReport -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";

            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);

            //Cannot use the CreateCustomerBuilder here because the masks are different
            var mainApplicantBuilder = Config.AUT == AUT.Wb
                                           ? CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(dateOfBirth).WithMiddleName(
                                                     RiskMask.TESTCustomerDateOfBirthIsCorrectSME)
                                           : CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(dateOfBirth).WithEmployer(
                                                     RiskMask.TESTCustomerDateOfBirthIsCorrect);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-644", "UKRISK-71"), Description("CallReport -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);

            //Cannot use the CreateCustomerBuilder here because the masks are different
            var mainApplicantBuilder = Config.AUT == AUT.Wb
                                           ? CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(dateOfBirth).WithMiddleName(
                                                     RiskMask.TESTCustomerDateOfBirthIsCorrectSME)
                                           : CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(dateOfBirth).WithEmployer(
                                                     RiskMask.TESTCustomerDateOfBirthIsCorrect);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);

        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-644", "UKRISK-71"), Description("CallReport -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        public void TestCallReportMainApplicantDateOfBirthNotProvided_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";
            var wrongDateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);

            //Cannot use the CreateCustomerBuilder here because the masks are different
            var mainApplicantBuilder = Config.AUT == AUT.Wb
                                           ? CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(wrongDateOfBirth).WithMiddleName(
                                                     RiskMask.TESTCustomerDateOfBirthIsCorrectSME)
                                           : CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(wrongDateOfBirth).WithEmployer(
                                                     RiskMask.TESTCustomerDateOfBirthIsCorrect);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);

        }

        #endregion

        #region Guarantor SME Specific

        /* Guarantor is Alive */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("CallReport -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestCallReportUnknownGuarantor_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicantIsNotDeceased),
                                    };

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("CallReport -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsAlive_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicantIsNotDeceased),
                                    };
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("CallReport -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicantIsNotDeceased),
                                    };


            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        /* Guarantor CIFAS check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("CallReport -> This test creates a loan for a guarantor that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicationElementNotCIFASFlagged),
                                    };

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);

        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("CallReport -> This test creates a loan for a guarantor that is CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportGuarantorIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicationElementNotCIFASFlagged),
                                    };

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);

        }

        /* Guarantor Data is available */

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors has data available")]
        public void TestCallReportGuarantorDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTCreditBureauDataIsAvailable),
                                    };

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors has data available")]
        public void TestCallReportGuarantorDataIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTCreditBureauDataIsAvailable),
                                    };

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        /* Guarantor is solvent */

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors is solvent")]
        public void TestCallReportGuarantorIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicantIsSolvent),
                                    };
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors is solvent")]
        public void TestCallReportGuarantorIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTApplicantIsSolvent),
                                    };

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        /* Guarantor DOB is correct */

        [Test, AUT(AUT.Wb), JIRA("SME-1138")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors entered the correct DOB")]
        public void TestCallReportGuarantorDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTCustomerDateOfBirthIsCorrectSME).WithDateOfBirth(dateOfBirth),
                                    };
            var mainApplicant = CustomerBuilder.New().Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1138")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors entered the correct DOB")]
        public void TestCallReportGuarantorDateOfBirthNotProvided_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";

            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTCustomerDateOfBirthIsCorrectSME).WithDateOfBirth(dateOfBirth),
                                    };
            var mainApplicant = CustomerBuilder.New().Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1138")]
        [Description("Callreport -> This test creates a loan and checks if the guarantors entered the correct DOB")]
        public void TestCallReportGuarantorDateOfBirthIsInCorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 1, 24), DateFormat.Date);

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTCustomerDateOfBirthIsCorrectSME).WithDateOfBirth(dateOfBirth),
                                    };
            var mainApplicant = CustomerBuilder.New().Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.PreAccepted, guarantorList);

            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
        }

        #endregion

        private static CustomerBuilder CreateCustomerBuilder(String forename, String surname, RiskMask riskMask, Date? dateOfBirth = null, Int64? cardNumber = null)
        {
            if (dateOfBirth == null)
                dateOfBirth = Get.GetDoB();

            return Config.AUT == AUT.Wb
                       ? CustomerBuilder.New().WithForename(forename).WithDateOfBirth((Date)dateOfBirth).WithSurname(surname).
                             WithMiddleName(riskMask)
                       : CustomerBuilder.New().WithForename(forename).WithDateOfBirth((Date)dateOfBirth).WithSurname(surname).WithEmployer(riskMask);
        }

        private static Application CreateL0Application(Customer mainApplicant, ApplicationDecisionStatus applicationDecision, List<CustomerBuilder> guarantors = null)
        {
            return Config.AUT == AUT.Wb ? CreateBusinessL0Application(mainApplicant, guarantors, applicationDecision) : CreateConsumerL0Application(mainApplicant, applicationDecision);
        }
        private static Application CreateLnApplication(Customer mainApplicant, ApplicationDecisionStatus applicationDecision, List<Customer> guarantors = null)
        {
            //STEP 2 - Create the application with code split
            return Config.AUT == AUT.Wb ? CreateBusinessLnApplication(mainApplicant, guarantors, applicationDecision) : CreateConsumerLnApplication(mainApplicant, applicationDecision);
        }
        private static Application CreateConsumerL0Application(Customer customer, ApplicationDecisionStatus applicationDecision)
        {
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(applicationDecision).Build();
            return RunApplicationAsserts(application);
        }
        private static Application CreateConsumerLnApplication(Customer customer, ApplicationDecisionStatus applicationDecision)
        {
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(applicationDecision).Build();
            return RunApplicationAsserts(application);
        }
        private static Application CreateBusinessL0Application(Customer mainDirector, List<CustomerBuilder> guarantors, ApplicationDecisionStatus applicationDecision)
        {
            //STEP2 - Create the company 
            var organisationBuilder = OrganisationBuilder.New(mainDirector).WithOrganisationNumber(GoodCompanyRegNumber);
            var organisation = organisationBuilder.Build();

            //STEP3 - Create the application
            var applicationBuilder = ApplicationBuilder.New(mainDirector, organisation).WithExpectedDecision(applicationDecision) as BusinessApplicationBuilder;

            //STEP4 - Create the guarantors list + send it to the application
            if (guarantors != null)
            {
                applicationBuilder.WithGuarantors(guarantors);

                foreach (var customerBuilder in guarantors)
                {
                    customerBuilder.ScrubForename(customerBuilder.Forename);
                    customerBuilder.ScrubSurname(customerBuilder.Surname);
                }
            }

            //STEP5 - Build the application + send the list of guarantors
            var application = applicationBuilder.Build();

            return RunApplicationAsserts(application);
        }
        private static Application CreateBusinessLnApplication(Customer mainDirector, List<Customer> guarantors, ApplicationDecisionStatus applicationDecision)
        {
            //THIS IS NOT IMPLEMENTED YET

            #region OLD CODE TO REFACTOR AT PROPER TIME

            ////STEP2 - Create the company 
            //var organisationBuilder = OrganisationBuilder.New(mainDirector).WithOrganisationNumber(GoodCompanyRegNumber);
            //var organisation = organisationBuilder.Build();

            ////STEP3 - Create the application
            //var applicationBuilder = ApplicationBuilder.New(mainDirector, organisation).WithExpectedDecision(applicationDecision) as BusinessApplicationBuilder;

            ////STEP4 - Create the guarantors list + send it to the application
            //if (guarantors != null)
            //{
            //    applicationBuilder.WithGuarantors(guarantors);
            //}

            ////STEP5 - Build the application + send the list of guarantors
            //var application = applicationBuilder.Build();

            //return RunApplicationAsserts(application);

            #endregion

            return null;

        }

        private static Application RunApplicationAsserts(Application application)
        {
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

        private List<RiskWorkflowEntity> VerifyRiskWorkflows(Guid applicationId, RiskWorkflowTypes riskWorkflowType, RiskWorkflowStatus expectedRiskWorkflowStatus, Int32 expectedNumberOfWorkflows)
        {
            Drive.Db.WaitForRiskWorkflowData(applicationId, riskWorkflowType, expectedNumberOfWorkflows, expectedRiskWorkflowStatus);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(applicationId, riskWorkflowType);
            Assert.AreEqual(expectedNumberOfWorkflows, riskWorkflows.Count, "There should be " + expectedNumberOfWorkflows + " workflows");

            foreach (var riskWorkflow in riskWorkflows)
            {
                Assert.AreEqual(expectedRiskWorkflowStatus, (RiskWorkflowStatus)riskWorkflow.Decision);
            }

            return riskWorkflows;
        }
        private void VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(RiskWorkflowEntity riskWorkflowEntity, RiskCheckpointDefinitionEnum checkpointDefinition, RiskCheckpointStatus checkpointStatus, RiskVerificationDefinitions riskVerification)
        {
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflowEntity.WorkflowId, checkpointStatus), Get.EnumToString(checkpointDefinition));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflowEntity.WorkflowId), Get.EnumToString(riskVerification));
        }

        private static void ScrubNames(CustomerBuilder customerBuilder)
        {
            customerBuilder.ScrubForename(customerBuilder.Forename);
            customerBuilder.ScrubSurname(customerBuilder.Surname);
        }
    }
}
