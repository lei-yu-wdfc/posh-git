using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagasCa;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class DelayBeforeApplicationClosedInMinutes
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;

        private readonly dynamic _opsSagasTakeBankPaymentInternalSagaEntity =
            Drive.Db.OpsSagasCa.TakeBankPaymentInternalSagaEntities;

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            Drive.Data.Ops.SetServiceConfiguration("Payments.DelayBeforeApplicationClosedInMinutes", 5);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            Drive.Data.Ops.SetServiceConfiguration("Payments.DelayBeforeApplicationClosedInMinutes", 0);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2372")]
        public void WhenDelayBeforeApplicationClosedInMinutesTimePeriodEndsThenLoanShouldClose()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).Single());

            application.MakeDueToday();

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            application.PutApplicationFurtherIntoArrears(3);

            TakeBankPaymentInternalSagaEntity takeBankPaymentInternalSaga = Do.Until(() => Drive.Db.OpsSagasCa.TakeBankPaymentInternalSagaEntities.Single(r => r.ApplicationId == application.Id));

            takeBankPaymentInternalSaga.PaymentTakenOnDate -= new TimeSpan(3, 0, 0, 0);
            takeBankPaymentInternalSaga.Submit(true);

            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = takeBankPaymentInternalSaga.Id });

            Do.Until(() => application.IsClosed);
        }
    }
}
