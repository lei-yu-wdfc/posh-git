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

            var billingAddressId = Do.With.Message("No billing address was added for a payment card").Until(() => Drive.Data.Payments.Db.PaymentCardsBase.FindByExternalId(organisation.GetPaymentCard()).BillingAddressId);
            var billingAddress = Drive.Data.Payments.Db.BillingAddress.FindByBillingAddressId(billingAddressId);

            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.Flat));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.HouseName) && string.IsNullOrEmpty(billingAddress.HouseNumber));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.Street));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.Country));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.County));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.PostCode));
            Assert.IsFalse(string.IsNullOrEmpty(billingAddress.Town));
            Assert.AreNotEqual(Guid.Empty, billingAddress.ExternalId);
        }
    }
}
