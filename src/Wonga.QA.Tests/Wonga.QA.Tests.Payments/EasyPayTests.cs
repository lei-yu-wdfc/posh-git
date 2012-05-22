using System;
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

		[Test, AUT(AUT.Za), JIRA("ZA-2395")]
		public void RepayUsingEasyPayWillCreateTransactionWhenPaymentReceived()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			//var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			var paymentsAppId = (int)(Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id)).ApplicationId;

			CreateEasyPayNumberForCustomer(customer);
			string easyPayNumber = GetEasyPayNumber(customer);

			Act(easyPayNumber, null, DateTime.UtcNow.Date, 10M);

			//This will cause payment to handle event and create transaction.
			var transactionReference = (string)(Do.Until(() => Drive.Data.Payments.Db.Transactions.FindByAmountAndApplicationId(-10M, paymentsAppId))).Reference;

			Assert.AreEqual("Payment from EasyPay", transactionReference);
		}

		#region Helpers

		private static void CreateEasyPayNumberForCustomer(Customer customer)
		{
			Drive.Msmq.Payments.Send(new GenerateRepaymentNumberCsCommand
			                         	{
			                         		AccountId = customer.Id
			                         	});
		}

		private string GetEasyPayNumber(Customer customer)
		{
			var repaymentAccount =
				Do.Until(() => _repaymentAccountsTable.FindBy(AccountId: customer.Id, RepaymentAccountType: 1));
			Assert.IsNotNull(repaymentAccount);
			Assert.IsNotNull(repaymentAccount.RepaymentNumber);
			return repaymentAccount.RepaymentNumber;
		}

		private static void Act(string easyPayNumber, string rawContent, DateTime actionDate, decimal amount)
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
