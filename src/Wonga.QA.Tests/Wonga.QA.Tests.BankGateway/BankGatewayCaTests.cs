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
		// TODO: Additional full end-to-end tests will be added here once we have the Scotia and BMO mock

		[Test, JIRA("CA-1931")]
		public void SendPaymentMessageShouldBeRoutedToBmo()
		{
			var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                Build();
			ApplicationBuilder.New(customer).Build();
		}

        [Test, JIRA("CA-1931")]
		public void SendPaymentMessageShouldBeRoutedToScotia()
		{
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("002").
                                WithBranchNumber("00018").
                                Build();
            ApplicationBuilder.New(customer).Build();
		}
	}
}
