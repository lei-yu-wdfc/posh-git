using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentsApiTests
    {
        [Test, AUT(AUT.Uk, AUT.Za, AUT.Ca)]
        public void CreateFixTermLoanApplciation()
        {
            //Customer customer = CustomerBuilder.New().Build();
            //ApplicationBuilder.New(customer).Build();

            Customer customer = new Customer(Guid.Parse("0A2BB1B5-65FB-4564-A226-2CFAA5D79BD2"));
            foreach (Application application in customer.GetApplications())
            {
                Console.WriteLine(application.Id);
            }
        }
    }
}
