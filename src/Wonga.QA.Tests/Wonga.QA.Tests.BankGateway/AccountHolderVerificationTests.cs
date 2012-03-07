using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	[Parallelizable(TestScope.All)]
	class AccountHolderVerificationTests
	{
		private const string TestMask = "test:BankAccountIsValid";

		private readonly Dictionary<string, string> _testModesToSetup = new Dictionary<string, string>
		                                                              	{
		                                                              		{"BankGateway.IsTestMode", "false"},
		                                                              		{"BankGateway.IsAccountVerificationTestMode", "false"},
		                                                              		{"Mocks.HyphenAHVWebServiceEnabled", "true"}
		                                                              	};

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			//Driver.Db.SetServiceConfigurations(_testModesToSetup);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061")]
		public void AccountHolderVerificationFeatureSwitchTurnedOff()
		{
		    var featureSwitchOn = Boolean.Parse(Driver.Db.GetServiceConfiguration("FeatureSwitch.ZA.UseHyphenAHVWebService").Value);
		    Assert.IsFalse(featureSwitchOn);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061")]
		[Ignore("Feature not turned on in live")]
		public void AccountHolderVerificationRequestForSingleApplication()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.ReadyToSign).Build();

			var response = WaitForAccountHolderVerificationResponse(application);

			var requestUsedWebService = AccountHolderVerificationResponseIsXml(response);
			Assert.IsTrue(requestUsedWebService);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061")]
		[Ignore("Feature not turned on in live")]
		public void AccountHolderVerificationWhenBankAccountChanged()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			var primaryAccountId = Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == customer.Id).PrimaryBankAccountId;

			Driver.Api.Commands.Post(AddBankAccountZaCommand.New(r =>
			                                                     	{
			                                                     		r.AccountId = customer.Id;
			                                                     		r.AccountNumber = 12345678902;
			                                                     	}));

			//Account has changed
			Do.Until(() => Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == customer.Id).PrimaryBankAccountId != primaryAccountId);

			var application = ApplicationBuilder.New(customer).Build();

			var response = WaitForAccountHolderVerificationResponse(application);

			var requestUsedWebService = AccountHolderVerificationResponseIsXml(response);
			Assert.IsTrue(requestUsedWebService);
		}

		#region Helpers

		private BankAccountVerificationResponseEntity WaitForAccountHolderVerificationResponse(Application application)
		{
			BankAccountVerificationEntity verification =
				Do.Until(() => Driver.Db.BankGateway.BankAccountVerifications
								.First(e => e.SenderReference == application.Id.ToString().ToLower()));

			BankAccountVerificationResponseEntity response =
				Do.Until(() => Driver.Db.BankGateway.BankAccountVerificationResponses
								.First(r => r.BankAccountVerificationId == verification.BankAccountVerificationId));

			return response;
		}

		private bool AccountHolderVerificationResponseIsXml(BankAccountVerificationResponseEntity response)
		{
			XDocument doc = XDocument.Parse(response.RawResponse);
			return doc != null;
		}

		#endregion
	}
}
