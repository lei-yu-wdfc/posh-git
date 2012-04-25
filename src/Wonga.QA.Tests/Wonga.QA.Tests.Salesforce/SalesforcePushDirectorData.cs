using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.ThirdParties.SalesforceApi;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    public class SalesforcePushDirectorData : SalesforceTestBase
    {
        [Test, AUT(AUT.Wb), JIRA("SME-1404", "SME-1405")]
        public void ShouldCreateSalesForceContactWithCustomerDetailsAndAddressForPrimaryDirector()
        {
            CustomerBuilder customer;
            CustomerBuilder secondaryDirector;

            InitializeApplication(out secondaryDirector, out customer);

            Do.With.Message("Details should exist in salesforce").Until(
                () =>
                ValidateContact(true, customer.Email,
                                               customer.DateOfBirth.DateTime, customer.Town, customer.Id.ToString()));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1404", "SME-1405")]
        public void ShouldCreateSalesForceContactWithCustomerDetailsAndAddressForSecondaryDirector()
        {
            CustomerBuilder customer;
            CustomerBuilder secondaryDirector;

            InitializeApplication(out secondaryDirector, out customer);

            Do.With.Message("Details should exist in salesforce").Until(
                () =>
                ValidateContact(false, secondaryDirector.Email,
                                               secondaryDirector.DateOfBirth.DateTime, secondaryDirector.Town,
                                               secondaryDirector.Id.ToString()));
        }

        private Contact ValidateContact(bool isPrimary, string email, DateTime dateOfBirth, string town, string accountId)
        {
            var contact = Salesforce.GetContactByAccountId(accountId);
            Assert.IsNotNull(contact, "Should contain a contact");
            Assert.AreEqual(isPrimary, contact.Is_Primary_Applicant__c.Value, "IsPrimary does not match");
            Assert.AreEqual(email.ToUpper(), contact.Email.ToUpper(), "Email addresses do not match");
            Assert.AreEqual(dateOfBirth, contact.Birthdate.Value, "Dates of birth do not match");
            Assert.AreEqual(town, contact.MailingCity, "Cities do not match");
            return contact;
        }

        private static void InitializeApplication(out CustomerBuilder secondaryDirector, out CustomerBuilder customer)
        {
            Date primaryDob = Get.GetDoB();

            customer = CustomerBuilder.New()
                .WithForename("PrimaryName")
                .WithSurname("PrimarySurname")
                .WithDateOfBirth(primaryDob)
                .WithTownInAddress("PrimaryTown");

            Date secondaryDob = Get.GetDoB();

            secondaryDirector = CustomerBuilder.New()
                .WithForename("SecondaryName")
                .WithSurname("SecondarySurname")
                .WithDateOfBirth(secondaryDob)
                .WithTownInAddress("SecondaryTown");

            var customerInstance = customer.Build();

            var org = OrganisationBuilder.New(customerInstance).Build();

            var app =
                ((BusinessApplicationBuilder)ApplicationBuilder.New(customerInstance, org)).WithGuarantors(
                    new[] { secondaryDirector }.ToList()).Build();
        }
    }
}
