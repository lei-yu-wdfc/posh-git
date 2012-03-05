using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [Parallelizable]
    public class GetAccountOptionsTests
    {
        [Test, AUT(AUT.Uk), JIRA("UK-823"),]
        public void Scenario1ExistingCustomerWithoutLiveLoan()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            Application application = ApplicationBuilder.New(customer).Build();

            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = customer.Id });
            Assert.GreaterThan(int.Parse(response.Values["ScenarioId"].Single()), 1);
            // ToDo: Assert Options
            //Assert.GreaterThan(int.Parse(response.Values["DaysTillRepaymentDate"].Single()), 0);
        }

    }
}
