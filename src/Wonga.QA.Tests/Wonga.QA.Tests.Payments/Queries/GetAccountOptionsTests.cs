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
    /// <summary>
    /// Not paralellizable because it is altering 
    /// </summary>
    [TestFixture, Parallelizable (TestScope.Self)]
    public class GetAccountOptionsTests
    {
        private string _resetExtendLoanDays = null;
        private string _resetExtendLoanDaysBeforeDueDate = null;
        [FixtureSetUp]
        public void Setup()
        {
            // Record value(s)
            _resetExtendLoanDays = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays").Value;
            _resetExtendLoanDaysBeforeDueDate = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate").Value;
        }

        [FixtureTearDown]
        public void TearDown()
        {
            // Reset value(s)
            var cfg1 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = _resetExtendLoanDays;
            cfg1.Submit();
            var cfg2 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = _resetExtendLoanDaysBeforeDueDate;
            cfg2.Submit();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario01ExistingCustomerWithoutLiveLoan()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            Driver.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId,CreatedOn = DateTime.Now.AddHours(-1)});
            Driver.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId,CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));
      
            // Go to DB and set Application to closed
            ApplicationEntity app = Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            app.ClosedOn = DateTime.UtcNow.AddDays(-1);
            app.Submit(true);
            
            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = 400.00M });
            Assert.AreEqual(1, int.Parse(response.Values["ScenarioId"].Single()));
            // ToDo: Assert Options

        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario02CustomerWithLiveLoanWithAvailableCreditTooEarlyToExtend()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();

            const string extendMinLoanDays = "7";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Driver.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Driver.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Driver.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            Do.Until(() => Driver.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);
            
            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = 400.00M });
            Assert.AreEqual(2,int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario03CustomerWithLiveLoanWithAvailableCreditCanExtendLoan()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            
            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Driver.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

           // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Driver.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Driver.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });


            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            Do.Until(() => Driver.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);

            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = 400.00M });
            Assert.AreEqual(3, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario04CustomerWithLiveLoanWithAvailableCreditCantExtendLoanDueTooMaxExtensions()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();

            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Driver.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Driver.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Driver.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });


            // Check App Exists in DB
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateExtensionFeeTransaction(appId);
            var trnGuid4 = CreateExtensionFeeTransaction(appId);
            var trnGuid5 = CreateExtensionFeeTransaction(appId);

            // Check transactions have been created
            Do.Until(() => Driver.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3 || itm.ExternalId == trnGuid4 || itm.ExternalId == trnGuid5) == 5);

            var response = Driver.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = 400.00M });
            Assert.AreEqual(4, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }
        #region "Helpers"

            private void CreateFixedTermLoanApplication(Guid appId, Guid accountId, Guid bankAccountId, Guid paymentCardId)
            {
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
            }

            private Guid CreateLoanAdvanceTransaction(Guid appId)
            {
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
                return trnGuid1;
            }

            private Guid CreateTransmissionFeeTransaction(Guid appId)
            {
                var trnGuid1 = Guid.NewGuid();

                Driver.Msmq.Payments.Send(new CreateTransactionCommand()
                {
                    ApplicationId = appId,
                    ExternalId = trnGuid1,
                    Amount = 5.50M,
                    Type = PaymentTransactionEnum.Fee,
                    Currency = CurrencyCodeIso4217Enum.GBP,
                    Mir = 30.0M,
                    PostedOn = DateTime.Now,
                    Scope = PaymentTransactionScopeEnum.Debit,
                    Reference = "Test Transmission fee"
                });
                return trnGuid1;
            }

            private Guid CreateExtensionFeeTransaction(Guid appId)
            {
                var trnGuid1 = Guid.NewGuid();

                Driver.Msmq.Payments.Send(new CreateTransactionCommand()
                {
                    ApplicationId = appId,
                    ExternalId = trnGuid1,
                    Amount = 20.00M,
                    Type = PaymentTransactionEnum.LoanExtensionFee,
                    Currency = CurrencyCodeIso4217Enum.GBP,
                    Mir = 30.0M,
                    PostedOn = DateTime.Now,
                    Scope = PaymentTransactionScopeEnum.Debit,
                    Reference = "Test Extension fee"
                });
                return trnGuid1;
            }

        #endregion
    }
}
