﻿using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using salesforceStatusAlias = Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.Self)]
    class SalesforceApplicationHardship
    {
        private readonly dynamic _fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
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
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            HardshipCycle(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void DueTodayApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            int id = ApplicationOperations.GetAppInternalId(application);
            TimeSpan span = _fixedTermLoanAppTab.FindByApplicationId(id).NextDueDate - DateTime.Today;
            application.RewindApplicationDates(span);
            SalesforceOperations.MakeDueToday(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DueToday);
            HardshipCycle(caseId, application);
         }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears(3);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.InArrears);
            HardshipCycle(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CreateCustomer();
            var application =SalesforceOperations.CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Fraud);
            HardshipCycle(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni), Pending("DCA not implemented")]
        public void DCAApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DCA);
            HardshipCycle(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void ComplaintApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Complaint );
            HardshipCycle(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview);
            HardshipCycle(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Refund);
            HardshipCycle(caseId, application);
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
        
        private void HardshipCycle(Guid caseId, Application application)
        {
            ApplicationOperations.ReportHardship(application, caseId);
            CheckSupressionTable(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship );
        }

        private void CheckSupressionTable(Application application)
        {
            int appInternalId = ApplicationOperations.GetAppInternalId(application);
            Do.Until(() => _commsSuppressionsRepo.FindAll(
                           _commsSuppressionsRepo.AccountId == application.AccountId && _commsSuppressionsRepo.Hardship == 1).Single());
            Do.Until(() => _paymentsSuppressionsRepo.FindAll(
                _paymentsSuppressionsRepo.ApplicationId == appInternalId && _paymentsSuppressionsRepo.HardshipSuppression == 1).Single());
        }

        #endregion helpers#

    }
}
