using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture, AUT(AUT.Wb), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
    public class SalesforcePushBusinessPaymentCardDataTests:SalesforceTestBase
    {
        private Customer customer;
        private Organisation organisation;

        [FixtureSetUp]
        public void InitFixture()
        {
            customer = CustomerBuilder.New().Build();
            organisation = OrganisationBuilder.New(customer).Build();
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1295"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
        public void SalesforceTC_ShouldPushPaymentCardDataToSF_WhenPaymentCardIsAdded()
        {
            var paymentCardId = organisation.GetPaymentCard();

            var paymentCard = Do.With.Message("SF should contain the newly inserted payment card").Until(() => Salesforce.GetPaymentCardById(paymentCardId, "AND p.Masked_Number__c != null"));

            var dbPaymentCard = Drive.Data.Payments.Db.PaymentCardsBase.FindByExternalId(paymentCardId);

            Assert.AreEqual(dbPaymentCard.Type, paymentCard.Type__c, "PaymentCard Type should match");
            Assert.AreEqual(dbPaymentCard.HolderName, paymentCard.Holder_Name__c, "Payment Card HolderName should match");
            Assert.AreEqual(dbPaymentCard.MaskedNumber, paymentCard.Masked_Number__c, "PaymentCard MaskedNumber should match");
            Assert.AreEqual(dbPaymentCard.IssueNo, paymentCard.Issue_No__c.ToString(), "PaymentCard IssueNo should match");
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1295"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
        public void SalesforceTC_ShouldPushPaymentCardBillingAddressToSF_WhenBillingAddressIsSet()
        {
            var paymentCardId = organisation.GetPaymentCard();

            var paymentCard = Do.With.Message("SF should contain the newly inserted payment card").Until(() => Salesforce.GetPaymentCardById(paymentCardId, "AND p.V3_Billing_Address_Id__c != null"));

            var dbPaymentCard = Drive.Data.Payments.Db.PaymentCardsBase.FindByExternalId(paymentCardId);
            var dbBillingAddress = Drive.Data.Payments.Db.BillingAddress.FindByBillingAddressId(dbPaymentCard.BillingAddressId);

            Assert.AreEqual(dbBillingAddress.ExternalId.ToString(), paymentCard.V3_Billing_Address_Id__c, "BillingAddress ExternalId should match");
            // AddressLines 1-2 are calculated now
            //Assert.AreEqual(dbBillingAddress.AddressLine1, paymentCard.Address_Line_1__c, "BillingAddress AddressLine1 should match");
            //Assert.AreEqual(dbBillingAddress.AddressLine2, paymentCard.Address_Line_2__c, "BillingAddress AddressLine2 should match");
            Assert.AreEqual(dbBillingAddress.Town, paymentCard.Town__c, "BillingAddress Town should match");
            Assert.AreEqual(dbBillingAddress.Country, paymentCard.Country__c, "BillingAddress Country should match");
            Assert.AreEqual(dbBillingAddress.County, paymentCard.County__c, "BillingAddress County should match");
            Assert.AreEqual(dbBillingAddress.PostCode, paymentCard.Post_Code__c, "BillingAddress PostCode should match");
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1295"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
        public void SalesforceTC_ShouldPushPaymentCardToOrganisationRelationToSF_WhenPaymentCardIsAdded()
        {
            var paymentCardId = organisation.GetPaymentCard();

            var paymentCard = Do.With.Message("SF should contain the newly inserted payment card").Until(() => Salesforce.GetPaymentCardById(paymentCardId, "AND p.Customer_Account__r.V3_Organization_Id__c != null"));

            Assert.AreEqual(organisation.Id.ToString(), paymentCard.Customer_Account__r.V3_Organization_Id__c, "Payment card is assigned to an incorrect organisation");
        }
    }
}
