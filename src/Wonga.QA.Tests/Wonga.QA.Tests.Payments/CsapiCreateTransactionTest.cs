using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Payments.Csapi.Commands.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.FileStorage.InternalMessages.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class CsapiCreateTransactionTest
    {
        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldInsertTransactionWhenValidRequestIsSubmitted()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var createTransactionCommand = new CreateTransactionCommand
                                               {
                                                   Amount = 500.50m,
                                                   ApplicationGuid = app.Id,
                                                   Currency = CurrencyCodeEnum.GBP,
                                                   Reference = String.Empty,
                                                   SalesForceUser = "test.user@wonga.com",
                                                   Scope = PaymentTransactionScopeEnum.Credit,
                                                   Type = PaymentTransactionEnum.Cheque
                                               };
            Drive.Cs.Commands.Post(createTransactionCommand);

            Do.Until(() => Drive.Db.Payments.Transactions.Single(t => t.Amount == 500.50m
                                                                       && t.ApplicationEntity.ExternalId == app.Id
                                                                       && t.Reference == ""
                                                                       && t.SalesForceUsername == "test.user@wonga.com"
                                                                       && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                       && t.Type == PaymentTransactionEnum.Cheque.ToString()));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldReturnSchemaErrorAndNotInsertTransactionsWhenSalesforceUserIsNull()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var createTransactionCommand = new CreateTransactionCommand
            {
                Amount = 500.50m,
                ApplicationGuid = app.Id,
                Currency = CurrencyCodeEnum.GBP,
                Reference = String.Empty,
                Scope = PaymentTransactionScopeEnum.Credit,
                Type = PaymentTransactionEnum.Cheque
            };
            Assert.Throws<SchemaException>(() => Drive.Cs.Commands.Post(createTransactionCommand));
            Assert.AreEqual(0,Drive.Db.Payments.Transactions.Count(t => t.Amount == 500.50m
                                                                       && t.ApplicationEntity.ExternalId == app.Id
                                                                       && t.Reference == ""
                                                                       && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                       && t.Type == PaymentTransactionEnum.Cheque.ToString()));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldReturnErrorAndNotInsertTransactionsWhenSalesforceUserIsEmpty()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var createTransactionCommand = new Wonga.QA.Framework.Cs.CreateTransactionCommand
            {
                Amount = 500.50m,
                ApplicationGuid = app.Id,
                Currency = CurrencyCodeEnum.GBP,
                Reference = String.Empty,
                SalesForceUser = String.Empty,
                Scope = PaymentTransactionScopeEnum.Credit,
                Type = PaymentTransactionEnum.Cheque
            };
            var exception = Assert.Throws <ValidatorException>(() => Drive.Cs.Commands.Post(createTransactionCommand));
            Assert.Contains(exception.Errors, "Payments_SFUserId_NotSupplied");
           Assert.AreEqual(0,Drive.Db.Payments.Transactions.Count(t => t.Amount == 500.50m
                                                                       && t.ApplicationEntity.ExternalId == app.Id
                                                                       && t.Reference == ""
                                                                       && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                       && t.Type == PaymentTransactionEnum.Cheque.ToString()));
        }

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        [Row(PaymentTransactionEnum.Cheque, PaymentTransactionScopeEnum.Debit)]
        [Row(PaymentTransactionEnum.Adjustment, PaymentTransactionScopeEnum.Debit)]
        public void PaymentsShouldReturnErrorAndNotInsertTransactionsWhenTypeOrScopeIsInvalid(PaymentTransactionEnum type, PaymentTransactionScopeEnum scope)
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var createTransactionCommand = new Wonga.QA.Framework.Cs.CreateTransactionCommand
            {
                Amount = 500.50m,
                ApplicationGuid = app.Id,
                Currency = CurrencyCodeEnum.GBP,
                Reference = "",
                SalesForceUser = "test.user@wonga.com",
                Scope = scope,
                Type = type
            };
            var exc = Assert.Throws<ValidatorException>(() => Drive.Cs.Commands.Post(createTransactionCommand));
            Assert.Contains(exc.Errors, "Payments_TransactionType_Invalid");
            Assert.AreEqual(0, Drive.Db.Payments.Transactions.Count(t => t.Amount == 500.50m
                                                                        && t.ApplicationEntity.ExternalId == app.Id
                                                                        && t.Reference == ""
                                                                        && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                        && t.Type == PaymentTransactionEnum.Cheque.ToString()));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1110")]
        public void SubmittingANegativeAmountWillThrow()
        {
            var customer = CustomerBuilder.New().Build();
            var app = ApplicationBuilder.New(customer).Build();

            var createTransactionCommand = new Wonga.QA.Framework.Cs.CreateTransactionCommand
            {
                Amount = -500.50m,
                ApplicationGuid = app.Id,
                Currency = CurrencyCodeEnum.GBP,
                Reference = String.Empty,
                SalesForceUser = "test.user@wonga.com",
                Scope = PaymentTransactionScopeEnum.Credit,
                Type = PaymentTransactionEnum.Cheque
            };


            Assert.Throws<ValidatorException>(() => Drive.Cs.Commands.Post(createTransactionCommand));
        }
    }
}
