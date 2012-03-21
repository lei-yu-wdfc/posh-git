using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class SalesforcePushFixedTermLoanApplicationDataTest
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

			// query saleforce to see if application exists.
			var salesforce = Drive.ThirdParties.Salesforce;

			var sfUsername = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
			var sfPassword = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
			var sfUrl = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");

			salesforce.SalesforceUsername = sfUsername.Value;
			salesforce.SalesforcePassword = sfPassword.Value;
			salesforce.SalesforceUrl = sfUrl.Value;

			Do.Until(() => salesforce.ApplicationExists(app.Id));
		}
	}
}
