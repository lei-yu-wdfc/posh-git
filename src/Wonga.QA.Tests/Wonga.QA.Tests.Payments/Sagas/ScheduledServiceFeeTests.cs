using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Sagas
{
    [TestFixture]
    [Parallelizable(TestScope.Self)]
    public class ScheduledServiceFeeTests
    {
        private const string Key_UtcNow =
            "Wonga.Payments.Validators.Za.CreateFixedTermLoanApplicationValidator.DateTime.UtcNow";

        [FixtureSetUp]
        public void FixtureSetup()
        {
            Drive.Data.Ops.SetServiceConfiguration(Key_UtcNow, "2012/06/16");    
        }

        [FixtureTearDown]
        public void FixtureTeardown()
        {
            Drive.Data.Ops.Db.ServiceConfigurations.Delete(Key: Key_UtcNow);
        }

        [Test]
        [AUT(AUT.Za), JIRA("ZA-2659")]
        public void ServiceFeePostedUpTo90DaysTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer)
                .WithLoanTerm(35)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();

            AssertForServiceFees(new List<DateTime>(){DateTime.UtcNow});       
        }

        private void AssertForServiceFees(List<DateTime> postedOnDates)
        {
            var trs = Drive.Data.Payments.Db.Transactions;
            var sfTrs = trs.FindAllByType("ServiceFee");
            Assert.AreEqual(postedOnDates.Count, sfTrs.Count());
            foreach (var t in sfTrs)
            {
                Assert.Exists(postedOnDates, d => d == t.PostedOn);
            }
        }
    }
}
