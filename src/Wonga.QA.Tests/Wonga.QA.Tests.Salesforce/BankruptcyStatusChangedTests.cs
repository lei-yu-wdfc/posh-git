using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using System;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture]
    [AUT(AUT.Uk)]
    [Parallelizable(TestScope.Self)]
    public class BankruptcyStatusChangedTests
    {
        private Framework.ThirdParties.Salesforce _sales;
        private readonly dynamic _loanDueDateNotifiSagaEntityTab = Drive.Data.OpsSagas.Db.LoanDueDateNotificationSagaEntity;
        private readonly dynamic _fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;

        #region setup#
        [SetUp]
        public void SetUp()
        {
            _sales = SalesforceOperations.SalesforceSetup();
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
            application.PutIntoArrears(10);
            application.CreateRepaymentArrangement();
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

        private void RewindApplicationDates(dynamic application)
        {
            int id = ApplicationOperations.GetAppInternalId(application);
            TimeSpan span = _fixedTermLoanAppTab.FindByApplicationId(id).NextDueDate - DateTime.Today;
            application.RewindApplicationDates(span);
        }

        private void MakeDueToday(dynamic application)
        {
            RewindApplicationDates(application);
            var ldd = _loanDueDateNotifiSagaEntityTab.FindAll(_loanDueDateNotifiSagaEntityTab.ApplicationId == application.Id).Single();
            if (Drive.Data.Ops.GetServiceConfiguration<bool>("Payments.FeatureSwitches.UseLoanDurationSaga") == false)
            {
                Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { SagaId = ldd.Id });
                _loanDueDateNotifiSagaEntityTab.Update(ldd);
            }
            else
            {
                //We should timeout the LoanDurationSaga...
                dynamic loanDurationSagaEntities = Drive.Data.OpsSagas.Db.LoanDurationSagaEntity;
                var loanDurationSaga = loanDurationSagaEntities.FindAllByAccountGuid(AccountGuid: application.AccountId).FirstOrDefault();
                Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage() { SagaId = loanDurationSaga.Id });
            }
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