using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.ThirdParties.SalesforceApi;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All), Pending("ZA-2565")]
	public class EasyPayNumberInSalesforceTests : SalesforceTestBase
	{
		[Test, AUT(AUT.Za), JIRA("ZA-2537"), Pending("ZA-2565")]
		public void EasyPayNumberIsPushedToSalesforceAfterAccountCreation()
		{
			Customer customer = CustomerBuilder.New().Build();
			string easyPayNumber = GetEasyPayNumber(customer.Id);
			Account salesforceAccount = GetSalesforceAccountWithEasyPayNumber(customer.Id);

			Assert.AreEqual(easyPayNumber, salesforceAccount.EasyPay_Number__c);
		}

		private static string GetEasyPayNumber(Guid accountId)
		{
			var repaymentAccount =
				Do.Until(() => Drive.Data.Payments.Db.RepaymentAccount.FindBy(
					AccountId: accountId, RepaymentAccountType: 1));
			Assert.IsNotNull(repaymentAccount.RepaymentNumber);
			return repaymentAccount.RepaymentNumber;
		}

		private Account GetSalesforceAccountWithEasyPayNumber(Guid accountId)
		{
			return Do.Until(() =>
			                	{
			                		Account a = Salesforce.GetAccountByAccountId(accountId.ToString());
			                		return a == null || a.EasyPay_Number__c == null ? null : a;
			                	});
		}
	}
}
