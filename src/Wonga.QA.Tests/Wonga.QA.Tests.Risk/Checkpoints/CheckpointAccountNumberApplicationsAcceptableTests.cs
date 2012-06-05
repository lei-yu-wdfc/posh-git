using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture]
	public class CheckpointAccountNumberApplicationsAcceptableTests
	{
		private const RiskMask TestMask = RiskMask.TESTAccountNumberApplicationsAcceptable;

		private const int DailyThreshold = 5;
		private const int ThirtyDayThreshold = 15;

		private Customer _customer;
		private Application _application;

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-2228", "CA-1879")]
		public void L0NumberOfApplicationsBelowThresholdIsAccepted()
		{
			_customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			_application = ApplicationBuilder.New(_customer).Build();

			AssertCheckpointAndVerificationExecution();
		}

		
		[Test, AUT(AUT.Za), JIRA("ZA-2228"), DependsOn("L0_NumberOfApplicationsBelowThresholdAccepted")]
		public void Ln_NumberOfApplicationsBelowThresholdAccepted()
		{
			_application.RepayOnDueDate();
			Application lnApplication = ApplicationBuilder.New(_customer).Build();

			AssertCheckpointAndVerificationExecution(lnApplication);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-2228", "CA-1879"), Timeout(0), Explicit("Long running time")]
		public void LnNumberOfApplicationDailyOverThresholdIsDeclined()
		{
			var customerLn = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).WithEmployerStatus("Unemployed").Build();
			
			for( int i = 0; i < DailyThreshold; i++)
			{
				ApplicationBuilder.New(customerLn).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
			}

			customerLn.UpdateEmployer(TestMask.ToString());

			Application lnApplication = ApplicationBuilder.New(customerLn).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			AssertApplicationDeclinedWithCorrectCheckPointAndVerification(lnApplication);
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-2228", "CA-1879"), Timeout(0), Explicit("Long running time")]
		public void LnNumberOfApplicationMonthlyOverThresholdIsDeclined()
		{
			//make one app per day up until the monthly threshold so that the daily limit is not exceeded
			var customerLn = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).WithEmployerStatus("Unemployed").Build();

			for (int i = 0; i < ThirtyDayThreshold; i++)
			{
				var dailyApplication = ApplicationBuilder.New(customerLn).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
				dailyApplication.RewindApplicationDatesForDays(ThirtyDayThreshold - i);
			}

			customerLn.UpdateEmployer(TestMask.ToString());

			Application lnApplication = ApplicationBuilder.New(customerLn).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			AssertApplicationDeclinedWithCorrectCheckPointAndVerification(lnApplication);
			
		}

		#region feature switch

		[Test, AUT(AUT.Ca), JIRA("CA-2309")]
		[Row(true)]
		[Row(false)]
		public void GivenNewCustomer_WhenFeatureSwitchIsConfigured_ThenCheckApplicationWorkflowContainsCheckpoint(bool featureSwitchValue)
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(GetFeatureSwitchKeyName(), featureSwitchValue);

			try
			{
				//don't use mask so that the workflow builder is run!
				_customer = CustomerBuilder.New().WithEmployer("Wonga").Build();

				_application = ApplicationBuilder.New(_customer).WithoutExpectedDecision().Build();

				AssertCheckpointAndVerificationExecution(featureSwitchValue);
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(GetFeatureSwitchKeyName(), currentValue);
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-2309")]
		[Row(true)]
		[Row(false)]
		public void GivenExistingCustomer_WhenFeatureSwitchIsConfigured_ThenApplicationWorkflowDoesNotContainCheckpoint(bool featureSwitchValue)
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(GetFeatureSwitchKeyName(), featureSwitchValue);

			try
			{
				_customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();

				_application = ApplicationBuilder.New(_customer).Build();
				_application.RepayOnDueDate();
                CustomerOperations.UpdateEmployerNameInRisk(customer.Id, "Wonga");
				
				var lnApplication = ApplicationBuilder.New(_customer).WithoutExpectedDecision().Build();

				AssertCheckpointAndVerificationExecution(lnApplication,false);
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(GetFeatureSwitchKeyName(), currentValue);
			}
		}

		#endregion

		private string GetFeatureSwitchKeyName()
		{
			switch (Config.AUT)
			{
				case AUT.Ca:
					return "FeatureSwitch.CA.AccountNumberApplicationsAcceptableCheckpoint";
			    default:

					throw new NotImplementedException();
			}
		}

		private void AssertApplicationDeclinedWithCorrectCheckPointAndVerification(Application loanApplication)
		{
			Assert.AreEqual(Get.EnumToString(RiskCheckpointDefinitionEnum.AccountNumberApplicationsAcceptable), loanApplication.FailedCheckpoint);
			AssertCheckpointAndVerificationExecution(loanApplication);
		}

		private void AssertCheckpointAndVerificationExecution(bool executed = true)
		{
			AssertCheckpointAndVerificationExecution(_application, executed);
		}

		private void AssertCheckpointAndVerificationExecution(Application loanApplication, bool executed = true)
		{
			AssertCheckpointExecution(loanApplication.Id, RiskCheckpointDefinitionEnum.AccountNumberApplicationsAcceptable, executed);
			AssertVerificationExecution(loanApplication.Id, RiskVerificationDefinitions.AccountNumberApplicationsAcceptableVerification, executed);
		}

		private void AssertCheckpointExecution(Guid applicationId, RiskCheckpointDefinitionEnum checkpoint, bool executed = true)
		{
			var checkpoints = Drive.Db.GetCheckpointDefinitionsForApplication(applicationId);
			Assert.AreEqual(executed, checkpoints.Any(c => c.Name == Get.EnumToString(checkpoint)));
		}

		private void AssertVerificationExecution(Guid applicationId, RiskVerificationDefinitions verification, bool executed = true)
		{
			var verifications = Drive.Db.GetVerificationDefinitionsForApplication(applicationId);
			Assert.AreEqual(executed, verifications.Any(v => v.Name == Get.EnumToString(verification)));
		}
	}
}
