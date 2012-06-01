using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Sagas
{
    [JIRA("ZA-2572")]
    [Parallelizable(TestScope.Self)]
    public class PayUPaymentConfirmationSagaTests
    {
        private int _appId;
        private int _paymentId;
        private dynamic _payment;

        [FixtureSetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            //Create payment request entry.
            var paymentDb = Drive.Data.Payments.Db;
            _appId = paymentDb.Applications.FindByExternalId(application.Id).ApplicationId;
        }

        [SetUp]
        public void TestSetup()
        {
            _payment = new
                           {
                               ApplicationId = _appId,
                               PaymentReference = Guid.NewGuid().ToString(),
                               TransactionAmount = 20.56M,
                               PartnerType = 0,
                               RequestedOn = DateTime.UtcNow
                           };
            var paymentDb = Drive.Data.Payments.Db;
            _paymentId = paymentDb.IncomingPartnerPayments.Insert(_payment).Id;

/*            var paymentResponse = new
                                      {
                                          PaymentId = _paymentId,
                                          CreatedOn = DateTime.UtcNow
                                      };
            paymentDb.IncomingPartnerPaymentResponses.Insert(paymentResponse);*/
        }

        [Test]
        [AUT(AUT.Za), JIRA("ZA-2572")]
        public void SendConfirmationTest()
        {
            var msg = new ConfirmIncomingPaymentCommand() {PaymentId = _paymentId};
            Drive.Msmq.Payments.Send(msg);
            //Check saga is created. and payment record is updated with ConfirmedOn date
            var sagasDb = Drive.Data.OpsSagas.Db;
            var saga = Do.Until(() => sagasDb.PayUPaymentConfirmationSagaEntity.FindByPaymentId(_paymentId));
            Assert.IsNotNull(saga);
            Assert.AreEqual(_payment.PaymentReference, saga.PaymentReference);
            Assert.AreEqual(_payment.TransactionAmount, saga.Amount);

            var paymentDb = Drive.Data.Payments.Db;
            var payment = paymentDb.IncomingPartnerPayments.FindById(_paymentId);
            Assert.AreEqual(DateTime.UtcNow.Date, payment.ConfirmedOn.Date);
        }

        [Test]
        [AUT(AUT.Za), JIRA("ZA-2572")]
        public void SuccessResponseCreateTransactionForApplicationTest()
        {
            var msg = new ConfirmIncomingPaymentCommand() { PaymentId = _paymentId };
            Drive.Msmq.Payments.Send(msg);

            var responseEvent = new IWantToVerifyPayUTransactionResponseZaEvent()
                      {
                          DateProcessed = DateTime.UtcNow,
                          PaymentId = _paymentId,
                          PaymentReferenceNumber = _payment.PaymentReference,
                          RawResponse = "SafeShopEncoded",
                          Result = PayUTransactionResultEnum.Successful
                      };
            Drive.Msmq.Payments.Send(responseEvent);

            //Asset response is saved. payment is updated for SuccessOn date. Transaction is created. Saga is completed.
            Assert.AreEqual(responseEvent.RawResponse, GetRawResponse());

            var paymentDb = Drive.Data.Payments.Db;
            var successOn = Do.Until(() => paymentDb.IncomingPartnerPayments.FindById(_paymentId).SuccessOn);
            Assert.AreEqual(DateTime.UtcNow.Date, successOn.Date);

            var trs = Do.Until(() => paymentDb.Transactions.FindByReference(_payment.PaymentReference));
            Assert.AreEqual(_payment.TransactionAmount * -1, trs.Amount);

            var saga = Drive.Data.OpsSagas.Db.PayUPaymentConfirmationSagaEntity.FindByPaymentId(_paymentId);
            Assert.IsNull(saga);
        }

        [Test]
        [AUT(AUT.Za), JIRA("ZA-2572")]
        public void FailedResponseTest()
        {
            var msg = new ConfirmIncomingPaymentCommand() { PaymentId = _paymentId };
            Drive.Msmq.Payments.Send(msg);

            var responseEvent = new IWantToVerifyPayUTransactionResponseZaEvent()
            {
                DateProcessed = DateTime.UtcNow,
                PaymentId = _paymentId,
                PaymentReferenceNumber = _payment.PaymentReference,
                RawResponse = "SafeShopEncoded",
                Result = PayUTransactionResultEnum.Failed
            };
            Drive.Msmq.Payments.Send(responseEvent);

            //Asset response is saved. payment is updated for ConfirmedFailedOn date. Transaction is NOT created. Saga is completed.
            Assert.AreEqual(responseEvent.RawResponse, GetRawResponse());

            var paymentDb = Drive.Data.Payments.Db;
            var confirmedFailedOn = Do.Until(() => paymentDb.IncomingPartnerPayments.FindById(_paymentId).ConfirmedFailedOn);
            Assert.AreEqual(DateTime.UtcNow.Date, confirmedFailedOn.Date);

            var trs = paymentDb.Transactions.FindByReference(_payment.PaymentReference);
            Assert.IsNull(trs);

            var saga = Drive.Data.OpsSagas.Db.PayUPaymentConfirmationSagaEntity.FindByPaymentId(_paymentId);
            Assert.IsNull(saga);            
        }

        [Test]
        [AUT(AUT.Za), JIRA("ZA-2572")]
        public void TimeoutSagaTest()
        {
            var msg = new ConfirmIncomingPaymentCommand() { PaymentId = _paymentId };
            Drive.Msmq.Payments.Send(msg);
            
            var saga = Do.Until(() => Drive.Data.OpsSagas.Db.PayUPaymentConfirmationSagaEntity.FindByPaymentId(_paymentId));

            var timeOutMessage = new TimeoutMessage()
            {
                Expires = DateTime.UtcNow,
                State = null,
                SagaId = saga.Id 
            };
            Drive.Msmq.Payments.Send(timeOutMessage);

            //payment is updated for FailedOn date. Transaction is NOT created. Saga is completed.
            var paymentDb = Drive.Data.Payments.Db;
            var failedOn = Do.Until(() => paymentDb.IncomingPartnerPayments.FindById(_paymentId).FailedOn);
            Assert.AreEqual(DateTime.UtcNow.Date, failedOn.Date);

            var trs = paymentDb.Transactions.FindByReference(_payment.PaymentReference);
            Assert.IsNull(trs);

            saga = Drive.Data.OpsSagas.Db.PayUPaymentConfirmationSagaEntity.FindByPaymentId(_paymentId);
            Assert.IsNull(saga);   
        }

        private string GetRawResponse()
        {
            var paymentDb = Drive.Data.Payments.Db;
            var rawRsp = Do.Until(() => paymentDb.IncomingPartnerPaymentResponses.FindByPaymentId(_paymentId).RawConfirmationResponse);
            return rawRsp;
        }
    }
}
