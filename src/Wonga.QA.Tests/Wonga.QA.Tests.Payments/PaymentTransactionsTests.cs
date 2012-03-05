using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture]
    public class PaymentTransactionsTests
    {
        /*
                Given a loan has been accepted and approved 
When the cash out has been sent to the business bank account 
Then create cashadvance, fee and interest transactions on the application.
    */
        [Test, AUT(AUT.Wb), JIRA("SME-787")]
        public void PaymentsShouldCreateTransactionsWhenLoanInitialAdvanceTransfered()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var app =
                ApplicationBuilder.New(customer, organization).WithExpectedDecision(
                    ApplicationDecisionStatusEnum.Accepted).Build();

            Assert.AreEqual(1, Do.Until(
                () =>
                GetTransactionCount(t => app.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                && t.Type == PaymentTransactionEnum.Fee.ToString())));

            Assert.AreEqual(1, Do.With().Timeout(TimeSpan.FromSeconds(5)).Until(
                () =>
                GetTransactionCount(t => app.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                && t.Type == PaymentTransactionEnum.Interest.ToString())));

            Assert.AreEqual(1, Do.With().Timeout(TimeSpan.FromSeconds(5)).Until(
                () =>
                GetTransactionCount(t => app.Id == t.ApplicationEntity.ExternalId && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                && t.Type == PaymentTransactionEnum.CashAdvance.ToString())));
        }

        private int GetTransactionCount(Func<TransactionEntity, bool> func)
        {
            return Driver.Db.Payments.Transactions.Count(func);
        }
    }
}
