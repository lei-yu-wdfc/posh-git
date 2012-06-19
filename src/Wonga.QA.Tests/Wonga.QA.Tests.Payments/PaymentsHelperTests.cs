using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentsHelperTests
    {
        [Test, AUT(AUT.Ca), Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void CloseOutExistingCustomerLoan()
        {
            var customerId = new Guid("<CustomerGuid>");

            var customer = new Customer(customerId);
            var application = customer.GetApplication();
            application.RepayOnDueDate();

            Assert.IsTrue(application.IsClosed);
        }

        [Test, AUT(AUT.Ca), Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void LnCustomers()
        {
            for (int i = 0; i < 50; i++)
            {
                var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
                var customer = customerBuilder.Build();
                var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();

                application.RepayOnDueDate();
                Console.WriteLine(customer.Email);
            }
        }

        [Test, AUT(AUT.Ca), Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void LnCustomer()
        {
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();

            application.RepayOnDueDate();
            Console.WriteLine(customer.Email);
        }

        [Test, AUT(AUT.Ca), Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void L0Customer()
        {
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            Console.WriteLine(customer.Email);
        }


        [Test, AUT(AUT.Ca), Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void CustomerInArrears()
        {
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            application.PutIntoArrears(3);
        }
    }
}
