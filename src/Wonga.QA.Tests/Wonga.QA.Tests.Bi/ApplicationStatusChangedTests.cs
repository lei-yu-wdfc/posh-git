using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
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
    public class ApplicationStatusChangedTests
    {
        private ServiceConfigurationEntity sfUsername; 
        private ServiceConfigurationEntity sfPassword;
        private ServiceConfigurationEntity sfUrl; 
        private Salesforce sales;
        private dynamic applicationRepo = Drive.Data.Payments.Db.Applications;

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
        public void FundsTransferred_SubmitsApplicactionStatusLive_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .Build();

            //wait for the payment to customer to be sent out
            Do.Until(() => applicationRepo.FindAll(applicationRepo.ExternalId == application.Id &&
                                                   applicationRepo.Transaction.ApplicationId == applicationRepo.Id &&
                                                   applicationRepo.Amount == loanAmount && applicationRepo.Type == "CashAdvance"));
            
            sales.SalesforceUsername = sfUsername.Value;
            sales.SalesforcePassword = sfPassword.Value;
            
            sales.SalesforceUrl = sfUrl.Value;
            Do.Until(() =>{ var app = sales.GetApplicationById(application.Id);
                            return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live; });
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