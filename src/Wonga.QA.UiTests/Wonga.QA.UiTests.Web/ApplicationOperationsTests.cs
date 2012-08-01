using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [TestFixture, Parallelizable(TestScope.All)]
    class ApplicationOperationsTests
    {
        [Test, Owner(Owner.MihailPodobivsky, Owner.KirillPolishyk), JIRA("QA-328")]
        public void Fraud()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.ConfirmFraud(application, customer, Guid.NewGuid());
        }

        [Test, Owner(Owner.MihailPodobivsky, Owner.KirillPolishyk), JIRA("QA-328")]
        public void HardShip()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.ReportHardship(application, Guid.NewGuid());
        }

        [Test, Owner(Owner.MihailPodobivsky, Owner.KirillPolishyk), JIRA("QA-329")]
        public void ManagementReview()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.ManagementReview(application, Guid.NewGuid());
        }

        [Test, Owner(Owner.MihailPodobivsky, Owner.KirillPolishyk), JIRA("QA-330")]
        public void Refund()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.Refundrequest(application, Guid.NewGuid());
        }
    }
}
