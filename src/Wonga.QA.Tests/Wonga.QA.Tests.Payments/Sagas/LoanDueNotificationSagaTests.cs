using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
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
            MakeDueStatusToday(application);
            Do.Until(() =>
                         {
                             var app = Salesforce.GetApplicationById(application.Id);
                             //NOTE: this string should not be hardcoded or we shoud use Status_ID__c
                             return app.Status__c == @"Due Today";
                         });
        }

        private void MakeDueStatusToday(Application application)
        {
            FixedTermLoanApplicationEntity app = Drive.Db.Payments.FixedTermLoanApplications.Single(b => b.ApplicationEntity.ExternalId == application.Id);
            TimeSpan span = app.NextDueDate.Value - DateTime.Today;
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            ApplicationEntity applicationEntity = app.ApplicationEntity;
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, span);

            LoanDueDateNotificationSagaEntity dueNotificationSaga = Drive.Db.OpsSagas.LoanDueDateNotificationSagaEntities.Single(s => s.ApplicationId == application.Id);
            Drive.Msmq.Payments.Send(new TimeoutMessage() { SagaId = dueNotificationSaga.Id });
            Do.With.While(dueNotificationSaga.Refresh);
        }

    }
}