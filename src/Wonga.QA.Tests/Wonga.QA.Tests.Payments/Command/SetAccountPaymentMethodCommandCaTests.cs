using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Command
{
	[TestFixture, Ignore("Commands are not used")]
	public class SetAccountPaymentMethodCommandCaTests
	{
		private readonly dynamic _dbAccountPreferences = Drive.Data.Payments.Db.AccountPreferences;
		private readonly dynamic _dbBankGateWayTransactions = Drive.Data.BankGateway.Db.Transactions;
		private readonly dynamic _dbEmails = Drive.Data.QaData.Db.Email;

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row(PaymentMethodEnum.BankAccount)]
		[Row(PaymentMethodEnum.ETransfer)]
		[Row(PaymentMethodEnum.PayPal)]
		public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenPreferredCashoutPaymentMethodIdIsUpdatedOnDatabase(PaymentMethodEnum paymentMethod)
		{
			var customer = ApplyForALoanAndRepay();

            var preference = PostSetAccountPaymentMethodCaCommandAndWait(paymentMethod, customer.Id);

			Assert.AreEqual(GetDbPaymentMethodId(paymentMethod), preference.CashoutPaymentMethodId);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row(PaymentMethodEnum.BankAccount)]
		[Row(PaymentMethodEnum.ETransfer)]
		[Row(PaymentMethodEnum.PayPal)]
		public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenGetAccountSummaryQueryResponseHasExpectedValue(PaymentMethodEnum expectedPaymentMethod)
		{
            var customer = ApplyForALoanAndRepay();

            PostSetAccountPaymentMethodCaCommandAndWait(expectedPaymentMethod, customer.Id);

            var response = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id});

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
        public void GivenAL0Customer_WhenHeTriesToSetAccountPaymentMethod_ThenApiReturnsError()
        {
            const string expectedError = "Payments_Customer_HasNotCompletedLoan";

        	var application = ApplyForALoan();

            try
            {
                PostSetAccountPaymentMethodCaCommand(application.AccountId, PaymentMethodEnum.BankAccount);

                Assert.Fail("Posting an invalid command should have failed");
            }
            catch (ValidatorException e)
            {
                Assert.Contains(e.Errors, expectedError);
            }
        }


		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		public void GivenALNCustomerWithALiveLoan_WhenHeTriesToSetAccountPaymentMethod_ThenApiReturnsError()
		{
			const string expectedError = "Payments_Customer_HasLiveLoans";

			Customer customer = ApplyForALoanAndRepay();

			ApplicationBuilder.New(customer).Build();

			try
			{
				PostSetAccountPaymentMethodCaCommand(customer.Id, PaymentMethodEnum.BankAccount);

				Assert.Fail("Posting an invalid command should have failed");
			}
			catch (ValidatorException e)
			{
				Assert.Contains(e.Errors, expectedError);
			}
		}


        [Test, AUT(AUT.Ca), JIRA("CA-1896")]
		[Row(PaymentMethodEnum.BankAccount, true)]
		[Row(PaymentMethodEnum.ETransfer, false)]
		[Row(PaymentMethodEnum.PayPal, false)]
		public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenCheckBankGatewayTransaction(PaymentMethodEnum paymentMethod, bool expectedBankGatewayTransaction)
		{
            var customer = ApplyForALoanAndRepay();

			PostSetAccountPaymentMethodCaCommandAndWait(paymentMethod, customer.Id);

			var application = ApplicationBuilder.New(customer).Build();

			var transaction = GetDbBankGatewayTransaction(application.Id);

			Assert.IsTrue((transaction != null) == expectedBankGatewayTransaction);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1896")]
		[Row(PaymentMethodEnum.BankAccount)]
		[Row(PaymentMethodEnum.ETransfer)]
		[Row(PaymentMethodEnum.PayPal)]
		public void SetAccountPaymentMethodCaCommand_WhenNewLoanIsPaid_ThenItShouldBeClosed(PaymentMethodEnum paymentMethod)
		{
			var customer = ApplyForALoanAndRepay();

			PostSetAccountPaymentMethodCaCommandAndWait(paymentMethod, customer.Id);

			var application = ApplyForAnotherLoanAndRepay(customer);

			Assert.IsTrue(application.IsClosed);
		}


        [Test, AUT(AUT.Ca), JIRA("CA-1896")]
        [Row(PaymentMethodEnum.ETransfer)]
        [Row(PaymentMethodEnum.PayPal)]
        public void SetAccountPaymentMethodCaCommand_WhenValidPaymentMethod_ThenEmailShouldBeSent(PaymentMethodEnum paymentMethod)
        {
        	const string emailAddress = "qa.wonga.com@gmail.com";
        	const string emailTemplate = "Email.ManualPaymentNotificationEmailTemplate";

        	int ? currentEmailId = GetMostRecentEmailId(emailAddress, emailTemplate);
            var customer = ApplyForALoanAndRepay();

            PostSetAccountPaymentMethodCaCommandAndWait(paymentMethod, customer.Id);

            var application = ApplicationBuilder.New(customer).Build();

			var emailTokens = GetEmailTokens(emailAddress, "Email.ManualPaymentNotificationEmailTemplate", currentEmailId);

        	AssertManualPaymentNotificationEmailTokens(paymentMethod, application, customer, emailTokens);
        }

		private static void AssertManualPaymentNotificationEmailTokens(PaymentMethodEnum expectedPaymentMethod, Application application,
		                                                               Customer customer, IEnumerable<EmailToken> emailTokens)
		{
			EmailToken emailToken = emailTokens.SingleOrDefault(et => et.Key == "Html_body");
			Assert.IsNotNull(emailToken);
			Assert.IsTrue(emailToken.Value.Contains(customer.GetEmail()));
			Assert.IsTrue(emailToken.Value.Contains(application.LoanAmount.ToString("C2", new CultureInfo("en-CA"))));
			Assert.IsTrue(emailToken.Value.Contains(application.AccountId.ToString()));
			Assert.IsTrue(emailToken.Value.Contains(expectedPaymentMethod.ToString()));
		}

		private int ? GetMostRecentEmailId(string email, string emailTemplateName)
		{
			var templateId = Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == emailTemplateName).Value;
			
			//there can be more than one email sent (get latest)
			var emailFound =
				_dbEmails.FindAll(_dbEmails.EmailAddress == email && _dbEmails.TemplateName == templateId)
					.OrderByDescending(_dbEmails.EmailId)
					.FirstOrDefault();

			return
				emailFound != null
					? emailFound.EmailId
					: null;
		}

        private List<EmailToken> GetEmailTokens(string email, String emailTemplateName, int ? higherThanEmailId)
        {
            var templateId = Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == emailTemplateName).Value;
            var db = new DbDriver().QaData;

			//there can be more than one email sent
            var emailId = Do.Until(() => db.Emails.Single(
				e => e.EmailAddress == email 
					&& e.TemplateName == templateId 
					&& (higherThanEmailId == null || e.EmailId > higherThanEmailId)).EmailId);
            return db.EmailTokens.Where(et => et.EmailId == emailId).ToList();
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

		private static Customer ApplyForALoanAndRepay()
		{
			Customer customer = CreateCustomer();
			var application = ApplicationBuilder.New(customer).Build();
		    application.RepayOnDueDate();

            return customer;
		}

		private static Application ApplyForAnotherLoanAndRepay(Customer customer)
		{
			var application = ApplicationBuilder.New(customer).Build();
			application.RepayOnDueDate();
			return application;
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
