using System;
using System.Linq;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentsApiTests
    {
        [Test, AUT(AUT.Uk, AUT.Za, AUT.Ca)]
        public void CreateFixTermLoanApplciation()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.Repay();
        }

        [Test]
        public void TESTE()
        {
            var cust = CustomerBuilder.New().Build();
            var comp = CompanyBuilder.New(cust).Build();
            var app = ApplicationBuilder.New(cust, comp).Build();
        }
    }
}
