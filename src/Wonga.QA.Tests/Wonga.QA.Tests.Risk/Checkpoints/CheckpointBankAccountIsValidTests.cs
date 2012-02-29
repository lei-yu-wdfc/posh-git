using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Enums;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	class CheckpointBankAccountIsValidTests
	{
		private const string TestMask = "test:BankAccountIsValid";

		[Test, AUT(AUT.Za), JIRA("ZA-1910")]
		public void CheckpointBankAccountIsValidShouldReturnReadyToSignStatus()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.ReadyToSign).Build();
		}

        [Test, AUT(AUT.Za)]
        [Ignore("Timeout too big - 10 minutes")]
        public void AhvCheckSuccessfulWhenBGInTestModeTest()
        {
            Customer cust = CustomerBuilder.New().WithEmployer(TestMask).Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatusEnum.ReadyToSign).Build();

            //Do.With().Timeout(10).Until(() => RiskApiCheckpointTests.SingleCheckPointVerification(app, CheckpointStatus.Verified, CheckpointDefinitionEnum.BankAccountIsValid));//BankGateway timeout after 5 minutes in test mode.
            var listOfCheckpoints = Do.With().Timeout(10).Until(() =>Application.GetExecutedCheckpointDefinitions(app.Id, CheckpointStatus.Verified));
            Assert.Contains(listOfCheckpoints, Data.EnumToString(CheckpointDefinitionEnum.UserAssistedFraudCheck));
        }
	}
}
