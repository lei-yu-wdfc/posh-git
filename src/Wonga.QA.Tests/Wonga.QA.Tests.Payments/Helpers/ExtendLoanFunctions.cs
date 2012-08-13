﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments;
using Wonga.QA.Framework.Msmq.Messages.Payments.Uk;
using Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Framework.Old;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public class ExtendLoanFunctions
    {
        public void TenDayLoanQuoteOnDayFiveSetup(Guid appId, Guid paymentCardId, Guid bankAccountId, Guid accountId, decimal trustRating)
        {
            const int extendOnDay = 5;
            CheckExtendMinSetting();

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreated() { AccountId = accountId });

            // Create Application
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId,paymentCardId);

            Drive.Msmq.Payments.Send(new IApplicationAccepted() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Thread.Sleep(250);
            Drive.Msmq.Payments.Send(new SignApplication() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.With.Interval(1).Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(appId));

            // Check transactions have been created
            var application = Drive.Data.Payments.Db.Applications.FindByExternalId(appId);
            Do.With
                .Message(() => String.Format("there are currently {0} trans", Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(application.ApplicationId).Count()))
                .Interval(1).Until<Boolean>(() => Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(application.ApplicationId).Count() == 2);

            // Rewind Application.AcceptedOn & Transactions 5 days 
            var app = new Application(appId);
            app.UpdateAcceptedOnDate(extendOnDay * -1);
            app.MoveTransactionDates(extendOnDay * -1);
        }


        private void CreateFixedTermLoanApplication(Guid appId, Guid accountId, Guid bankAccountId, Guid paymentCardId, int dueInDays = 5)
        {
            Drive.Msmq.Payments.Send(new AddBankAccount()
            {
                AccountId = accountId,
                AccountNumber = "10032650",
                AccountOpenDate = DateTime.UtcNow.AddYears(-3),
                BankAccountId = bankAccountId,
                BankCode = "161027",
                BankName = "Royal Bank of Scotland",
                HolderName = "Mr Test Test",
                CountryCode = "UK",
                CreatedOn = DateTime.UtcNow
            });

            Drive.Msmq.Payments.Send(new CreateFixedTermLoanApplication()
            {
                ApplicationId = appId,
                AccountId = accountId,
                PromiseDate = DateTime.UtcNow.AddDays(dueInDays),
                BankAccountId = bankAccountId,
                PaymentCardId = paymentCardId,
                LoanAmount = 100.0M,
                Currency = CurrencyCodeIso4217Enum.GBP,
                CreatedOn = DateTime.UtcNow
            });

            Thread.Sleep(500);
            Drive.Msmq.Payments.Send(new IBankAccountValidated()
            {
                BankAccountId = bankAccountId,
                IsValid = true
            });
        }

        private void CheckExtendMinSetting()
        {
            // Check Loan Extension is Enabled
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            if (cfg1.Value != "1")
                throw new Exception("Unable to run test, ExtendLoanMinDays must be set to 1 in service configuration");
        }

    }
}
