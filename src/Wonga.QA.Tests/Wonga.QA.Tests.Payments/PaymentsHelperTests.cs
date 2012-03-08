using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentsHelperTests
    {
        [Test, Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void CloseOutExistingCustomerLoan()
        {
            var customerId = new Guid("<CustomerGuid>");

            var customer = new Customer(customerId);
            var application = customer.GetApplication();
            application.RepayOnDueDate();

            Assert.IsTrue(application.IsClosed);
        }

        [Test, Ignore("Partial test to assist QA. Not to run on buildsite")]
        public void VaryVariableInterestRates()
        {
            SetPaymentFunctions.SetVariableInterestRates((decimal)1.8 * -1);
        }
    }
}
