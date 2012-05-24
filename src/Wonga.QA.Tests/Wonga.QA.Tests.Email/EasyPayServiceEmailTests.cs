using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Email
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All), Pending("ZA-2565")]
	public class EasyPayServiceEmailTests
	{
		[Test, AUT(AUT.Za), JIRA("ZA-2289", "ZA-2396"), Pending("ZA-2565")]
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

		[Test, AUT(AUT.Za), JIRA("ZA-2289", "ZA-2396"), Pending("ZA-2565")]
		public void EmailIsSentWhenNoApplicationCanBeFoundForEasyPayNumber()
		{
			var customer = CustomerBuilder.New().Build();

			EmailIsSentForCustomerWithNoOpenApplication(customer.Id);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2289", "ZA-2396"), Pending("ZA-2565")]
		public void EmailIsSentWhenNoOpenApplicationCanBeFoundForEasyPayNumber()
		{
			var customer = CustomerBuilder.New().Build();
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			EmailIsSentForCustomerWithNoOpenApplication(customer.Id);
		}

		#region Helpers

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

		private static void EmailIsSentForCustomerWithNoOpenApplication(Guid accountId)
		{
			DateTime actionDate = DateTime.UtcNow;
			const decimal amount = 100.25m;
			string amountString = amount.ToString("F2", CultureInfo.InvariantCulture);
			string easyPayNumber = GetEasyPayNumber(accountId);
			string rawContent = GenerateRawContent(amountString, easyPayNumber, actionDate);

			Act(easyPayNumber, rawContent, actionDate, amount);

			AssertNoOpenApplicationEmailIsSent(easyPayNumber, amountString, accountId);
		}

		private static string GetEasyPayNumber(Guid accountId)
		{
			var repaymentAccount =
				Do.Until(() => Drive.Data.Payments.Db.RepaymentAccount.FindBy(
					AccountId: accountId, RepaymentAccountType: 1));
			Assert.IsNotNull(repaymentAccount);
			Assert.IsNotNull(repaymentAccount.RepaymentNumber);
			return repaymentAccount.RepaymentNumber;
		}

		private static void AssertInvalidEasyPayNumberEmailIsSent(string amountString, string easyPayNumber)
		{
			var query = CreateEmailTokensQuery(easyPayNumber, "34407");

			AssertTokensContain(query, amountString, easyPayNumber);
		}

		private static void AssertNoOpenApplicationEmailIsSent(string easyPayNumber, string amountString, Guid accountId)
		{
			var query = CreateEmailTokensQuery(easyPayNumber, "34408");

			AssertTokensContain(query, amountString, easyPayNumber, accountId.ToString());
		}

		private static dynamic CreateEmailTokensQuery(string easyPayNumber, string templateName)
		{
			const string financeEmailAddress = "qa.wonga.com+financeza@gmail.com";
			dynamic emailTokenTable = Drive.Data.QaData.Db.EmailToken;

			return Do.Until(() => emailTokenTable.FindAll(
				emailTokenTable.Email.EmailAddress == financeEmailAddress &&
				emailTokenTable.Email.TemplateName == templateName &&
				emailTokenTable.Value.Like(string.Format("%{0}%", easyPayNumber))));
		}

		private static void AssertTokensContain(dynamic query, params string[] values)
		{
			foreach (string value in values)
			{
				Assert.Contains(GetEmailToken(query, "Html_body").Value, value);
				Assert.Contains(GetEmailToken(query, "Plain_body").Value, value);
			}
		}

		private static dynamic GetEmailToken(dynamic query, string key)
		{
			return Do.Until(() => query.FindBy(Key: key));
		}

		#endregion
	}
}
