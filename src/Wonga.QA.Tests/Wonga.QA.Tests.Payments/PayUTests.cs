using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [JIRA("ZA-2572")]
    [Parallelizable(TestScope.All)]
    public class PayUTests
    {
       // private int _appId;
       // private int _paymentId;
       // private dynamic _payment;

		private dynamic _incomingPartnerPaymentsDB = Drive.Data.Payments.Db.IncomingPartnerPayments;

    	private Application _appSuccessfulPay;
    	private Application _appFailedPay;

    	private decimal _amount;
    	private static readonly Guid MerchantReferenceSuccess = Guid.NewGuid();
    	private static readonly Guid MerchantReferenceFailed = Guid.NewGuid();

    	private const string PayUMockKey = "Mocks.PayUEnabled";
    	private static readonly string PayUMockValue = Drive.Data.Ops.GetServiceConfiguration<string>(PayUMockKey);

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			_appSuccessfulPay = ApplicationBuilder.New(CustomerBuilder.New().Build()).Build();
			_appFailedPay = ApplicationBuilder.New(CustomerBuilder.New().Build()).Build();

			_amount = _appSuccessfulPay.GetBalanceToday();

			Drive.Data.Ops.SetServiceConfiguration(PayUMockKey, false);
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			if( string.IsNullOrEmpty(PayUMockValue))
			{
				Drive.Data.Ops.Db.DeleteByKey(PayUMockKey);
			}

			else
			{
				Drive.Data.Ops.SetServiceConfiguration(PayUMockKey, PayUMockValue);
			}
		}

		#region SuccessfulPayment

		[Test, AUT(AUT.Za), JIRA("ZA-2572")]
		public void CreatePayUPaymentRequestTest()
		{
			CreatePayUPaymentRequest(_appSuccessfulPay, MerchantReferenceSuccess, _amount);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2572"), DependsOn("CreatePayUPaymentRequestTest")]
		public void CreatePayUPaymentResponseViaCommandTest()
		{
			CreatePayUPaymentResponse(_appSuccessfulPay, MerchantReferenceSuccess);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2572"), DependsOn("CreatePayUPaymentResponseViaCommandTest")]
		public void SuccessfulPaymentTest()
		{
			GeneratePaymentVerificationResult(MerchantReferenceSuccess, PayUTransactionResultEnum.Successful);
			Do.Until(() => _incomingPartnerPaymentsDB.FindByPaymentReference(MerchantReferenceSuccess.ToString()).SuccessOn);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2572"), DependsOn("SuccessfulPaymentTest")]
		public void SuccessfulPaymentCreatesTransactionTest()
		{
			Do.Until(() => Drive.Data.Payments.Db.Transactions.FindByReference(MerchantReferenceSuccess.ToString()));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2572"), DependsOn("SuccessfulPaymentTest")]
		public void RepayingInFullClosedApplicationTest()
		{
			Do.Until(() => _appSuccessfulPay.IsClosed);
		}

		#endregion

		#region FailedPayment

		[Test, AUT(AUT.Za), JIRA("ZA-2572")]
		public void CreatePayUPaymentResponseViaTimeoutTest()
		{
			CreatePayUPaymentRequest(_appFailedPay, MerchantReferenceFailed, 100);
			TimeoutPayUPaymentRequest(MerchantReferenceFailed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2572"), DependsOn("CreatePayUPaymentResponseViaTimeoutTest")]
		public void FailedPaymentTest()
		{
			GeneratePaymentVerificationResult(MerchantReferenceFailed, PayUTransactionResultEnum.Failed);
			Do.Until(() => _incomingPartnerPaymentsDB.FindByPaymentReference(MerchantReferenceFailed.ToString()).ConfirmedFailedOn);
		}

		#endregion

		#region Helpers

		//private string GetRawResponse()
		//{
		//    var paymentDb = Drive.Data.Payments.Db;
		//    var rawRsp = Do.Until(() => paymentDb.IncomingPartnerPaymentResponses.FindByPaymentId(_paymentId).RawConfirmationResponse);
		//    return rawRsp;
		//}

		private void CreatePayUPaymentRequest(Application application, Guid merchantReference, decimal amount )
		{
			var command = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentRequestZaCommand
			{
				ApplicationId = application.Id,
				PaymentReference = merchantReference,
				TransactionAmount = amount,
				RequestedOn = DateTime.Now,
			};

			Drive.Api.Commands.Post(command);

			Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReference.ToString()).FirstOrDefault());
		}

		private void CreatePayUPaymentResponse(Application application, Guid merchantReference)
		{
			var saveResponsecommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentResponseZaCommand
			{
				ApplicationId = application.Id,
				PaymentReference = merchantReference,
				RawRequestResponse = "RawRequestResponseData"
			};

			Drive.Api.Commands.Post(saveResponsecommand);

			Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReference.ToString() &&
																						_incomingPartnerPaymentsDB.SuccessOn != null).FirstOrDefault());
		}

		private void TimeoutPayUPaymentRequest(Guid merchantReference)
		{
			var id = Drive.Data.OpsSagas.Db.PayUProcessRequestResponseSagaEntity.FindByPaymentReference(merchantReference).Id;

			Drive.Msmq.Payments.Send(new TimeoutMessage{SagaId = id});

			Do.Until(() => Drive.Data.OpsSagas.Db.PayUPaymentConfirmationSagaEntity.FindByPaymentReference(merchantReference.ToString()));
		}

		private void GeneratePaymentVerificationResult(Guid merchantReference, PayUTransactionResultEnum result)
		{
			var paymentId = (int)_incomingPartnerPaymentsDB.FindByPaymentReference(merchantReference.ToString()).Id;
			var rawResponse = "rawresponse";

			var msg = new IWantToVerifyPayUTransactionResponseZaEvent
			{
				DateProcessed = DateTime.UtcNow,
				PaymentId = paymentId,
				PaymentReferenceNumber = merchantReference.ToString(),
				RawResponse = rawResponse,
				Result = result
			};

			Drive.Msmq.Payments.Send(msg);

			Do.Until(() => Drive.Data.Payments.Db.IncomingPartnerPaymentResponses.FindByPaymentId(paymentId));
		}

		#endregion



	}
}
