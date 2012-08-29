using System;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;

namespace Wonga.QA.Tests.Payments
{
	class PaymentOperations
	{
		private static readonly dynamic PaymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;
		private static readonly dynamic PaymentTransactions = Drive.Data.Payments.Db.Transactions;

		public static void TakePayment(Guid id, Guid paymentCard,decimal amount)
		{
			Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = amount,
				ApplicationId = id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = paymentCard,
				PaymentId = Guid.NewGuid(),
				SalesforceUser = "test.user@wonga.com"
			});
		}

		public static void CheckPaymentTransaction(int applicationID, decimal paymentamount)
		{
			paymentamount = paymentamount * -1;
			Do.Until<bool>(() => PaymentTransactions.FindAll(PaymentTransactions.ApplicationId == applicationID && PaymentTransactions.Amount == paymentamount).Count() == 1);
		}

		public static void CheckPaymentsSupressionTransaction(int applicationId,decimal amount)
		{
			Do.Until<bool>(() => PaymentRequests.FindAll(PaymentRequests.ApplicationId == applicationId && PaymentRequests.Amount == amount &&
												   PaymentRequests.StatusDescription == "Payment Collections Suppressed").Count()>0);
		}
	}
}
