using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    public class SalesforcePushBusinessBankAccountDetails : SalesforceTestBase
    {
        [Test, AUT(AUT.Wb), JIRA("SME-192"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
        public void SalesforceTC_ShouldPushBusinessBankAccountToSF_WhenBankAccountIsvalidated()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                Build() as BusinessApplication;

            var bankAccountId = Do.Until(organization.GetValidBankAccount);

            Do.With.Message("Bank account for a given organization should exist in SF").Until(() => Salesforce.GetBankAccountById(bankAccountId));
        }
    }
}
