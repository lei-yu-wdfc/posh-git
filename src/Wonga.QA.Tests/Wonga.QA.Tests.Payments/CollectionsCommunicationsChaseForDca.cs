using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using ProvinceEnum = Wonga.QA.Framework.Api.ProvinceEnum;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class CollectionsCommunicationsChaseForDca
    {
        private const string SendCollectionsReminderA2Email = "Email.SendCollectionsReminderA2Email";
        private const string SendCollectionsReminderA3Email = "Email.SendCollectionsReminderA3Email";
        private const string SendCollectionsReminderA4Email = "Email.SendCollectionsReminderA4Email";
        private const string SendCollectionsReminderA5Email = "Email.SendCollectionsReminderA5Email";
        private const string SendCollectionsReminderA6Email = "Email.SendCollectionsReminderA6Email";

        [FixtureSetUp]
        public static void FixtureSetUp()
        {
        }

        [FixtureTearDown]
        public static void FixtureTearDown()
        {
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810")]
        public void WhenLoanGoesIntoArrearsThenA2EmailShouldBeSentOnDay0()
        {
            const String amount = "130.00";
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            var customerForename = customerBuilder.Forename;

            application.PutApplicationIntoArrears();

            var emailTokens = GetEmailTokens(customer, SendCollectionsReminderA2Email);

            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", SendCollectionsReminderA2Email);
            
            Assert.IsTrue(emailTokens[0].Value == customerForename);
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customer.Email);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810")]
        public void WhenLoanGoesIntoArrearsThenA3EmailShouldBeSentOnDay6()
        {
            const String amount = "130.54";
            const int numberOfDaysInArrears = 6;
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            var customerForename = customerBuilder.Forename;

            application.PutApplicationIntoArrears(numberOfDaysInArrears);

            TimeoutInArrearsNoticeSaga(application, numberOfDaysInArrears);

            var emailTokens = GetEmailTokens(customer, SendCollectionsReminderA3Email);

            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", SendCollectionsReminderA3Email);
            
            Assert.IsTrue(emailTokens[0].Value == customerForename);
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customer.Email);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810")]
        public void WhenLoanGoesIntoArrearsThenA4EmailShouldBeSentOnDay13()
        {
            const String amount = "131.18";
            const int numberOfDaysInArrears = 13;
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            var customerForename = customerBuilder.Forename;

            application.PutApplicationIntoArrears(numberOfDaysInArrears);

            TimeoutInArrearsNoticeSaga(application, numberOfDaysInArrears);

            var emailTokens = GetEmailTokens(customer, SendCollectionsReminderA4Email);

            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", SendCollectionsReminderA4Email);
            
            Assert.IsTrue(emailTokens[0].Value == customerForename);
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customer.Email);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810")]
        public void WhenLoanGoesIntoArrearsThenA5EmailShouldBeSentOnDay20()
        {
            const String amount = "131.81";
            const int numberOfDaysInArrears = 20;
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            var customerForename = customerBuilder.Forename;

            application.PutApplicationIntoArrears(numberOfDaysInArrears);

            TimeoutInArrearsNoticeSaga(application, numberOfDaysInArrears);

            var emailTokens = GetEmailTokens(customer, SendCollectionsReminderA5Email);

            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", SendCollectionsReminderA5Email);
            
            Assert.IsTrue(emailTokens[0].Value == customerForename);
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customer.Email);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810")]
        public void WhenLoanGoesIntoArrearsThenA6EmailShouldBeSentOnDay27()
        {
            const String amount = "132.44";
            const int numberOfDaysInArrears = 27;
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            var customerForename = customerBuilder.Forename;

            application.PutApplicationIntoArrears(numberOfDaysInArrears);

            TimeoutInArrearsNoticeSaga(application, numberOfDaysInArrears);

            var emailTokens = GetEmailTokens(customer, SendCollectionsReminderA6Email);

            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", SendCollectionsReminderA6Email);
            
            Assert.IsTrue(emailTokens[0].Value == customerForename);
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customer.Email);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), ExpectedException(typeof(DoException))]
        public void WhenLoanGoesIntoArrearsThenThereShouldBeNoEmailSentOnDay34()
        {
            const int numberOfDaysInArrears = 34;
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.ON);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();

            application.PutApplicationIntoArrears(numberOfDaysInArrears);

            TimeoutInArrearsNoticeSaga(application, numberOfDaysInArrears);

            Assert.IsTrue(VerifyTotalNumberOfEmailsSent(customer, 7));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810")]
        public void WhenLoanGoesIntoArrearsForBcCustomerThenA2EmailShouldBeSentOnDay0WithoutDefaultChargeBeingApplied()
        {
            const String amount = "110.00";
            var customerBuilder = CustomerBuilder.New().WithProvinceInAddress(ProvinceEnum.BC);
            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
            var customerForename = customerBuilder.Forename;

            application.PutApplicationIntoArrears();

            var emailTokens = GetEmailTokens(customer, SendCollectionsReminderA2Email);

            Assert.IsFalse(emailTokens.Count == 0, "Could not find email for template {0}", SendCollectionsReminderA2Email);
            
            Assert.IsTrue(emailTokens[0].Value == customerForename);
            Assert.IsTrue(emailTokens[1].Value == amount, "{0} is not equal to {1}", emailTokens[1].Value, amount);
            Assert.IsTrue(emailTokens[2].Value == customer.Email);
        }

        private string getEmailTemplateId(string emailTemplateName)
        {
            return Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == emailTemplateName).Value;
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

        private List<EmailToken> GetEmailTokens(Customer customer, String emailTemplateName)
        {
            var db = new DbDriver().QaData;
            var emailId = Do.Until(() => db.Emails.Single( e => e.EmailAddress == customer.Email && e.TemplateName == getEmailTemplateId(emailTemplateName)).EmailId);
            return db.EmailTokens.Where(et => et.EmailId == emailId).ToList();
        }

        private bool VerifyTotalNumberOfEmailsSent(Customer customer, int toltalSent)
        {
            var db = new DbDriver().QaData;
            return Do.With.Timeout(2).Until(() => db.Emails.Where(e => e.EmailAddress == customer.Email).Count() > toltalSent);
        }
    }
}
