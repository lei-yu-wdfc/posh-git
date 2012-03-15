using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using AddBankAccountUkCommand = Wonga.QA.Framework.Msmq.AddBankAccountUkCommand;
using CreateFixedTermLoanApplicationCommand = Wonga.QA.Framework.Msmq.CreateFixedTermLoanApplicationCommand;
using SignApplicationCommand = Wonga.QA.Framework.Msmq.SignApplicationCommand;


namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class GetAccountOptionsTests
    {
        private string _resetExtendLoanDays = null;
        [SetUp]
        public void Setup()
        {
            // Record value
            var cfg = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            _resetExtendLoanDays = cfg.Value;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset value
            var cfg = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            //  if (cfg.Value != _resetExtendLoanDays)
            cfg.Value = _resetExtendLoanDays;
            cfg.Submit();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario1ExistingCustomerWithoutLiveLoan()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();


       /*     Driver.Msmq.Payments.Send(new AddBankAccountUkCommand() { BankCode = "161017",
                                                                      AccountId = accountId,
                                                                      AccountNumber = "10062650",
                                                                      AccountOpenDate = DateTime.Now.AddYears(-3),
                                                                      BankAccountId = bankAccountId,
                                                                      BankName = "RBS",
                                                                      ClientId = Guid.NewGuid(),
                                                                      CountryCode = "GBP"
                                                                     });
            */
            // Create Application 
            Driver.Msmq.Payments.Send(new CreateFixedTermLoanApplicationCommand()
            {
                ApplicationId = appId,
                AccountId = accountId,
                PromiseDate = DateTime.UtcNow.AddDays(10),
                BankAccountId = bankAccountId,
                PaymentCardId = paymentCardId,
                LoanAmount = 100.0M,
                Currency = CurrencyCodeIso4217Enum.GBP,
                CreatedOn = DateTime.UtcNow
            });

            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            Driver.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId,CreatedOn = DateTime.Now.AddHours(-1)});
            Driver.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId,CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));
      
            // Go to DB and set Application to closed
            ApplicationEntity app = Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            app.ClosedOn = DateTime.UtcNow;
            app.Submit(true);
            
            /*
           
            Driver.Msmq.Payments.Send(new CreateTransactionCommand()
                                                                    {
                                                                        ApplicationId = appId,
                                                                        ExternalId = Guid.NewGuid(),
                                                                        Amount = -115.50M,
                                                                        Type = PaymentTransactionEnum.CardPayment,
                                                                        Currency = CurrencyCodeIso4217Enum.GBP,
                                                                        Mir = 30.0M,
                                                                        PostedOn = DateTime.Now,
                                                                        Scope = PaymentTransactionScopeEnum.Credit,
                                                                        Reference = "Test Card Payment Fee"
                                                                    });

            */
            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = 400.00M });
            Assert.GreaterThan(int.Parse(response.Values["ScenarioId"].Single()), 1);
            // ToDo: Assert Options
            //Assert.GreaterThan(int.Parse(response.Values["DaysTillRepaymentDate"].Single()), 0);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823"),]
        public void Scenario02CustomerWithLiveLoanWithAvailableCreditTooEarlyToExtend()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const string extendMinLoanDays = "11";
            

            // Create Account so that time zone can be looked up
            Driver.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var app = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            app.Value = extendMinLoanDays;
            app.Submit();

            // Create Application 
            Driver.Msmq.Payments.Send(new CreateFixedTermLoanApplicationCommand()
            {
                ApplicationId = appId,
                AccountId = accountId,
                PromiseDate = DateTime.UtcNow.AddDays(10),
                BankAccountId = bankAccountId,
                PaymentCardId = paymentCardId,
                LoanAmount = 100.0M,
                Currency = CurrencyCodeIso4217Enum.GBP,
                CreatedOn = DateTime.UtcNow
            });

            Driver.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Driver.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });


            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = Guid.NewGuid();
            Driver.Msmq.Payments.Send(new CreateTransactionCommand()
            {
                ApplicationId = appId,
                ExternalId = trnGuid1,
                Amount = 100.00M,
                Type = PaymentTransactionEnum.CashAdvance,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Mir = 30.0M,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Debit,
                Reference = "Test Cash Advance"
            });

            var trnGuid2 = Guid.NewGuid();
            Driver.Msmq.Payments.Send(new CreateTransactionCommand()
            {
                ApplicationId = appId,
                ExternalId = trnGuid2,
                Amount = 5.50M,
                Type = PaymentTransactionEnum.Fee,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Mir = 30.0M,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Debit,
                Reference = "Test Transmission Fee"
            });

            Do.Until(() => Driver.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);
            
            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = 400.00M });
            Assert.GreaterThan(int.Parse(response.Values["ScenarioId"].Single()), 2);
        }

    }
}
