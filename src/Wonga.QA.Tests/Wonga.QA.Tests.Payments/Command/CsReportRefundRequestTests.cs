using System;
using MbUnit.Framework;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class CsReportRefundRequestTests
    {
        [Test]
        public void Test()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(customer).Build();
            Guid caseId = Guid.NewGuid();


            Drive.Cs.Commands.Post();
        }
    }
}