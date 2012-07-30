using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using salesforceStatusAlias = Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus ;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = 0), Pending("BI customer Management Bug preventing test from pass")]
    [Parallelizable(TestScope.All)]
   class SalesforceApplicationComplaint
    {
        private Framework.ThirdParties.Salesforce _sales; 
        private readonly dynamic _loanDueDateNotifiSagaEntityTab = Drive.Data.OpsSagas.Db.LoanDueDateNotificationSagaEntity;
        private readonly dynamic _fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
        private readonly dynamic _applicationRepo = Drive.Data.Payments.Db.Applications;
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
        public void AcceptedApplicationCompliantCycle()
        {  
            var caseId = Guid.NewGuid();
            var cust = CreateCustomer();
            var application = ApplicationBuilder.New(cust).WithOutSigning().Build();
            CompliantCycle(caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Accepted );
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            CompliantCycle( caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application,(double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni) ]
        public void DueTodayApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            int id =ApplicationOperations.GetAppInternalId(application);
            TimeSpan span = _fixedTermLoanAppTab.FindByApplicationId(id).NextDueDate - DateTime.Today;
            application.RewindApplicationDates(span);
            MakeDueToday(application);
            CompliantCycle( caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DueToday);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.ExpireCard();  
            application.PutIntoArrears(3);
            CompliantCycle( caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.InArrears);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CreateCustomer(); 
            var application =SalesforceOperations.CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Fraud);
            CompliantCycle(caseId, application);
            ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("DCA not implemented")]
        public void DCAApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DCA);
            CompliantCycle(caseId, application);
            ApplicationOperations.RevokeDca(application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("DMP Not implemneted") ]
        public void DmpRepaymentArrangementApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears();
            application.CreateRepaymentArrangement();
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.RepaymentArrangement );
            CompliantCycle(caseId, application);
            application.CancelRepaymentArrangement(); 
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void HardshipApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship );
            CompliantCycle(caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void BankruptApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportBankrupt(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Bankrupt);
            CompliantCycle(caseId, application);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Bankrupt);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview );
            CompliantCycle(caseId, application);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Refund);
            CompliantCycle(caseId, application);
            
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

        private void CompliantCycle(Guid caseId, Application application)
        {
            ApplicationOperations.ReportComplaint(application, caseId);
            SalesforceOperations.CheckSalesApplicationStatus(application,
                                        (double) salesforceStatusAlias.Complaint);
            CheckSupressionTable(application);
            ApplicationOperations.RemoveComplaint(application, caseId);
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

        #endregion helpers#

    }
}
