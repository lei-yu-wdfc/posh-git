using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System.Threading;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture, Parallelizable(TestScope.All)]
	public class IncomingPartnerPaymentCommandTests
    {
    	private dynamic _payUProcessRequestResponseSagaEntityDB = Drive.Data.OpsSagas.Db.PayUProcessRequestResponseSagaEntity;
		private dynamic _transactionsDB = Drive.Data.Payments.Db.Transactions;
		private dynamic _incomingPartnerPaymentResponsesDB = Drive.Data.Payments.Db.IncomingPartnerPaymentResponses;
		private dynamic _incomingPartnerPaymentsDB = Drive.Data.Payments.Db.IncomingPartnerPayments;
		private dynamic _applicationDB = Drive.Data.Payments.Db.Applications;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2571")]
		public void SaveIncomingPartnerPaymentRequest_Expect_IncomingPartnerPaymentCreated()
		{
			string merchantReferenceNumber = Guid.NewGuid().ToString();
			Decimal transactionAmount = 1000;

			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var command = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentRequestZaCommand
			{
				ApplicationId = app.Id,
				 PaymentReference = merchantReferenceNumber,
				 TransactionAmount = transactionAmount,
				 RequestedOn = DateTime.Now,
			};

			//Act
			Drive.Api.Commands.Post(command);
			
			var incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber).FirstOrDefault());
			var application = _applicationDB.FindByExternalId(app.Id);

			//Assert
			Assert.AreEqual(transactionAmount, incomingPartnerPayment.TransactionAmount);
			Assert.AreEqual(application.ApplicationId, incomingPartnerPayment.ApplicationId);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2571")]
		public void SaveIncomingPartnerPaymentResponse_Expect_IncomingPartnerPaymentResponseCreated()
		{
			string merchantReferenceNumber = Guid.NewGuid().ToString();
			Decimal transactionAmount = 1000;

			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var saveRequestcommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentRequestZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				TransactionAmount = transactionAmount,
				RequestedOn = DateTime.Now,
			};

			Drive.Api.Commands.Post(saveRequestcommand);

			var incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber).FirstOrDefault());

			var saveResponsecommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentResponseZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				RawRequestResponse = "RawRequestResponseData", 
			};

			//Act
			Drive.Api.Commands.Post(saveResponsecommand);

			var incomingPartnerPaymentResponses = Do.Until(() => _incomingPartnerPaymentResponsesDB.FindAll(_incomingPartnerPaymentResponsesDB.PaymentId == incomingPartnerPayment.id).FirstOrDefault());

			incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber &&
																						_incomingPartnerPaymentsDB.SuccessOn != null).FirstOrDefault());

			//Assert
			Assert.IsNotNull(incomingPartnerPaymentResponses);
			Assert.AreEqual("RawRequestResponseData", incomingPartnerPaymentResponses.RawRequestResponse);
			Assert.IsNotNull(incomingPartnerPayment);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2571, ZA-2572, ZA-2523")]
		public void SaveIncomingPartnerPaymentResponse_Expect_ConfirmationSuccess()
		{
			//Arrange
			string merchantReferenceNumber = Guid.NewGuid().ToString();
			Decimal transactionAmount = 1000;

			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var saveRequestcommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentRequestZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				TransactionAmount = transactionAmount,
				RequestedOn = DateTime.Now,
			};

			Drive.Api.Commands.Post(saveRequestcommand);

			var incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber).FirstOrDefault());

			var saveResponsecommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentResponseZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				RawRequestResponse = "RawRequestResponseData",
			};

			//Act
			Drive.Api.Commands.Post(saveResponsecommand);



			incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber &&
																						_incomingPartnerPaymentsDB.ConfirmedOn != null &&
																						_incomingPartnerPaymentsDB.SuccessOn != null)
																						.FirstOrDefault());

			var incomingPartnerPaymentResponses = Do.Until(() => _incomingPartnerPaymentResponsesDB.FindAll(_incomingPartnerPaymentResponsesDB.PaymentId == incomingPartnerPayment.id &&
																											_incomingPartnerPaymentResponsesDB.RawConfirmationResponse != null)
																											.FirstOrDefault());

			//Assert
			Assert.IsNotNull(incomingPartnerPayment != null);
			Assert.IsNotNull(incomingPartnerPaymentResponses != null);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2571, ZA-2572, ZA-2523")]
		public void SaveIncomingPartnerPaymentResponse_Expect_DirectBankPaymentTransactionsCreated()
		{
			string merchantReferenceNumber = Guid.NewGuid().ToString();
			Decimal transactionAmount = 1000;

			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var saveRequestcommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentRequestZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				TransactionAmount = transactionAmount,
				RequestedOn = DateTime.Now,
			};

			Drive.Api.Commands.Post(saveRequestcommand);

			var incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber).FirstOrDefault());

			var saveResponsecommand = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentResponseZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				RawRequestResponse = "RawRequestResponseData",
			};

			//Act
			Drive.Api.Commands.Post(saveResponsecommand);

			var incomingPartnerPaymentResponses = Do.Until(() => _incomingPartnerPaymentResponsesDB.FindAll(_incomingPartnerPaymentResponsesDB.PaymentId == incomingPartnerPayment.id)
																											.FirstOrDefault());

			var application = _applicationDB.FindByExternalId(app.Id);
			var transaction = Do.Until(() => _transactionsDB.FindAll(_transactionsDB.ApplicationId == application.ApplicationId &&
																	_transactionsDB.Reference == merchantReferenceNumber &&
																	_transactionsDB.Type == PaymentTransactionType.DirectBankPayment.ToString() &&
																	_transactionsDB.Amount == (-1) * transactionAmount)
																	.Single());

			Assert.IsNotNull(incomingPartnerPayment != null);
			Assert.IsNotNull(incomingPartnerPaymentResponses != null);
			Assert.IsNotNull(transaction);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2571")]
		public void SaveIncomingPartnerPaymentRequest_WaitForTimeOut_Expect_DirectBankPaymentTransactionsCreated()
		{
			//Arrange
			string merchantReferenceNumber = Guid.NewGuid().ToString();
			Decimal transactionAmount = 1000;

			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var command = new Wonga.QA.Framework.Api.SaveIncomingPartnerPaymentRequestZaCommand
			{
				ApplicationId = app.Id,
				PaymentReference = merchantReferenceNumber,
				TransactionAmount = transactionAmount,
				RequestedOn = DateTime.Now,
			};

			//Act
			Drive.Api.Commands.Post(command);

			var sagaEntity = Do.Until(() => _payUProcessRequestResponseSagaEntityDB.FindAll(_payUProcessRequestResponseSagaEntityDB.PaymentReference == merchantReferenceNumber).Single());

			//Timeout Immediately 
			Drive.Msmq.Payments.Send(new TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id });

			var application = _applicationDB.FindByExternalId(app.Id);
			var transaction = Do.Until(() => _transactionsDB.FindAll(_transactionsDB.ApplicationId == application.ApplicationId &&
																	_transactionsDB.Reference == merchantReferenceNumber &&
																	_transactionsDB.Type == PaymentTransactionType.DirectBankPayment.ToString() &&
																	_transactionsDB.Amount == (-1) * transactionAmount)
																	.Single());

			Assert.IsNotNull(transaction);
		}

	}
}