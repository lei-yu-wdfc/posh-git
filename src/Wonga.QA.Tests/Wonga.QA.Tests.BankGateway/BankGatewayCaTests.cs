using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	[TestFixture, AUT(AUT.Ca), Parallelizable(TestScope.All)]
	public class BankGatewayCaTests
	{
		[Test, JIRA("CA-1931")]
		public void SendPaymentMessageWithRealAccountShouldBeRoutedToBmo()
		{
			var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(1641421).
								WithSurname("Wonga").WithForename("Canada Inc").
                                Build();
			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == application.Id && t.BankIntegrationId == 2));
		}

        [Test, JIRA("CA-1931")]
		public void SendPaymentMessageShouldBeRoutedToScotia()
		{
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("002").
                                WithBranchNumber("00018").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == application.Id && t.BankIntegrationId == 1));
		}
	}
}
