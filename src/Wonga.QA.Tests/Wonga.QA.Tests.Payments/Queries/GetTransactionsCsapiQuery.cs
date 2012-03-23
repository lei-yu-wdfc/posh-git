using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public class GetTransactionsCsapiQuery
    {
        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldReturnAllTransactionsWhenThereAreTransactionsForAGivenApplication()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            Do.Until(() => Drive.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == app.Id));

            var query = new Framework.Cs.GetTransactionsQuery
                            {
                                ApplicationGuid = app.Id
                            };

            var response = Drive.Cs.Queries.Post(query);

            Assert.IsNotNull(response);
            Assert.Contains(response.Values["Type"], "Fee");
            Assert.Contains(response.Values["Type"], "CashAdvance");
            Assert.Contains(response.Values["Type"], "Interest");
        }

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldReturnNoTransactionsWhenThereAreNoTransactionsForAGivenApplication()
        {
            var customer = CustomerBuilder.New().WithMiddleName("FailingName").Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var query = new Framework.Cs.GetTransactionsQuery
            {
                ApplicationGuid = app.Id
            };

            var response = Drive.Cs.Queries.Post(query);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Root.Descendants().Count(d => d.Name.LocalName == "Transaction"));
        }
    }
}
