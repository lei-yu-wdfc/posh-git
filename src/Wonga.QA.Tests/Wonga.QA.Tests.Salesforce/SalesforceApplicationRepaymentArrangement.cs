using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.All)]
    class SalesforceApplicationRepaymentArrangement
    {
        

        #region setup#
        [SetUp]
        public void SetUp()
        {
            SalesforceOperations.SalesforceSetup();
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationRepaymentArrangementCycle()
        {
            var customer = CustomerBuilder.New().Build();;
            var application = CreateLiveApplication(customer);
            RepaymentArrangementCycle(customer,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void DueTodayApplicationRepaymentArrangementCycle()
        {
            var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            SalesforceOperations.RewindDatesToMakeDueToday(application);
            SalesforceOperations.MakeDueToday(application);
            RepaymentArrangementCycle(customer,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationRepaymentArrangementCycle()
        {
            var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            application.PutIntoArrears(3);
            RepaymentArrangementCycle(customer,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationRepaymentArrangementCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();;
            var application = SalesforceOperations.CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            RepaymentArrangementCycle(customer,application);
        }
        
        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void HardshipApplicationRepaymentArrangementCycle()
        {
        	var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
            RepaymentArrangementCycle(customer,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ComplaintApplicationRepaymentArrangementpCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            ApplicationOperations.ReportComplaint(application, caseId);
            RepaymentArrangementCycle(customer,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationRepaymentArrangementCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview);
            RepaymentArrangementCycle(customer,application);
            ApplicationOperations.RemoveManagementReview(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationRepaymentArrangementCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            RepaymentArrangementCycle(customer,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni),Pending("Cancel RA") ]
        public void ManagementReviewRepaymentArrangementCycleWhileApplicationGoesDueTodayAndInToArrears()
        {
            var caseId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();; 
            var application = CreateLiveApplication(customer);
            SalesforceOperations.CheckPreviousStatus(application.Id, Framework.ThirdParties.Salesforce.ApplicationStatus.TermsAgreed.ToString(), Framework.ThirdParties.Salesforce.ApplicationStatus.Live.ToString());
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, Framework.ThirdParties.Salesforce.ApplicationStatus.Live.ToString(), Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview.ToString());
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview);
            application.CreateRepaymentArrangement();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
            SalesforceOperations.CheckPreviousStatus(application.Id, Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview.ToString(), Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement.ToString());
            SalesforceOperations.RewindDatesToMakeDueToday(application);
            SalesforceOperations.MakeDueToday(application);
            application.PutIntoArrears(3);
            application.CancelRepaymentArrangement();
            SalesforceOperations.CheckPreviousStatus(application.Id, Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement.ToString(), Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview.ToString());
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview.ToString(), Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears.ToString());
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
        }

        #region Helpers#

        private Application CreateLiveApplication(Customer customer)
        {
            Application application = SalesforceOperations.CreateApplication(customer);
            return application;
        }

        private void RepaymentArrangementCycle(Customer customer,Application application)
        {
            SalesforceOperations.CreateRepaymentArrangement(customer,application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
        }

        #endregion helpers#

    }
}
