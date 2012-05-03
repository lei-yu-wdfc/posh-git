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
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data.Extensions;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[AUT(AUT.Ca)]
	public class CheckpointApplicationElementNotOnCSBlacklistOnlyDoNotRelendVerificationTests
	{
		private const RiskMask TestMask = RiskMask.TESTDoNotRelend;
		private const string UseDoNotRelendFlagKey = "Risk.UseDoNotRelendFlag";

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void GivenNewCustomer_WhenDoNotRelendFlagIsNotUsed_ThenTheApplicationWorkflowDoesNotContainCheckpoint()
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, false);

			try
			{
				//don't use mask so that the workflow builder is run!
				var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();

				var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).Build();

				AssertCheckpointIsNotPartOfApplication(application.Id, RiskCheckpointDefinitionEnum.FraudListCheck);
				AssertVerificationIsNotPartOfApplication(application.Id, RiskVerificationDefinitions.DoNotRelendVerification);
				//for CA this is never run
				AssertVerificationIsNotPartOfApplication(application.Id, RiskVerificationDefinitions.FraudBlacklistVerification);
				
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}


		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void GivenNewCustomer_WhenDoNotRelendFlagIsUsed_ThenTheApplicationWorkflowContainsCheckpoint()
		{
			bool currentValue = Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, true);

			try
			{
				//don't use mask so that the workflow builder is run!
				var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();

				var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).Build();

				AssertCheckpointIsPartOfApplication(application.Id, RiskCheckpointDefinitionEnum.FraudListCheck);
				AssertVerificationIsPartOfApplication(application.Id, RiskVerificationDefinitions.DoNotRelendVerification);
				//for CA this is never run
				AssertVerificationIsNotPartOfApplication(application.Id, RiskVerificationDefinitions.FraudBlacklistVerification);
			}
			finally
			{
				Drive.Data.Ops.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}		

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void GivenNewCustomer_WhenItIsNotMarkedWithDoNotRelend_ThenTheApplicationIsAccepted()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			var riskAccount = Do.With.Timeout(1).Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id));
			Assert.IsFalse(riskAccount.DoNotRelend);

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void GivenNewCustomer_WhenItIsMarkedWithDoNotRelend_ThenTheApplicationIsDeclined()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

			Drive.Msmq.Risk.Send(new RegisterDoNotRelendCommand {AccountId = customer.Id, DoNotRelend = true});
			Do.Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id).DoNotRelend);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void GivenExistingCustomer_WhenItIsNotMarkedWithDoNotRelend_ThenTheApplicationIsAccepted()
		{
			var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();

			var l0Application = ApplicationBuilder.New(customer).Build();
			l0Application.RepayOnDueDate();

			Drive.Db.UpdateEmployerName(customer.Id, TestMask.ToString());

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		public void GivenExistingCustomer_WhenItIsMarkedWithDoNotRelend_ThenTheApplicationIsDeclined()
		{
			var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();
			var l0Application = ApplicationBuilder.New(customer).Build();
			l0Application.RepayOnDueDate();

			Drive.Db.UpdateEmployerName(customer.Id, TestMask.ToString());

			Drive.Msmq.Risk.Send(new RegisterDoNotRelendCommand { AccountId = customer.Id, DoNotRelend = true });
			Do.Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(customer.Id).DoNotRelend);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

		private void AssertCheckpointIsNotPartOfApplication(Guid applicationId, RiskCheckpointDefinitionEnum checkpoint)
		{
			var checkpoints = Drive.Db.GetCheckpointDefinitionsForApplication(applicationId);
			Assert.IsFalse(checkpoints.Any(c => c.Name == Get.EnumToString(checkpoint)));
		}

		private void AssertCheckpointIsPartOfApplication(Guid applicationId, RiskCheckpointDefinitionEnum checkpoint)
		{
			var checkpoints = Drive.Db.GetCheckpointDefinitionsForApplication(applicationId);
			Assert.IsTrue(checkpoints.Any(c => c.Name == Get.EnumToString(checkpoint)));
		}

		private void AssertVerificationIsNotPartOfApplication(Guid applicationId, RiskVerificationDefinitions verification)
		{
			var verifications = Drive.Db.GetVerificationDefinitionsForApplication(applicationId);
			Assert.IsFalse(verifications.Any(v => v.Name == Get.EnumToString(verification)));
		}

		private void AssertVerificationIsPartOfApplication(Guid applicationId, RiskVerificationDefinitions verification)
		{
			var verifications = Drive.Db.GetVerificationDefinitionsForApplication(applicationId);
			Assert.IsTrue(verifications.Any(v => v.Name == Get.EnumToString(verification)));
		}
		
	}
}
