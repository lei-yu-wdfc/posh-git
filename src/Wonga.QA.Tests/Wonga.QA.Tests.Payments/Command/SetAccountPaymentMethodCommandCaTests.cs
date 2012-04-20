using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Command
{
	[TestFixture]
	public class SetAccountPaymentMethodCommandCaTests
	{
		private readonly dynamic _dbAccountPreferences = Drive.Data.Payments.Db.AccountPreferences;
		private readonly dynamic _dbBankGateWayTransactions = Drive.Data.BankGateway.Db.Transactions;

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row(PaymentMethodEnum.BankAccount)]
		[Row(PaymentMethodEnum.ETransfer)]
		[Row(PaymentMethodEnum.PayPal)]
		public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenPreferredCashoutPaymentMethodIdIsUpdatedOnDatabase(PaymentMethodEnum paymentMethod)
		{
			var application = ApplyForALoan();

			var preference = PostSetAccountPaymentMethodCaCommandAndWait(paymentMethod, application.AccountId);

			Assert.AreEqual(GetDbPaymentMethodId(paymentMethod), preference.CashoutPaymentMethodId);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row(PaymentMethodEnum.BankAccount)]
		[Row(PaymentMethodEnum.ETransfer)]
		[Row(PaymentMethodEnum.PayPal)]
		public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenGetAccountSummaryQueryResponseHasExpectedValue(PaymentMethodEnum expectedPaymentMethod)
		{
			var application = ApplyForALoan();

			PostSetAccountPaymentMethodCaCommandAndWait(expectedPaymentMethod, application.AccountId);

			var response = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = application.AccountId});

			Assert.AreEqual(expectedPaymentMethod.ToString(), response.Values["CashoutPaymentMethod"].Single());
		}


		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row("NonExistentPaymentMethod", "Ops_RequestXmlInvalid")]
		[Row("", "Ops_RequestXmlInvalid")]
		[Row(null, "Ops_RequestXmlInvalid")]
		public void SetAccountPaymentMethodCaCommand_WhenInvalidPaymentMethod_ThenApiReturnsError(string invalidPaymentMethod, string expectedError)
		{
			var customer = CreateCustomer();

			try
			{
				PostSetAccountPaymentMethodCaCommand(customer.Id, invalidPaymentMethod);

				Assert.Fail("Posting an invalid command should have failed");
			}
			catch (ValidatorException e)
			{
				Assert.Contains(e.Errors, expectedError);
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		public void SetAccountPaymentMethodCaCommand_WhenAccountIdDoesNotExist_ThenApiReturnsError()
		{
			const string expectedError = "AccountId_NotFound";

			try
			{
				PostSetAccountPaymentMethodCaCommand(Guid.NewGuid(), PaymentMethodEnum.BankAccount);

				Assert.Fail("Posting an invalid command should have failed");
			}
			catch (ValidatorException e)
			{
				Assert.Contains(e.Errors, expectedError);
			}
		}


		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row(PaymentMethodEnum.BankAccount, true)]
		[Row(PaymentMethodEnum.ETransfer, false)]
		[Row(PaymentMethodEnum.PayPal, false)]
		public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenCheckBankGatewayTransaction(PaymentMethodEnum paymentMethod, bool expectedBankGatewayTransaction)
		{
			var customer = CreateCustomer();

			PostSetAccountPaymentMethodCaCommandAndWait(paymentMethod, customer.Id);

			var application = ApplicationBuilder.New(customer).Build();

			var transaction = GetDbBankGatewayTransaction(application.Id);

			Assert.IsTrue((transaction != null) == expectedBankGatewayTransaction);
		}

		private dynamic PostSetAccountPaymentMethodCaCommandAndWait(PaymentMethodEnum paymentMethod, Guid accountId)
		{
			PostSetAccountPaymentMethodCaCommand(accountId, paymentMethod);

			return WaitForAccountPreferenceWithPaymentMethodId(accountId, paymentMethod);
		}

		private static ApiResponse PostSetAccountPaymentMethodCaCommand(Guid accountId, object paymentMethod)
		{
			var command = new SetAccountPaymentMethodCaCommand
			              	{
			              		AccountId = accountId,
			              		CashoutPaymentMethod = paymentMethod
			              	};

			return Drive.Api.Commands.Post(command);
		}

		private static Application ApplyForALoan()
		{
			Customer customer = CreateCustomer();
			return ApplicationBuilder.New(customer).Build();
		}

		private static Customer CreateCustomer()
		{
			return CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
		}

		private dynamic WaitForAccountPreferenceWithPaymentMethodId(Guid accountId, PaymentMethodEnum paymentMethod)
		{
			int paymentMethodId = GetDbPaymentMethodId(paymentMethod);

			var preference = Do.Until(
				() =>
				_dbAccountPreferences.FindAll(
					_dbAccountPreferences.AccountId == accountId
					&&
					_dbAccountPreferences.CashoutPaymentMethodId == paymentMethodId
					).First());
			return preference;
		}


		private dynamic GetDbBankGatewayTransaction(Guid applicationId)
		{
			return _dbBankGateWayTransactions.FindAll(
					_dbBankGateWayTransactions.ApplicationId == applicationId
					).FirstOrDefault();
		}

		private int GetDbPaymentMethodId(PaymentMethodEnum  paymentMethod)
		{
			return (int) (paymentMethod) + 1;
		}
	}
}
