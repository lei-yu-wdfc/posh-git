﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Db.Bi;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        [JIRA("DI-603")]
        [Description("This will create an account, some transactions and then request accrued interest to be calculated")]
        public void ApplyAccountDetailsStoredInBiTable()
        {

            //// Create a customer, changing a couple of details so that everything stands out in testing
            // var customer = CustomerBuilder.New().WithForename("Interest").WithSurname("CalculationTest").Build();

            // If nothing in Payments.Applications, check the Payment.Handler service is running
            // If it wont start, then purge the messages...
            var customer = CustomerBuilder.New().Build();

            // Now check the database, wait up to 30 seconds for the Account record to appear
            var account = Do.Until(() => Drive.Db.Comms.CustomerDetails.Where(x => x.AccountId == customer.Id).Single());

            // Check that the customer exists
            Assert.IsNotNull(customer);

            // Now add the application for the customer
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var appl = Do.Until(() => Drive.Db.Payments.Applications.Where(x => x.AccountId == customer.Id).Single());

            customer.UpdateSurname("InterestTesting");

            Drive.Msmq.Payments.Send(new CreateTransactionCommand
            {
                Amount = 100M,
                ApplicationId = application.Id,
                Currency = CurrencyCodeIso4217Enum.GBP,
                ExternalId = Guid.NewGuid(),
                ComponentTransactionId = Guid.Empty,
                PostedOn = DateTime.Now,
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
                PostedOn = DateTime.Now,
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
                BatchSendTime = DateTime.Now,
                CreatedOn = DateTime.Now,
                PaymentReference = 12313212,
                EffectiveDate = DateTime.Now,
                SagaId = Guid.NewGuid(),
                TransactionAmount = 123.75M,
                ValueDate = DateTime.Now
            });

            // We need to get the ApplicationSKey from BI.Application
            // To get that we need to join the Payments.Applications table ExternalId to 
            // to BI.Application ApplicationNKey

            var bi_appl = Do.Until(() => Drive.Db.Bi.Applications.Where(x => x.ApplicationNKey == appl.ExternalId).Single());

            //                                       ApplicationId = bi_appl.ApplicationSKey

            Drive.Msmq.Bi.Send(new CalcAccruedInterestCommand
            {
                ApplicationId = bi_appl.ApplicationSKey
            }
            );
        }


    }
}
