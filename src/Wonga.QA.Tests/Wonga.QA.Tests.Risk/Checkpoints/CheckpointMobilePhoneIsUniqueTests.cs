using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Risk.MobilePhone
{
    public class CheckpointMobilePhoneIsUniqueTests
    {
        private const String GoodCompanyRegNumber = "00000086";
        private const String MobilePhoneNumber = "07771269999";

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1107"), Description("This will test if the main directors phone is not in our system")]
        public void IfMobilePhoneIsUniqueForMainApplicant_LoanIsAccepted()
        {
            var db = Drive.Db;
            db.RemovePhoneNumberFromRiskDb(MobilePhoneNumber);

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(),MobilePhoneNumber);
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber,
                                                           RiskMask.TESTMobilePhoneIsUnique,
                                                           ApplicationDecisionStatus.Accepted);
            Drive.Db.WaitForRiskWorkflowData(application.Id,RiskWorkflowTypes.MainApplicant,1,RiskWorkflowStatus.Verified);
            var mainApplicantRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id,RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");

            var workflowCheckpointsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
            var workflowVerificationsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

            Assert.IsNotNull(workflowVerificationsWithDefinitions.Count, "There should be verifications in the workflow");
            Assert.IsNotNull(workflowCheckpointsWithDefinitions.Count, "There should be checkpoints in the workflow");

            Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.MobilePhoneIsUniqueVerification));
            Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.MobilePhoneIsUnique));

            foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
            {
                Assert.AreEqual(RiskCheckpointStatus.Verified, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1107"), Description("This will test if the main directors phone is not in our system")]
        public void IfMobilePhoneNotUniqueForMainApplicant_LoanIsDeclined()
        {
            var db = Drive.Db;
            db.AddPhoneNumberToRiskDb(MobilePhoneNumber);

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), MobilePhoneNumber);
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber,
                                                           RiskMask.TESTMobilePhoneIsUnique,
                                                           ApplicationDecisionStatus.Declined);
            Drive.Db.WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.MainApplicant, 1, RiskWorkflowStatus.Failed);
            var mainApplicantRiskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);

            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");

            var workflowCheckpointsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
            var workflowVerificationsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

            Assert.IsNotNull(workflowVerificationsWithDefinitions.Count, "There should be verifications in the workflow");
            Assert.IsNotNull(workflowCheckpointsWithDefinitions.Count, "There should be checkpoints in the workflow");

            Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.MobilePhoneIsUniqueVerification));
            Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.MobilePhoneIsUnique));

            foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
            {
                Assert.AreEqual(RiskCheckpointStatus.Failed, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1168"), Description("This will test if the guarantor phone is not in our system")]
        public void IfMobilePhoneIsUniqueForGuarantor_LoanIsAccepted()
        {
            var listOfGuarantors = new List<Customer>();
            var db = Drive.Db;
            db.RemovePhoneNumberFromRiskDb(MobilePhoneNumber);
           
            var guarantor = new Customer(Guid.NewGuid(), Get.RandomEmail(), "Roger John", "Clarke",
                                         Get.GetDoB(), MobilePhoneNumber)
                                {MiddleName = RiskMask.TESTMobilePhoneIsUnique.ToString()};
            listOfGuarantors.Add(guarantor);

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber,
                                                           RiskMask.TESTNoCheck,
                                                           ApplicationDecisionStatus.Accepted,listOfGuarantors);

            Drive.Db.WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor, 1, RiskWorkflowStatus.Verified);
            var guarantorsWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id,
                                                                                    RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorsWorkflow.Count, listOfGuarantors.Count, "There should be "+listOfGuarantors.Count+" risk workflow");

            var workflowCheckpointsWithDefinitions = guarantorsWorkflow[0].WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
            var workflowVerificationsWithDefinitions = guarantorsWorkflow[0].WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

            Assert.IsNotNull(workflowVerificationsWithDefinitions.Count, "There should be verifications in the workflow");
            Assert.IsNotNull(workflowCheckpointsWithDefinitions.Count, "There should be checkpoints in the workflow");

            Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.MobilePhoneIsUniqueVerification));
            Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.MobilePhoneIsUnique));

            foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
            {
                Assert.AreEqual(RiskCheckpointStatus.Verified, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1168"), Description("This will test if the guarantor phone is not in our system")]
        public void IfMobilePhoneIsNotUniqueForGuarantor_LoanIsDeclined()
        {
            var listOfGuarantors = new List<Customer>();
            var db = Drive.Db;
            db.AddPhoneNumberToRiskDb(MobilePhoneNumber);
            
            var guarantor = new Customer(Guid.NewGuid(), Get.RandomEmail(), "Roger John", "Clarke",
                                         Get.GetDoB(), MobilePhoneNumber) { MiddleName = RiskMask.TESTMobilePhoneIsUnique.ToString() };
            listOfGuarantors.Add(guarantor);

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());
            var application = CreateApplicationWithAsserts(mainApplicant, GoodCompanyRegNumber,
                                                           RiskMask.TESTNoCheck,
                                                           ApplicationDecisionStatus.PreAccepted, listOfGuarantors);

            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = application.Id }).Values["ApplicationDecisionStatus"].Single()) == ApplicationDecisionStatus.Declined);
            Drive.Db.WaitForRiskWorkflowData(application.Id, RiskWorkflowTypes.Guarantor, 1, RiskWorkflowStatus.Failed);
            var guarantorsWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id,
                                                                                    RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorsWorkflow.Count, listOfGuarantors.Count, "There should be " + listOfGuarantors.Count + " risk workflow");

            var workflowCheckpointsWithDefinitions = guarantorsWorkflow[0].WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
            var workflowVerificationsWithDefinitions = guarantorsWorkflow[0].WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

            Assert.IsNotNull(workflowVerificationsWithDefinitions.Count, "There should be verifications in the workflow");
            Assert.IsNotNull(workflowCheckpointsWithDefinitions.Count, "There should be checkpoints in the workflow");

            Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.MobilePhoneIsUniqueVerification));
            Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.MobilePhoneIsUnique));

            foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
            {
                Assert.AreEqual(RiskCheckpointStatus.Failed, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
            }
        }

        private static Application CreateApplicationWithAsserts(Customer mainApplicant, String companyRegisteredNumber, RiskMask middlenameMask, ApplicationDecisionStatus applicationDecision, List<Customer> guarantors = null)
        {
            var customerBuilder = CustomerBuilder.New(mainApplicant.Id);
            customerBuilder.ScrubForename(mainApplicant.Forename);
            customerBuilder.ScrubSurname(mainApplicant.Surname);
            //STEP 1 - Create the main director
            var mainDirector = customerBuilder.WithMiddleName(middlenameMask).WithForename(mainApplicant.Forename).WithSurname(mainApplicant.Surname).WithDateOfBirth(mainApplicant.DateOfBirth).WithMobileNumber(mainApplicant.MobilePhoneNumber).Build();

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
       
    }
}
