using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.ThirdParties;
using System;
using System.Collections.Generic;


namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    [AUT(AUT.Uk)]
    [Parallelizable(TestScope.Self)]
    public class ApplicationStatusChangedTests
    {
        private ServiceConfigurationEntity sfUsername; 
        private ServiceConfigurationEntity sfPassword;
        private ServiceConfigurationEntity sfUrl; 
        private Salesforce sales;
        private dynamic applicationRepo = Drive.Data.Payments.Db.Applications;
        private dynamic commsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private dynamic paymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;

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

        [Test]
        [AUT(AUT.Uk), JIRA("UK-819")]
        [Description("Verifies that after funds have been transferred to the customer application status 'Live' will be set in salesforce")]
        [Parallelizable]
        public void FundsTransferred_SubmitsApplicactionStatusLive_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .Build();

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent { AccountId = application.AccountId, 
                                                                  ApplicationId = application.Id, 
                                                                  TransactionId = Guid.NewGuid() });

            //wait for the payment to customer to be sent out
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));

            Do.Until(() =>{ var app = sales.GetApplicationById(application.Id);
                            return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live; });

            // wait until suppression record is created
            Do.Until(() =>commsSuppressionsRepo.FindAll(
                            commsSuppressionsRepo.AccountId == application.AccountId && commsSuppressionsRepo.ApplicationId == application.Id));
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-925")]
        [Description("Verifies that when a live application is moved to complaint status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInComplaint_SubmitsComplaintStatus_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .Build();

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                TransactionId = Guid.NewGuid()
            });

            //wait for the payment to customer to be sent out
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));

            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });


            // Make a complaint
            var cmd = new CsReportComplaintCommand()
                                {
                                    AccountId = application.AccountId,
                                    ApplicationId = application.Id,
                                    CaseId = Guid.NewGuid()
                                };
            Drive.Cs.Commands.Post(cmd);

            // wait until sales force moves to complaint
            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Complaint;
            });


            // wait until suppression record is created
            Do.Until(() => commsSuppressionsRepo.FindAll(
                            commsSuppressionsRepo.AccountId == application.AccountId && commsSuppressionsRepo.Complaint==1));
            Do.Until(() => paymentsSuppressionsRepo.FindAll(
                paymentsSuppressionsRepo.ApplicationId == application.Id && paymentsSuppressionsRepo.Complaint == 1));
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-925")]
        [Description("Verifies that when a live application is moved to complaint status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInBankruptcy_SubmitsBankruptStatus_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .Build();

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                TransactionId = Guid.NewGuid()
            });

            //wait for the payment to customer to be sent out
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));

            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });


            // report bankruptcy
            var cmd = new CsReportBankruptcyCommand()  
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = Guid.NewGuid()
            };
            Drive.Cs.Commands.Post(cmd);

            // wait until sales force moves to bankrupt
            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Bankrupt;
            });


            // wait until suppression record is created in comms and payments
            Do.Until(() => paymentsSuppressionsRepo.FindAll(
                paymentsSuppressionsRepo.ApplicationId == application.Id && paymentsSuppressionsRepo.Bankruptcy == 1));
            Do.Until(() => commsSuppressionsRepo.FindAll(
                            commsSuppressionsRepo.AccountId == application.AccountId && commsSuppressionsRepo.Bankruptcy == 1));

        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-925")]
        [Description("Verifies that when a live application is moved to complaint status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInHardship_SubmitsHardshipStatus_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .Build();

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                TransactionId = Guid.NewGuid()
            });

            //wait for the payment to customer to be sent out
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));

            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });


            // report hardship
            var cmd = new CsReportHardshipCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = Guid.NewGuid()
            };
            Drive.Cs.Commands.Post(cmd);

            // wait until sales force moves to hardship
            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Hardship;
            });


            // wait until suppression record is created in comms and payments
            Do.Until(() => paymentsSuppressionsRepo.FindAll(
                paymentsSuppressionsRepo.ApplicationId == application.Id && paymentsSuppressionsRepo.Hardship == 1));
            Do.Until(() => commsSuppressionsRepo.FindAll(
                            commsSuppressionsRepo.AccountId == application.AccountId && commsSuppressionsRepo.Hardship == 1));

        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1816")]
        [Description("Verifies that when a live application is moved to management review status salesforce is informed and a suppression record is created")]
        [Parallelizable]
        public void ApplicationInManagementReview_SubmitsManagementReviewStatus_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .Build();

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                TransactionId = Guid.NewGuid()
            });

            //wait for the payment to customer to be sent out
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));

            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });


            // Make a complaint
            var cmd = new CsReportManagementReviewCommand()
            {
                AccountId = application.AccountId,
                ApplicationId = application.Id,
                CaseId = Guid.NewGuid()
            };
            Drive.Cs.Commands.Post(cmd);

            // wait until sales force moves to complaint
            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.ManagementReview;
            });


        }


        [Test]
        [AUT(AUT.Uk), JIRA("UK-984")]
        [Description("Verifies that after funds have been transferred to the customer application status 'Live' will be set in salesforce")]
        public void VerifyThatPreLiveStatusesAreReflectedInSalesForce()
        {
            const decimal loanAmount = 222.22m;
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).Build();

            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));

            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent { AccountId = application.AccountId, 
                                                                  ApplicationId = application.Id,
                                                                  TransactionId = Guid.NewGuid() });

            bool present = ConfirmStatusValues(application.Id, new string[] { "Accepted", "Terms Agreed", "Live (Not Due)" });
            Assert.IsTrue(present);
        }

        private bool ConfirmStatusValues(Guid appId, IEnumerable<string> statuses)
        {
            var appHistory = sales.GetApplicationHistoryById(appId, "Status__c");
            var matched = from s in statuses 
                          from ah in appHistory 
                          where ah.NewValue != null && ah.NewValue.ToString().ToUpperInvariant() == s.ToUpperInvariant() 
                          select s;

            return matched.Count() == statuses.Count();
        }

        
    }
}