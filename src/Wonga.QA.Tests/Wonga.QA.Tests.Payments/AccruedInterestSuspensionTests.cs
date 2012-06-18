using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.Self)]
    public class AccruedInterestSuspensionTests
    {
        private const string InArrearsMaxInterestDaysKey = "Payments.InArrearsMaxInterestDays";
        /// <summary>
        /// Transactions db table
        /// </summary>
        private dynamic _transactionsDb;

        private dynamic _serviceConfigDb;

        private Application _application;
        private Customer _customer;
        [SetUp]
        public void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer).WithLoanTerm(12).Build();
            _transactionsDb = Drive.Data.Payments.Db.Transactions;
            _serviceConfigDb = Drive.Data.Ops.Db.ServiceConfigurations;
        }

        [Test]
        public void GoingIntoArrears_Creates_SuspendInterestTransaction_InTheFuture()
        {
            _application.PutIntoArrears(60);
            dynamic suspendTransaction = null;
            string offset = _serviceConfigDb.FindBy(Key: InArrearsMaxInterestDaysKey).Value.ToString();
            int intOffset = Int32.Parse(offset);
            Do.Until(() => suspendTransaction = _transactionsDb
                                                    .FindBy(Type: PaymentTransactionEnum.SuspendInterestAccrual,
                                                            PostedOn: DateTime.Today.AddDays(intOffset)));
            Assert.IsNotNull(suspendTransaction);
        }
    }
}