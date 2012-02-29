using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	class AccountHolderVerificationTests
	{
		private const string TestMask = "test:BankAccountIsValid";

		[Test, AUT(AUT.Za), JIRA("ZA-2061")]
		public void AccountHolderVerificationFeatureSwitchTurnedOn()
		{
			Assert.IsTrue(IsFeatureSwitchTurnedOn());
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061")]
		public void AccountHolderVerificationResponseRecievedImmediately()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.ReadyToSign).Build();

			var saga = Do.Until(() => Driver.Db.OpsSagas.BankAccountVerificationSagaEntities.Single(a => a.ApplicationId == application.Id));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061")]
		public void AccountHolderVerificationWhenBankAccountChanged()
		{
		}

		#region Helpers

		public void SetFeatureSwitch(bool switchOn)
		{
			var db = new DbDriver();
			db.Ops.ServiceConfigurations.Single(a => a.Key == "FeatureSwitch.ZA.UseHyphenAHVWebService").Value = switchOn.ToString();
			db.Ops.SubmitChanges();
		}

		public bool IsFeatureSwitchTurnedOn()
		{
			return bool.Parse(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "FeatureSwitch.ZA.UseHyphenAHVWebService").Value);
		}

		#endregion
	}
}
