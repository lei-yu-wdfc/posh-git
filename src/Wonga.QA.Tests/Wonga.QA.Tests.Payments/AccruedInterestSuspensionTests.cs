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
        /// <summary>
        /// Transactions db table
        /// </summary>
        private dynamic _transactionsDb;

        private Application _application;
        private Customer _customer;
        private DateTime _promiseDate; 
        [SetUp]
        public void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _promiseDate = DateTime.Now.AddDays(12);
            _application = ApplicationBuilder.New(_customer).WithPromiseDate(new Date(_promiseDate)).Build();
            _transactionsDb = Drive.Data.Payments.Db.Transactions;
        }

        [Test]
        public void GoingIntoArrears_Creates_SuspendInterestTransaction()
        {
            _application.PutApplicationIntoArrears(1);
            dynamic suspendTransaction = null;
            Do.Until(() => suspendTransaction = _transactionsDb
                                                    .FindBy(Type: PaymentTransactionEnum.SuspendInterestAccrual,
                                                            PostedOn: _promiseDate.AddDays(60)));
            Assert.IsNotNull(suspendTransaction);
        }
    }
}