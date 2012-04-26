using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class LnRiskApiCheckpointAndVerificationTests : BaseLnRiskApiCheckpointAndVerificationTests
	{
		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsNotMinor_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

            LNApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsEmployed_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

            LNApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenElementNotOnCsBlacklist_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

            LNApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.FraudListCheck, "FraudBlacklistVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenApplicationDeviceNotOnBlacklist_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

            LNApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenApplicationDeviceIsOnBlacklist_ThenDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(IovationMockResponse.Deny, ApplicationDecisionStatus.Declined);

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

            LNApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenMonthlyIncomeEnoughForRepayment_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			var expectedVerificationNames = new List<string> { "MonthlyIncomeVerification", "MonthlyIncomeBCVerification" };

            LNApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.MonthlyIncomeLimitCheck, expectedVerificationNames);

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenNoSuspiciousApplicationActivity_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.SuspiciousActivityCheck, "SuspiciousActivityVerification");
		}

		[Test, AUT(AUT.Ca)]
        public void GivenLNApplicant_WhenDirectFraudCheck_ThenIsAccepted()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Accepted);

			LNApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.UserAssistedFraudCheck,
				"DirectFraudCheckVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenBankAccountDoesNotMatchApplicant_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			//no verifications???
			var expectedVerificationNames = new List<string>();

			LNApplicationWithSingleCheckPointAndVerifications(
                RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
				expectedVerificationNames);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsNotDeceased_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.ApplicantIsAlive,
				"CreditBureauCustomerIsAliveVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenIsSolvent_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.CustomerIsSolvent,
				"CreditBureauCustomerIsSolventVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenCustomerDateOfBirthIsCorrect_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			LNApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
				"DateOfBirthIsCorrectVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenLNApplicant_WhenCreditBureauDataIsNotAvailable_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			LNApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
				"CreditBureauDataIsAvailableVerification");

		}

       

	}
}
