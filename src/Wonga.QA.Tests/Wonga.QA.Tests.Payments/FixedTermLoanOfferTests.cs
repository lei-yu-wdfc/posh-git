using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, Parallelizable(TestScope.All)]
    public class FixedTermLoanOfferTests
    {
		[Test, AUT(AUT.Za), JIRA("ZA-2024"), Pending("ZA-2565")]
        [Row("2012-2-23", 30, 36, Order = 0)]
        [Row("2012-3-1", 23, 30, Order = 1)]
        [Row("2012-8-1", 24, 30, Order = 2)]
        public void GetFixedTermLoanOfferTest(DateTime today, int defaultTerm, int maxTerm)
        {
            var response = Drive.Api.Queries.Post(new GetFixedTermLoanOfferZaQuery());
            
            Assert.AreEqual(defaultTerm, int.Parse(response.Values["TermDefault"].Single()));
            Assert.AreEqual(maxTerm, int.Parse(response.Values["TermMax"].Single()));
        }
	}
}
