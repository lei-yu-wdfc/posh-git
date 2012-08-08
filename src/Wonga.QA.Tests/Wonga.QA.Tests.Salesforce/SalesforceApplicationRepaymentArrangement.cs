using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.Self)]
    [Pending("Repayment Arrangement bug 823")]
    class SalesforceApplicationRepaymentArrangement
    {
        private readonly dynamic _commsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private readonly dynamic _paymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;

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
            var application = CreateLiveApplication();
            RepaymentArrangementCycle( application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void DueTodayApplicationRepaymentArrangementCycle()
        {
            var application = CreateLiveApplication();
            SalesforceOperations.RewindDatesToMakeDueToday(application);
            SalesforceOperations.MakeDueToday(application);
            RepaymentArrangementCycle( application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationRepaymentArrangementCycle()
        {
            var application = CreateLiveApplication();
            application.PutIntoArrears(3);
            RepaymentArrangementCycle( application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationRepaymentArrangementCycle()
        {
            var caseId = new Guid();
            var customer = CreateCustomer();
            var application = SalesforceOperations.CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            RepaymentArrangementCycle( application);
            ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }
        
        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void HardshipApplicationRepaymentArrangementCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
            RepaymentArrangementCycle( application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void BankruptApplicationRepaymentArrangementCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportBankrupt(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
            RepaymentArrangementCycle( application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ComplaintApplicationRepaymentArrangementpCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportComplaint(application, caseId);
            RepaymentArrangementCycle(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationRepaymentArrangementCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview);
            RepaymentArrangementCycle( application);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationRepaymentArrangementCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            RepaymentArrangementCycle( application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-62"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewRepaymentArrangementCycleWhileApplicationGoesDueTodayAndInToArrears()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
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

        private Application CreateLiveApplication()
        {
            var customer = CreateCustomer();
            Application application = SalesforceOperations.CreateApplication(customer);
            return application;
        }

        private static Customer CreateCustomer()
        {
            return CustomerBuilder.New().Build();
        }

        private void RepaymentArrangementCycle(Application application)
        {
            application.CreateRepaymentArrangement();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
            application.CancelRepaymentArrangement();
        }

        private void ReportComplaint( Application application)
        {
            var caseId = new Guid();
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application,
                                                            (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint);
            CheckSupressionTable(application);
        }

        private void CheckSupressionTable(Application application)
        {
            int appInternalId = ApplicationOperations.GetAppInternalId(application);
            Do.Until(() => _commsSuppressionsRepo.FindAll(
                           _commsSuppressionsRepo.AccountId == application.AccountId && _commsSuppressionsRepo.Complaint == 1).Single());
            Do.Until(() => _paymentsSuppressionsRepo.FindAll(
                _paymentsSuppressionsRepo.ApplicationId == appInternalId && _paymentsSuppressionsRepo.ComplaintSuppression == 1).Single());
        }

      
        #endregion helpers#

    }
}
