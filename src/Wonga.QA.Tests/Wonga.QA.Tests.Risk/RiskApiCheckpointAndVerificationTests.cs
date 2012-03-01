using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using MbUnit.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.QAData;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{

    public class RiskApiCheckpointAndVerificationTests
    {
        /// <summary>
        /// helper class to setup application builder
        /// </summary>
        private class ApplicationBuilderConfig
        {
			public IovationMockResponse IovationBlackBox { get; private set; }

            public ApplicationDecisionStatusEnum ExpectedDecisionStatus { get; private set; }

			public ApplicationBuilderConfig(IovationMockResponse iovationBlackBox = IovationMockResponse.Allow, ApplicationDecisionStatusEnum expectedDecisionStatus = ApplicationDecisionStatusEnum.Accepted)
            {
                IovationBlackBox = iovationBlackBox;
                ExpectedDecisionStatus = expectedDecisionStatus;
            }

            public ApplicationBuilderConfig(ApplicationDecisionStatusEnum expectedDecisionStatus)
				: this(IovationMockResponse.Allow, expectedDecisionStatus)
            {

            }
        }


		const string RiskIovationResponseTimeoutSecondsKeyName = "Risk.IovationResponseTimeoutSeconds";

        private ApplicationBuilderConfig _builderConfig;

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenIsNotMinor_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
        }

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenIsNotMinorOnBritishColumbia_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

            //NOTE: for BC min age to be adult is 19 Y.
            // to test other provinces with < 18 Y we get a comms error
            CustomerBuilder builder = CustomerBuilder.New()
                .WithEmployer("test:ApplicantIsNotMinor")
                .WithProvinceInAddress(ProvinceEnum.BC)
                .WithDateOfBirth(new Date(DateTime.Now.AddYears(-18), DateFormat.Date));

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                builder, CheckpointDefinitionEnum.ApplicantIsNotMinor,
                "ApplicantIsNotMinorVerification");

        }


        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenIsIsEmployed_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
        }

        //TODO:
        /*
        
        [Test, AUT(AUT.Ca)]
        [Explicit]
        public void GivenL0Applicant_WhenElementNotOnCsBlacklistFailed()
        {
            //need to send a RegisterFraudMessage with AccountId and HasFraud = true			
            L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.FraudListCheck, "FraudBlacklistVerification", "test:ApplicationElementNotOnCSBlacklist", CheckpointStatus.Failed);
        }
        */

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenElementNotOnCsBlacklist_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.FraudListCheck, "FraudBlacklistVerification");
        }

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenApplicationDeviceNotOnBlacklist_ThenIsAccepted()
        {
            //TODO: to test timeout need to change this : update qadata.dbo.IovationDataOutput set WaitTimeInSeconds = 80

            _builderConfig = new ApplicationBuilderConfig();

            var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

            L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
        }

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenApplicationDeviceIsOnBlacklist_ThenDeclined()
        {
            //TODO: to test timeout need to change this : update qadata.dbo.IovationDataOutput set WaitTimeInSeconds = 80

            _builderConfig = new ApplicationBuilderConfig(IovationMockResponse.Deny, ApplicationDecisionStatusEnum.Declined);

            var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

            L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
        }

		//COMMENTED UNTIL BUG IN FW IS FIXED (but works locally when IovationDataOutput has PK)
		[Test, AUT(AUT.Ca)]
		[Row(IovationMockResponse.Allow)]
		[Row(IovationMockResponse.Deny)]
		public void GivenL0Applicant_WhenApplicationDeviceBlacklistTimesout_ThenIsAccepted(IovationMockResponse iovationMockResponse)
		{
			int ? currentMockIovationWaitSeconds = null;
			int ? currentRiskIovationResponseTimeoutSeconds = null;
			try
			{
				// make sure the mocked iovation takes longer to respond than risk to timeout
				currentMockIovationWaitSeconds = SetIovationMockWaitTimeSecondsForMockResponse(iovationMockResponse, 30);
				currentRiskIovationResponseTimeoutSeconds = SetRiskIovationResponseTimeoutSeconds(5);

				_builderConfig = new ApplicationBuilderConfig(iovationMockResponse);

				var expectedVerifications = new List<string> {"IovationVerification", "IovationAutoReviewVerification"};

				L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
			}
			finally
			{
				//allways revert to previous values
				if(currentMockIovationWaitSeconds.HasValue)
				{
					SetIovationMockWaitTimeSecondsForMockResponse(iovationMockResponse, currentMockIovationWaitSeconds.Value);
				}

				if(currentRiskIovationResponseTimeoutSeconds.HasValue)
				{
					SetRiskIovationResponseTimeoutSeconds(currentRiskIovationResponseTimeoutSeconds.Value);
				}				
			}
		}
		
        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenMonthlyIncomeEnoughForRepayment_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            var expectedVerificationNames = new List<string> { "MonthlyIncomeVerification", "MonthlyIncomeBCVerification" };

            L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.MonthlyIncomeLimitCheck, expectedVerificationNames);

        }

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenMonthlyIncomeNotEnoughForRepayment_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

            var expectedVerificationNames = new List<string> { "MonthlyIncomeVerification", "MonthlyIncomeBCVerification" };


            CustomerBuilder builder = CustomerBuilder.New()
                .WithEmployer("test:MonthlyIncomeEnoughForRepayment")
                .WithNetMonthlyIncome(10);

            L0ApplicationWithSingleCheckPointAndVerifications(builder, CheckpointDefinitionEnum.MonthlyIncomeLimitCheck, expectedVerificationNames);

        }



        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenNoSuspiciousApplicationActivity_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                CheckpointDefinitionEnum.SuspiciousActivityCheck, "SuspiciousActivityVerification");
        }


        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenDirectFraudCheck_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                CheckpointDefinitionEnum.UserAssistedFraudCheck, 
                "DirectFraudCheckVerification");
        }

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenBankAccountDoesNotMatchApplicant_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

            //no verifications???
            var expectedVerificationNames = new List<string> ();

            L0ApplicationWithSingleCheckPointAndVerifications(
                CheckpointDefinitionEnum.BankAccountMatchesTheApplicant, 
                expectedVerificationNames);
        }


        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenIsNotDeceased_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                CheckpointDefinitionEnum.ApplicantIsAlive,
                "CreditBureauCustomerIsAliveVerification");
        
        }

        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenIsSolvent_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                CheckpointDefinitionEnum.CustomerIsSolvent,
                "CreditBureauCustomerIsSolventVerification");

        }


        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenCustomerDateOfBirthIsCorrect_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                CheckpointDefinitionEnum.DateOfBirthIsCorrect,
                "DateOfBirthIsCorrectVerification");

        }

        
        [Test, AUT(AUT.Ca)]
        public void GivenL0Applicant_WhenCreditBureauDataIsNotAvailable_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

            L0ApplicationWithSingleCheckPointAndSingleVerification(
                CheckpointDefinitionEnum.CreditBureauDataIsAvailable,
                "CreditBureauDataIsAvailableVerification");

        }

        #region UKTests

        [Test, AUT(AUT.Uk)]
        public void GivenL0Applicant_WhenCustomerIsUnEmployed_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);
            CustomerBuilder builder = CustomerBuilder.New()
                .WithEmployerStatus("Unemployed").WithEmployer("test:EmployedMask");
            L0ApplicationWithSingleCheckPointAndSingleVerification(builder,CheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
        }


        [Test, AUT(AUT.Uk)]
        public void GivenL0Applicant_WhenCustomerIsEmployed_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();
            L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
        }

        [Test, AUT(AUT.Uk)]
        public void GivenL0Applicant_WhenIsUnderAged_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);
            CustomerBuilder builder = CustomerBuilder.New()
                .WithEmployer("test:ApplicantIsNotMinor")
                .WithDateOfBirth(new Date(DateTime.Now.AddYears(-18), DateFormat.Date));
            L0ApplicationWithSingleCheckPointAndSingleVerification(
                builder, CheckpointDefinitionEnum.ApplicantIsNotMinor,
                "ApplicantIsNotMinorVerification");
        }

        [Test, AUT(AUT.Uk)]
        public void GivenL0Applicant_WhenCustomerIsNotMinor_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();
            L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
        }

        #endregion

        #region helpers

		/// <summary>
		/// changes configuration for iovation timeout and returns current value
		/// </summary>
		/// <param name="seconds">new value for timeout in secs</param>
		/// <returns>current timeout value in secs</returns>
		private static int SetRiskIovationResponseTimeoutSeconds(int seconds)
		{
			var dbDriver = new DbDriver();
			var serviceConfigurationEntity = dbDriver.Ops.ServiceConfigurations.Single(sc => sc.Key == RiskIovationResponseTimeoutSecondsKeyName);
			var currentValue = serviceConfigurationEntity.Value;
			var newValue = seconds.ToString();
			if (currentValue != newValue)
			{
				serviceConfigurationEntity.Value = newValue;
				dbDriver.Ops.SubmitChanges();
			}
			return Int32.Parse(currentValue);
		}

		private static int SetIovationMockWaitTimeSecondsForMockResponse(IovationMockResponse response, int seconds)
		{			
			string iovationType = response.ToString();
			var iovationDataOutput = Driver.Db.QAData.IovationDataOutputs.Single(io => io.Type == iovationType);
			var currentValue = iovationDataOutput.WaitTimeInSeconds;
			iovationDataOutput.WaitTimeInSeconds = seconds;
			iovationDataOutput.Submit();

			return currentValue;
		}
		

		private Application LNTest()
		{
			Customer cust = CustomerBuilder.New().Build();

			Application l0App = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

			l0App.RepayOnDueDate();

			EmploymentDetailEntity employmentDetails = Driver.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == cust.Id);
			employmentDetails.EmployerName = "test:DirectFraud";
			employmentDetails.Submit();

			Application lNApp = ApplicationBuilder.New(cust).WithIovationBlackBox("Allow")
			   .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

			//Assert.IsTrue(RiskApiCheckpointTests.SingleCheckPointVerification(lNApp, CheckpointStatus.Verified,CheckpointDefinitionEnum.UserAssistedFraudCheck));
			Assert.Contains(Application.GetExecutedCheckpointDefinitions(lNApp.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.UserAssistedFraudCheck));
		}

		private void AssertCheckpointAndVerifications(CheckpointStatus expectedStatus, IEnumerable<string> expectedVerificationNames, CheckpointDefinitionEnum checkpoint, Application application)
        {
			//first check what was the failed checkpoint if expected status is declined			
			AssertFailedCheckpointOnApplication(application, expectedStatus, checkpoint);

        	RiskApplicationEntity riskApplication = new DbDriver().Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);

            AssertCheckpointOnApplicationDbEntity(expectedStatus, checkpoint, riskApplication);

        	AssertVerificationsOnApplicationDbEntity(expectedVerificationNames, riskApplication);
        }

    	private void AssertVerificationsOnApplicationDbEntity(IEnumerable<string> expectedVerificationNames, RiskApplicationEntity riskApplication)
    	{
    		Assert.AreEqual(expectedVerificationNames.Count(), riskApplication.WorkflowVerifications.Count());

    		foreach (string expectedVerificationName in expectedVerificationNames)
    		{
    			Assert.IsTrue(
					riskApplication.WorkflowVerifications.Any(v => v.VerificationDefinitionEntity.Name == expectedVerificationName),
    				String.Format("Verification name should be {0}", expectedVerificationName));
    		}
    	}

    	private static void AssertCheckpointOnApplicationDbEntity(CheckpointStatus expectedStatus, CheckpointDefinitionEnum checkpoint, RiskApplicationEntity riskApplication)
    	{
    		var dbCheckpoint = riskApplication.WorkflowCheckpoints.Single(r => r.RiskApplicationId == riskApplication.RiskApplicationId);

    		Assert.AreEqual(Convert.ToByte(expectedStatus), dbCheckpoint.CheckpointStatus);

    		string checkpointName = dbCheckpoint.CheckpointDefinitionEntity.Name;

    		Assert.AreEqual(Data.EnumToString(checkpoint), checkpointName);
    	}

    	/// <summary>
		/// this method asserts on what was returned on the API call for the failed checkpoit. Only applicable for declined applications.
		/// </summary>
		/// <param name="application">the application object</param>
		/// <param name="expectedFailedCheckpoint">the expected checkpoint to fail</param>
    	private static void AssertFailedCheckpointOnApplication(Application application, CheckpointDefinitionEnum ? expectedFailedCheckpoint)
    	{
			if (expectedFailedCheckpoint.HasValue)
    		{
    			Assert.AreEqual(Data.EnumToString(expectedFailedCheckpoint), application.FailedCheckpoint);
    		}
    		else
    		{
    			Assert.IsTrue(string.IsNullOrEmpty(application.FailedCheckpoint));
    		}
    	}

		private static void AssertFailedCheckpointOnApplication(Application application, CheckpointStatus expectedStatus, CheckpointDefinitionEnum checkpoint)
		{
			AssertFailedCheckpointOnApplication(application,
			                                    expectedStatus == CheckpointStatus.Failed 
													? checkpoint 
													: (CheckpointDefinitionEnum ?)null);			
		}

    	private Application L0ApplicationWithSingleCheckPointAndVerifications(CustomerBuilder customerBuilder, CheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames)
        {
            if (_builderConfig == null)
            {
                //use the default
                _builderConfig = new ApplicationBuilderConfig();
            }

            CheckpointStatus expectedStatus = GetExpectedCheckpointStatus(_builderConfig.ExpectedDecisionStatus);

            Customer cust = customerBuilder.Build();

            Application app = ApplicationBuilder.New(cust)
                .WithIovationBlackBox(_builderConfig.IovationBlackBox.ToString())
                .WithExpectedDecision(_builderConfig.ExpectedDecisionStatus).Build();

            AssertCheckpointAndVerifications(expectedStatus, expectedVerificationNames, checkpointDefinition, app);

            return app;	
        }

        private Application L0ApplicationWithSingleCheckPointAndSingleVerification(CustomerBuilder customerBuilder, CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
        {
            return L0ApplicationWithSingleCheckPointAndVerifications(customerBuilder, checkpointDefinition, new[] { expectedVerificationName });
        }

        private Application L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
        {
			return L0ApplicationWithSingleCheckPointAndVerifications(checkpointDefinition, new List<string> {expectedVerificationName});
        }

        private Application L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames, string employerNameTestMask)
        {
            return L0ApplicationWithSingleCheckPointAndVerifications(
                CustomerBuilder.New().WithEmployer(employerNameTestMask),
                checkpointDefinition, expectedVerificationNames);
        }

        private Application L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames)
        {
            return L0ApplicationWithSingleCheckPointAndVerifications(
                        checkpointDefinition, 
                        expectedVerificationNames,
                        GetEmployerNameMaskFromCheckpointDefinition(checkpointDefinition));
        }


        private string GetEmployerNameMaskFromCheckpointDefinition(CheckpointDefinitionEnum checkpointDefinition)
        {
        	var className = GetCheckpointClassName(checkpointDefinition);

        	return string.Format("test:{0}", className);
        }

		//TODO: check why this does not work for middle name???
		private string GetMiddleNameMaskFromCheckpointDefinition(CheckpointDefinitionEnum checkpointDefinition)
		{
			var className = GetCheckpointClassName(checkpointDefinition);

			return string.Format("TEST{0}", className);
		}

    	private static string GetCheckpointClassName(CheckpointDefinitionEnum checkpointDefinition)
    	{
			//type name has this format:
    		//"Wonga.Risk.Checkpoints.ApplicationElementNotOnCSBlacklist, Wonga.Risk.Checkpoints, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    		var checkpoint = new DbDriver().Risk.CheckpointDefinitions.SingleOrDefault(cd => cd.Name == Data.EnumToString(checkpointDefinition));
    		if (checkpoint == null)
    		{
    			throw new ArgumentOutOfRangeException("checkpointDefinition", checkpointDefinition, "Not found in DB");
    		}

    		string[] assemblyTokens = checkpoint.TypeName.Split(',');
    		if (assemblyTokens.Length < 1)
    		{
    			throw new ArgumentOutOfRangeException("checkpointDefinition", checkpointDefinition,
    			                                      string.Format("Invalid Checkpoint TypeName in DB: {0}", checkpoint.TypeName));
    		}

    		//1st elem is the full namespace of type
    		string typeFullName = assemblyTokens[0];
    		string[] namespaceTokens = typeFullName.Split('.');

    		//last elem is the className... mask is Test:className
    		string className =
    			namespaceTokens.Length == 0
    				? typeFullName
    				: namespaceTokens.Last();
    		return className;
    	}

    	private static CheckpointStatus GetExpectedCheckpointStatus(ApplicationDecisionStatusEnum decision)
        {
            switch (decision)
            {
                case ApplicationDecisionStatusEnum.Declined:
                    return CheckpointStatus.Failed;

                case ApplicationDecisionStatusEnum.Pending:
                case ApplicationDecisionStatusEnum.WaitForData:

                    return CheckpointStatus.Pending;

                case ApplicationDecisionStatusEnum.Accepted:
                case ApplicationDecisionStatusEnum.ReadyToSign:
                    return CheckpointStatus.Verified;

                default:
                    throw new InvalidEnumArgumentException("decision", (int)decision, decision.GetType());
            }
        }

    	#endregion

    }
}
