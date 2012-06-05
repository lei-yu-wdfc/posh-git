using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class CreateApplicationTests
    {
		[Test, AUT(AUT.Za), JIRA("ZA-2024")]
        public void CreateApplicationDueDayOnSaturdayTest()
        {
            Guid appId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();
            var promisedDate = FindPromiseDateOfExpectedDayOfWeek(DayOfWeek.Saturday);
            Drive.Api.Commands.Post(new CreateFixedTermLoanApplicationCommand()
                                         {
                                             ApplicationId = appId,
                                             AccountId = customer.Id,
                                             PromiseDate = promisedDate.Date.ToString("yyyy-MM-dd"),
                                             BankAccountId = customer.GetBankAccount(),
                                             LoanAmount = 100.0M,
                                             Currency = "ZAR"
                                         });
           var app = Do.Until(() => Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == appId));
        	
            Assert.AreEqual(promisedDate.GetNextWorkingDay(), app.NextDueDate);
        }

        private DateTime FindPromiseDateOfExpectedDayOfWeek(DayOfWeek expectedDayOfWeek)
        {
            Console.WriteLine("Looking for an promised date which is on {0}...", expectedDayOfWeek);
            var promiseDate = DateTime.UtcNow.AddDays(15);
            while (promiseDate.DayOfWeek != expectedDayOfWeek) promiseDate = promiseDate.AddDays(1);
            Console.WriteLine("Use PromiseDate = {0}", promiseDate);
            return promiseDate;
        }

        [Test, AUT(AUT.Wb), JIRA("SME-849")]
        public void PaymentsShouldAddLoanReferenceNumberWhenApplicaitonIsCreated()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                Build();

            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == app.Id).LoanReference);
        }
    }
}
