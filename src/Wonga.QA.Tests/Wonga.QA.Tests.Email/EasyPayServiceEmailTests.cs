using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Email
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All)]
	public class EasyPayServiceEmailTests
	{
		private const string FinanceEmailAddress = "qa.wonga.com+financeza@gmail.com";
		private const string InvalidEasyPayNumberReceivedEmailTemplate = "34407";
		private const string NoOpenApplicationForEasyPayNumberReceivedEmailTemplate = "34408";
		private const string HtmlBody = "Html_body";
		private const string PlainBody = "Plain_body";

		private readonly dynamic _emailTokenTable = Drive.Data.QaData.Db.EmailToken;
		private readonly dynamic _repaymentAccountsTable = Drive.Data.Payments.Db.RepaymentAccount;


		[Test, AUT(AUT.Za), JIRA("ZA-2289", "ZA-2396")]
		public void EmailIsSentWhenNoAccountCanBeFoundForEasyPayNumber()
		{
			DateTime actionDate = DateTime.UtcNow;
			const decimal amount = 100.25m;
			string easyPayNumber = Guid.NewGuid().ToString();
			string amountString = amount.ToString("F2", CultureInfo.InvariantCulture);
			string rawContent = GenerateRawContent(amountString, easyPayNumber, actionDate);

			Act(easyPayNumber, rawContent, actionDate, amount);

			AssertInvalidEasyPayNumberEmailIsSent(amountString, easyPayNumber);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2289", "ZA-2396")]
		public void EmailIsSentWhenNoApplicationCanBeFoundForEasyPayNumber()
		{
			var customer = CustomerBuilder.New().Build();
			CreateEasyPayNumberForCustomer(customer);

			EmailIsSentForCustomerWithNoOpenApplication(customer);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2289", "ZA-2396")]
		public void EmailIsSentWhenNoOpenApplicationCanBeFoundForEasyPayNumber()
		{
			var customer = CustomerBuilder.New().Build();
			CreateEasyPayNumberForCustomer(customer);
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			EmailIsSentForCustomerWithNoOpenApplication(customer);
		}

		#region Helpers

		private void EmailIsSentForCustomerWithNoOpenApplication(Customer customer)
		{
			DateTime actionDate = DateTime.UtcNow;
			const decimal amount = 100.25m;
			string amountString = amount.ToString("F2", CultureInfo.InvariantCulture);
			string easyPayNumber = GetEasyPayNumber(customer);
			string rawContent = GenerateRawContent(amountString, easyPayNumber, actionDate);

			Act(easyPayNumber, rawContent, actionDate, amount);

			AssertNoOpenApplicationEmailIsSent(customer, easyPayNumber, amountString);
		}

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

		private static string GenerateRawContent(string amountString, string easyPayNumber, DateTime actionDate)
		{
			return string.Format(
				"ActionDate={0};RepaymentNumber={1};Amount={2};Fee=5.38;Collector=006001007036034",
				actionDate,
				easyPayNumber,
				amountString);
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

		private void AssertInvalidEasyPayNumberEmailIsSent(string amountString, string easyPayNumber)
		{
			var query = CreateEmailTokensQuery(easyPayNumber, InvalidEasyPayNumberReceivedEmailTemplate);
			var htmlToken = GetEmailToken(query, HtmlBody);
			var plainToken = GetEmailToken(query, PlainBody);

			Assert.IsNotNull(htmlToken);
			Assert.IsNotNull(plainToken);

			Assert.Contains(htmlToken.Value, amountString);
			Assert.Contains(plainToken.Value, amountString);

			Assert.Contains(htmlToken.Value, easyPayNumber);
			Assert.Contains(plainToken.Value, easyPayNumber);
		}

		private void AssertNoOpenApplicationEmailIsSent(Customer customer, string easyPayNumber, string amountString)
		{
			var query = CreateEmailTokensQuery(easyPayNumber, NoOpenApplicationForEasyPayNumberReceivedEmailTemplate);
			var htmlToken = GetEmailToken(query, HtmlBody);
			var plainToken = GetEmailToken(query, PlainBody);

			Assert.IsNotNull(htmlToken);
			Assert.IsNotNull(plainToken);

			Assert.Contains(htmlToken.Value, amountString);
			Assert.Contains(plainToken.Value, amountString);

			Assert.Contains(htmlToken.Value, easyPayNumber);
			Assert.Contains(plainToken.Value, easyPayNumber);

			Assert.Contains(htmlToken.Value, customer.Id.ToString());
			Assert.Contains(plainToken.Value, customer.Id.ToString());
		}

		private dynamic CreateEmailTokensQuery(string easyPayNumber, string templateName)
		{
			return Do.Until(() => _emailTokenTable.FindAll(
				_emailTokenTable.Email.EmailAddress == FinanceEmailAddress &&
				_emailTokenTable.Email.TemplateName == templateName &&
				_emailTokenTable.Value.Like(string.Format("%{0}%", easyPayNumber))));
		}

		private static dynamic GetEmailToken(dynamic query, string key)
		{
			return Do.Until(() => query.FindBy(Key: key));
		}

		#endregion
	}
}
