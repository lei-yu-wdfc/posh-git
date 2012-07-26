using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Sms
{
    [TestFixture]
    [AUT(AUT.Uk)]
    [Parallelizable(TestScope.All)]
    public class LiveLoanSmsTests
    {
        private Customer _customer;
        private Application _application;
        private decimal _loanAmount;
        private DateTime _promiseDate;

        private string _mobilePhone;
        private string _mobilePhonePart;

        private dynamic _dueDateReminderSagaTable;
        private dynamic _servicConfigTable;
        private dynamic _fixedTermAppsTable;
        private dynamic _applicationsTable;
        private dynamic _createAndStoreSagaTable;
        private dynamic _smsTable;

        [SetUp]
        public void Setup()
        {
            _dueDateReminderSagaTable = Drive.Data.OpsSagas.Db.DueDateApproachingNotificationSagaEntity;
            _servicConfigTable = Drive.Data.Ops.Db.ServiceConfigurations;
            _fixedTermAppsTable = Drive.Data.Payments.Db.FixedTermLoanApplications;
            _applicationsTable = Drive.Data.Payments.Db.Applications;
            _createAndStoreSagaTable = Drive.Data.OpsSagas.Db.CreateAndStorePaymentNotificationSmsSagaEntity;
            _smsTable = Drive.Data.Sms.Db.SmsMessages;
            _mobilePhonePart = String.Format("700900{0}",GetPhoneNumber());
            _mobilePhone = BuildPhoneNumber(_mobilePhonePart);
            _customer = CustomerBuilder.New().WithMobileNumber(_mobilePhone).Build();
            _loanAmount = 222.22m;
            _promiseDate = DateTime.UtcNow.AddDays(12);

            var mobileVerificationEntity = Do.Until(() => Drive.Data.Comms.Db.MobilePhoneVerifications.FindByMobilePhone(MobilePhone: _mobilePhone));

            Assert.IsNotNull(mobileVerificationEntity);
            Assert.IsNotNull(mobileVerificationEntity.Pin);

            //Force the mobile phone number to be verified successfully..
            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new CompleteMobilePhoneVerificationCommand
            {
                Pin = mobileVerificationEntity.Pin,
                VerificationId = mobileVerificationEntity.VerificationId
            }));
        }

        [Test]
        [AUT(AUT.Uk), Owner(Owner.PiotrWalat)]
        public void LiveLoan_ComingDueInOneDay_SendsReminderTextMessage()
        {
            BuildApplicationWithTerm(12);
            
            dynamic reminderSaga = null;
            Do.Until(() => reminderSaga = _dueDateReminderSagaTable.FindBy(ApplicationId: _application.Id));
            dynamic fixedTermApp = null;
            dynamic app = null;
            Do.Until(() => app = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => fixedTermApp = _fixedTermAppsTable.FindBy(ApplicationId: app.ApplicationId));

            DateTime newNextDueDate = DateTime.UtcNow.Date.AddDays(1);
            _fixedTermAppsTable.UpdateByApplicationId(ApplicationId: app.ApplicationId,
                NextDueDate: newNextDueDate,
                PromiseDate: newNextDueDate);

            Drive.Msmq.Payments.Send(new TimeoutMessage()
            {
                SagaId = reminderSaga.Id,
                Expires = DateTime.UtcNow.AddSeconds(-2)
            });

            dynamic createSaga = null;

            Do.Until(() => createSaga = _createAndStoreSagaTable.FindBy(ApplicationId: _application.Id));
            var now = DateTime.Now;
            Drive.Msmq.FileStorage.Send(new TimeoutMessage() { SagaId = createSaga.Id, Expires = DateTime.UtcNow.AddSeconds(-2) });

            var messageText = string.Format("Hi. Just a quick reminder that we will collect {0} from your debit card tomorrow. Please ensure there are enough funds in your account. Thanks, Wonga Team.",
                229.97.ToString("C2", new CultureInfo("en-GB")));

            AssertSmsIsSent(FormatPhoneNumber(_mobilePhonePart),
                messageText,
                now);
        }

        private void BuildApplicationWithTerm(int term)
        {
            _application = ApplicationBuilder.New(_customer)
                .WithLoanAmount(_loanAmount)
                .WithLoanTerm(term)
                .Build();
        }

        private void AssertSmsIsSent(string formattedPhoneNumber, string text, DateTime createdAfter)
        {
            Assert.IsNotNull(Do.Until(() => _smsTable.Find(_smsTable.MobilePhoneNumber == formattedPhoneNumber
                                                            && _smsTable.MessageText.Like(String.Format("%{0}%", text)) 
                                                            && _smsTable.CreatedOn > createdAfter
                                                            )));
        }

        protected string GetPhoneNumber()
        {
            return Get.RandomInt(100, 999).ToString(CultureInfo.InvariantCulture);
        }

        protected string BuildPhoneNumber(string n)
        {
            return String.Format("07{0}", n);
        }

        protected string FormatPhoneNumber(string unformatted)
        {
            return "447" + unformatted;
        }
    }
}
