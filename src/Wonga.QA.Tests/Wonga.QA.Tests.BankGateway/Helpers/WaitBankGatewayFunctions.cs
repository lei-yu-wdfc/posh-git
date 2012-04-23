using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Tests.BankGateway.Enums;

namespace Wonga.QA.Tests.BankGateway.Helpers
{
    public static class WaitBankGatewayFunctions
    {
        public static TransactionEntity WaitForStatusOfTransaction(Guid applicationGuid, BankGatewayTransactionStatus expectedStatus = BankGatewayTransactionStatus.Paid)
        {
            TransactionEntity transaction = null;

            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(a => (a.ApplicationId == applicationGuid && a.TransactionStatus == (int)expectedStatus)));

            return transaction;
        }

        public static void WaitForStatusOfLatestTransaction(Guid applicationGuid, BankGatewayTransactionStatus expectedStatus = BankGatewayTransactionStatus.Paid, long expectNumberOfTransactions = 2)
        {
            Do.Until( () => Drive.Db.BankGateway.Transactions.LongCount(a => a.ApplicationId == applicationGuid) == expectNumberOfTransactions);
            var transactionId = Drive.Db.BankGateway.Transactions.Max(a => a.TransactionId);
            Do.Until(() => Drive.Db.BankGateway.Transactions.Single( a => (a.TransactionId == transactionId && a.TransactionStatus == (int)expectedStatus)));
        }

        public static void WaitForAckForTransaction(TransactionEntity transaction)
        {
            Do.Until(() => Drive.Db.BankGateway.Acknowledges.Any(ack => ack.TransactionID == transaction.TransactionId));
        }
    }
}
