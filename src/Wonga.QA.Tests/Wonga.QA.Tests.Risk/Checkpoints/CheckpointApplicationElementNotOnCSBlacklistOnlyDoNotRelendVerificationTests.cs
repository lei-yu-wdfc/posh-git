using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[AUT(AUT.Ca)]
	public class CheckpointApplicationElementNotOnCSBlacklistOnlyDoNotRelendVerificationTests
	{
		private const RiskMask TestMask = RiskMask.TESTDoNotRelend;
		private const string UseDoNotRelendFlagKey = "Risk.UseDoNotRelendFlag";

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		[Row(false)]
		[Row(true)]
		public void GivenNewCustomer_WhenItIsNotFlagged_ThenTheApplicationIsAccepted(bool useDoNotRelendFlag)
		{
			bool currentValue = Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, useDoNotRelendFlag);
			try
			{
				var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

				var riskAccount = Do.With.Timeout(1).Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id));
				Assert.IsFalse(riskAccount.DoNotRelend);

				ApplicationBuilder.New(customer).Build();
			}
			finally 
			{
				Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
				
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		[Row(false, ApplicationDecisionStatus.Accepted)]
		[Row(true, ApplicationDecisionStatus.Declined)]
		public void GivenNewCustomer_WhenItIsFlagged_ThenTheApplicationDependsOnConfigurationForUseDoNotRelendFlag(bool useDoNotRelendFlag, ApplicationDecisionStatus expectedStatus)
		{
			bool currentValue = Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, useDoNotRelendFlag);

			try
			{
				var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();

				Drive.Msmq.Risk.Send(new RegisterDoNotRelendCommand {AccountId = customer.Id, DoNotRelend = true});
				Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id).DoNotRelend);

				ApplicationBuilder.New(customer).WithExpectedDecision(expectedStatus).Build();
			}
			finally
			{
				Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		[Row(false)]
		[Row(true)]
		public void GivenExistingCustomer_WhenItIsNotFlagged_ThenTheApplicationIsAccepted(bool useDoNotRelendFlag)
		{
			bool currentValue = Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, useDoNotRelendFlag);
			try
			{
				var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();

				var l0Application = ApplicationBuilder.New(customer).Build();
				l0Application.RepayOnDueDate();

				Drive.Db.UpdateEmployerName(customer.Id, TestMask.ToString());

				ApplicationBuilder.New(customer).Build();
			}
			finally
			{
				Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1974")]
		[Row(false, ApplicationDecisionStatus.Accepted)]
		[Row(true, ApplicationDecisionStatus.Declined)]
		public void GivenExistingCustomer_WhenItIsFlagged_ThenTheApplicationDependsOnConfigurationForUseDoNotRelendFlag(bool useDoNotRelendFlag, ApplicationDecisionStatus expectedStatus)
		{
			bool currentValue = Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, useDoNotRelendFlag);

			try
			{
				var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();
				var l0Application = ApplicationBuilder.New(customer).Build();
				l0Application.RepayOnDueDate();

				Drive.Db.UpdateEmployerName(customer.Id, TestMask.ToString());

				Drive.Msmq.Risk.Send(new RegisterDoNotRelendCommand { AccountId = customer.Id, DoNotRelend = true });
				Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == customer.Id).DoNotRelend);

				ApplicationBuilder.New(customer).WithExpectedDecision(expectedStatus).Build();
			}
			finally
			{
				Drive.Db.Ops.ServiceConfigurations.SetServiceConfiguration(UseDoNotRelendFlagKey, currentValue);
			}
		}

	}
}
