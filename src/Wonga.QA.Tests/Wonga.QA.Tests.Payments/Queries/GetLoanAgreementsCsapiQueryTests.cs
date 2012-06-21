using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.FileStorage.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
//using CreateTransactionCommand = Wonga.QA.Framework.Cs.CreateTransactionCommand;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class GetLoanAgreementsCsapiQueryTests
    {
        //private dynamic _transactions = Drive.Data.Payments.Db.Transactions;
		private dynamic _applications = Drive.Data.Payments.Db.Applications;

        [Test]
        [AUT(AUT.Uk), JIRA("WIN-880")]
        public void GetLoanAgreementQuery_ForActiveApplication_Returns_NonEmptyAgreement()
        {
            Customer customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            var query = new GetLoanAgreementQuery()
                            {
                                ApplicationId = application.Id
                            };
            ApiResponse response = Drive.Api.Queries.Post(query);
            string contentValue = response.Values["AgreementContent"].Single();
            Assert.IsFalse(String.IsNullOrWhiteSpace(contentValue));
        }

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