using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Comms.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture(Order=0)]
    [Parallelizable(TestScope.All)]
   class SalesforceApplicationComplaint
    {
        private ServiceConfigurationEntity _sfUsername;
        private ServiceConfigurationEntity _sfPassword;
        private ServiceConfigurationEntity _sfUrl;
        private Framework.ThirdParties.Salesforce _sales;
        private readonly dynamic _applicationRepo = Drive.Data.Payments.Db.Applications;
        private readonly dynamic _commsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private readonly dynamic _paymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;

        #region setup#
        [SetUp]
        public void SetUp()
        {
            _sfUsername = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.UserName");
            _sfPassword = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Password");
            _sfUrl = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Url");
            _sales = Drive.ThirdParties.Salesforce;
            _sales.SalesforceUsername = _sfUsername.Value;
            _sales.SalesforcePassword = _sfPassword.Value;
            _sales.SalesforceUrl = _sfUrl.Value;
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void UserPendingApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var cust = CreateCustomer();
            var application = ApplicationBuilder.New(cust).WithOutSigning().Build();
            CompliantCycle(caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Accepted );
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            CompliantCycle( caseId, application);
            CheckSalesApplicationStatus(application,(double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni) , Pending("Want to figure how to make application due to day") ]
        public void DueTodayApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.MakeDueToday();
            CompliantCycle( caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
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
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.InArrears);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void SuspectFraudApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CreateCustomer(); 
            var application = CreateApplication(customer);
            ApplicationOperations.SuspectFraud(application, customer, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            CompliantCycle(caseId, application);
            ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("DCA not implemented")]
        public void DCAApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Dca(application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DCA);
            CompliantCycle(caseId, application);
            ApplicationOperations.RevokeDca(application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni),Pending("DMP Not implemneted") ]
        public void DmpRepaymentArrangementApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.PutIntoArrears();
            application.CreateRepaymentArrangement();
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.RepaymentArrangement );
            CompliantCycle(caseId, application);
            application.CancelRepaymentArrangement(); 
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void HardshipApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportHardship(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship );
            CompliantCycle(caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void BankruptApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ReportBankrupt(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
            CompliantCycle(caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.ManagementReview(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview );
            CompliantCycle(caseId, application);
            ApplicationOperations.RemoveManagementReview(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ApplicationOperations.Refundrequest(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            CompliantCycle(caseId, application);
            
        }

        #region Helpers#

        private Application CreateLiveApplication()
        {
            var customer = CreateCustomer();
            Application application =CreateApplication(customer);
            return application;
        }

        private static Customer CreateCustomer()
        {
            return CustomerBuilder.New().Build();
        }

        private int GetAppInternalId(Application application)
        {
            return (_applicationRepo.FindAll(_applicationRepo.ExternalID == application.Id).Single().ApplicationId);
        }

        private Application CreateApplication(Customer customer)
        {
            var application=ApplicationBuilder.New(customer)
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
                                                   _applicationRepo.Type == "CashAdvance"));
          
            Do.With.Timeout(2).Until(() =>
            {
                var app = _sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null &&
                       app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });
           
            return application;
        }
        
        private void CompliantCycle(Guid caseId, Application application)
        {
            ReportComplaint(application, caseId);
            CheckSupressionTable(application);
            RemoveComplaint(application, caseId);
        }
       
        private void ReportComplaint(Application application,Guid caseId)
        {
            var cmd = new CsReportComplaintCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = caseId 
            };

            Drive.Cs.Commands.Post(cmd);

            Do.With.Timeout(2).Until(() =>
            {
                var app = _sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint;
            });
        }

        private void CheckSupressionTable(Application application)
        {
            int appInternalId = GetAppInternalId(application);
            Do.Until(() => _commsSuppressionsRepo.FindAll(
                           _commsSuppressionsRepo.AccountId == application.AccountId && _commsSuppressionsRepo.Complaint == 1).Single());
            Do.Until(() => _paymentsSuppressionsRepo.FindAll(
                _paymentsSuppressionsRepo.ApplicationId == appInternalId && _paymentsSuppressionsRepo.ComplaintSuppression == 1).Single());
        }

        private void RemoveComplaint(Application application,Guid caseId)
        {
            var removeComplaint = new CsRemoveComplaintCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = caseId,
            };
            Drive.Cs.Commands.Post(removeComplaint);
        }
        
        private void CheckSalesApplicationStatus(Application application,double status)
        {
            Do.With.Timeout(2).Until(() =>
            {
                var app = _sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == status;
            });
        }

       
        #endregion helpers#

    }
}
