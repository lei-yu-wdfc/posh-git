using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using System;

namespace Wonga.QA.Tests.Salesforce
{
    [AUT(AUT.Uk)]
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.Self)]
    public class BankruptcyStatusChangedTests
    {

        #region setup#
        [SetUp]
        public void SetUp()
        {
            SalesforceOperations.SalesforceSetup();
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when a live application is moved to complaint status salesforce is informed and a suppression record is created")]
        public void ApplicationInBankruptcy_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is due today and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationDueToday_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            MakeDueToday(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in arrears and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationInArrears_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            application.PutIntoArrears(5);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in fraud and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationFraud_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            Customer customer = application.GetCustomer();
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh), Pending("DCA Not implemented")]
        [Description("Verifies that when an application is in DCA status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationDCA_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DCA);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in hardship and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationInHardhsip_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in a repayment arrangement and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationInRepaymentArrangement_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            SalesforceOperations.CreateRepaymentArrangement(application.GetCustomer(), application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in complaint status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationInComplaint_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in management review status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationInManagementReview_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview);
            ReportBankruptcy(application, caseId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in refund status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        public void ApplicationInRefund_SubmitsBankruptStatus_ToSalesforce()
        {
            var caseId = Guid.NewGuid();
            Application application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            ReportBankruptcy(application, caseId);
        }

        #region Helpers#

        private void ReportBankruptcy(dynamic application, Guid caseId)
        {
            ApplicationOperations.ReportBankrupt(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
        }

        private void MakeDueToday(dynamic application)
        {
            SalesforceOperations.RewindDatesToMakeDueToday(application);
            SalesforceOperations.MakeDueToday(application);
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