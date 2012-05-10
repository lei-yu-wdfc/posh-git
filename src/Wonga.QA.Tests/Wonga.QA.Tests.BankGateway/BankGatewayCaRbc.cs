using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture]
    public class BankGatewayCaRbc
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;

        [Test, AUT(AUT.Ca), JIRA("CA-1995"), FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenCustomerEntersInstitutionNumber003ThenBankGatewayShouldRouteTransactionToRbc()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("003").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc).Single());
        }

		[Test, AUT(AUT.Ca), JIRA("CA-1995"), FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
		public void WhenMultipleTransactionsAreBatchedHapyPath()
		{
			var applicationIds = new List<Guid>();

			using (new RbcBatchSending())
			{
				for (int i = 0; i < 3; i++)
				{
					var customer = CustomerBuilder.New().
						WithInstitutionNumber("003").
						WithBranchNumber("00022").
						Build();
					var application = ApplicationBuilder.New(customer).Build();
					applicationIds.Add(application.Id);

					Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
					                                _bgTrans.BankIntegrationId == (int) BankGatewayIntegrationId.Rbc).Single());
				}
			}

			// TODO: Add more solid assertions in here
		}
	}
}
