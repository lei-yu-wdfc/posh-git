using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class L0RiskApiCheckpointAndVerificationTests : BaseL0RiskApiCheckpointAndVerificationTests
	{
		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenIsNotMinor_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenIsNotMinorOnBritishColumbia_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			//NOTE: for BC min age to be adult is 19 Y.
			// to test other provinces with < 18 Y we get a comms error
			CustomerBuilder builder = CustomerBuilder.New()
				.WithEmployer(RiskMask.TESTApplicantIsNotMinor)
				.WithProvinceInAddress(ProvinceEnum.BC)
				.WithDateOfBirth(new Date(DateTime.Now.AddYears(-18), DateFormat.Date));

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                builder, RiskCheckpointDefinitionEnum.ApplicantIsNotMinor,
				"ApplicantIsNotMinorVerification");

		}

        
		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenIsEmployed_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenElementNotOnCsBlacklist_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

            L0ApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.FraudListCheck, "FraudBlacklistVerification");
		}


		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenApplicationDeviceNotOnBlacklist_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

            L0ApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenApplicationDeviceIsOnBlacklist_ThenDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(IovationMockResponse.Deny, ApplicationDecisionStatus.Declined);

			var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

            L0ApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
		}

		[Test, AUT(AUT.Ca)]
		[Row(IovationMockResponse.Allow)]
		[Row(IovationMockResponse.Deny)]
		public void GivenL0Applicant_WhenApplicationDeviceBlacklistTimesout_ThenIsAccepted(IovationMockResponse iovationMockResponse)
		{
			int? currentMockIovationWaitSeconds = null;
			int? currentRiskIovationResponseTimeoutSeconds = null;
			try
			{
				// make sure the mocked iovation takes longer to respond than risk to timeout
				currentMockIovationWaitSeconds = SetIovationMockWaitTimeSecondsForMockResponse(iovationMockResponse, 30);
				currentRiskIovationResponseTimeoutSeconds = SetRiskIovationResponseTimeoutSeconds(5);

				_builderConfig = new ApplicationBuilderConfig(iovationMockResponse);

				var expectedVerifications = new List<string> { "IovationVerification", "IovationAutoReviewVerification" };

                L0ApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.HardwareBlacklistCheck, expectedVerifications);
			}
			finally
			{
				//allways revert to previous values
				if (currentMockIovationWaitSeconds.HasValue)
				{
					SetIovationMockWaitTimeSecondsForMockResponse(iovationMockResponse, currentMockIovationWaitSeconds.Value);
				}

				if (currentRiskIovationResponseTimeoutSeconds.HasValue)
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

            L0ApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum.MonthlyIncomeLimitCheck, expectedVerificationNames);

		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenMonthlyIncomeNotEnoughForRepayment_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			var expectedVerificationNames = new List<string> { "MonthlyIncomeVerification", "MonthlyIncomeBCVerification" };


			CustomerBuilder builder = CustomerBuilder.New()
				.WithEmployer(RiskMask.TESTMonthlyIncomeEnoughForRepayment)
				.WithNetMonthlyIncome(10);

            L0ApplicationWithSingleCheckPointAndVerifications(builder, RiskCheckpointDefinitionEnum.MonthlyIncomeLimitCheck, expectedVerificationNames);

		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenNoSuspiciousApplicationActivity_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.SuspiciousActivityCheck, "SuspiciousActivityVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenDirectFraudCheck_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.UserAssistedFraudCheck,
				"DirectFraudCheckVerification");
		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenBankAccountDoesNotMatchApplicant_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			//no verifications???
			var expectedVerificationNames = new List<string>();

			L0ApplicationWithSingleCheckPointAndVerifications(
                RiskCheckpointDefinitionEnum.BankAccountMatchesTheApplicant,
				expectedVerificationNames);
		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenIsNotDeceased_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.ApplicantIsAlive,
				"CreditBureauCustomerIsAliveVerification");

		}

		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenIsSolvent_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.CustomerIsSolvent,
				"CreditBureauCustomerIsSolventVerification");

		}


		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenCustomerDateOfBirthIsCorrect_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.DateOfBirthIsCorrect,
				"DateOfBirthIsCorrectVerification");

		}


		[Test, AUT(AUT.Ca)]
		public void GivenL0Applicant_WhenCreditBureauDataIsNotAvailable_ThenIsDeclined()
		{
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);

			L0ApplicationWithSingleCheckPointAndSingleVerification(
                RiskCheckpointDefinitionEnum.CreditBureauDataIsAvailable,
				"CreditBureauDataIsAvailableVerification");

		}

        [Test, AUT(AUT.Uk)]
        public void GivenL0Applicant_WhenIsUnderAged_ThenIsDeclined()
        {
            _builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatus.Declined);
            CustomerBuilder builder = CustomerBuilder.New()
                .WithEmployer(RiskMask.TESTApplicantIsNotMinor)
                .WithDateOfBirth(new Date(DateTime.Now.AddYears(-18), DateFormat.Date));
            L0ApplicationWithSingleCheckPointAndSingleVerification(
                builder, RiskCheckpointDefinitionEnum.ApplicantIsNotMinor,
                "ApplicantIsNotMinorVerification");
        }

        [Test, AUT(AUT.Uk)]
        public void GivenL0Applicant_WhenCustomerIsNotMinor_ThenIsAccepted()
        {
            _builderConfig = new ApplicationBuilderConfig();
            L0ApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
        }
	}
}
