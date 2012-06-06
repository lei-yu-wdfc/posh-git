using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public class RepayLoanFunctions
    {
        const string PaymentCardType = "Visa";
        private const string PaymentCardNumber = "4444333322221111";

        public void RepayEarlyOnLoanStartDate(Guid appId, Guid paymentCardId, Guid bankAccountId, Guid accountId, decimal trustRating, int dueInDays = 5)
        {
            

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Create Application
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId, dueInDays);

            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Thread.Sleep(500);
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.With.Interval(1).Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(appId));

            // Check transactions have been created
            var application = Drive.Data.Payments.Db.Applications.FindByExternalId(appId);
            Do.With
                .Message(() => String.Format("there are currently {0} trans", Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(application.ApplicationId).Count()))
                .Interval(1).Until<Boolean>(() => Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(application.ApplicationId).Count() == 2);

        }

        private void CreateFixedTermLoanApplication(Guid appId, Guid accountId, Guid bankAccountId, Guid paymentCardId, int dueInDays)
        {
            Drive.Msmq.Payments.Send(new AddBankAccountUkCommand()
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

            Drive.Msmq.Payments.Send(new CreateFixedTermLoanApplicationUkCommand()
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

            Drive.Msmq.Payments.Send(new AddPaymentCardCommand  ()
            {
                PaymentCardId = paymentCardId,
                AccountId = accountId,
                CardType = PaymentCardType,
                CreatedOn = DateTime.UtcNow,
                ExpiryDateXml = DateTime.Now.AddYears(1).ToString("yyyy-MM"),
                HolderName = "Mickey Mouse",
                Number = PaymentCardNumber,
                IsCreditCard = false,
                IsPrimary = true
            });

            Thread.Sleep(500);
            Drive.Msmq.Payments.Send(new IBankAccountValidatedEvent()
            {
                BankAccountId = bankAccountId,
                IsValid = true
            });

            // Authorize Payment Card
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: paymentCardId, AuthorizedOn: DateTime.UtcNow);
            
        }

    }
}
