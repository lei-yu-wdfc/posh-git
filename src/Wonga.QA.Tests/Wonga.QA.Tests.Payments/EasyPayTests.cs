﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All)]
	public class EasyPayTests
	{
		private readonly dynamic _repaymentAccountsTable = Drive.Data.Payments.Db.RepaymentAccount;
		private Application _application;
		private string _easyPayNumber;
		private const decimal LoanAmount = 500;
		private const decimal PartialRepayAmount = 10;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			var customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).WithLoanAmount(LoanAmount).Build();
			_easyPayNumber = GetEasyPayNumber(customer);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), ExpectedException(typeof(DoException))]
		public void RepayUsingEasyPayPartialRepaymentBeforeDueDateDoesntCloseApplication()
		{
			RepayWithEasyPay(_easyPayNumber, null, DateTime.UtcNow.Date, PartialRepayAmount);

			Do.With.Timeout(new TimeSpan(0, 0, 0, 10)).Until(() => _application.IsClosed);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), DependsOn("RepayUsingEasyPayPartialRepaymentBeforeDueDateDoesntCloseApplication")]
		public void RepayUsingEasyPayWillCreateTransactionWhenPaymentReceived()
		{
			var paymentsAppId = (int)(Drive.Data.Payments.Db.Applications.FindByExternalId(_application.Id)).ApplicationId;
			var transactionReference = (string)(Do.Until(() => Drive.Data.Payments.Db.Transactions.FindByAmountAndApplicationId(-PartialRepayAmount, paymentsAppId))).Reference;

			Assert.AreEqual("Payment from EasyPay", transactionReference);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), DependsOn("RepayUsingEasyPayPartialRepaymentBeforeDueDateDoesntCloseApplication")]
		public void RepayUsingEasyPayFullRepaymentBeforeDueDateClosesApplication()
		{
			Do.Until(() => _application.GetBalance() > LoanAmount); //Race condition where loan amount can equal balance
			var balance = _application.GetBalanceToday();
			RepayWithEasyPay(_easyPayNumber, null, DateTime.UtcNow.Date, balance);

			Do.With.Until(() => _application.IsClosed);
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