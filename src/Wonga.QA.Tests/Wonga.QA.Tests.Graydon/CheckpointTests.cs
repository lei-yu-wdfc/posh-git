using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Risk;

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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth,Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantMatchesBusinessBureauData,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id,RiskWorkflowTypes.MainApplicant,RiskWorkflowStatus.Verified,1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - DOB is incorrect")]
        public void TestGraydonDirectorDoesNotMatchApplicantDob_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1977, 12, 21), DateFormat.Date); //wrong DOB
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantMatchesBusinessBureauData,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - Organisation without a director")]
        public void TestGraydonDirectorDoesNotMatchApplicant_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "Kathleen";
            const String surname = "Bridson";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantMatchesBusinessBureauData,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-935"), Description("Graydon -> This test creates a loan and checks the main director for the negative case - Organisation director is secretary")]
        public void TestGraydonDirectorIsSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "ROBERT";
            const String surname = "COOMBE";
            var dateOfBirth = new Date(new DateTime(1960, 01, 09), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantMatchesBusinessBureauData,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantMatchesBusinessBureauData,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.MainApplicantMatchesBusinessBureauDataVerification);
        }

        #endregion

        #region Business Bureau Data Is Available

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        [Description("Graydon -> This test creates a loan and checks if the company has graydon data available")]
        public void TestGraydonBusinessBureauMainApplicantDataIsAvailable_LoanIsApproved()
        {
            const String goodCompanyRegNumber = "00000086";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessBureauDataIsAvailable,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.BusinessDataAvailableInGraydonVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-156")]
        [Description("Graydon -> This test creates a loan and checks if the company has graydon data for the negative case - Organisation is unknown to Graydon")]
        public void TestGraydonBusinessBureauMainApplicantDataIsUnavailable_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "99999999";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessBureauDataIsAvailable,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessBureauDataIsAvailable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.BusinessDataAvailableInGraydonVerification);
        }
        
        #endregion

        #region Business is Trading 

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        [Description("Graydon -> This test creates a loan and checks if the company is trading")]
        public void TestGraydonBusinessIsTrading_LoanIsApproved()
        {
            const String goodCompanyRegNumber = "00000086";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessIsCurrentlyTrading,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessIsCurrentlyTrading,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.GraydonBusinessIsTradingVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-160")]
        [Description("Graydon -> This test creates a loan and checks if the company is trading for the negative case")]
        public void TestGraydonBusinessIsNotTrading_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "90000001";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessIsCurrentlyTrading,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessIsCurrentlyTrading,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.GraydonBusinessIsTradingVerification);
        }

        #endregion

        #region Business Payment Score is Acceptable

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys payment score is acceptable")]
        public void TestGraydonBusinessPaymentScoreIsAcceptable_LoanIsApproved()
        {
            //good score = 60
            const String goodCompanyRegNumber = "00000086";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessPaymentScoreIsAcceptable,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.GraydonPaymentScoreVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys payment score is acceptable for the negative case - Payment score is low")]
        public void TestGraydonBusinessPaymentScoreIsLow_LoanIsDeclined()
        {
            //bad score = 59
            const String goodCompanyRegNumber = "99999903";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessPaymentScoreIsAcceptable,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.GraydonPaymentScoreVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-162")]
        [Description("Graydon -> This test creates a loan and checks if the companys with no payment score")]
        public void TestGraydonBusinessNoPaymentScore_LoanIsApproved()
        {
            //bad score = 59
            const String goodCompanyRegNumber = "99999904";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessPaymentScoreIsAcceptable,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessPaymentScoreIsAcceptable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.GraydonPaymentScoreVerification);
        }

        #endregion

        #region Business Performance Score Is Acceptable

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        [Description("Graydon -> This test creates a loan and checks if the companys performance score is acceptable")]
        public void TestGraydonBusinessPerformanceScoreIsAcceptable_LoanIsApproved()
        {
            //good augur score = 446
            const String goodCompanyRegNumber = "00000086";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessPerformanceScoreIsAcceptaple,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.GraydonAugurScoreVerification);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-159")]
        [Description("Graydon -> This test creates a loan and checks if the companys performance score is acceptable for the negatice case - Performance score is too low")]
        public void TestGraydonBusinessPerformanceScoreIsTooLow_LoanIsDeclined()
        {
            //good augur score = 300
            const String goodCompanyRegNumber = "99999902";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessPerformanceScoreIsAcceptaple,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessPerformanceScoreIsAcceptaple,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.GraydonAugurScoreVerification);
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantDurationAcceptable,
                                                           ApplicationDecisionStatus.Accepted);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date for the negative case - Newer than threshold")]
        public void TestGraydonDirectorAppointedDateIsMoreRecentThanThreshold_LoanIsDeclined()
        {
            const string configKey = "Risk.Wb.Uk.GraydonDirectorDurationDaysAcceptableThreshold";
            const int newThreshold = 100 * 365;
            var oldThreshold = Drive.Db.GetServiceConfiguration(configKey).Value;

            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            Drive.Db.SetServiceConfiguration(configKey,newThreshold.ToString());

            try
            {
                var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                               RiskMask.TESTMainApplicantDurationAcceptable,
                                                               ApplicationDecisionStatus.Declined);

                var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
                var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

                VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                         RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable,
                                                                         RiskCheckpointStatus.Failed,
                                                                         RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification);

            }
            finally
            {
                if (!string.IsNullOrEmpty(oldThreshold))
                    Drive.Db.SetServiceConfiguration(configKey,oldThreshold);
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantDurationAcceptable,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification);

        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-937"), Description("Graydon -> This test creates a loan and checks the main director appointed date for the negative case - Director does not match applicant")]
        public void TestGraydonDirectorAppointedDate_DirectorIsSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8),
                                             Get.GetDoB(), Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTMainApplicantDurationAcceptable,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.MainApplicantDurationAcceptable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.MainApplicantDurationAcceptableVerification);
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var listOfGuarantors = new List<Customer>();

            for (int i = 0; i < numberOfGuarantors; i++)
            {
                listOfGuarantors.Add(new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone()));
            }

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber, RiskMask.TESTNoCheck,
                                                           ApplicationDecisionStatus.Accepted,listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);
            var guarantorsRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.Guarantor, RiskWorkflowStatus.Verified, numberOfGuarantors);
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var listOfGuarantors = new List<Customer>();

            for (int i = 0; i < numberOfGuarantors; i++)
            {
                listOfGuarantors.Add(new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(), Get.GetMobilePhone()));
            }

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTNumberOfDirectorsMatchesBusinessBureauData,
                                                           ApplicationDecisionStatus.Declined, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);
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
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessDateOfIncorporationAcceptable,
                                                           ApplicationDecisionStatus.Accepted);
            
            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.BusinessDateOfIncorporationAcceptableVerification);

        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-161"), Description("Graydon -> This test creates a loan and checks the date of the incorporation for the negative case - The date of incorporation is unacceptable")]
        public void TestGraydonCompanyIncorporationDateIsUnnacceptabled_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "99999905";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTBusinessDateOfIncorporationAcceptable,
                                                           ApplicationDecisionStatus.Declined);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.BusinessDateOfIncorporationAcceptableVerification);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-161"), Description("Graydon -> This test creates a loan and checks the date of the incorporation for the negative case - The date of incorporation is more recent than threshold")]
        public void TestGraydonCompanyIncorporationDateIsMoreRecentThanThreshold_LoanIsDeclined()
        {
            const string configKey = "Risk.Wb.Uk.GraydonCompanyIncorporationMonthsAcceptableThreshold";
            const int newThreshold = 500 * 12;
            var oldThreshold = Drive.Db.GetServiceConfiguration(configKey).Value;

            const String goodCompanyRegNumber = "99999905";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());

            Drive.Db.SetServiceConfiguration(configKey, newThreshold.ToString());

            try
            {
                var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                               RiskMask.TESTBusinessDateOfIncorporationAcceptable,
                                                               ApplicationDecisionStatus.Declined);

                var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
                var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

                VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                         RiskCheckpointDefinitionEnum.BusinessDateOfIncorporationAcceptable,
                                                                         RiskCheckpointStatus.Failed,
                                                                         RiskVerificationDefinitions.BusinessDateOfIncorporationAcceptableVerification);

            }
            finally
            {
                if (!string.IsNullOrEmpty(oldThreshold))
                    Drive.Db.SetServiceConfiguration(configKey, oldThreshold);
            }
        }

        #endregion

        #region Guarantors names match business bureau data 

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-936"), Description("Graydon -> This test creates a loan and checks the personal details of the guarantors ")]
        public void TestGraydonGuarantorsNamesMatchDirectorsName_LoanIsAccepted()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var listOfGuarantors = new List<Customer>
                                       {
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Roger John", "Clarke",
                                                        Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Anthony Charles Bramham",
                                                        "Lister", Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Timothy Charles", "Monckton",
                                                        Get.GetDoB(),Get.GetMobilePhone())
                                       };
            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTGuarantorNamesMatchBusinessBureauData,
                                                           ApplicationDecisionStatus.Accepted, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.GuarantorNamesMatchBusinessBureauData,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.GuarantorNamesMatchBusinessBureauDataVerification);

        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-936"), Description("Graydon -> This test creates a loan and checks the personal details of the guarantors ")]
        public void TestGraydonGuarantorsInitialsMatchDirectorsForenameAndSurnameInitials_LoanIsAccepted()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var listOfGuarantors = new List<Customer>
                                       {
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "RXXXXXXXXX", "Clarke",
                                                        Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "AXXXXXXXXX",
                                                        "Lister", Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "TXXXXXXXXX", "Monckton",
                                                        Get.GetDoB(),Get.GetMobilePhone())
                                       };
            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTGuarantorNamesMatchBusinessBureauData,
                                                           ApplicationDecisionStatus.Accepted, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Verified, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.GuarantorNamesMatchBusinessBureauData,
                                                                     RiskCheckpointStatus.Verified,
                                                                     RiskVerificationDefinitions.GuarantorNamesMatchBusinessBureauDataVerification);

        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-936"), Description("Graydon -> This test creates a loan and checks the personal details of the guarantors ")]
        public void TestGraydonGuarantorNameIncludesSecretary_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var listOfGuarantors = new List<Customer>
                                       {
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Roger John", "Clarke",
                                                        Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Anthony Charles Bramham",
                                                        "Lister", Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Timothy Charles", "Monckton",
                                                        Get.GetDoB(),Get.GetMobilePhone()),
                                                        //Secretary
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Robert Nigel James", "Coombe",
                                                        Get.GetDoB(),Get.GetMobilePhone())
                                       };
            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTGuarantorNamesMatchBusinessBureauData,
                                                           ApplicationDecisionStatus.Declined, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.GuarantorNamesMatchBusinessBureauData,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.GuarantorNamesMatchBusinessBureauDataVerification);

        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-936"), Description("Graydon -> This test creates a loan and checks the personal details of the guarantors ")]
        public void TestGraydonGuarantorNamesDoNotMatchBureauData_LoanIsDeclined()
        {
            const String goodCompanyRegNumber = "00000086";
            const String forename = "JOHN";
            const String surname = "WILKINS";
            var dateOfBirth = new Date(new DateTime(1964, 6, 20), DateFormat.Date);
            var mainApplicant = new Customer(Guid.NewGuid(), Get.RandomEmail(), forename, surname, dateOfBirth, Get.GetMobilePhone());
            var listOfGuarantors = new List<Customer>
                                       {
                                           //The wrong one
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Roger John", "Wrong",
                                                        Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Anthony Charles Bramham",
                                                        "Lister", Get.GetDoB(),Get.GetMobilePhone()),
                                           new Customer(Guid.NewGuid(), Get.RandomEmail(), "Timothy Charles", "Monckton",
                                                        Get.GetDoB(),Get.GetMobilePhone()),
                                       };
            var application = CreateApplicationWithAsserts(mainApplicant, goodCompanyRegNumber,
                                                           RiskMask.TESTGuarantorNamesMatchBusinessBureauData,
                                                           ApplicationDecisionStatus.Declined, listOfGuarantors);

            var mainApplicantRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.MainApplicant, RiskWorkflowStatus.Verified, 1);
            var businessRiskWorkflows = VerifyRiskWorkflows(application.Id, RiskWorkflowTypes.BusinessVerification, RiskWorkflowStatus.Failed, 1);

            VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(businessRiskWorkflows[0],
                                                                     RiskCheckpointDefinitionEnum.GuarantorNamesMatchBusinessBureauData,
                                                                     RiskCheckpointStatus.Failed,
                                                                     RiskVerificationDefinitions.GuarantorNamesMatchBusinessBureauDataVerification);
        }

        #endregion

        private static Application CreateApplicationWithAsserts(Customer mainApplicant, String companyRegisteredNumber, RiskMask middlenameMask ,ApplicationDecisionStatus applicationDecision, List<Customer> guarantors = null )
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
            if (guarantors!=null)
            {
                applicationBuilder.WithGuarantors(guarantors);
            }

            //STEP5 - Build the application + send the list of guarantors
            var application = applicationBuilder.Build();

            if (applicationDecision != ApplicationDecisionStatus.Declined)
            {
                //STE6 - Build the extra guarantors + sign
                if (guarantors!=null)
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

        private List<RiskWorkflowEntity> VerifyRiskWorkflows(Guid applicationId,RiskWorkflowTypes riskWorkflowType,RiskWorkflowStatus expectedRiskWorkflowStatus,Int32 expectedNumberOfWorkflows)
        {
            Drive.Db.WaitForRiskWorkflowData(applicationId, riskWorkflowType,expectedNumberOfWorkflows,expectedRiskWorkflowStatus);
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(applicationId, riskWorkflowType);
            Assert.AreEqual(expectedNumberOfWorkflows,riskWorkflows.Count,"There should be "+expectedNumberOfWorkflows+ " workflows");

            foreach (var riskWorkflow in riskWorkflows)
            {
                Assert.AreEqual(expectedRiskWorkflowStatus, (RiskWorkflowStatus)riskWorkflow.Decision);
            }

            return riskWorkflows;
        }

        private void VerifyCheckpointDefinitionAndVerificationForRiskWorkflow(RiskWorkflowEntity riskWorkflowEntity,RiskCheckpointDefinitionEnum checkpointDefinition, RiskCheckpointStatus checkpointStatus, RiskVerificationDefinitions riskVerification)
        {
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId && p.CheckpointStatus != 0));
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflowEntity.WorkflowId, checkpointStatus), Get.EnumToString(checkpointDefinition));
            Assert.Contains(Drive.Db.GetExecutedVerificationDefinitionNamesForRiskWorkflow(riskWorkflowEntity.WorkflowId), Get.EnumToString(riskVerification));
        }
    }
}
