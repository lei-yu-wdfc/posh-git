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
			public string IovationBlackBox { get; private set; }

			public ApplicationDecisionStatusEnum ExpectedDecisionStatus { get; private set; }

			public ApplicationBuilderConfig(string iovationBlackBox = "Allow", ApplicationDecisionStatusEnum expectedDecisionStatus = ApplicationDecisionStatusEnum.Accepted)
			{
				IovationBlackBox = iovationBlackBox;
				ExpectedDecisionStatus = expectedDecisionStatus;
			}

			public ApplicationBuilderConfig(ApplicationDecisionStatusEnum expectedDecisionStatus)
				: this("Allow", expectedDecisionStatus)
			{

			}
		}


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
		public void GivenL0Applicant_WhenApplicationDevicIsOnBlacklist_ThenDeclined()
		{
			//TODO: to test timeout need to change this : update qadata.dbo.IovationDataOutput set WaitTimeInSeconds = 80

			_builderConfig = new ApplicationBuilderConfig("Deny", ApplicationDecisionStatusEnum.Declined);

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

			L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
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


		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenPaymentCardIsNotValid_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

			//no verifications ???
			var expectedVerificationNames = new List<string>();

			L0ApplicationWithSingleCheckPointAndVerifications(
				CheckpointDefinitionEnum.PaymentCardIsValid,
				expectedVerificationNames);

		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenApplicationElementNotCifasFlagged_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig();

			//no verifications ???
			var expectedVerificationNames = new List<string>();

			L0ApplicationWithSingleCheckPointAndVerifications(
				CheckpointDefinitionEnum.CIFASFraudCheck,
				expectedVerificationNames);
		}


		#region helpers

		private void AssertCheckpointAndVerifications(Application application, CheckpointStatus expectedStatus, IEnumerable<string> expectedVerificationNames, CheckpointDefinitionEnum checkpoint)
		{
			var db = new DbDriver();

			RiskApplicationEntity riskApplication = db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);

			var dbCheckpoint = db.Risk.WorkflowCheckpoints.Single(r => r.RiskApplicationId == riskApplication.RiskApplicationId);

			Assert.AreEqual(Convert.ToByte(expectedStatus), dbCheckpoint.CheckpointStatus);

			string checkpointName =
				db.Risk.CheckpointDefinitions.Single(r => r.CheckpointDefinitionId == dbCheckpoint.CheckpointDefinitionId).Name;

			Assert.AreEqual(Data.EnumToString(checkpoint), checkpointName);

			var verifications = riskApplication.WorkflowVerifications.ToList();

			AssertVerifications(expectedVerificationNames, verifications);
			
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
				.WithIovationBlackBox(_builderConfig.IovationBlackBox)
				.WithExpectedDecision(_builderConfig.ExpectedDecisionStatus).Build();

			AssertCheckpointAndVerifications(app, expectedStatus, expectedVerificationNames, checkpointDefinition);

			return app;	
		}

		private Application L0ApplicationWithSingleCheckPointAndSingleVerification(CustomerBuilder customerBuilder, CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
		{
			return L0ApplicationWithSingleCheckPointAndVerifications(customerBuilder, checkpointDefinition, new[] { expectedVerificationName });
		}

		private Application L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName, string employerNameTestMask)
		{
			return L0ApplicationWithSingleCheckPointAndSingleVerification(
				CustomerBuilder.New().WithEmployer(employerNameTestMask),
				checkpointDefinition, expectedVerificationName);
		}

		private Application L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
		{
			return L0ApplicationWithSingleCheckPointAndSingleVerification(
				checkpointDefinition, expectedVerificationName,
				GetEmployerNameMaskFromCheckpointDefinition(checkpointDefinition));
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
			//return String.Format("test:{0}", Data.EnumToString(checkpointDefinition));
			//type name has this format:
			//"Wonga.Risk.Checkpoints.ApplicationElementNotOnCSBlacklist, Wonga.Risk.Checkpoints, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
			var checkpoint = new DbDriver().Risk.CheckpointDefinitions.SingleOrDefault(cd => cd.Name == Data.EnumToString(checkpointDefinition));
			if(checkpoint == null)
			{
				throw new ArgumentOutOfRangeException("checkpointDefinition", checkpointDefinition, "Not found in DB");
			}

			string [] assemblyTokens = checkpoint.TypeName.Split(',');
			if(assemblyTokens.Length < 1)
			{
				throw new ArgumentOutOfRangeException("checkpointDefinition", checkpointDefinition, string.Format("Invalid Checkpoint TypeName in DB: {0}", checkpoint.TypeName));
			}

			//1st elem is the full namespace of type
			string typeFullName = assemblyTokens[0];
			string[] namespaceTokens = typeFullName.Split('.');
			
			//last elem is the className... mask is Test:className
			string className = 
				namespaceTokens.Length == 0
					? typeFullName
					: namespaceTokens.Last();

			return string.Format("test:{0}", className);									
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

		private void AssertVerifications(IEnumerable<string> expectedVerificationNames, IEnumerable<WorkflowVerificationEntity> actualVerifications)
		{
			Assert.AreEqual(expectedVerificationNames.Count(), actualVerifications.Count());

			foreach (string expectedVerificationName in expectedVerificationNames)
			{
				Assert.IsTrue(
					actualVerifications.Any(v => v.VerificationDefinitionEntity.Name == expectedVerificationName),
					String.Format("Verification name should be {0}", expectedVerificationName));
			}
		}
		#endregion

	}
}
