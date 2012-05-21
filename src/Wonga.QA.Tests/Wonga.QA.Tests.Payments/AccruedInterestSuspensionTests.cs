using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    [Ignore("Work in progress")]
    public class AccruedInterestSuspensionTests
    {
        private dynamic _transactionsDb;

        private Application _application;
        private Customer _customer;
        private DateTime _promiseDate; 
        [SetUp]
        public void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer).Build();

            _transactionsDb = Drive.Data.Payments.Db.Transactions;
        }

        [Test]
        public void GoingIntoArrears_Creates_SuspendInterestTransaction()
        {
            _application.PutApplicationIntoArrears(1);
            dynamic suspendTransaction = null;
            Do.Until(() => suspendTransaction = _transactionsDb
                                                    .FindBy(Type: PaymentTransactionEnum.SuspendInterestAccrual));
        }
    }
}