using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using UpdateLoanTermWbUkCommand = Wonga.QA.Framework.Api.UpdateLoanTermWbUkCommand;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    public class SalesforcePushBusinessLoanApplicationDetails : SalesforceTestBase
    {
    	private const string getApplicationWithUpdatedTerm =
    		"Select l.Number_Of_Weeks__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.Number_Of_Weeks__c = {1}";

    	private const string getApplicationWithSignedOnDate =
    		"Select l.SignedOn__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.SignedOn__c != null";

    	private const string getApplicationWithUpdatedStatus =
    		"Select l.Status_ID__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.Status_ID__c = {1}";

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushBusinessLoanApplicationDetailsToSF_WhenApplicationIsCreated()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(3).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id));

			var salesforceApplication = Do.Until(() => Salesforce.GetApplicationById(application.Id));

			Assert.AreEqual(Convert.ToDouble(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.LoanAmount), salesforceApplication.Loan_Amount__c, "Expected loan amount is incorrect.");
			Assert.AreEqual(applicationEntity.ApplicationDate, salesforceApplication.Application_Date__c, "Expected applicationDate is incorrect.");
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushRelationshipBetweenBusinessLoanApplicationAndOrganisationToSF_WhenApplicationAndOrganisationAreBothCreated()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(3).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var salesforceApplication = Do.Until(() => Salesforce.GetApplicationWithOrganisationById(application.Id, organisation.Id));

			Assert.AreEqual(organisation.Id.ToString(), salesforceApplication.Customer_Account__r.V3_Organization_Id__c, "Expected organisationId is incorrect.");
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

			var query = string.Format(getApplicationWithUpdatedTerm, application.Id, newLoanTerm);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushBusinessLoanApplicationSignOnDateToSF_WhenAllGuarantorsHaveSignedApplicationTerms()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var query = string.Format(getApplicationWithSignedOnDate, application.Id);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375"), Explicit]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenMainApplicantIsAccepted()
		{
			const int acceptedInPrincipleStatus = 101;
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(2).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var query = String.Format(getApplicationWithUpdatedStatus, application.Id, acceptedInPrincipleStatus);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsAccepted()
		{
			const int acceptedStatus = 111;
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var query = String.Format(getApplicationWithUpdatedStatus, application.Id, acceptedStatus);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsDeclined()
		{
			const int declinedStatus = 110;
	
			var customer = CustomerBuilder.New().WithMiddleName("Middle").Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			var query = String.Format(getApplicationWithUpdatedStatus, application.Id, declinedStatus);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-811")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsAddedToInArrears()
		{
			const int inArrearsStatus = 113;

			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutApplicationIntoArrears();

			var query = string.Format(getApplicationWithUpdatedStatus, application.Id, inArrearsStatus);

			Do.With.Timeout(2).Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));													
		}

		[Test, AUT(AUT.Wb), JIRA("SME-892")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsRemovedFromArrears()
		{
			const int loanLiveStatus = 112;

			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutApplicationIntoArrears() as BusinessApplication;

			var arrearsAmount = application.GetArrearsAmount();

			//fire transaction for pay off arrears
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = arrearsAmount,
				ApplicationId = application.Id,
				Currency = CurrencyCodeIso4217Enum.GBP,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.Now,
				Scope = PaymentTransactionScopeEnum.Credit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Cheque
			});

			var removedFromArrearsQuery = string.Format(getApplicationWithUpdatedStatus, application.Id, loanLiveStatus);
			Do.With.Timeout(2).Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, removedFromArrearsQuery));													
		}

        [Test, AUT(AUT.Wb), JIRA("SME-849")]
        public void PaymentsShouldPushLoanReferenceNumberToSFWhenApplicationIsCreated()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                Build();

            Do.With.Message("Salesforce should contain loan application with non-empty loan reference").Until(() => SalesforceContainsAppWithLoanReference(app));
        }

        private bool SalesforceContainsAppWithLoanReference(Application app)
        {
            var sfLoanApplication = Salesforce.GetApplicationById(app.Id);
            if (sfLoanApplication != null)
            {
                return !string.IsNullOrEmpty(sfLoanApplication.CCIN__c);
            }
            return false;
        }
    }
}
