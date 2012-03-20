using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    public class SalesforcePushBusinessBankAccountDetails
    {
        [Test]
        public void SalesforceTC_ShouldPushBusinessBankAccountToSF_WhenBankAccountIsvalidated()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                Build() as BusinessApplication;

            var bankAccountId = Do.Until(organization.GetValidBankAccount);

            var salesforce = Driver.ThirdParties.Salesforce;

            var sfUsername = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
            var sfPassword = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
            var sfUrl = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");

            salesforce.SalesforceUsername = sfUsername.Value;
            salesforce.SalesforcePassword = sfPassword.Value;
            salesforce.SalesforceUrl = sfUrl.Value;

            Do.With().Message("Bank account for a given organization should exist in SF").Until(
                () => salesforce.GetBankAccountById(bankAccountId));
        }
    }
}
