using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries.Wb.Uk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class GetBusinessApplicationSummaryCsapiQuery
    {
        [Test, JIRA("SME-375"), AUT(AUT.Wb)]
        public void PaymentsShouldReturnBusinessApplicationSummaryWhenApplicationExists()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanTerm(20).Build();

            var response = Drive.Cs.Queries.Post(new GetBusinessApplicationSummaryWbUkQuery
                                                        {
                                                            ApplicationGuid = app.Id
                                                        });

            Assert.IsNotNull(response);
            Assert.AreEqual("20", response.Values["LoanTerm"].SingleOrDefault());
            Assert.AreEqual(10000m, decimal.Parse(response.Values["PrincipalLoanAmount"].SingleOrDefault()));
        }

        [Test, JIRA("SME-375"), AUT(AUT.Wb)]
        public void PaymentsShouldReturnNullWhenApplicationDoesNotExists()
        {
            var response = Drive.Cs.Queries.Post(new GetBusinessApplicationSummaryWbUkQuery
            {
                ApplicationGuid = Guid.NewGuid()
            });

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Root.Descendants().Count(n => n.Name.LocalName == "GetBusinessApplicationSummaryResponse"));
        }
    }
}
