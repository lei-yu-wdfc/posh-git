using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentSagaBaseTests
    {
        [Test, AUT(AUT.Uk, AUT.Za, AUT.Ca), Parallelizable]
        public void UpdateTimeoutTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            

        }
    }
}
