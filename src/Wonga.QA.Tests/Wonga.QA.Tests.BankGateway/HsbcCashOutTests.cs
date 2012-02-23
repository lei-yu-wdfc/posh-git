using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class HsbcCashOutTests
    {
        [FixtureSetUp]
        public void TurnOffTestMode()
        {
            ServiceConfigurationEntity mode = Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");
            if (mode != null && Boolean.Parse(mode.Value))
            {
                mode.Value = false.ToString();
                mode.Submit();
            }
        }

        [Test, JIRA("UK-493"), Pending("Files are not uploaded in WIP2/RC2, waiting for a fix")]
        public void CashOutFileIsSent()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == application.Id).TransactionStatus);
            //todo find the timeout to speed up status=3
        }
    }
}
