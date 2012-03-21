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
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    public class SalesforcePushBusinessLoanApplicationDetails
    {
        [Test, AUT(AUT.Wb), JIRA("SME-849")]
        public void PaymentsShoulPushLoanReferenceNumberToSFWhenApplicaitonIsCreated()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                Build();

            var salesforce = Drive.ThirdParties.Salesforce;

            var sfUsername = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
            var sfPassword = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
            var sfUrl = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");

            salesforce.SalesforceUsername = sfUsername.Value;
            salesforce.SalesforcePassword = sfPassword.Value;
            salesforce.SalesforceUrl = sfUrl.Value;

            Do.With().Message("Salesforce should contain loan application with non-empty loan reference").Until(() => SalesforceContainsAppWithLoanReference(app, salesforce));
        }

        private bool SalesforceContainsAppWithLoanReference(Application app, Framework.ThirdParties.Salesforce salesforce)
        {
            var sfLoanApplication = salesforce.GetApplicationById(app.Id);
            if (sfLoanApplication != null)
            {
                return !string.IsNullOrEmpty(sfLoanApplication.CCIN__c);
            }
            return false;
        }
    }
}
