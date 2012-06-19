using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Messages.Bi.Messages;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        [AUT(AUT.Uk)]
        [JIRA("DI-603")]
        [Explicit]
        [Description("This will create an account, some transactions and then request accrued interest to be calculated")]
        public void ApplyAccountDetailsStoredInBiTable()
        {

            //// Create a customer, changing a couple of details so that everything stands out in testing
            // var customer = CustomerBuilder.New().WithForename("Interest").WithSurname("CalculationTest").Build();

            // If nothing in Payments.Applications, check the Payment.Handler service is running
            // If it wont start, then purge the messages...
            Customer customer = CustomerBuilder.New().Build();

            // Now check the database, wait up to 30 seconds for the Account record to appear
            var account = Do.Until(() => Drive.Data.Comms.Db.comms.CustomerDetails.FindBy(AccountId: customer.Id));

            // Check that the customer exists
            Assert.IsNotNull(customer);

            // Now add the application for the customer
            Application application = ApplicationBuilder.New(customer).WithLoanTerm(12).Build();

            var appl = Do.Until(() => Drive.Data.Payments.Db.payments.Applications.FindBy(AccountId: customer.Id));

            application.UpdateAcceptedOnDate(-5);

            customer.UpdateSurname("InterestTesting");

            DateTime today = DateTime.Now;
            Drive.Msmq.Payments.Send(new CreateTransactionCommand
            {
                Amount = 100M,
                ApplicationId = application.Id,
                Currency = CurrencyCodeIso4217Enum.GBP,
                ExternalId = Guid.NewGuid(),
                ComponentTransactionId = Guid.Empty,
                PostedOn = today.AddDays(-3) ,
                Scope = PaymentTransactionScopeEnum.Credit,
                Source = PaymentTransactionSourceEnum.System,
                Type = PaymentTransactionEnum.DirectBankPayment
            });

            Drive.Msmq.Payments.Send(new CreateTransactionCommand
            {
                Amount = 130M,
                ApplicationId = application.Id,
                Currency = CurrencyCodeIso4217Enum.GBP,
                ExternalId = Guid.NewGuid(),
                ComponentTransactionId = Guid.Empty,
                PostedOn = today.AddDays(-3),
                Scope = PaymentTransactionScopeEnum.Credit,
                Source = PaymentTransactionSourceEnum.System,
                Type = PaymentTransactionEnum.Cheque
            });

            Drive.Msmq.Payments.Send(new PaymentSentCommand
            {
                ApplicationId = application.Id,
                BankAccountNumber = "1231231",
                BankCode = "12311",
                BatchNumber = 1,
                BatchSendTime = today.AddDays(-2),
                CreatedOn = today.AddDays(-2),
                PaymentReference = 12313212,
                EffectiveDate = today.AddDays(-2),
                SagaId = Guid.NewGuid(),
                TransactionAmount = 123.75M,
                ValueDate = today.AddDays(-2)
            });


            // We need to get the ApplicationSKey from BI.Application
            // To get that we need to join the Payments.Applications table ExternalId to 
            // to BI.Application ApplicationNKey

            var bi_appl = Do.Until(() => Drive.Data.Bi.Db.bi.Application.FindBy(ApplicationNKey: appl.ExternalId));

            Drive.Msmq.Bi.Send(new CalcAccruedInterestCommand
            {
                ApplicationGuid = appl.ExternalId,
                Timestamp = DateTime.Now
            }
            );
        }


    }
}
