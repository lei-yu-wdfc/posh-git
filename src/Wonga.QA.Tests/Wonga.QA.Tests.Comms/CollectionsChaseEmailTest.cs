using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Comms.Helpers;
using Wonga.QA.Tests.Core;
using ProvinceEnum = Wonga.QA.Framework.Api.Enums.ProvinceEnum;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture, Parallelizable(TestScope.Descendants), CoreTest] //Can be only on level 3 because it changes configuration
    class CollectionsChaseEmailTest
    {
        private static readonly dynamic EmailTable = Drive.Data.QaData.Db.Email;

        private Application _applicationZa;
        private Application _applicationCa;
        private Customer customerCa;

        private const int TemplateA2 = 22843;
        private const int TemplateA3 = 22879;
        private const int TemplateA4 = 22880;
        private const int TemplateA5 = 22881;
        private const int TemplateA6 = 22882;
        private const int TemplateA7 = 22883;
        private const int TemplateA8 = 22884;
        private const string SendCollectionsReminderA2Email = "Email.SendCollectionsReminderA2Email";
        private const string SendCollectionsReminderA3Email = "Email.SendCollectionsReminderA3Email";
        private const string SendCollectionsReminderA4Email = "Email.SendCollectionsReminderA4Email";
        private const string SendCollectionsReminderA5Email = "Email.SendCollectionsReminderA5Email";
        private const string SendCollectionsReminderA6Email = "Email.SendCollectionsReminderA6Email";

        [FixtureSetUp]
        public void FixtureSetup()
        {
            if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
                Assert.Inconclusive("Bankgateway is in test mode");

            switch (Config.AUT)
            {
                case AUT.Za:
                    var customerZa = CustomerBuilder.New().Build();
                    _applicationZa = ApplicationBuilder.New(customerZa).Build();
                    break;
                case AUT.Ca:
                    customerCa = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON).Build();
                    _applicationCa = ApplicationBuilder.New(customerCa).WithLoanTerm(10).Build();
                    break;

            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-206")]
        public void A2EmailIsSent()
        {
            switch(Config.AUT)
            {
                case AUT.Za:
                    VerifyEmailIsSentAfterDaysInArrearsZa(0, TemplateA2);
                    break;
                case AUT.Ca:
                    VerifyEmailIsSentAfterDaysInArrearsCa(0, SendCollectionsReminderA2Email, "130.00");
                    break;
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-206"), DependsOn("A2EmailIsSent")]
        public void A3EmailIsSent()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    VerifyEmailIsSentAfterDaysInArrearsZa(3, TemplateA3);
                    break;
                case AUT.Ca:
                    VerifyEmailIsSentAfterDaysInArrearsCa(6, SendCollectionsReminderA3Email, "130.54");
                    break;
            }
            
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-206"), DependsOn("A3EmailIsSent")]
        public void A4EmailIsSent()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    VerifyEmailIsSentAfterDaysInArrearsZa(15, TemplateA4);
                    break;
                case AUT.Ca:
                    VerifyEmailIsSentAfterDaysInArrearsCa(13, SendCollectionsReminderA4Email, "131.18");
                    break;
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-206"), DependsOn("A4EmailIsSent")]
        public void A5EmailIsSent()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    VerifyEmailIsSentAfterDaysInArrearsZa(20, TemplateA5);
                    break;
                case AUT.Ca:
                    VerifyEmailIsSentAfterDaysInArrearsCa(20, SendCollectionsReminderA5Email, "131.81");
                    break;
            }
            
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-206"), DependsOn("A5EmailIsSent")]
        public void A6EmailIsSent()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    VerifyEmailIsSentAfterDaysInArrearsZa(30, TemplateA6);
                    break;
                case AUT.Ca:
                    VerifyEmailIsSentAfterDaysInArrearsCa(27, SendCollectionsReminderA6Email, "132.44");
                    break;
            }
           
        }

        [Test, AUT(AUT.Za), JIRA("QA-206"), DependsOn("A6EmailIsSent")]
        public void A7EmailIsSent()
        {
            VerifyEmailIsSentAfterDaysInArrearsZa(40, TemplateA7);
        }

        [Test, AUT(AUT.Za), JIRA("QA-206"), DependsOn("A7EmailIsSent")]
        public void A8EmailIsSent()
        {
            VerifyEmailIsSentAfterDaysInArrearsZa(45, TemplateA8);
        }

        #region Helpers

        private void VerifyEmailIsSentAfterDaysInArrearsZa(uint daysInArrears, int template)
        {

            _applicationZa.PutIntoArrears(daysInArrears);

            if (daysInArrears > 0) //Saga is created after first email sent
                TimeoutCollectionsChaseSagaForDays(_applicationZa, daysInArrears);

            AssertEmailIsSent(_applicationZa.GetCustomer().Email, template);

        }

        private void VerifyEmailIsSentAfterDaysInArrearsCa(uint daysInArrears, string template, string amount)
        {
            _applicationCa.PutIntoArrears(daysInArrears);
            TimeoutInArrearsNoticeSaga(_applicationCa, Convert.ToInt32(daysInArrears));
            var emailTokens = GetEmailTokens(customerCa, template);
            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", template);
            Assert.IsTrue(emailTokens[0].Value == customerCa.GetCustomerForename());
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customerCa.Email);
        }

        private void TimeoutCollectionsChaseSagaForDays(Application application, uint days)
        {
            var sagaId = (Guid)Do.Until(() => (Drive.Data.OpsSagas.Db.CollectionsChaseSagaEntity.FindByApplicationId(application.Id))).Id;

            Drive.Data.OpsSagas.Db.CollectionsChaseSagaEntity.UpdateById(Id: sagaId, DueDate: DateTime.Today.AddDays(-days));
            Drive.Msmq.Comms.Send(new TimeoutMessage { SagaId = sagaId });
        }

        private static void TimeoutInArrearsNoticeSaga(Application application, int numberOfDaysInArrears)
        {
            var inArrearsNoticeSaga =
                Do.Until(() => Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Single(e => e.ApplicationId == application.Id));

            for (var i = 0; i < numberOfDaysInArrears; i++)
            {
                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = inArrearsNoticeSaga.Id });
            }
        }

        private void AssertEmailIsSent(string email, int template)
        {
            Assert.IsNotNull(
                Do.Until(() =>
                         EmailTable.Find(
                            EmailTable.EmailAddress == email &&
                            EmailTable.TemplateName == template)));
        }

        private List<EmailToken> GetEmailTokens(Customer customer, String emailTemplateName)
        {
            var db = new DbDriver().QaData;
            var emailId = Do.Until(() => db.Emails.Single(e => e.EmailAddress == customer.Email && e.TemplateName == getEmailTemplateId(emailTemplateName)).EmailId);
            return db.EmailTokens.Where(et => et.EmailId == emailId).ToList();
        }

        private string getEmailTemplateId(string emailTemplateName)
        {
            return Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == emailTemplateName).Value;
        }
        #endregion
    }
}