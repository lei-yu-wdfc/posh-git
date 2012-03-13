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
    public class GetBusinessApplicationSummaryCsapiQuery
    {
        [Test, JIRA("SME-375"), AUT(AUT.Wb), Pending("Not fully implemented")]
        public void PaymentsShouldReturnBusinessApplicationSummaryWhenApplicationExists()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer,organisation).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            Do.Until(() => Driver.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == app.Id));

            var response = Driver.Cs.Queries.Post(new Wonga.QA.Framework.Cs.GetBusinessApplicationSummaryWbUkQuery
                                                        {
                                                            ApplicationGuid = app.Id
                                                        });

            Assert.IsNotNull(response);
        //    Assert.AreEqual();
        }
    }
}
