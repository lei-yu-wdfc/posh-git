using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CardPayment
{
    public class CheckpointsTests
    {
        private const String GoodCompanyRegNumber = "00000086";

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        public void TestCardPaymentMainApplicantPaymentCardIsValid_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicantBuilder =
                CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(
                    Int64.Parse("4444333322221111")).WithMiddleName(RiskMask.TESTRiskPaymentCardIsValid);
            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var mainApplicantRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, mainApplicantRiskWorkflows.Count, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.PaymentCardIsValid));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CardPaymentPaymentCardIsValidVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        [Ignore]
        public void TestCardPaymentMainApplicantPaymentCardIsNotValid_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var mainApplicantBuilder =
               CustomerBuilder.New().WithForename(forename).WithSurname(surname).WithPaymentCardNumber(
                   Int64.Parse("9999888877776666")).WithMiddleName(RiskMask.TESTRiskPaymentCardIsValid);

            var application = CreateApplicationWithAsserts(mainApplicantBuilder, GoodCompanyRegNumber, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var mainApplicantRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, mainApplicantRiskWorkflows.Count, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.PaymentCardIsValid));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CardPaymentPaymentCardIsValidVerification));
        }

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
        private void WaitForRiskWorkflowData(Guid applicationId, RiskWorkflowTypes riskWorkflowType)
        {
            Do.Until(
                () =>
                Drive.Db.Risk.RiskWorkflows.Any(
                    p => p.ApplicationId == applicationId && (RiskWorkflowTypes)p.WorkflowType == riskWorkflowType));
        }
    }
}
