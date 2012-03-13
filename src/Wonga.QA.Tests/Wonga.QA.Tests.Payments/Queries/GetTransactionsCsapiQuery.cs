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

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public class GetTransactionsCsapiQuery
    {
        [Test]
        public void PaymentsShouldReturnAllTransactionsWhenThereAreTransactionsForAGivenApplication()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Do.Until(() => Driver.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == app.Id));

            var query = new Framework.Cs.GetTransactionsQuery
                            {
                                ApplicationGuid = app.Id
                            };

            var response = Driver.Cs.Queries.Post(query);

            Assert.IsNotNull(response);
            Assert.Contains(response.Values["Type"], "Fee");
            Assert.Contains(response.Values["Type"], "CashAdvance");
            Assert.Contains(response.Values["Type"], "Interest");
        }

        [Test]
        public void PaymentsShouldReturnNoTransactionsWhenThereAreNoTransactionsForAGivenApplication()
        {
            var customer = CustomerBuilder.New().WithMiddleName("FailingName").Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

            var query = new Framework.Cs.GetTransactionsQuery
            {
                ApplicationGuid = app.Id
            };

            var response = Driver.Cs.Queries.Post(query);

            Assert.IsNotNull(response);
            Assert.AreEqual(0,response.Values["Transaction"].Count());
        }
    }
}
