using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest;
using NHamcrest.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	public class ServiceFeesTests
	{
		[Test, AUT(AUT.Za), JIRA("ZA-1969"), Pending("Feature switch might be off")]
		public void VerifyServiceFeesArePostedUpfront()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();
			PaymentsDatabase paymentsDatabase = Driver.Db.Payments;

			var applicationEntity = paymentsDatabase.Applications.Single(a => a.ExternalId == application.Id);
			var serviceFees = paymentsDatabase.Transactions.Where(t =>
			                                                      t.ApplicationId == applicationEntity.ApplicationId &&
			                                                      t.Type == "ServiceFee");

			Assert.That(serviceFees.Count(), Is.EqualTo(4));
			Assert.That(serviceFees,
			            Has.Item<TransactionEntity>(
			            	new CustomMatcher<TransactionEntity>("", t => t.PostedOn <= DateTime.UtcNow)));
		}
	}
}
