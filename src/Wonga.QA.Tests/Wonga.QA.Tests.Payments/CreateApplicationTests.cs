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
    public class CreateApplicationTests
    {
        [Test, AUT(AUT.Za), JIRA("ZA-2024")]

        public void CreateApplicationDueDayOnSaturdayTest()
        {
            Guid appId = Guid.NewGuid();
            var customer = CustomerBuilder.New().Build();
            var promisedDate = FindPromiseDateOfSaturday(DayOfWeek.Saturday);
            Driver.Api.Commands.Post(new CreateFixedTermLoanApplicationCommand()
                                         {
                                             ApplicationId = appId,
                                             AccountId = customer.Id,
                                             PromiseDate = promisedDate.Date.ToString("yyyy-MM-dd"),
                                             BankAccountId = customer.GetBankAccount(),
                                             LoanAmount = 100.0M,
                                             Currency = "ZAR"
                                         });
           var app = Do.Until(() => Driver.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == appId));

            Assert.AreEqual(promisedDate.Date, app.NextDueDate);
        }

        private DateTime FindPromiseDateOfSaturday(DayOfWeek expectedDayOfWeek)
        {
            Console.WriteLine("Looking for an promised date which is on Saturday...");
            var promiseDate = DateTime.UtcNow.AddDays(15);
            while (promiseDate.DayOfWeek != expectedDayOfWeek) promiseDate = promiseDate.AddDays(1);
            Console.WriteLine("Use PromiseDate = {0}", promiseDate);
            return promiseDate;
        }
    }
}
