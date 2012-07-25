using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Comms.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands;
using Wonga.QA.Framework.Db.Ops;
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
        public void FraudApplicationCompliantCycle()
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

            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
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
            int appInternalId=GetAppInternalId(application ) 
            var flagToDcaCommand = new FlagApplicationToDcaCommand
            {
                ApplicationId = application.Id
            };
            Drive.Cs.Commands.Post(flagToDcaCommand);
            Do.With.Timeout(1).Until(
               () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, FraudSuppression: true));
            
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
               () => _paymentsSuppressionsTable.FindBy(ApplicationId: appInternalId, FraudSuppression: false));
            
        }
        #endregion helpers#

    }
}
