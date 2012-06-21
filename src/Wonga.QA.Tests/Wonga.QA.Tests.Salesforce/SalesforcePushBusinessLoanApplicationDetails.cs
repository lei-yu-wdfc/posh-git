using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;
using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Payments.Csapi.Commands.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.FileStorage.InternalMessages.PaymentTransactionScopeEnum;
using UpdateLoanTermWbUkCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk.UpdateLoanTermWbUkCommand;

namespace Wonga.QA.Tests.Salesforce
{
    // Can't run in parallel for the moment, as this overloads SF TC and all of them timeout :)
    //[Parallelizable(TestScope.All)]
    [TestFixture]
    public class SalesforcePushBusinessLoanApplicationDetails : SalesforceTestBase
    {
    	private const string GetApplicationWithUpdatedTerm =
    		"Select l.Number_Of_Weeks__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.Number_Of_Weeks__c = {1}";

    	private const string GetApplicationWithSignedOnDate =
    		"Select l.SignedOn__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.SignedOn__c != null";

    	private const string GetApplicationWithUpdatedStatus =
    		"Select l.Status_ID__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.Status_ID__c = {1}";

        [Test, AUT(AUT.Wb), JIRA("SME-375")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushBusinessLoanApplicationDetailsToSF_WhenApplicationIsCreated()
		{
			var customer = CustomerBuilder.New().Build();
            //IF ANYONE LOOKS HERE ! PLEASE TELL ALEX P WHY YOU NEED 3 GUARANTORS ??? EVENT SO, WITH THE OLD CODE YOU DID NOT BUILD THEM
			//var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(3).Build();
            var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id));

			var salesforceApplication = Do.Until(() => Salesforce.GetApplicationById(application.Id));

			Assert.AreEqual(Convert.ToDouble(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.LoanAmount), salesforceApplication.Loan_Amount__c, "Expected loan amount is incorrect.");
			Assert.AreEqual(applicationEntity.ApplicationDate, salesforceApplication.Application_Date__c, "Expected applicationDate is incorrect.");
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushRelationshipBetweenBusinessLoanApplicationAndOrganisationToSF_WhenApplicationAndOrganisationAreBothCreated()
		{
			var customer = CustomerBuilder.New().Build();
			//var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(3).Build();
            var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var salesforceApplication = Do.Until(() => Salesforce.GetApplicationWithOrganisationById(application.Id, organisation.Id));

			Assert.AreEqual(organisation.Id.ToString(), salesforceApplication.Customer_Account__r.V3_Organization_Id__c, "Expected organisationId is incorrect.");
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
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

			var query = string.Format(GetApplicationWithUpdatedTerm, application.Id, newLoanTerm);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushBusinessLoanApplicationSignOnDateToSF_WhenAllGuarantorsHaveSignedApplicationTerms()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var query = string.Format(GetApplicationWithSignedOnDate, application.Id);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-375")] 
        [Ignore]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenMainApplicantIsAccepted()
		{
			var customer = CustomerBuilder.New().Build();
			//var organisation = OrganisationBuilder.New(customer).WithSoManySecondaryDirectors(2).Build();
            var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

            var query = String.Format(GetApplicationWithUpdatedStatus, application.Id, (int)Framework.ThirdParties.Salesforce.BusinessLoanApplicationStatus.AcceptedInPrinciple);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsAccepted()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var query = String.Format(GetApplicationWithUpdatedStatus, application.Id, (int)Framework.ThirdParties.Salesforce.BusinessLoanApplicationStatus.LoanApproved);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsDeclined()
		{
			var customer = CustomerBuilder.New().WithMiddleName("Middle").Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			var query = String.Format(GetApplicationWithUpdatedStatus, application.Id, (int)Framework.ThirdParties.Salesforce.BusinessLoanApplicationStatus.LoanDeclined);

			Do.Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));
		}

        [Test, AUT(AUT.Wb), JIRA("SME-811")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsAddedToInArrears()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears();

			var query = string.Format(GetApplicationWithUpdatedStatus, application.Id, (int)Framework.ThirdParties.Salesforce.BusinessLoanApplicationStatus.InArrears);

			Do.With.Timeout(2).Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, query));													
		}

        [Test, AUT(AUT.Wb), JIRA("SME-892")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void PaymentsShouldPushNewBusinessLoanApplicationStatusToSF_WhenApplicationIsRemovedFromArrears()
		{
			var customer = CustomerBuilder.New().Build();
			var organization = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organization)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears() as BusinessApplication;

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

			var removedFromArrearsQuery = string.Format(GetApplicationWithUpdatedStatus, application.Id, (int)Framework.ThirdParties.Salesforce.BusinessLoanApplicationStatus.Live);
			Do.With.Timeout(2).Until(() => Salesforce.GetApplicationByCustomQuery(application.Id, removedFromArrearsQuery));													
		}

        [Test, AUT(AUT.Wb), JIRA("SME-849")] 
        [Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
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
