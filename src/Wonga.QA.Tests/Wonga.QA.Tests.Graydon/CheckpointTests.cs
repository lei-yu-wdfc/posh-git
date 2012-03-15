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

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantMatchesBusinessBureauDataVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - DOB is incorrect")]
        public void TestGraydonDirectorDoesNotMatchApplicantDob_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1977, 12, 21), DateFormat.Date); //wrong DOB

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantMatchesBusinessBureauDataVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - Organisation without a director")]
        public void TestGraydonDirectorDoesNotMatchApplicant_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "Kathleen";
            const String surname = "Bridson";
            var dateOfBirth = Data.GetDoB();

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantMatchesBusinessBureauDataVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - Organisation director is secretary")]
        public void TestGraydonDirectorIsSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "ROBERT";
            const String surname = "COOMBE";
            var dateOfBirth = new Date(new DateTime(1960, 01, 09), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantMatchesBusinessBureauDataVerification));
        }

        #endregion

        #region Business Bureau Data Is Available

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        [Description("Graydon -> This test creates a loan and checks if the company has graydon data available")]
        public void TestGraydonBusinessBureauDataIsAvailable_LoanIsApproved()
        {
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessBureauDataIsAvailable, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.BusinessBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.BusinessDataAvailableInGraydonVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        [Description("Graydon -> This test creates a loan and checks if the company has graydon data for the negative case - Organisation is unknown to Graydon")]
        public void TestGraydonBusinessBureauDataIsUnavailable_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "99999999";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessBureauDataIsAvailable, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.BusinessBureauDataIsAvailable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.BusinessDataAvailableInGraydonVerification));
        }
        
        #endregion

        #region Business is Trading 

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        [Description("Graydon -> This test creates a loan and checks if the company is trading")]
        public void TestGraydonBusinessIsTrading_LoanIsApproved()
        {
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessIsCurrentlyTrading, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.BusinessIsCurrentlyTrading));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonBusinessIsTradingVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        [Description("Graydon -> This test creates a loan and checks if the company is trading for the negative case")]
        public void TestGraydonBusinessIsNotTrading_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "90000001";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessIsCurrentlyTrading, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.BusinessIsCurrentlyTrading));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonBusinessIsTradingVerification));
        }

        #endregion

        #region Business Payment Score is Acceptable

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys payment score is acceptable")]
        public void TestGraydonBusinessPaymentScoreIsAcceptable_LoanIsApproved()
        {
            //good score = 60
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonPaymentScoreVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys payment score is acceptable for the negative case - Payment score is low")]
        public void TestGraydonBusinessPaymentScoreIsLow_LoanIsDeclined()
        {
            //bad score = 59
            const String goodCompanyRegNumber = "99999903";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonPaymentScoreVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys with no payment score")]
        public void TestGraydonBusinessNoPaymentScore_LoanIsApproved()
        {
            //bad score = 59
            const String goodCompanyRegNumber = "99999904";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessPaymentScoreIsAcceptable, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonPaymentScoreVerification));
        }

        #endregion

        #region Business Performance Score Is Acceptable

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        [Description("Graydon -> This test creates a loan and checks if the companys performance score is acceptable")]
        public void TestGraydonBusinessPerformanceScoreIsAcceptable_LoanIsApproved()
        {
            //good augur score = 446
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessPerformanceScoreIsAcceptaple, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonAugurScoreVerification));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        [Description("Graydon -> This test creates a loan and checks if the companys performance score is acceptable for the negatice case - Performance score is too low")]
        public void TestGraydonBusinessPerformanceScoreIsTooLow_LoanIsDeclined()
        {
            //good augur score = 300
            const String goodCompanyRegNumber = "99999902";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessPerformanceScoreIsAcceptaple, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.GraydonAugurScoreVerification));
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

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantDurationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantDurationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantDurationAcceptableVerification));
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
                var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantDurationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
                var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
                Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId,CheckpointStatus.Failed),Data.EnumToString(CheckpointDefinitionEnum.MainApplicantDurationAcceptable));
                Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId),Data.EnumToString(VerificationDefinitionsEnum.MainApplicantDurationAcceptableVerification));
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

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantDurationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantDurationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantDurationAcceptableVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date for the negative case - Director does not match applicant")]
        public void TestGraydonDirectorAppointedDate_DirectorIsSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTMainApplicantDurationAcceptable, Data.RandomString(7), Data.RandomString(7), Data.GetDoB(), goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.MainApplicantDurationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.MainApplicantDurationAcceptableVerification));
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

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTNumberOfDirectorsMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted,numberOfGuarantors);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.NumberOfDirectorsMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.NumberOfDirectorsMatchesBusinessBureauDataVerification));
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

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTNumberOfDirectorsMatchesBusinessBureauData, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined, numberOfGuarantors);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.NumberOfDirectorsMatchesBusinessBureauData));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.NumberOfDirectorsMatchesBusinessBureauDataVerification));
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

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessDateOfIncorporationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Accepted);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.BusinessDateOfIncorporationAcceptableVerification));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-161"), Description("Graydon -> This test creates a loan and checks the date of the incorporation for the negative case - The date of incorporation is unacceptable")]
        public void TestGraydonCompanyIncorporationDateIsUnnacceptabled_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "99999905";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);

            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessDateOfIncorporationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable));
            Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.BusinessDateOfIncorporationAcceptableVerification));
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
                var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTBusinessDateOfIncorporationAcceptable, forename, surname, dateOfBirth, goodCompanyRegNumber, ApplicationDecisionStatusEnum.Declined);
                var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
                Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable));
                Assert.Contains(Application.GetExecutedVerificationDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId), Data.EnumToString(VerificationDefinitionsEnum.BusinessDateOfIncorporationAcceptableVerification));
            }
            finally
            {
                if (!string.IsNullOrEmpty(oldThreshhold))
                    UpdateServiceConfigurationKey(configKey, oldThreshhold);
            }
        }

        #endregion
       
        private static Application CreateApplicationWithAsserts(RiskMiddlenameMask middlenameMask, String forename, String surname, Date dateOfBirth, String companyRegisteredNumber, ApplicationDecisionStatusEnum applicationDecision, int numberOfSecondaryDirectors = 0)
        {
            var customer = CustomerBuilder.New().WithMiddleName(middlenameMask.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organisationBuilder = OrganisationBuilder.New(customer).WithOrganisationNumber(companyRegisteredNumber).WithSoManySecondaryDirectors(numberOfSecondaryDirectors);
            var organisation = organisationBuilder.Build();

            if (numberOfSecondaryDirectors > 0)
            {
                organisationBuilder.BuildSecondaryDirectors();
            }

            var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(applicationDecision).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
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
            var opsDb = Driver.Db.Ops;
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
