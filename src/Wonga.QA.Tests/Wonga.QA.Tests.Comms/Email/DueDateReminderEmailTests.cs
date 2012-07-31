using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
    [AUT(AUT.Uk)]
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class DueDateReminderEmailTests
    {
        private Customer _customer;
        private Application _application;
        private decimal _loanAmount;
        private DateTime _promiseDate;

        private dynamic _dueDateReminderSagaTable;
        private dynamic _servicConfigTable;
        private dynamic _fixedTermAppsTable;
        private dynamic _applicationsTable;
        private dynamic _emailTable;

        const string NotificationThresholdCfgKey = "Payments.DueDateNotificationThresholdTimeInMinutes";


        [SetUp]
        public void Setup()
        {
            _dueDateReminderSagaTable = Drive.Data.OpsSagas.Db.DueDateApproachingNotificationSagaEntity;
            _servicConfigTable = Drive.Data.Ops.Db.ServiceConfigurations;
            _fixedTermAppsTable = Drive.Data.Payments.Db.FixedTermLoanApplications;
            _applicationsTable = Drive.Data.Payments.Db.Applications;
            _emailTable = Drive.Data.QaData.Db.Email;

            _customer = CustomerBuilder.New().Build();
            _loanAmount = 222.22m;
        }


        private void BuildApplicationWithPromiseDate(DateTime date)
        {
            _application = ApplicationBuilder.New(_customer)
                .WithLoanAmount(_loanAmount)
                .WithPromiseDate(new Date(date))
                .Build();
        }

        private void BuildApplicationWithTerm(int term)
        {
            _application = ApplicationBuilder.New(_customer)
                .WithLoanAmount(_loanAmount)
                .WithLoanTerm(term)
                .Build();
        }

        [AUT(AUT.Uk), Owner(Owner.PiotrWalat)]
        [Test]
        public void LiveFixedTermLoan_ComingDue_SendsA1TemplateEmailToCustomer()
        {
            _promiseDate = DateTime.UtcNow.AddDays(12);
            BuildApplicationWithPromiseDate(_promiseDate);

            dynamic reminderSaga = null;
            Do.Until(() => reminderSaga = _dueDateReminderSagaTable.FindBy(ApplicationId: _application.Id));
            dynamic threshold = _servicConfigTable.FindBy(Key: NotificationThresholdCfgKey);
            int thresholdInDays = 3;
            if(threshold != null)
            {
                thresholdInDays = Int32.Parse(threshold.Value);
            }
            dynamic fixedTermApp = null;
            dynamic app = null;
            Do.Until(() => app = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => fixedTermApp = _fixedTermAppsTable.FindBy(ApplicationId: app.ApplicationId));
            
            //Note: this is not completely right as due date is in user time.
            DateTime newNextDueDate = DateTime.UtcNow.AddDays(thresholdInDays);
            _fixedTermAppsTable
                .UpdateByApplicationId(ApplicationId: app.ApplicationId, NextDueDate: newNextDueDate, PromiseDate:newNextDueDate);

            //this should trigger reminder email
            Drive.Msmq.Payments.Send(new TimeoutMessage()
                                         {
                                             SagaId = reminderSaga.Id, Expires = DateTime.UtcNow.AddSeconds(-2)
                                         });
            dynamic email = Do.Until(() => _emailTable.FindBy(EmailAddress: _customer.Email, TemplateName: "34250"));
            Assert.IsNotNull(email);
        }

        [AUT(AUT.Uk), Owner(Owner.PiotrWalat)]
        [Test]
        public void LiveFixedTermLoan_WithThreeDayTerm_DoesNotSendA1TemplateEmailToCustomer()
        {
            PerformTestWithTerm(3);
            Assert.IsNull(_emailTable.FindBy(EmailAddress: _customer.Email, TemplateName: "34250"));
        }

        [AUT(AUT.Uk), Owner(Owner.PiotrWalat)]
        [Test]
        public void LiveFixedTermLoan_WithTwoDayTerm_DoesNotSendA1TemplateEmailToCustomer()
        {
            PerformTestWithTerm(2);
            Assert.IsNull(_emailTable.FindBy(EmailAddress: _customer.Email, TemplateName: "34250"));
        }

        [AUT(AUT.Uk), Owner(Owner.PiotrWalat)]
        [Test]
        public void LiveFixedTermLoan_WithOneDayTerm_DoesNotSendA1TemplateEmailToCustomer()
        {
            PerformTestWithTerm(1);
            Assert.IsNull(_emailTable.FindBy(EmailAddress: _customer.Email, TemplateName: "34250"));
        }

        private void PerformTestWithTerm(int term)
        {
            BuildApplicationWithTerm(term);
            dynamic fixedTermApp = null;
            dynamic app = null;
            Do.Until(() => app = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => fixedTermApp = _fixedTermAppsTable.FindBy(ApplicationId: app.ApplicationId));

            dynamic reminderSaga = null;
            Do.Until(() => reminderSaga = _dueDateReminderSagaTable.FindBy(ApplicationId: _application.Id));
        }
    }
}
