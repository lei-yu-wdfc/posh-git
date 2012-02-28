using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MbUnit.Framework;
using NHamcrest.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	public class HyphenAHVWebServiceTests
	{
		private readonly Dictionary<string, bool> _testModesToSetup =
			new Dictionary<string, bool>
				{
					{"BankGateway.IsTestMode", false},
					{"BankGateway.BankGateway.IsAccountVerificationTestMode", false},
					{"Mocks.HyphenAHVWebServiceEnabled", true}
				};

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			SetupTestModes();
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ResetTestModes();
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1146")]
		public void ItWorks()
		{
			Customer customer = CustomerBuilder.New()
				.WithEmployer("test:BankAccountIsValid")
				.WithBankAccountNumber(Data.RandomLong(10000000000, 99999999999))
				.Build();
			Application application = ApplicationBuilder.New(customer)
			    .Build();
			string applicationId = application.Id.ToString().ToLower();

			BankAccountVerificationEntity verification =
				Do.Until(() => Driver.Db.BankGateway.BankAccountVerifications
				               	.First(e => e.SenderReference == applicationId));

			BankAccountVerificationResponseEntity response =
				Do.Until(() => Driver.Db.BankGateway.BankAccountVerificationResponses
				               	.First(r => r.BankAccountVerificationId == verification.BankAccountVerificationId));

			XDocument doc = XDocument.Parse(response.RawResponse);
			Assert.That(doc, Is.NotNull());
		}

		private void SetupTestModes()
		{
			foreach (string testModeKey in _testModesToSetup.Keys.ToList())
			{
				ServiceConfigurationEntity mode = GetServiceConfiguration(testModeKey);
				if (mode == null)
				{
					continue;
				}

				bool original = bool.Parse(mode.Value);
				mode.Value = _testModesToSetup[testModeKey].ToString(CultureInfo.InvariantCulture);
				mode.Submit();
				_testModesToSetup[testModeKey] = original;
			}
		}

		private void ResetTestModes()
		{
			foreach (string testModeKey in _testModesToSetup.Keys.ToList())
			{
				ServiceConfigurationEntity mode = GetServiceConfiguration(testModeKey);
				if (mode == null)
				{
					continue;
				}

				mode.Value = _testModesToSetup[testModeKey].ToString(CultureInfo.InvariantCulture);
				mode.Submit();
			}
		}

		private static ServiceConfigurationEntity GetServiceConfiguration(string key)
		{
			return Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == key);
		}
	}
}
