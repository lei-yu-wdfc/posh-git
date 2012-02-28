using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	public class GetApplicationSummaryTests
	{
		[Test, AUT(AUT.Uk), JIRA("UK-795")]
		[Ignore]
		public void GetAccountSummaryTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			ApplicationBuilder.New(customer).Build();

			var response = Driver.Api.Queries.Post(new GetAccountSummaryQuery {AccountId = customer.Id});
			//£100 loan for 10 days.
			Assert.AreEqual(115.91M, decimal.Parse(response.Values["CurrentLoanRepaymentAmountOnDueDate"].Single()));
			Assert.AreEqual(DateTime.Today.AddDays(10), DateTime.Parse(response.Values["CurrentLoanDueDate"].Single()));
		}
	}
}
