using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
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
    class SalesforceApplicationComplaint
    {
        private ServiceConfigurationEntity sfUsername;
        private ServiceConfigurationEntity sfPassword;
        private ServiceConfigurationEntity sfUrl;
        private Framework.ThirdParties.Salesforce sales;
        private dynamic applicationRepo = Drive.Data.Payments.Db.Applications;
        private dynamic commsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private dynamic paymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
        private dynamic  _paymentsSuppressionsTable = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
        private readonly dynamic _repaymentArrangementsTab = Drive.Data.Payments.Db.RepaymentArrangements;

        #region setup#
        [SetUp]
        public void SetUp()
        {
            sfUsername = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.UserName");
            sfPassword = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Password");
            sfUrl = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Url");
            sales = Drive.ThirdParties.Salesforce;
            sales.SalesforceUsername = sfUsername.Value;
            sales.SalesforcePassword = sfPassword.Value;
            sales.SalesforceUrl = sfUrl.Value;
        }
        #endregion setup#

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void LiveApplicationCompliantCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
            CompliantCycle( caseId, application);
            CheckSalesApplicationStatus(application,(double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void DueTodayApplicationCompliantCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
            application.MakeDueToday();
            CompliantCycle( caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DueToday);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ArrearApplicationCompliantCycle()
        {
            var caseId = new Guid();
            var application = CreateLiveApplication();
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
            SuspectFraud(application,customer, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Fraud);
            CompliantCycle(caseId, application);
            ConfirmNotFraud(application,customer, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void DCAApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            Dca(application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.DCA);
            CompliantCycle(caseId, application);
            RevokeDca(application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
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
            MakeHardship(application,caseId );
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship );
            CompliantCycle(caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void BankruptApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            MakeBankrupt(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship);
            CompliantCycle(caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ManagementReviewApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            ManagementReview(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview );
            CompliantCycle(caseId, application);
            RemoveManagementReview(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void RefundApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            Refundrequest(application, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Refund);
            CompliantCycle(caseId, application);
            //RemoveManagementReview(application, caseId);
            //CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
        }


        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void PaidInFullApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var application = CreateLiveApplication();
            application.RepayOnDueDate();  
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.PaidInFull);
            CompliantCycle(caseId, application);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.PaidInFull);
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-129"), Owner(Owner.AnilKrishnamaneni)]
        public void ConfirmFraudApplicationCompliantCycle()
        {
            var caseId = Guid.NewGuid();
            var customer = CreateCustomer();
            var application = CreateApplication(customer);
            ConfirmFraud(application, customer, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.WrittenOff);
            CompliantCycle(caseId, application);
            ConfirmNotFraud(application, customer, caseId);
            CheckSalesApplicationStatus(application, (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live);
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
            return (applicationRepo.FindAll(applicationRepo.ExternalID == application.Id).Single().ApplicationId);
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
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Type == "CashAdvance"));

            //Do.Until(() =>
            //{
            //    var app = sales.GetApplicationById(application.Id);
            //    return app.Status_ID__c != null &&
            //           app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            //});
           
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

            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint;
            });
        }

        private void CheckSupressionTable(Application application)
        {
            int appInternalId = GetAppInternalId(application);
            Do.Until(() => commsSuppressionsRepo.FindAll(
                           commsSuppressionsRepo.AccountId == application.AccountId && commsSuppressionsRepo.Complaint == 1).Single());
            Do.Until(() => paymentsSuppressionsRepo.FindAll(
                paymentsSuppressionsRepo.ApplicationId == appInternalId && paymentsSuppressionsRepo.ComplaintSuppression == 1).Single());
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
            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == status;
            });
        }

        private void SuspectFraud(Application application,Customer cust, Guid caseId )
        {
            int appInternalId = GetAppInternalId(application);
            Drive.Cs.Commands.Post(new SuspectFraudCommand() { AccountId = cust.Id, CaseId = caseId });
            Do.With.Timeout(1).Until(
                () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, FraudSuppression: true));
        }

        private void ConfirmNotFraud(Application application,Customer cust, Guid caseId)
        {
            int appInternalId = GetAppInternalId(application);
            Drive.Cs.Commands.Post(new ConfirmNotFraudCommand() { AccountId = cust.Id , CaseId = caseId });
            Do.With.Timeout(1).Until(
                () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, FraudSuppression: false ));
        }

        private void Dca(Application  application)
        {
            int appInternalId = GetAppInternalId(application);
            var flagToDcaCommand = new FlagApplicationToDcaCommand
            {
                ApplicationId = application.Id
            };
            Drive.Cs.Commands.Post(flagToDcaCommand);
            Do.With.Timeout(1).Until(
               () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, DCASuppression: true));
            
        }

           private void RevokeDca(Application  application)
           {
               int appInternalId = GetAppInternalId(application);
            var flagToDcaCommand = new RevokeApplicationFromDcaCommand() 
            {
                ApplicationId = application.Id
            };
            Drive.Cs.Commands.Post(flagToDcaCommand);
            Do.With.Timeout(1).Until(
               () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, DCASuppression: false));
            
        }

        private void CancelRepaymnetArrangent(Application application)
        {

            var dbApplication = applicationRepo.FindAll(applicationRepo.ExternalId == application.Id).Single();
            var repaymentArrangement = Do.Until(() => _repaymentArrangementsTab.FindAll(_repaymentArrangementsTab.ApplicationId == dbApplication.ApplicationId).Single());
            Drive.Cs.Commands.Post(new CancelRepaymentArrangementCommand()
            {
                RepaymentArrangementId = repaymentArrangement.ExternalId
            });

            Do.Until(() => Drive.Data.Payments.Db.RepaymentArrangements.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).CanceledOn != null);
            
        }

        private static RepaymentArrangementEntity GetRepaymentArrangement(Application application)
        {
            var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            var repaymentArrangement = Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));

            return repaymentArrangement;
        }

        private void MakeHardship(Application application,Guid caseId)
        {
            int appInternalId = GetAppInternalId(application);
            Drive.Cs.Commands.Post(new CsReportHardshipCommand()
                                       {
                                           AccountId = application.AccountId,
                                           ApplicationId = application.Id,
                                           CaseId = caseId
                                       }); 
            Do.With.Timeout(1).Until(
             () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, HardshipSuppression: true));
        }

        private void MakeBankrupt(Application application, Guid caseId)
        {
            int appInternalId = GetAppInternalId(application);
            Drive.Cs.Commands.Post(new CsReportBankruptcyCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = caseId
            });
            Do.With.Timeout(1).Until(
             () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, BankruptSuppression: true));
        }

        private void ManagementReview(Application application, Guid caseId)
        {
            
            Drive.Cs.Commands.Post(new CsReportManagementReviewCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = caseId
            });
           
        }

        private void RemoveManagementReview(Application application, Guid caseId)
        {
            
            Drive.Cs.Commands.Post(new CsRemoveManagementReviewCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = caseId
            });
           
        }

        private void Refundrequest(Application application, Guid caseId)
        {
            Drive.Cs.Commands.Post(new CsReportRefundRequestCommand() 
            {
                ApplicationId = application.Id,
                CaseId = caseId
            });

        }

        private void ConfirmFraud(Application application, Customer cust, Guid caseId)
        {
            int appInternalId = GetAppInternalId(application);
            Drive.Cs.Commands.Post(new ConfirmFraudCommand() { AccountId = cust.Id, CaseId = caseId });
            Do.With.Timeout(1).Until(
                () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, FraudSuppression: true));
        }

        #endregion helpers#

    }
}
