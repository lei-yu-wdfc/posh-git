using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                             Get.GetMobilePhone()) { CardNumber = Int64.Parse("4444333322221111") };
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTRiskPaymentCardIsValid, ApplicationDecisionStatus.Accepted);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var mainApplicantRiskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, mainApplicantRiskWorkflows.Count, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.PaymentCardIsValid));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CardPaymentPaymentCardIsValidVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-136")]
        [Ignore]
        public void TestCardPaymentMainApplicantPaymentCardIsNotValid_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(),
                                             Get.GetMobilePhone()) { CardNumber = Int64.Parse("9999888877776666") };
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber, RiskMask.TESTRiskPaymentCardIsValid, ApplicationDecisionStatus.Declined);

            WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant);
            var mainApplicantRiskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(1, mainApplicantRiskWorkflows.Count, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == mainApplicantRiskWorkflows[0].RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.PaymentCardIsValid));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(mainApplicantRiskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.CardPaymentPaymentCardIsValidVerification));
        }

        private static Application CreateApplicationWithAsserts(Customer mainApplicant, String companyRegisteredNumber, RiskMask middlenameMask, ApplicationDecisionStatus applicationDecision, List<Customer> guarantors = null)
        {
            var customerBuilder = CustomerBuilder.New(mainApplicant.Id);
            customerBuilder.ScrubForename(mainApplicant.Forename);
            customerBuilder.ScrubSurname(mainApplicant.Surname);

            if (!string.IsNullOrEmpty(mainApplicant.CardNumber.ToString()))
                customerBuilder.WithPaymentCardNumber(mainApplicant.CardNumber);

            if (!string.IsNullOrEmpty(mainApplicant.MobilePhoneNumber))
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

        private void WaitForRiskWorkflowData(Guid applicationId, RiskWorkflowTypes riskWorkflowType)
        {
            Do.Until(
                () =>
                Drive.Db.Risk.RiskWorkflows.Any(
                    p => p.ApplicationId == applicationId && (RiskWorkflowTypes)p.WorkflowType == riskWorkflowType));
        }
    }
}
