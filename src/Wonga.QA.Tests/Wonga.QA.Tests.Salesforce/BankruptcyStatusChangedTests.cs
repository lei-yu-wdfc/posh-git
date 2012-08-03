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
    [Pending("BI customer Management Bug preventing test from pass")]
    public class BankruptcyStatusChangedTests
    {
        private Framework.ThirdParties.Salesforce _sales;
        private readonly dynamic _applicationRepo = Drive.Data.Payments.Db.Applications;
        private readonly dynamic _commsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private readonly dynamic _paymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
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
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.JonHurd)]
        [Description("Verifies that when a live application is moved to complaint status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInBankruptcy_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is due today and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationDueToday_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            MakeDueToday(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in arrears and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInArrears_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            application.PutIntoArrears(5);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in fraud and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationFraud_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            Customer customer = CustomerBuilder.New().Build();
            ApplicationOperations.ConfirmFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in DCA status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationDCA_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DCA);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in hardship and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInHardhsip_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in a repayment arrangement and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInRepaymentArrangement_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            application.CreateRepaymentArrangement();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in complaint status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInComplaint_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in management review status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInManagementReview_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-149"), Owner(Owner.ShaneMcHugh)]
        [Description("Verifies that when an application is in refund status and it is moved to bankrupt status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInRefund_SubmitsBankruptStatus_ToSalesforce()
        {
            int appInternalId;
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication(out appInternalId);
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            ReportBankruptcy(application, caseId, appInternalId);
        }

        #region Helpers#

        private void ReportBankruptcy(dynamic application, Guid caseId, int appInternalId)
        {
            ApplicationOperations.ReportBankrupt(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);

            // wait until suppression record is created
            Do.Until(() => _commsSuppressionsRepo.FindAll(
                            _commsSuppressionsRepo.AccountId == application.AccountId && _commsSuppressionsRepo.Bankruptcy == 1).Single());
            Do.Until(() => _paymentsSuppressionsRepo.FindAll(
                _paymentsSuppressionsRepo.ApplicationId == appInternalId && _paymentsSuppressionsRepo.BankruptcySuppression == 1).Single());
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

        /// <summary>
        /// create a live application and confirm that salesforce knows its live
        /// </summary>
        /// <returns></returns>
        private Application CreateLiveApplication(out int appInternalId)
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                .WithLoanAmount(loanAmount)
                .Build();

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferred
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                TransactionId = Guid.NewGuid()
            });

            //wait for the payment to customer to be sent out
            Do.Until(() => _applicationRepo.FindAll(_applicationRepo.ExternalId == application.Id &&
                                                   _applicationRepo.Transaction.ApplicationId == _applicationRepo.Id &&
                                                   _applicationRepo.Amount == loanAmount && _applicationRepo.Type == "CashAdvance"));

            appInternalId = _applicationRepo.FindAll(_applicationRepo.ExternalID == application.Id).Single().ApplicationId;

            Do.Until(() =>
            {
                var app = _sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null &&
                       app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });
            return application;
        }

        #endregion helpers#
    }
}