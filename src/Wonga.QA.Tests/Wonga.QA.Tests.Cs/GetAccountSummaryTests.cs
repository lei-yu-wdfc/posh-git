using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Cs
{
	[TestFixture, Parallelizable(TestScope.All), Pending("ZA-2565")]
    public class GetAccountSummaryTests
    {
		[Test, AUT(AUT.Za), JIRA("ZA-2470"), Pending("ZA-2565")]
        public void GetTest()
        {
            var payDate = DateTime.Now.AddDays(15);
            Customer customer = CustomerBuilder.New()
                .WithNextPayDate(new Date(payDate))
                .Build();
            Do.Until(customer.GetBankAccount);
            ApplicationBuilder.New(customer).Build();

            var response = Drive.Cs.Queries.Post(new CsGetAccountSummaryQuery { AccountId = customer.Id });

            Assert.AreEqual(payDate.Date, DateTime.Parse(response.Values["NextPayDay"].Single()));
            Assert.IsNotNull(response.Values["PayFrequencyType"].Single());
        }
    }
}
