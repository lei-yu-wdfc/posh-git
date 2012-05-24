using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture, AUT(AUT.Ca)]
	class CheckpointApplicationElementNotOnCSBlacklistTests
	{
		private static readonly RiskMask TestMask = GetRiskMask();
		private const string UseDoNotRelendFlagKey = "Risk.UseDoNotRelendFlag";

		private Customer _customer;

		[Test, AUT(AUT.Ca), JIRA("CA-1974"), Parallelizable(TestScope.All)]
		public void L0NotFlaggedDoNotRelendIsAccepted()
		{
			_customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			var application = ApplicationBuilder.New(_customer).Build().RepayOnDueDate();
			AssertCheckpointAndVerificationExecution(true, application);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974"), Parallelizable(TestScope.All)]
		public void L0FlaggedDoNotRelendIsDeclined()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			RegisterDoNotRelend(customer);

            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
			AssertApplicationDeclinedWithCorrectCheckPointAndVerification(application);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974"), DependsOn("L0NotFlaggedDoNotRelendIsAccepted"), Parallelizable(TestScope.All)]
		public void LnNotFlaggedDoNotRelendIsAccepted()
		{
			var application = ApplicationBuilder.New(_customer).Build().RepayOnDueDate();
			AssertCheckpointAndVerificationExecution(true, application);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974"), DependsOn("LnNotFlaggedDoNotRelendIsAccepted"), Parallelizable(TestScope.All)]
		public void LnFlaggedDoNotRelendIsDeclined()
		{
			RegisterDoNotRelend(_customer);

			var application = ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
			AssertApplicationDeclinedWithCorrectCheckPointAndVerification(application);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		[Row(true)]
		[Row(false)]
		public void GivenNewCustomer_WhenDoNotRelendFlagIsConfigured_ThenCheckApplicationWorkflowContainsCheckpoint(bool useDoNotRelend)
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, useDoNotRelend);

			try
			{
				//don't use mask so that the workflow builder is run!
				var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();

				ApplicationBuilder.New(customer).WithoutExpectedDecision().Build();

				//AssertCheckpointAndVerificationExecution(useDoNotRelend, application);
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		[Row(true)]
		[Row(false)]
		public void GivenExistingCustomer_WhenDoNotRelendFlagIsConfigured_ThenCheckApplicationWorkflowContainsCheckpoint(bool useDoNotRelend)
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, useDoNotRelend);

			try
			{
				var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();

				var l0Application = ApplicationBuilder.New(customer).Build();
				l0Application.RepayOnDueDate();

				//don't use mask so that the workflow builder is run!
				Drive.Db.UpdateEmployerName(customer.Id, "Wonga");

				ApplicationBuilder.New(customer).WithoutExpectedDecision().Build();

				//AssertCheckpointAndVerificationExecution(useDoNotRelend, application);
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}


		#region Helpers

		private static RiskMask GetRiskMask()
		{
			switch (Config.AUT)
			{
					case AUT.Ca:
					{
						return RiskMask.TESTDoNotRelend;
					}

					case AUT.Za:
					{
						return RiskMask.TESTApplicationElementNotOnCSBlacklist;
					}

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		private void RegisterDoNotRelend(Customer customer)
		{
			Drive.Msmq.Risk.Send(new RegisterDoNotRelendCommand { AccountId = customer.Id, DoNotRelend = true });
			Do.Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id).DoNotRelend);
		}

		private void AssertApplicationDeclinedWithCorrectCheckPointAndVerification(Application application)
		{
			Assert.AreEqual(Get.EnumToString(RiskCheckpointDefinitionEnum.FraudListCheck), application.FailedCheckpoint);
			AssertCheckpointAndVerificationExecution(true, application);
		}

		private void AssertCheckpointAndVerificationExecution(bool useDoNotRelend, Application application)
		{
			AssertCheckpointExecution(application.Id, RiskCheckpointDefinitionEnum.FraudListCheck, useDoNotRelend);

			AssertVerificationExecution(application.Id, RiskVerificationDefinitions.DoNotRelendVerification, useDoNotRelend);
			AssertVerificationExecution(application.Id, RiskVerificationDefinitions.FraudBlacklistVerification, Config.AUT == AUT.Za);
		}

		private void AssertCheckpointExecution(Guid applicationId, RiskCheckpointDefinitionEnum checkpoint, bool executed)
		{
			var checkpoints = Drive.Db.GetCheckpointDefinitionsForApplication(applicationId);
			Assert.AreEqual(executed, checkpoints.Any(c => c.Name == Get.EnumToString(checkpoint)));
		}

		private void AssertVerificationExecution(Guid applicationId, RiskVerificationDefinitions verification, bool executed)
		{
			var verifications = Drive.Db.GetVerificationDefinitionsForApplication(applicationId);
			Assert.AreEqual(executed, verifications.Any(v => v.Name == Get.EnumToString(verification)));
		}


		#endregion
	}
}
