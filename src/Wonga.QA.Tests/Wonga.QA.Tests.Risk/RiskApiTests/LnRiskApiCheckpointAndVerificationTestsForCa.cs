using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class LnRiskApiCheckpointAndVerificationTestsForCa : BaseLnRiskApiCheckpointAndVerificationTests
	{
		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsNotMinor_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsEmployed_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenElementNotOnCsBlacklist_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.FraudListCheck, "FraudBlacklistVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenApplicationDeviceNotOnBlacklist_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

			LNApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenApplicationDeviceIsOnBlacklist_ThenDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(IovationMockResponse.Deny, ApplicationDecisionStatusEnum.Declined);

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

			LNApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenMonthlyIncomeEnoughForRepayment_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			var expectedVerificationNames = new List<string> { "MonthlyIncomeVerification", "MonthlyIncomeBCVerification" };

			LNApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum.MonthlyIncomeLimitCheck, expectedVerificationNames);

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenNoSuspiciousApplicationActivity_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
				CheckpointDefinitionEnum.SuspiciousActivityCheck, "SuspiciousActivityVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenDirectFraudCheck_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

			LNApplicationWithSingleCheckPointAndSingleVerification(
				CheckpointDefinitionEnum.UserAssistedFraudCheck,
				"DirectFraudCheckVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenBankAccountDoesNotMatchApplicant_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

			//no verifications???
			var expectedVerificationNames = new List<string>();

			LNApplicationWithSingleCheckPointAndVerifications(
				CheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
				expectedVerificationNames);
		}


		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsNotDeceased_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
				CheckpointDefinitionEnum.ApplicantIsAlive,
				"CreditBureauCustomerIsAliveVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsSolvent_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
				CheckpointDefinitionEnum.CustomerIsSolvent,
				"CreditBureauCustomerIsSolventVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenCustomerDateOfBirthIsCorrect_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
				CheckpointDefinitionEnum.DateOfBirthIsCorrect,
				"DateOfBirthIsCorrectVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenCreditBureauDataIsNotAvailable_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);

			LNApplicationWithSingleCheckPointAndSingleVerification(
				CheckpointDefinitionEnum.CreditBureauDataIsAvailable,
				"CreditBureauDataIsAvailableVerification");

		}

	}
}
