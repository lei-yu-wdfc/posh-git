using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    public class SalesforcePushBusinessLoanApplicationDetails
    {
		private Framework.ThirdParties.Salesforce salesforce;

		[SetUp]
		public void SetUp()
		{
			salesforce = Drive.ThirdParties.Salesforce;

			var sfUsername = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
			var sfPassword = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
			var sfUrl = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");

			salesforce.SalesforceUsername = sfUsername.Value;
			salesforce.SalesforcePassword = sfPassword.Value;
			salesforce.SalesforceUrl = sfUrl.Value;
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushBusinessLoanApplicationDetailsToSF_WhenApplicationIsCreated()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(3).Build();
            var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id));

			var salesforceApplication = Do.Until(() => salesforce.GetApplicationById(application.Id));

			Assert.AreEqual(Convert.ToDouble(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.LoanAmount), salesforceApplication.Loan_Amount__c, "Expected loan amount is incorrect.");
			Assert.AreEqual(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.OrganisationId.ToString(), salesforceApplication.Customer_Account__r.V3_Organization_Id__c, "Expected organisationId is incorrect.");
			Assert.AreEqual(applicationEntity.ApplicationDate, salesforceApplication.Application_Date__c, "Expected applicationDate is incorrect.");
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushBusinessLoanApplicationDetailsToSF_WhenApplicationTermIsUpdated()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id));
			var newLoanTerm = applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.Term + 1;

			Drive.Api.Commands.Post(new UpdateLoanTermWbUkCommand
			                         	{
			                         		ApplicationId = application.Id, 
											Term = newLoanTerm
			                         	});

			Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id
				&& a.BusinessFixedInstallmentLoanApplicationEntity.Term == newLoanTerm));

			var salesforceApplication = Do.Until(() => salesforce.GetApplicationById(application.Id));

			Assert.AreEqual(newLoanTerm, salesforceApplication.Number_Of_Weeks__c);
		}

        [Test, AUT(AUT.Wb), JIRA("SME-849")]
        public void PaymentsShouldPushLoanReferenceNumberToSFWhenApplicationIsCreated()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                Build();

            Do.With().Message("Salesforce should contain loan application with non-empty loan reference").Until(() => SalesforceContainsAppWithLoanReference(app));
        }

        private bool SalesforceContainsAppWithLoanReference(Application app)
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
