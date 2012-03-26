using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class SalesforcePushFixedTermLoanApplicationDataTest : SalesforceTestBase
	{
		[Test]
        [AUT(AUT.Uk), JIRA("UK-808")]
		[Description("Verify that the application data has been pushed to SF")]
		public void FixedTermLoanApplicationDataPushedToSalesforce()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			var app = ApplicationBuilder.New(customer).Build();

			Do.With().Timeout(TimeSpan.FromSeconds(5)).Until(() => Salesforce.ApplicationExists(app.Id));
		}
	}
}
