using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Easypay.Za;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All)]
	public class EasyPayTests
	{
		private readonly dynamic _repaymentAccountsTable = Drive.Data.Payments.Db.RepaymentAccount;

		[Test, AUT(AUT.Za), JIRA("ZA-2395")]
		public void RepayUsingEasyPayWillCreateTransactionWhenPaymentReceived()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			string easyPayNumber = GetEasyPayNumber(customer);
			RepayWithEasyPay(easyPayNumber, null, DateTime.UtcNow.Date, 10M);

			var paymentsAppId = (int)(Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id)).ApplicationId;
			var transactionReference = (string)(Do.Until(() => Drive.Data.Payments.Db.Transactions.FindByAmountAndApplicationId(-10M, paymentsAppId))).Reference;

			Assert.AreEqual("Payment from EasyPay", transactionReference);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395")]
		public void RepayUsingEasyPayFullRepaymentBeforeDueDateClosesApplication()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var balance = application.GetBalanceToday();
			string easyPayNumber = GetEasyPayNumber(customer);
			RepayWithEasyPay(easyPayNumber, null, DateTime.UtcNow.Date, balance);

			Do.With.Timeout(1).Until(() => application.IsClosed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395") , ExpectedException(typeof(DoException))]
		public void RepayUsingEasyPayPartialRepaymentBeforeDueDateDoesntCloseApplication()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var balance = application.GetBalanceToday();
			string easyPayNumber = GetEasyPayNumber(customer);
			RepayWithEasyPay(easyPayNumber, null, DateTime.UtcNow.Date, balance / 2);

			Do.With.Timeout(new TimeSpan(0, 0, 0, 10)).Until(() => application.IsClosed);
		}

		#region Helpers

		private string GetEasyPayNumber(Customer customer)
		{
			var repaymentAccount =
				Do.Until(() => _repaymentAccountsTable.FindBy(AccountId: customer.Id, RepaymentAccountType: 1));
			Assert.IsNotNull(repaymentAccount);
			Assert.IsNotNull(repaymentAccount.RepaymentNumber);
			return repaymentAccount.RepaymentNumber;
		}

		private static void RepayWithEasyPay(string easyPayNumber, string rawContent, DateTime actionDate, decimal amount)
		{
			Drive.Msmq.EasyPay.Send(new PaymentResponseDetailRecordZaCommand
			                        	{
			                        		ActionDate = actionDate,
			                        		Amount = amount,
			                        		RepaymentNumber = easyPayNumber,
			                        		Filename = "TestFile",
			                        		RawContent = rawContent
			                        	});
		}

		#endregion
	}
}
