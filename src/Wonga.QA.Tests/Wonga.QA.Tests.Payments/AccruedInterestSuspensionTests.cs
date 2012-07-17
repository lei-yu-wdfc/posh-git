using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
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
        private dynamic _transactionsTable;
        private dynamic _serviceConfigTable;
        private dynamic _applicationsTable;
        private dynamic _fixedTermApplicationsTable;

        private Application _application;
        private Customer _customer;

        [SetUp]
        public void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer).WithLoanTerm(12).Build();
            _transactionsTable = Drive.Data.Payments.Db.Transactions;
            _serviceConfigTable = Drive.Data.Ops.Db.ServiceConfigurations;
            _fixedTermApplicationsTable = Drive.Data.Payments.Db.FixedTermLoanApplications;
            _applicationsTable = Drive.Data.Payments.Db.Applications;
        }

        [Test]
        public void GoingIntoArrears_Creates_SuspendInterestTransaction_InTheFuture()
        {
            //Make sure the payment attempt fails by changing the expiry date of the card.
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: _customer.GetPaymentCard(),
                                                    ExpiryDate: new DateTime(DateTime.Now.Year - 1, 1, 31));
            _application.PutIntoArrears(60);
            dynamic suspendTransaction = null;
            dynamic application = null;
            dynamic fixedTermApplication = null;
            string offset = _serviceConfigTable.FindBy(Key: InArrearsMaxInterestDaysKey).Value.ToString();
            int intOffset = Int32.Parse(offset);

            Do.Until(() => application = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => fixedTermApplication = _fixedTermApplicationsTable.FindBy(ApplicationId: application.ApplicationId));
            DateTime expectedDate = ((DateTime)fixedTermApplication.NextDueDate).AddDays(intOffset);

            Do.Until(() => suspendTransaction = _transactionsTable
                                                    .FindBy(Type: PaymentTransactionEnum.SuspendInterestAccrual,
                                                            ApplicationId: application.ApplicationId, 
                                                            PostedOn: expectedDate));
            Assert.IsNotNull(suspendTransaction);
        }
    }
}