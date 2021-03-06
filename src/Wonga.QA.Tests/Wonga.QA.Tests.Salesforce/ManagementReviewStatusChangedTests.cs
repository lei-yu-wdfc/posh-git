﻿using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using System;

namespace Wonga.QA.Tests.Salesforce
{

    [TestFixture(Order = -1)]
    [AUT(AUT.Uk)]
    [Parallelizable(TestScope.Self)]
    public class ManagementReviewStatusChangedTests
    {

        #region setup#
        [SetUp]
        public void SetUp()
        {
            SalesforceOperations.SalesforceSetup();
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in due today status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationDueToday_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.ExpireCard();
            application.MakeDueToday();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in arrears status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationInArrears_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears(5);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
        }


        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in fraud status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationFraud_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            Customer customer = application.GetCustomer();
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in hardship status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationHardship_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in bankrupt status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationBankrupt_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportBankrupt(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in complaint status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationComplaint_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in Repayment Arrangement status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationRepaymentArrangement_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            SalesforceOperations.CreateRepaymentArrangement(application.GetCustomer(), application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in Refund status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationInRefund_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh), Pending("DCA Not implemented")]
        [Description("Verifies that when an application is in DCA status and it is moved to management review status salesforce is informed. When the management review status is removed return to previous status")]
        public void ApplicationInDCA_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DCA);
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DCA);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-140"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in live status and it is moved to management review status salesforce is informed")]
        public void ApplicationLive_SubmitsManagementReview_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ManagementReview(application, caseId);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        #region Helpers#

        private void ManagementReview(dynamic application, Guid caseId)
        {
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview);
        }

        private Application CreateLiveApplication()
        {
            var customer = CustomerBuilder.New().Build();
            Application application = SalesforceOperations.CreateApplication(customer);
            return application;
        }

        #endregion helpers#

    }
}