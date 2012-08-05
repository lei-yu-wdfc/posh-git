using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using salesforceStatusAlias = Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.Self)]
    class SalesforceApplicationRefund
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
            RewindDatesToMakeDueToday(application);
            MakeDueToday(application);
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
