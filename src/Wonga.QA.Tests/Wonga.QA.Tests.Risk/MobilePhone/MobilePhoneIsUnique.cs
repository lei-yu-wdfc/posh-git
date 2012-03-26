using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Tests.Risk.MobilePhone
{
    public class MobilePhoneIsUnique
    {
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1107"), Description("This will test if the main directors phone is not in our system")]
        public void IfMobilePhoneIsUnique_LoanIsAccepted()
        {
            const String goodCompanyRegNumber = "00000086";
            const String mobilePhoneNumber = "07771269574";

            //Clean the mobile number from DB 
            var riskDb = Drive.Db.Risk;
            var entities = riskDb.RiskAccountMobilePhones.Where(p => p.MobilePhone == mobilePhoneNumber).ToList();
            if (entities.Count > 0)
            {
                riskDb.RiskAccountMobilePhones.DeleteAllOnSubmit(entities);
                riskDb.SubmitChanges();
            }

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(),mobilePhoneNumber);
            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMobilePhoneIsUnique,
                                                           ApplicationDecisionStatus.Accepted);
            var mainApplicantRiskWorkflows = Application.GetWorkflowsForApplication(application.Id,
                                                                                    RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");

            var workflowCheckpointsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
            var workflowVerificationsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

            Assert.IsNotNull(workflowVerificationsWithDefinitions, "There should be verifications in the workflow");
            Assert.IsNotNull(workflowCheckpointsWithDefinitions, "There should be checkpoints in the workflow");

            Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.MobilePhoneIsUniqueVerification));
            Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.MobilePhoneIsUnique));

            foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
            {
                Assert.AreEqual(RiskCheckpointStatus.Verified, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1107"), Description("This will test if the main directors phone is not in our system")]
        public void IfMobilePhoneNotUnique_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String mobilePhoneNumber = "07771269574";
            var tempId = Guid.NewGuid();
            var riskDb = Drive.Db.Risk;


            //Add the mobile number to Risk DB 
            var riskAccountEntity = new RiskAccountEntity()
                                        {
                                            AccountId = tempId,
                                            AccountRank = 1,
                                            HasAccount = true,
                                            CreditLimit = 400,
                                            ConfirmedFraud = false,
                                            DateOfBirth = Get.GetDoB(),
                                            DoNotRelend = false,
                                            Forename = Get.RandomString(8),
                                            IsDebtSale = false,
                                            IsDispute = false,
                                            IsHardship = false,
                                            Postcode = "KT2 5DL",
                                            MaidenName = Get.RandomString(8),
                                            Middlename = Get.RandomString(8),
                                            Surname = Get.RandomString(8)
                                        };
            var riskAccoutnMobilePhoneEntity = new RiskAccountMobilePhoneEntity()
            {
                AccountId = tempId,
                DateUpdated = new DateTime(2010,10,06).ToDate(),
                MobilePhone = mobilePhoneNumber,
            };

            riskDb.RiskAccounts.InsertOnSubmit(riskAccountEntity);
            riskDb.RiskAccountMobilePhones.InsertOnSubmit(riskAccoutnMobilePhoneEntity);
            riskDb.SubmitChanges();

            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), mobilePhoneNumber);
            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMobilePhoneIsUnique,
                                                           ApplicationDecisionStatus.Declined);
            var mainApplicantRiskWorkflows = Application.GetWorkflowsForApplication(application.Id,
                                                                                    RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(mainApplicantRiskWorkflows.Count, 1, "There should be 1 risk workflow");

            var workflowCheckpointsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
            var workflowVerificationsWithDefinitions = mainApplicantRiskWorkflows[0].WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

            Assert.IsNotNull(workflowVerificationsWithDefinitions, "There should be verifications in the workflow");
            Assert.IsNotNull(workflowCheckpointsWithDefinitions, "There should be checkpoints in the workflow");

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
                    foreach (var guarantor in guarantors)
                    {
                        var guarantorCustomerBuilder = CustomerBuilder.New(guarantor.Id);
                        guarantorCustomerBuilder.ScrubForename(guarantor.Forename);
                        guarantorCustomerBuilder.ScrubSurname(guarantor.Surname);

                        guarantorCustomerBuilder.WithEmailAddress(guarantor.Email).WithForename(guarantor.Forename)
                            .WithSurname(guarantor.Surname).WithDateOfBirth(guarantor.DateOfBirth).WithMobileNumber(guarantor.MobilePhoneNumber).Build();
                    }
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
