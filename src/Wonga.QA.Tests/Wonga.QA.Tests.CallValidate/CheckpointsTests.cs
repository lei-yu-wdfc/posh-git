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
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CallValidate
{
    [Parallelizable(TestScope.All)]
    public class CheckpointsTests
    {
        private const String GoodCompanyRegNumber = "00000086";

        #region Main Applicant

        /* Main applicant payment card is valid */

        //TODO : Merge these tests

        [Test, AUT(AUT.Uk), JIRA("UK-869")]
        public void L0Applicant_WhenPaymentCardNotValidated_LoanIsDeclined()
        {
            var customer = CustomerBuilder
                .New()
                .WithPaymentCardNumber(Int64.Parse("9999888877776666"))
                .WithEmployer(RiskMask.TESTCallValidatePaymentCardIsValid)
                .Build();

            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-869")]
        public void L0Applicant_WhenPaymentCardValidated_LoanIsApproved()
        {
            var customer = CustomerBuilder
                .New()
                .WithPaymentCardNumber(Int64.Parse("1111222233334444"))
                .WithEmployer(RiskMask.TESTCallValidatePaymentCardIsValid)
                .Build();

            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }



        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        public void TestCallValidateMainApplicantPaymentCardIsValid_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder =
                CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(
                    Int64.Parse("1111222233334444")).WithMiddleName(RiskMask.TESTCallValidatePaymentCardIsValid);


            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CallValidatePaymentCardIsValidVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        public void TestCallValidateMainApplicantPaymentCardIsInvalid_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder =
                CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(
                    Int64.Parse("9999888877776666")).WithMiddleName(RiskMask.TESTCallValidatePaymentCardIsValid);

            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CallValidatePaymentCardIsValidVerification);
        }

        /*Main applicant bank account is valid */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-137")]
        public void TestCallValidateMainApplicantBankAccountMatch_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("00000190", "180002").WithMiddleName(RiskMask.TESTCallValidateBankAccountMatchedToApplicant);
            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-137")]
        public void TestCallValidateMainApplicantBankAccountDoesNotMatch_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder = CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("33069079", "204422").WithMiddleName(RiskMask.TESTCallValidateBankAccountMatchedToApplicant);
            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(mainApplicantRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
        }

        #endregion

        #region Guarantors

        /* Guarantor Payment card is valid */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1155")]
        public void TestCallValidateGuarantorPaymentCardIsValid_LoanIsApproved()
        {
            const String forename = "Ashely";
            const String surname = "Marma";
            var paymentCardNumber = Int64.Parse("1111222233334444");

            var mainApplicantBuilder = CustomerBuilder.New();
            var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(paymentCardNumber).WithMiddleName(RiskMask.TESTCallValidatePaymentCardIsValid),
                                       };


            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Accepted, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CallValidatePaymentCardIsValidVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1155")]
        public void TestCallValidateGuarantorPaymentCardIsInvalid_LoanIsDeclined()
        {
            const String forename = "Ashely";
            const String surname = "Marma";
            var paymentCardNumber = Int64.Parse("9999888877776666");

            var mainApplicantBuilder = CustomerBuilder.New();
            var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(paymentCardNumber).WithMiddleName(RiskMask.TESTCallValidatePaymentCardIsValid),
                                       };


            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.PreAccepted, listOfGuarantors);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.PaymentCardIsValid,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CallValidatePaymentCardIsValidVerification);
        }

        /* Guarantor Bank Account is valid */

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1157")]
        public void TestCallValidateGuarantorBankAccountMatch_LoanIsApproved()
        {
            const String forename = "Ashely";
            const String surname = "Marma";

            var mainApplicantBuilder = CustomerBuilder.New();
            var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("00000190", "180002").WithMiddleName(RiskMask.TESTCallValidateBankAccountMatchedToApplicant),
                                       };


            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Accepted, listOfGuarantors);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1157")]
        public void TestCallValidateGuarantorBankAccountDoesNotMatch_LoanIsDeclined()
        {
            const String forename = "Ashely";
            const String surname = "Marma";

            var mainApplicantBuilder = CustomerBuilder.New();
            var listOfGuarantors = new List<CustomerBuilder>
                                       {
                                           CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithBankAccountNumber("33069079", "204422").WithMiddleName(RiskMask.TESTCallValidateBankAccountMatchedToApplicant),
                                       };


            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.PreAccepted, listOfGuarantors);
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);

            var guarantorRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(guarantorRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.CallValidateAndExperianBankAccountVerification);
        }

        #endregion

        private static Application CreateApplicationWithAsserts(CustomerBuilder mainApplicantBuilder, String companyRegisteredNumber, ApplicationDecisionStatus applicationDecision, List<CustomerBuilder> guarantors = null)
        {
            mainApplicantBuilder.ScrubForename(mainApplicantBuilder.Forename);
            mainApplicantBuilder.ScrubSurname(mainApplicantBuilder.Surname);

            //STEP 1 - Create the main director
            var mainDirector = mainApplicantBuilder.Build();

            //STEP2 - Create the company
            var organisationBuilder = OrganisationBuilder.New(mainDirector).WithOrganisationNumber(companyRegisteredNumber);
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
