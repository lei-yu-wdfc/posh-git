using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
<<<<<<< HEAD
using System.Xml.Linq;
=======
>>>>>>> AT for PayU ZA-2570 and ZA-2571
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture, Parallelizable(TestScope.All)]
	public class PayUCommandTests
	{
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
		public void SavePayURequest_Expect_Success()
		{
			Guid merchantReferenceNumber = Guid.NewGuid();
			Decimal transactionAmount = 1000;

			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

<<<<<<< HEAD
			var command = new Wonga.QA.Framework.Api.SavePayURequestZaCommand
=======
			var command = new SavePayURequestZaCommand
>>>>>>> AT for PayU ZA-2570 and ZA-2571
			{
				ApplicationId = app.Id,
				 MerchantReferenceNumber = merchantReferenceNumber,
				 TransactionAmount = transactionAmount,
				 RequestedOn = DateTime.Now,
			};

			//Act
			Drive.Api.Commands.Post(command);
			
			var incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber.ToString()).FirstOrDefault());
			var application = _applicationDB.FindByExternalId(app.Id);

			//Assert
			Assert.AreEqual(transactionAmount, incomingPartnerPayment.TransactionAmount);
			Assert.AreEqual(application.ApplicationId, incomingPartnerPayment.ApplicationId);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2571")]
		public void SavePayUResponse_Expect_Success()
		{
			Guid merchantReferenceNumber = Guid.NewGuid();
			Decimal transactionAmount = 1000;

			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

<<<<<<< HEAD
			var saveRequestcommand = new Wonga.QA.Framework.Api.SavePayURequestZaCommand
=======
			var saveRequestcommand = new SavePayURequestZaCommand
>>>>>>> AT for PayU ZA-2570 and ZA-2571
			{
				ApplicationId = app.Id,
				MerchantReferenceNumber = merchantReferenceNumber,
				TransactionAmount = transactionAmount,
				RequestedOn = DateTime.Now,
			};

			Drive.Api.Commands.Post(saveRequestcommand);

			var incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber.ToString()).FirstOrDefault());

<<<<<<< HEAD
			var saveResponsecommand = new Wonga.QA.Framework.Api.SavePayUResponseZaCommand
=======
			var saveResponsecommand = new SavePayUResponseZaCommand
>>>>>>> AT for PayU ZA-2570 and ZA-2571
			{
				ApplicationId = app.Id,
				MerchantReferenceNumber = merchantReferenceNumber,
				TransactionAmount = transactionAmount,
<<<<<<< HEAD
				RawRequestResponse = "RawRequestResponseData", 
=======
				RawRequestResponse = "RawRequestResponseData",
>>>>>>> AT for PayU ZA-2570 and ZA-2571
			};

			//Act
			Drive.Api.Commands.Post(saveResponsecommand);

			var incomingPartnerPaymentResponses = Do.Until(() => _incomingPartnerPaymentResponsesDB.FindAll(_incomingPartnerPaymentResponsesDB.PaymentId == incomingPartnerPayment.id).FirstOrDefault());

			incomingPartnerPayment = Do.Until(() => _incomingPartnerPaymentsDB.FindAll(_incomingPartnerPaymentsDB.PaymentReference == merchantReferenceNumber.ToString() &&
																						_incomingPartnerPaymentsDB.SuccessOn != null).FirstOrDefault());

			//Assert
			Assert.IsNotNull(incomingPartnerPaymentResponses);
			Assert.AreEqual("RawRequestResponseData", incomingPartnerPaymentResponses.RawRequestResponse);
			Assert.IsNotNull(incomingPartnerPayment);
<<<<<<< HEAD
		}
=======
		}		
>>>>>>> AT for PayU ZA-2570 and ZA-2571
	}
}