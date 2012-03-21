using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class LnRiskApiCheckpointAndVerificationTests : BaseLnRiskApiCheckpointAndVerificationTests
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
        public void GivenLNApplicant_WhenDirectFraudCheck_ThenIsAccepted()
		{   
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Accepted);

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

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityDeclined()
        {
            int suspiciousDuration = Data.RandomInt(1,
                Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value));

            int suspiciousDaysSinceLastLoan = Data.RandomInt(1,
                Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value));

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(suspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            L0app.RewindToDayOfLoanTerm(suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Data.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Declined)
                .Build();

            Assert.AreEqual(LnApp.FailedCheckpoint, Data.EnumToString(CheckpointDefinitionEnum.SuspiciousActivityCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityAcceptedDueToUnsuspiciousDuration()
        {
            int unsuspiciousDuration = Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value) + 1;

            int suspiciousDaysSinceLastLoan = Data.RandomInt(1,
                Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value));

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            L0app.RewindToDayOfLoanTerm(suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Data.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityAcceptedDueToUnsuspiciousDaysSinceLastLoan()
        {
            int unsuspiciousDuration = Data.RandomInt(1,
                Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value));

            int suspiciousDaysSinceLastLoan = Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(
                a => a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value) + 1;

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            L0app.RewindToDayOfLoanTerm(suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Data.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityAccepted()
        {
            int unsuspiciousDuration = Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value) + 1;

            int suspiciousDaysSinceLastLoan = Convert.ToInt16(Driver.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Data.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value) + 1;

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            L0app.RewindToDayOfLoanTerm(suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Data.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted)
                .Build();
        }

	}
}
