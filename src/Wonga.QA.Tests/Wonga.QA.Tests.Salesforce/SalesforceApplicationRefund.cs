﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using salesforceStatusAlias = Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.Self)]
    class SalesforceApplicationRefund
    {
        #region setup#
        [SetUp]
        public void SetUp()
        {
            SalesforceOperations.SalesforceSetup();
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
        public void DueTodayApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.ExpireCard();  
            application .MakeDueToday(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DueToday);
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears(3);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.InArrears);
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var customer = CreateCustomer();
            var application = SalesforceOperations.CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Fraud);
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni), Pending("DCA not implemented")]
        public void DCAApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DCA);
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni), Pending("DMP Not implemneted")]
        public void DmpRepaymentArrangementApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears();
            application.CreateRepaymentArrangement();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.RepaymentArrangement);
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
        public void HardshipApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship);
            Refund(caseId, application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
        public void ComplaintApplicationHardshipCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Complaint);
            Refund(caseId,application);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationRefund()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview);
            Refund(caseId, application);
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

        private void Refund(Guid caseId, Application application)
        {
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application,(double)salesforceStatusAlias.Refund);
        }

       #endregion helpers#
    }
}
