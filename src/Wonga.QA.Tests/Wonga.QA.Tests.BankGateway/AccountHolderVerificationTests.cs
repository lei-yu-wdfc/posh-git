﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Za;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	[Parallelizable(TestScope.All), Pending("ZA-2565")]
	class AccountHolderVerificationTests
	{
		private const RiskMask TestMask = RiskMask.TESTBankAccountIsValid;

		private readonly Dictionary<string, string> _testModesToSetup = new Dictionary<string, string>
		                                                              	{
		                                                              		{"BankGateway.IsAccountVerificationTestMode", "false"},
		                                                              		{"Mocks.HyphenAHVWebServiceEnabled", "true"}
		                                                              	};

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
				Assert.Inconclusive("Bankgateway is in test mode");

			Drive.Db.SetServiceConfigurations(_testModesToSetup);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061"), Pending("ZA-2565")]
		public void AccountHolderVerificationFeatureSwitchTurnedOn()
		{
		    var featureSwitchOn = Boolean.Parse(Drive.Db.GetServiceConfiguration("FeatureSwitch.ZA.UseHyphenAHVWebService").Value);
		    Assert.IsTrue(featureSwitchOn);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061"), Pending("ZA-2565")]
		public void AccountHolderVerificationRequestForSingleApplication()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.ReadyToSign).Build();
			
			var response = WaitForAccountHolderVerificationResponse(application);

			var requestUsedWebService = AccountHolderVerificationResponseIsXml(response);
			Assert.IsTrue(requestUsedWebService);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2061"), Pending("ZA-2565")]
		public void AccountHolderVerificationWhenBankAccountChanged()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			var primaryAccountId = Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == customer.Id).PrimaryBankAccountId;

			Drive.Api.Commands.Post(AddBankAccountZaCommand.New(r =>
			                                                     	{
			                                                     		r.AccountId = customer.Id;
			                                                     		r.AccountNumber = 12345678902;
			                                                     	}));

			//Account has changed
			Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == customer.Id).PrimaryBankAccountId != primaryAccountId);

			var application = ApplicationBuilder.New(customer).Build();

			var response = WaitForAccountHolderVerificationResponse(application);

			var requestUsedWebService = AccountHolderVerificationResponseIsXml(response);
			Assert.IsTrue(requestUsedWebService);
		}


		#region Helpers

		private BankAccountVerificationResponseEntity WaitForAccountHolderVerificationResponse(Application application)
		{
			BankAccountVerificationEntity verification =
				Do.Until(() => Drive.Db.BankGateway.BankAccountVerifications
								.First(e => e.SenderReference == application.Id.ToString().ToLower()));

			BankAccountVerificationResponseEntity response =
				Do.Until(() => Drive.Db.BankGateway.BankAccountVerificationResponses
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
