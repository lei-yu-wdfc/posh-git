using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Salesforce;

namespace Wonga.QA.Tests.Payments
{
    [JIRA("UK-924")]
    public class LoanDueNotificationSagaTests : SalesforceTestBase
    {
        [Test]
        [AUT(AUT.Uk), JIRA("UK-924")]
        public void SubmitApplication_CreatesSagaEntity()
        {
            decimal loanAmount = 350m;
            int loanTerm = 12;
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Application application = ApplicationBuilder.New(customer)
                .WithLoanTerm(loanTerm)
                .WithLoanAmount(loanAmount).Build();
            Do.Until(() => Drive.Db.OpsSagas.LoanDueDateNotificationSagaEntities
                               .Single(saga => saga.AccountId == customer.Id
                                               && saga.ApplicationId == application.Id
                                               && saga.TermsAgreed == true
                                               && saga.ApplicationAccepted == true));
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-924")]
        public void MakeDueToday_ChangesSalesforceApplicationStatus_ToDueToday()
        {
            decimal loanAmount = 350m;
            int loanTerm = 12;
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Application application = ApplicationBuilder.New(customer)
                .WithLoanTerm(loanTerm)
                .WithLoanAmount(loanAmount).Build();
            Do.Until(() => Drive.Db.OpsSagas.LoanDueDateNotificationSagaEntities
                               .Single(saga => saga.AccountId == customer.Id
                                               && saga.ApplicationId == application.Id
                                               && saga.TermsAgreed == true
                                               && saga.ApplicationAccepted == true));
            application.MakeDueStatusToday();
            Do.Until(() =>
                         {
                             var app = Salesforce.GetApplicationById(application.Id);
                             //NOTE: this string should not be hardcoded or we shoud use Status_ID__c
                             return app.Status__c == @"Due Today";
                         });
        }
    }
}