using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;

namespace Wonga.QA.UiTests.Web
{
    class ApplicationOperationsTests
    {
        [Test]
        public void Fraud()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.ConfirmFraud(application,customer,Guid.NewGuid());
        }

        [Test]
        public void HardShip()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.ReportHardship(application, Guid.NewGuid());
        }

        [Test]
        public void ManagementReview()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.ManagementReview(application, Guid.NewGuid());
        }

        [Test]
        public void Refund()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            ApplicationOperations.Refundrequest(application, Guid.NewGuid());
        }
    }
}
