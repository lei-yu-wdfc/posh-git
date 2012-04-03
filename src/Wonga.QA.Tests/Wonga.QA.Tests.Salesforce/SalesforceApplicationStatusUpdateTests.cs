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

namespace Wonga.QA.Tests.Salesforce
{
    [JIRA("UK-924")]
    [AUT(AUT.Uk)]
    public class SalesforceApplicationStatusUpdateTests : SalesforceTestBase
    {
        private static Application CreateApplication(out Customer customer)
        {
            decimal loanAmount = 350m;
            int loanTerm = 12;
            customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Application application = ApplicationBuilder.New(customer)
                .WithLoanTerm(loanTerm)
                .WithLoanAmount(loanAmount).Build();
            return application;
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-924")]
        public void MakeDueToday_ChangesSalesforceApplicationStatus_ToDueToday()
        {
            Customer customer;
            Application application = CreateApplication(out customer);
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

        [Test]
        [AUT(AUT.Uk), JIRA("UK-924")]
        [Ignore("PaymentSchedulingSaga issue in UK prevents this from working atm")]
        public void PutIntoArrears_ChangesSalesforceApplicationStatus_ToInArrears()
        {
            Customer customer;
            Application application = CreateApplication(out customer);
            FixedTermLoanSagaEntity loanSagaEntity = null;
            ScheduledPaymentSagaEntity paymentSagaEntity = null;
            Do.Until(() => loanSagaEntity = Drive.Db.OpsSagas.FixedTermLoanSagaEntities.SingleOrDefault(s => s.ApplicationGuid == application.Id));
            application.MakeDueToday();
            Do.Until(() => paymentSagaEntity = Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.SingleOrDefault(s => s.ApplicationGuid == application.Id));
            Drive.Msmq.Payments.Send(new TimeoutMessage() { SagaId = paymentSagaEntity.Id });
            Do.With.While(paymentSagaEntity.Refresh);
            Do.Until(() => paymentSagaEntity = Drive.Db.OpsSagas.ScheduledPaymentSagaEntities
                .SingleOrDefault(s => s.ApplicationGuid == application.Id && s.PaymentsAttempted == 2));
            Drive.Msmq.Payments.Send(new TimeoutMessage() { SagaId = paymentSagaEntity.Id });
            Do.Until(() =>
            {
                var app = Salesforce.GetApplicationById(application.Id);
                //NOTE: this string should not be hardcoded or we shoud use Status_ID__c
                return app.Status__c == @"In Arrears";
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