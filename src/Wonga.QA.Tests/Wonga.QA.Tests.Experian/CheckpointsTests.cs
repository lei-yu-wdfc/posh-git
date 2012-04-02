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
using Wonga.QA.Framework.Db.Risk;
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

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the Kathleen customer that is alive, then checks the risk checkpoint")]
        public void TestExperianMainApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsNotDeceased,ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the customer that is dead, then checks the risk checkpoint")]
        [Pending("Waiting for Risk to fix this issue")]
        public void TestExperianMainApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsNotDeceased,ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
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

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Waiting for Risk to fix this issue")]
        public void TestExperianMainApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicationElementNotCIFASFlagged, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
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

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Wb),]
        [Description("Experian -> This test creates a loan and checks if the main applicant has data available")]
        [Pending("Waiting for Risk to fix this issue")]
        public void TestExperianMainApplicantIsNotAvailable_LoanIsDeclined()
        {
            const String forename = "Unknown";
            const String surname = "Customer";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTExperianCreditBureauDataIsAvailable, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
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

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        [Pending("Waiting for Risk to fix this issue")]
        public void TestExperianMainApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianApplicantIsSolvent, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);

        }

        /* Main applicant DOB check */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestExperianMainApplicantDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var correctDateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, correctDateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianCustomerDateOfBirthIsCorrect, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        [Pending("Waiting for Risk to fix this issue")]
        public void TestExperianMainApplicantDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant,GoodCompanyRegNumber,RiskMask.TESTExperianCustomerDateOfBirthIsCorrect,ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.DateOfBirthIsCorrectVerification);
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                             Get.GetMobilePhone()) {CardNumber = Int64.Parse("4929188001506313")};
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTExperianPaymentCardIsValid, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.ExperianPaymentCardIsValidVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        [Pending("Waiting for Risk to fix this issue")]
        public void TestExperianMainApplicantPaymentCardIsNotValid_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                             Get.GetMobilePhone()) { CardNumber = Int64.Parse("9999888877776666") };
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTExperianPaymentCardIsValid, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.ExperianPaymentCardIsValidVerification);
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

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.ApplicantIsAlive,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsAliveVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1147"), Description("Experian -> This test creates a loan for the unknown guarantor that is alive, then checks the risk checkpoint")]
        [Pending("Waiting for Risk to fix this issue")]
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

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CIFASFraudCheck,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCifasFraudCheckVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1144"), Description("Experian -> This test creates a loan for a guarantor that is  CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Waiting for Risk to fix this issue")]
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

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauDataIsAvailableVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1141")]
        [Description("Experian -> This test creates a loan and checks if the guarantors has data available")]
        [Pending("Waiting for Risk to fix this issue")]
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

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1150")]
        [Description("Experian -> This test creates a loan and checks if the guarantor is solvent")]
        [Pending("Waiting for Risk to fix this issue")]
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

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.CustomerIsSolvent,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CreditBureauCustomerIsSolventVerification);
        }

        #endregion

        private static Application CreateApplicationWithAsserts(Customer mainApplicant, String companyRegisteredNumber, RiskMask middlenameMask, ApplicationDecisionStatus applicationDecision, List<Customer> guarantors = null)
        {
            var customerBuilder = CustomerBuilder.New(mainApplicant.Id);
            customerBuilder.ScrubForename(mainApplicant.Forename);
            customerBuilder.ScrubSurname(mainApplicant.Surname);

            if (mainApplicant.CardNumber != 0)
                customerBuilder.WithPaymentCardNumber(mainApplicant.CardNumber);

            if(!string.IsNullOrEmpty(mainApplicant.MobilePhoneNumber))
                customerBuilder.WithMobileNumber(mainApplicant.MobilePhoneNumber);

            //STEP 1 - Create the main director
            var mainDirector = customerBuilder.WithMiddleName(middlenameMask.ToString()).WithForename(mainApplicant.Forename).WithSurname(mainApplicant.Surname).WithDateOfBirth(mainApplicant.DateOfBirth).Build();

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

    }
}
