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
            Console.WriteLine(Driver.Db.OpsSagas.Log.GetType());
            Console.WriteLine(Driver.Db.Payments.Log.GetType());
            return;

            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.Repay();
        }
    }
}
