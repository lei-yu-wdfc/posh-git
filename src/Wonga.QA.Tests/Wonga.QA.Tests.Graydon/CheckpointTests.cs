using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Graydon
{
    [Parallelizable(TestScope.All)]
    public class CheckpointTests
    {
        #region Main Applicant Matches Business Bureau Data

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director")]
        public void TestGraydonDirectorMatchesApplicant_LoanIsAccepted()
        {
            /*This consts need to be externalized. I know how to do it but dont have the time.
             * All your base are belong to us*/

            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - DOB is incorrect")]
        public void TestGraydonDirectorDoesNotMatchApplicantDob_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1977, 12, 21), DateFormat.Date); //wrong DOB

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - Organisation without a director")]
        public void TestGraydonDirectorDoesNotMatchApplicant_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "Kathleen";
            const String surname = "Bridson";
            var dateOfBirth = Get.GetDoB();

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count,"There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - Organisation director is secretary")]
        public void TestGraydonDirectorIsSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "ROBERT";
            const String surname = "COOMBE";
            var dateOfBirth = new Date(new DateTime(1960, 01, 09), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification));
        }

        #endregion

        #region Business Bureau Data Is Available

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        [Description("Graydon -> This test creates a loan and checks if the company has graydon data available")]
        public void TestGraydonBusinessBureauDataIsAvailable_LoanIsApproved()
        {
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessBureauDataIsAvailable, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.BusinessDataAvailableInGraydonVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        [Description("Graydon -> This test creates a loan and checks if the company has graydon data for the negative case - Organisation is unknown to Graydon")]
        public void TestGraydonBusinessBureauDataIsUnavailable_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "99999999";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessBureauDataIsAvailable, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.BusinessDataAvailableInGraydonVerification));
        }
        
        #endregion

        #region Business is Trading 

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        [Description("Graydon -> This test creates a loan and checks if the company is trading")]
        public void TestGraydonBusinessIsTrading_LoanIsApproved()
        {
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessIsCurrentlyTrading, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessIsCurrentlyTrading));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonBusinessIsTradingVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        [Description("Graydon -> This test creates a loan and checks if the company is trading for the negative case")]
        public void TestGraydonBusinessIsNotTrading_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "90000001";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessIsCurrentlyTrading, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessIsCurrentlyTrading));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonBusinessIsTradingVerification));
        }

        #endregion

        #region Business Payment Score is Acceptable

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys payment score is acceptable")]
        public void TestGraydonBusinessPaymentScoreIsAcceptable_LoanIsApproved()
        {
            //good score = 60
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessPaymentScoreIsAcceptable, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonPaymentScoreVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys payment score is acceptable for the negative case - Payment score is low")]
        public void TestGraydonBusinessPaymentScoreIsLow_LoanIsDeclined()
        {
            //bad score = 59
            const String goodCompanyRegNumber = "99999903";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessPaymentScoreIsAcceptable, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonPaymentScoreVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys with no payment score")]
        public void TestGraydonBusinessNoPaymentScore_LoanIsApproved()
        {
            //bad score = 59
            const String goodCompanyRegNumber = "99999904";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessPaymentScoreIsAcceptable, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonPaymentScoreVerification));
        }

        #endregion

        #region Business Performance Score Is Acceptable

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        [Description("Graydon -> This test creates a loan and checks if the companys performance score is acceptable")]
        public void TestGraydonBusinessPerformanceScoreIsAcceptable_LoanIsApproved()
        {
            //good augur score = 446
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessPerformanceScoreIsAcceptaple, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonAugurScoreVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        [Description("Graydon -> This test creates a loan and checks if the companys performance score is acceptable for the negatice case - Performance score is too low")]
        public void TestGraydonBusinessPerformanceScoreIsTooLow_LoanIsDeclined()
        {
            //good augur score = 300
            const String goodCompanyRegNumber = "99999902";

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessPerformanceScoreIsAcceptaple, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.GraydonAugurScoreVerification));
        }

        #endregion

        #region Main Applicant Duration Acceptable

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date")]
        public void TestGraydonDirectorAppointedDateIsOlderThanThreshold_LoanIsAccepted()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantDurationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count,  "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date for the negative case - Newer than threshold")]
        public void TestGraydonDirectorAppointedDateIsMoreRecentThanThreshold_LoanIsDeclined()
        {
            const string configKey = "Risk.Wb.Uk.GraydonDirectorDurationDaysAcceptableThreshold";
            const int newThreshold = 100 * 365;

            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var oldThreshhold = UpdateServiceConfigurationKey(configKey, newThreshold.ToString());

            try
            {
                var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantDurationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
                var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
                Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
                Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable));
                Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId),Get.EnumToString(RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification));
            }
            finally
            {
                if(!string.IsNullOrEmpty(oldThreshhold))
                    UpdateServiceConfigurationKey(configKey, oldThreshhold);
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date for the negative case - Director does not match applicant")]
        public void TestGraydonDirectorAppointedDate_DirectorDoesNotMatchApplicant_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "ROBERT";
            const String surname = "COOMBE";
            var dateOfBirth = new Date(new DateTime(1960, 01, 09), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantDurationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date for the negative case - Director does not match applicant")]
        public void TestGraydonDirectorAppointedDate_DirectorIsSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMask.TESTMainApplicantDurationAcceptable, Get.RandomString(7), Get.RandomString(7), Get.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification));
        }

        #endregion

        #region Number of Directors matches Business Bureau Data

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-155"), Description("Graydon -> This test creates a loan and checks the number of guarantors")]
        public void TestGraydonNumberOfGuarantorsMatchesNumberOfDirectors_LoanIsAccepted()
        {
            /*This consts need to be externalized. I know how to do it but dont have the time.
             * All your base are belong to us*/
            const int numberOfGuarantors = 3;
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTNoCheck, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.PreAccepted,numberOfGuarantors);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(numberOfGuarantors + 1, riskWorkflows.Count, "There should be 4 risk workflows");

           /* Verify the workflow
            * Get the verifications for the workflow 
            * Verify the verifications for the workflow
            * Get the checkpoints for the workflow
            * Verify the checkpoints + status + name */
            foreach (var riskWorkflowEntity in riskWorkflows)
            {
                Assert.AreEqual(RiskWorkflowStatus.Verified, (RiskWorkflowStatus)riskWorkflowEntity.Decision);

                var workflowCheckpointsWithDefinitions = riskWorkflowEntity.WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint,p =>p.CheckpointDefinitionEntity.Name);
                var workflowVerificationsWithDefinitions = riskWorkflowEntity.WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

                Assert.IsNotNull(workflowVerificationsWithDefinitions, "There should be verifications in the workflow");
                Assert.IsNotNull(workflowCheckpointsWithDefinitions, "There should be checkpoints in the workflow");

                Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.NumberOfDirectorsMatchesBusinessBureauDataVerification));
                Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.NumberOfDirectorsMatchesBusinessBureauData));

                foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
                {
                    Assert.AreEqual(RiskCheckpointStatus.Verified, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
                }
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-155"), Description("Graydon -> This test creates a loan and checks the number of guarantors for the negative case - Number of guarantors does not match ")]
        public void TestGraydonNumberOfGuarantorsDoesNotMatchNumberOfDirectors_LoanIsDeclined()
        {
            /*This consts need to be externalized. I know how to do it but dont have the time.
             * All your base are belong to us*/
            const int numberOfGuarantors = 1;
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTNumberOfDirectorsMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined, numberOfGuarantors);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(numberOfGuarantors + 1, riskWorkflows.Count, "There should be 2 risk workflows");

            /* Verify the workflow
            * Get the verifications for the workflow 
            * Verify the verifications for the workflow
            * Get the checkpoints for the workflow
            * Verify the checkpoints + status + name */
            foreach (var riskWorkflowEntity in riskWorkflows)
            {
                Assert.AreEqual(RiskWorkflowStatus.Failed, (RiskWorkflowStatus)riskWorkflowEntity.Decision);

                var workflowCheckpointsWithDefinitions = riskWorkflowEntity.WorkflowCheckpoints.ToDictionary(checkpoint => checkpoint, p => p.CheckpointDefinitionEntity.Name);
                var workflowVerificationsWithDefinitions = riskWorkflowEntity.WorkflowVerifications.ToDictionary(verification => verification, p => p.VerificationDefinitionEntity.Name);

                Assert.IsNotNull(workflowVerificationsWithDefinitions, "There should be verifications in the workflow");
                Assert.IsNotNull(workflowCheckpointsWithDefinitions, "There should be checkpoints in the workflow");

                Assert.Contains(workflowVerificationsWithDefinitions.Values.ToList(), Get.EnumToString(RiskVerificationDefinitions.NumberOfDirectorsMatchesBusinessBureauDataVerification));
                Assert.Contains(workflowCheckpointsWithDefinitions.Values.ToList(), Get.EnumToString(RiskCheckpointDefinitionEnum.NumberOfDirectorsMatchesBusinessBureauData));

                foreach (var checkpointWithDefinition in workflowCheckpointsWithDefinitions)
                {
                    Assert.AreEqual(RiskCheckpointStatus.Failed, (RiskCheckpointStatus)checkpointWithDefinition.Key.CheckpointStatus);
                }
            }

        }

        #endregion

        #region Business Date Of Incorporation is Acceptable

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-161"), Description("Graydon -> This test creates a loan and checks the date of the incorporation")]
        public void TestGraydonCompanyIncorporationDateIsOlderThanThreshold_LoanIsAccepted()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessDateOfIncorporationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.BusinessDateOfIncorporationAcceptableVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-161"), Description("Graydon -> This test creates a loan and checks the date of the incorporation for the negative case - The date of incorporation is unacceptable")]
        public void TestGraydonCompanyIncorporationDateIsUnnacceptabled_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "99999905";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessDateOfIncorporationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.BusinessDateOfIncorporationAcceptableVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-161"), Description("Graydon -> This test creates a loan and checks the date of the incorporation for the negative case - The date of incorporation is more recent than threshold")]
        public void TestGraydonCompanyIncorporationDateIsMoreRecentThanThreshold_LoanIsDeclined()
        {
            const string configKey = "Risk.Wb.Uk.GraydonCompanyIncorporationMonthsAcceptableThreshold";
            const int newThreshold = 500 * 12;

            const String goodCompanyRegNumber = "99999905";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var oldThreshhold = UpdateServiceConfigurationKey(configKey, newThreshold.ToString());

            try
            {
                var application = CreateApplicationWithAsserts(RiskMask.TESTBusinessDateOfIncorporationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatus.Declined);
                var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
                Assert.AreEqual(1,riskWorkflows.Count, "There should be 1 risk workflow");
                Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable));
                Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Get.EnumToString(RiskVerificationDefinitions.BusinessDateOfIncorporationAcceptableVerification));
            }
            finally
            {
                if (!string.IsNullOrEmpty(oldThreshhold))
                    UpdateServiceConfigurationKey(configKey, oldThreshhold);
            }
        }

        #endregion

        private static Application CreateApplicationWithAsserts(RiskMask middlenameMask, String forename, String surname, Date dateOfBirth, String companyRegisteredNumber, ApplicationDecisionStatus applicationDecision, int numberOfSecondaryDirectors = 0)
        {
            //STEP 1 - Create the main director
            var mainDirector = CustomerBuilder.New().WithMiddleName(middlenameMask.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();

            //STEP2 - Create the company
            var organisationBuilder = OrganisationBuilder.New(mainDirector).WithOrganisationNumber(companyRegisteredNumber);
            var organisation = organisationBuilder.Build();


            //STEP3 - Create the application
            var applicationBuilder = ApplicationBuilder.New(mainDirector, organisation).WithExpectedDecision(applicationDecision) as BusinessApplicationBuilder;
            var listOfGuarantors = new List<Customer>();

            //STEP4 - Create the guarantors list + send it to the application
            if (numberOfSecondaryDirectors > 0)
            {
                for (int i = 0; i < numberOfSecondaryDirectors; i++)
                {
                    var guarantor = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8));
                    listOfGuarantors.Add(guarantor);
                }

                applicationBuilder.WithGuarantors(listOfGuarantors);
            }

            //STEP5 - Build the application + send the list of guarantors
            var application = applicationBuilder.Build();

            //STE6 - Build the extra guarantors + sign
            if (numberOfSecondaryDirectors > 0)
            {
                foreach (var guarantor in listOfGuarantors)
                {
                    CustomerBuilder.New(guarantor.Id).WithEmailAddress(guarantor.Email).WithForename(guarantor.Forename)
                        .WithSurname(guarantor.Surname).Build();
                }
                applicationBuilder.SignApplicationForSecondaryDirectors();
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
        private static String UpdateServiceConfigurationKey(String configKey,String newConfigValue)
        {
            String currentValue;
            var opsDb = Drive.Db.Ops;
            var config = opsDb.ServiceConfigurations.FirstOrDefault(p => p.Key == configKey);
            if(config !=null)
            {
                currentValue = config.Value;
                config.Value = newConfigValue;
                opsDb.SubmitChanges();
            }
            else
			{
				throw new ArgumentOutOfRangeException("configKey", string.Format("Missing service configuration key: {0}", configKey));
			}
            return currentValue;
        }
    }
}
