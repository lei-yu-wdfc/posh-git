using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Experian
{
    [Parallelizable(TestScope.All)]
    public class ExperianRiskCheckpointTests
    {
        private const String GoodCompanyRegNumber = "00000086";

        #region MainApplicant

        /*********************************************************************************************************
         * This region should be used for all consumer for all regions (UK,CA,etc)                               *
         * Please check CreateApplicationWithAsserts for splitting the code between Consumer and Business        *
         * Please create all Experian tests in this solution                                                     *
         * ******************************************************************************************************/

        /* Main Applicant Solvent L0 */

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-638", "UK-854"), Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestExperianMainCustomerIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianCustomerIsSolvent);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            //STEP 2 - We create the application - here is where the code is split
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            /*STEP 3 - We verify the Risk Workflow + Checkpoints + Definitions
             * Please use the RiskWorkflowTypes enum */

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        /* Main Applicant Solvent LN */

        [Test, AUT(AUT.Uk)]
        [JIRA( "UK-854"), Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void Ln_TestExperianMainApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianCustomerIsSolvent);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            //STEP 2 - We create the application - here is where the code is split
            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();

            var lnApplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Accepted);

            /*STEP 3 - We verify the Risk Workflow + Checkpoints + Definitions
             * Please use the RiskWorkflowTypes enum */

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnApplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-854"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void Ln_TestExperianMainApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "Insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTExperianCustomerIsSolvent.ToString());
            var lnApplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnApplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        /* Main Applicant is Alive L0 */

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-575","UK-853"), Description("Experian -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        public void TestExperianUnknownMainApplicant_LoanIsApproved()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicantIsNotDeceased);
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
        [JIRA("SME-575","UK-853"), Description("Experian -> This test creates a loan for the Kathleen customer that is alive, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicantIsNotDeceased);
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
        [JIRA("SME-575","UK-853"), Description("Experian -> This test creates a loan for the customer that is dead, then checks the risk checkpoint")]
        [Pending("Experian still not working")]
        public void TestExperianMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            //const String surname = "DeceasedFlagDetected";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicantIsNotDeceased);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        /* Main Applicant is Alive LN */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-853")]
        public void Ln_ExperianMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicantIsNotDeceased);
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
        public void Ln_ExperianMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTExperianApplicantIsNotDeceased.ToString());

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        /* Main Applicant CIFAS L0 */

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicationElementNotCIFASFlagged);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Experian still not working")]
        public void TestExperianMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "Insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicationElementNotCIFASFlagged);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        /* Main Applicant CIFAS LN */

		[Test, AUT(AUT.Uk)]
        [JIRA("UK-852")]
        public void Ln_ExperianMainApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "Bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianApplicationElementNotCIFASFlagged);
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
        [JIRA("UK-852"),
		Category(TestCategories.CoreTest)]
        public void Ln_ExperianMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "Laura";
            const String surname = "Insolvent";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTExperianApplicationElementNotCIFASFlagged.ToString());

            var lnAplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnAplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        /* Main Applicant Data is Available L0 */

        [Test, AUT(AUT.Wb,AUT.Uk), JIRA("UK-851")]
        [Description("Experian -> This test creates a loan and checks if the main applicant has data available")]
        public void TestExperianMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String forename = "john";
            const String surname = "konor";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianCreditBureauDataIsAvailable);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Wb,AUT.Uk), JIRA("UK-851")]
        [Description("Experian -> This test creates a loan and checks if the main applicant has data available")]
        [Pending("Experian not working")]
        public void TestExperianMainApplicantIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "John";
            const String surname = "InsufficientBureauData";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianCreditBureauDataIsAvailable);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        /* Main Applicant Data is Available LN */

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-851"), Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void Ln_TestExperianMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String forename = "john";
            const String surname = "konor";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianCreditBureauDataIsAvailable);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            //STEP 2 - We create the application - here is where the code is split
            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();

            var lnApplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Accepted);

            /*STEP 3 - We verify the Risk Workflow + Checkpoints + Definitions
             * Please use the RiskWorkflowTypes enum */

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnApplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-851"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void Ln_TestExperianMainApplicantDataIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTEmployedMask);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var l0Application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
            l0Application.RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(mainApplicant.Id, RiskMask.TESTExperianCreditBureauDataIsAvailable.ToString());

            var lnApplication = CreateLnApplication(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(lnApplication.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

		[Test, AUT(AUT.Wb, AUT.Uk)]
		[JIRA("SME-638"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
		[Pending("Experian still not working")]
		public void TestExperianMainApplicantIsInsolvent_LoanIsDeclined()
		{
			const String forename = "laura";
			const String surname = "Insolvent";

			var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianCustomerIsSolvent);
			ScrubNames(mainApplicantBuilder);
			var mainApplicant = mainApplicantBuilder.Build();

			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);
			var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
																	 RiskCheckpointDefinitionEnum.CustomerIsSolvent,
																	 RiskCheckpointStatus.Failed,
																	 RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
		}

        /* Main applicant DOB check */

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-644", "UKRISK-71"), Description("Experian -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var correctDateOfBirth = new Date(new DateTime(1992, 01, 24), DateFormat.Date);

            //Cannot use the CreateCustomerBuilder here because the masks are different
            var mainApplicantBuilder = Config.AUT == AUT.Wb
                                           ? CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(correctDateOfBirth).WithMiddleName(
                                                     RiskMask.TESTExperianCustomerDateOfBirthIsCorrectSME)
                                           : CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(correctDateOfBirth).WithEmployer(
                                                     RiskMask.TESTExperianCustomerDateOfBirthIsCorrect);

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
        [JIRA("SME-644", "UKRISK-71"), Description("Experian -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);

            var mainApplicantBuilder = Config.AUT == AUT.Wb
                                           ? CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(dateOfBirth).WithMiddleName(
                                                     RiskMask.TESTExperianCustomerDateOfBirthIsCorrectSME)
                                           : CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                                 WithDateOfBirth(dateOfBirth).WithEmployer(
                                                     RiskMask.TESTExperianCustomerDateOfBirthIsCorrect);
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
        [JIRA("SME-644", "UKRISK-71"), Description("Experian -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsNotProvided_LoanIsApproved()
        {
            const String forename = "john";
            const String surname = "DOBIsNotProvided";
            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);

            var mainApplicantBuilder = Config.AUT == AUT.Wb
                               ? CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                     WithDateOfBirth(dateOfBirth).WithMiddleName(
                                         RiskMask.TESTExperianCustomerDateOfBirthIsCorrectSME)
                               : CustomerBuilder.New().WithForename(forename).WithSurname(surname).
                                     WithDateOfBirth(dateOfBirth).WithEmployer(
                                         RiskMask.TESTExperianCustomerDateOfBirthIsCorrect);
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
        }

        /* Main Applicant Payment Card Is Valid */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        public void TestExperianMainApplicantPaymentCardIsValid_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianPaymentCardIsValid, null, Int64.Parse("4929188001506313"));
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();

            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.ExperianPaymentCardIsValidVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        public void TestExperianMainApplicantPaymentCardIsNotValid_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CreateCustomerBuilder(forename, surname, RiskMask.TESTExperianPaymentCardIsValid, null, Int64.Parse("9999888877776666"));
            ScrubNames(mainApplicantBuilder);
            var mainApplicant = mainApplicantBuilder.Build();
            var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.ExperianPaymentCardIsValidVerification);
        }

		/* Main Applicant Bank Account Match */

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-137", "SME-1359")]
		public void TestExperianMainApplicantBankAccountMatches_LoanIsApproved()
		{
			const String forename = "Ashely";
			const String surname = "Marma";

			var mainApplicantBuilder = CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("34012583", "070116").WithMiddleName(RiskMask.TESTExperianBankAccountMatchedToApplicant);
			ScrubNames(mainApplicantBuilder);
			var mainApplicant = mainApplicantBuilder.Build();
			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted);
			
			var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
																	 RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
																	 RiskCheckpointStatus.Verified,
																	 RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
		}

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-137", "SME-1359")]
		public void TestExperianMainApplicantBankAccountDoesNotMatch_LoanIsDeclined()
		{
			const String forename = "Ashely";
			const String surname = "Marma";

			var mainApplicantBuilder = CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("12345112", "074456").WithMiddleName(RiskMask.TESTExperianBankAccountMatchedToApplicant);
			ScrubNames(mainApplicantBuilder);
			var mainApplicant = mainApplicantBuilder.Build();
			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Declined);
			
			var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
																	 RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
																	 RiskCheckpointStatus.Failed,
																	 RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
		}

        #endregion

        #region Guarantor (SME Specific)

        /* Guarantor Is Alive */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestExperianUnknownGuarantor_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianApplicantIsNotDeceased),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        public void TestExperianGuarantorIsAlive_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianApplicantIsNotDeceased),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        [Pending("Experian still not working")]
        public void TestExperianGuarantorIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianApplicantIsNotDeceased),
                                    };


            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        /* Guarantor CIFAS check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("Experian -> This test creates a loan for a guarantor that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestExperianGuarantorIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianApplicationElementNotCIFASFlagged),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("Experian -> This test creates a loan for a guarantor that is  CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Experian still not working")]
        public void TestExperianGuarantorIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "Insolvent";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianApplicationElementNotCIFASFlagged),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);

        }

        /* Guarantor Data is available */

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Experian -> This test creates a loan and checks if the guarantors has data available")]
        public void TestExperianGuarantorDataIsAvailable_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCreditBureauDataIsAvailable),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Experian -> This test creates a loan and checks if the guarantors has data available")]
        [Pending("Experian still not working")]
        public void TestExperianGuarantorDataIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";

            var mainApplicant = CustomerBuilder.New().Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCreditBureauDataIsAvailable),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        /* Guarantor is solvent */

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Experian -> This test creates a loan and checks if the guarantor is solvent")]
        public void TestExperianGuarantorIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCustomerIsSolvent),
                                    };

            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Accepted, guarantorList);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Experian -> This test creates a loan and checks if the guarantor is solvent")]
        [Pending("Experian still not working")]
        public void TestExperianGuarantorIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "Insolvent";

            var mainApplicant = CustomerBuilder.New().Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCustomerIsSolvent),
                                    };


            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.PreAccepted, guarantorList);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        /* Payment card is valid */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1155")]
        public void TestExperianGuarantorPaymentCardIsValid_LoanIsApproved()
        {
            const String forename = "Ashely";
            const String surname = "Marma";
            var paymentCardNumber = Int64.Parse("4929188001506313");

            var mainApplicant = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(paymentCardNumber).WithMiddleName(RiskMask.TESTExperianPaymentCardIsValid),
                                       };


            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.Accepted, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.ExperianPaymentCardIsValidVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1155")]
        public void TestExperianGuarantorPaymentCardIsInValid_LoanIsDeclined()
        {
            const String forename = "Ashely";
            const String surname = "Marma";
            var paymentCardNumber = Int64.Parse("9999888877776666");

            var mainApplicant = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(paymentCardNumber).WithMiddleName(RiskMask.TESTExperianPaymentCardIsValid),
                                       };


            var application = CreateL0Application(mainApplicant,  ApplicationDecisionStatus.PreAccepted, listOfGuarantors);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.ExperianPaymentCardIsValidVerification);
        }

        /* Bank account is valid */

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-1155")]
		public void TestExperianGuarantorBankAccountMatch_LoanIsApproved()
		{
			const String forename = "Ashely";
			const String surname = "Marma";
			
			var mainApplicant = CustomerBuilder.New().Build();
			var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("34012583", "070116").WithMiddleName(RiskMask.TESTExperianBankAccountMatchedToApplicant),
                                       };


			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, listOfGuarantors);
			Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
			
			var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
			var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
																	 RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
																	 RiskCheckpointStatus.Verified,
																	 RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
		}

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-1155")]
		public void TestExperianGuarantorBankAccountDoesNotMatch_LoanIsDeclined()
		{
			const String forename = "Ashely";
			const String surname = "Marma";

			var mainApplicant = CustomerBuilder.New().Build();
			var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("12345112", "074456").WithMiddleName(RiskMask.TESTExperianBankAccountMatchedToApplicant),
                                       };


			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.PreAccepted, listOfGuarantors);
			Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
			
			var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
			var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
																	 RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
																	 RiskCheckpointStatus.Failed,
																	 RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
		}

		/* DOB is corect */

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-1138"), Description("Experian -> This test creates a loan for a guarantor with the correct date of birth, then checks the risk checkpoint")]
		public void TestExperianGuarantorDateOfBirthIsCorrect_LoanIsApproved()
		{
			const String forename = "kathleen";
			const String surname = "bridson";
			var correctDateOfBirth = new Date(new DateTime(1988, 10, 22), DateFormat.Date);

			var mainApplicant = CustomerBuilder.New().Build();
			var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCustomerDateOfBirthIsCorrectSME).WithDateOfBirth(correctDateOfBirth),
                                    };

			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);
			Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
			
			var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
																	 RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
																	 RiskCheckpointStatus.Verified,
																	 RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
		}

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-1138"), Description("Experian -> This test creates a loan for a guarantor with the correct date of birth, then checks the risk checkpoint")]
		public void TestExperianGuarantorDateOfBirthNotProvided_LoanIsApproved()
		{
			const String forename = "unknown";
			const String surname = "customer";
			var correctDateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);

			var mainApplicant = CustomerBuilder.New().Build();
			var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCustomerDateOfBirthIsCorrectSME).WithDateOfBirth(correctDateOfBirth),
                                    };

			var application = CreateL0Application(mainApplicant, ApplicationDecisionStatus.Accepted, guarantorList);
			Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
			
			var guarantorWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

			VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorWorkflows[0],
																	 RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
																	 RiskCheckpointStatus.Verified,
																	 RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
		}

		[Test, AUT(AUT.Wb)]
		[JIRA("SME-1138"), Description("Experian -> This test creates a loan for a guarantor with the correct date of birth, then checks the risk checkpoint")]
		[Pending("Experian not working yet!")]
		public void TestExperianGuarantorDateOfBirthIsIncorrect_LoanIsDeclined()
		{
			const String forename = "kathleen";
			const String surname = "bridson";
			var correctDateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);

			var mainApplicant = CustomerBuilder.New().Build();
			var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithMiddleName(RiskMask.TESTExperianCustomerDateOfBirthIsCorrectSME).WithDateOfBirth(correctDateOfBirth),
                                    };

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
