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

		[Test, JIRA("CA-1880"), Ignore("This is not a full end-to-end test but used to explicitly excercise the routing logic in the CA gateway.")]
		public void SendPaymentMessageShouldBeRoutedToBmo()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var sendPaymentMessage = new SendPaymentCommand
			                         	{
			                         		AccountId = customer.Id,
											Amount = 100,
											ApplicationId = application.Id,
											BankAccount = "1234567890",
											BankCode = "001-00001",
											Currency = CurrencyCodeIso4217Enum.CAD
			                         	};

			Drive.Msmq.BankGateway.Send(sendPaymentMessage);
		}

		[Test, JIRA("CA-1880"), Ignore("This is not a full end-to-end test but used to explicitly excercise the routing logic in the CA gateway.")]
		public void SendPaymentMessageShouldBeRoutedToScotia()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var sendPaymentMessage = new SendPaymentCommand
										{
											AccountId = customer.Id,
											Amount = 100,
											ApplicationId = application.Id,
											BankAccount = "1234567890",
											BankCode = "011-12345",
											Currency = CurrencyCodeIso4217Enum.CAD
										};

			Drive.Msmq.BankGateway.Send(sendPaymentMessage);
		}
	}
}
