using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[AUT(AUT.Za), Explicit("Must investigate issue with mock")]
	class CheckpointBankAccountIsValidTests
	{
        private const RiskMask TestMask = RiskMask.TESTBankAccountIsValid;
		private static readonly string AhvMockOriginalValue = Drive.Db.GetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled").Value;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			Drive.Db.SetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled", "false");
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			Drive.Db.SetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled", AhvMockOriginalValue);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1910")]
		public void CheckpointBankAccountIsValidShouldReturnReadyToSignStatus()
		{
			var bankAccountNumber = Get.GetBankAccountNumber();

			var customer = CustomerBuilder.New().WithEmployer(TestMask).WithBankAccountNumber(bankAccountNumber).Build();
            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.ReadyToSign).Build();
		}
	}
}
