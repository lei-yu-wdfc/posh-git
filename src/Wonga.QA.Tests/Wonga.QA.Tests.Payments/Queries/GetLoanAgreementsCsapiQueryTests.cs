using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public class GetLoanAgreementsCsapiQueryTests
    {
        [Test]
        [AUT(AUT.Uk), JIRA("UK-1197")]
        public void Query_ShouldReturnNoLoans_WhenCustomersHasNoApplications()
        {
            Customer customer = CustomerBuilder.New().Build();
            var query = new GetLoanAgreementsQuery() {AccountId = customer.Id, IsActive = null};
            CsResponse response = Drive.Cs.Queries.Post(query);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1197")]
        public void Query_ShouldReturnAllLoans_WhenCustomersHasApplications()
        {
            Customer customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            var query = new GetLoanAgreementsQuery() { AccountId = customer.Id, IsActive = null };
            CsResponse response = Drive.Cs.Queries.Post(query);
            Application[] applications = customer.GetApplications();
            //Assert.IsTrue(applications.Length, response.Values);

            foreach (var app in applications)
            {
                Assert.IsTrue(response.Values["ApplicationId"].SingleOrDefault(i=>i == app.Id.ToString()) != null);
            }
        }
    }
}