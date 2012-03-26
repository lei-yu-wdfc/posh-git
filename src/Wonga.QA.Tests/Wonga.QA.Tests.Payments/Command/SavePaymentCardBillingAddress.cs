using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Linq;

namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture]
    public class SavePaymentCardBillingAddress
    {
        [Test, AUT(AUT.Wb)]
        public void PaymentsShouldInsertBillingAddressWhenSavePaymentCardBillingAddressExists()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();

            var billingAddress = Do.With.Message("No billing address was added for a payment card").Until(() => Drive.Db.Payments.PaymentCardsBases.Single(c => c.ExternalId == organisation.GetPaymentCard()).BillingAddressEntity);

            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.AddressLine1));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.AddressLine2));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.Country));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.County));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.PostCode));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.Town));
            Assert.AreNotEqual(Guid.Empty, billingAddress.ExternalId);
        }
    }
}
