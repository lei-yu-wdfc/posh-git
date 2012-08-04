using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using salesforceStatusAlias = Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus ;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.All)]
   class SalesforceApplicationComplaint
    {
        private Framework.ThirdParties.Salesforce _sales; 
        private readonly dynamic _loanDueDateNotifiSagaEntityTab = Drive.Data.OpsSagas.Db.LoanDueDateNotificationSagaEntity;
        private readonly dynamic _fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
        private readonly dynamic _commsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private readonly dynamic _paymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;

        #region setup#
        [SetUp]
        public void SetUp()
        {
            _sales=SalesforceOperations.SalesforceSetup();
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void AcceptedApplicationComplaintCycle()
        {  
            var caseId = Guid.NewGuid();
            var cust = CreateCustomer();
            var application = ApplicationBuilder.New(cust).WithOutSigning().Build();
            ComplaintCycle(caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Accepted );
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ComplaintCycle( caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application,(double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni) ]
        public void DueTodayApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            RewindDatesToMakeDueToday(application);
            MakeDueToday(application);
            ComplaintCycle( caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DueToday);
        }
        
        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears(3);
            ComplaintCycle( caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.InArrears);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CreateCustomer(); 
            var application =SalesforceOperations.CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Fraud);
            ComplaintCycle(caseId, application);
            ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("DCA not implemented")]
        public void DCAApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DCA);
            ComplaintCycle(caseId, application);
            ApplicationOperations.RevokeDca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("DMP Not implemneted") ]
        public void DmpRepaymentArrangementApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears();
            application.CreateRepaymentArrangement();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.RepaymentArrangement );
            ComplaintCycle(caseId, application);
            application.CancelRepaymentArrangement(); 
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void HardshipApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship );
            ComplaintCycle(caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void BankruptApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportBankrupt(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Bankrupt);
            ComplaintCycle(caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Bankrupt);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview );
            ComplaintCycle(caseId, application);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationComplaintCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Refund);
            ComplaintCycle(caseId, application);
            
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni), Pending("Waiting for Fix to RC")]
        public void ManagementReviewComplaintCycleWhileApplicationGoesDueToday()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.TermsAgreed .ToString(), salesforceStatusAlias.Live .ToString()); 
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.Live.ToString(), salesforceStatusAlias.ManagementReview.ToString()); 
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview);
            ReportComplaint(caseId, application);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.ManagementReview.ToString(), salesforceStatusAlias.Complaint.ToString()); 
            RewindDatesToMakeDueToday(application);
            MakeDueToday(application);
            ApplicationOperations.RemoveComplaint(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.Complaint .ToString(), salesforceStatusAlias.ManagementReview.ToString()); 
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.ManagementReview.ToString(), salesforceStatusAlias.DueToday.ToString()); 
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DueToday);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("Waiting for Fix to RC") ]
        public void ManagementReviewComplaintCycleWhileApplicationGoesDueTodayAndInToArrears()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.TermsAgreed.ToString(), salesforceStatusAlias.Live.ToString());
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.Live.ToString(), salesforceStatusAlias.ManagementReview.ToString());
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview);
            ReportComplaint(caseId, application);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.ManagementReview.ToString(), salesforceStatusAlias.Complaint.ToString());
            RewindDatesToMakeDueToday(application);
            MakeDueToday(application);
            application.PutIntoArrears(3);
            ApplicationOperations.RemoveComplaint(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.Complaint.ToString(), salesforceStatusAlias.ManagementReview.ToString());
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckPreviousStatus(application.Id, salesforceStatusAlias.ManagementReview.ToString(), salesforceStatusAlias.InArrears.ToString());
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.InArrears );
        }

        #region Helpers#

        private Application CreateLiveApplication()
        {
            var customer = CreateCustomer();
            Application application =SalesforceOperations.CreateApplication(customer);
            return application;
        }

        private static Customer CreateCustomer()
        {
            return CustomerBuilder.New().Build();
        }

        private void ComplaintCycle(Guid caseId, Application application)
        {
            ReportComplaint(caseId, application);
            ApplicationOperations.RemoveComplaint(application, caseId);
        }

        private void ReportComplaint(Guid caseId, Application application)
        {
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application,
                                                            (double) salesforceStatusAlias.Complaint);
            CheckSupressionTable(application);
        }

        private void CheckSupressionTable(Application application)
        {
            int appInternalId =ApplicationOperations.GetAppInternalId(application);
            Do.Until(() => _commsSuppressionsRepo.FindAll(
                           _commsSuppressionsRepo.AccountId == application.AccountId && _commsSuppressionsRepo.Complaint == 1).Single());
            Do.Until(() => _paymentsSuppressionsRepo.FindAll(
                _paymentsSuppressionsRepo.ApplicationId == appInternalId && _paymentsSuppressionsRepo.ComplaintSuppression == 1).Single());
        }

        public void MakeDueToday(dynamic application)
        {
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

        private void RewindDatesToMakeDueToday(Application application)
        {
            int id = ApplicationOperations.GetAppInternalId(application);
            TimeSpan span = _fixedTermLoanAppTab.FindByApplicationId(id).NextDueDate - DateTime.Today;
            application.RewindApplicationDates(span);
        }

        #endregion helpers#

    }
}
