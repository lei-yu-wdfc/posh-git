﻿using System;
using System.ComponentModel;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class GetAccountSummaryQueryTests
	{
		[Test, AUT(AUT.Uk), JIRA("UK-795")]
		[Ignore]
		public void GetAccountSummaryTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			ApplicationBuilder.New(customer).Build();

			var response = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
			//£100 loan for 10 days.
			Assert.AreEqual(115.91M, decimal.Parse(response.Values["CurrentLoanRepaymentAmountOnDueDate"].Single()));
			Assert.AreEqual(DateTime.Today.AddDays(10), DateTime.Parse(response.Values["CurrentLoanDueDate"].Single()));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		public void GetAccountSummary_WhenNoPreferredCashoutPaymentMethod_ThenGetNullCashoutPaymentMethod()
		{
			Customer customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
			ApplicationBuilder.New(customer).Build();

			var response = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });

			Assert.IsNull(response.Values["CashoutPaymentMethod"].Single());
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1972")]
		public void GetAccountSummary_return_EasyPayNumber()
		{
			Customer customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
			ApplicationBuilder.New(customer).Build();

			//Assert
			dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == customer.Id)
													.FirstOrDefault());
			Assert.IsNotNull(ra);
			Assert.IsNotNull(ra.RepaymentNumber);

			var response = Drive.Api.Queries.Post(new GetAccountSummaryZaQuery() { AccountId = customer.Id });

			Assert.IsNotNull(response.Values["EasyPayNumber"].Single());
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1951")]
		[Row(PaymentMethodEnum.BankAccount)]
		[Row(PaymentMethodEnum.ETransfer)]
		[Row(PaymentMethodEnum.PayPal)]
		public void GetAccountSummary_WhenPreferredCashoutPaymentMethod_ThenGetExpectedCashoutPaymentMethod(PaymentMethodEnum expectedPaymentMethod)
		{
			Customer customer = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
			Application application = ApplicationBuilder.New(customer).Build();

			UpdateDbCashoutPaymentMethodForAccountPreference(application.AccountId, expectedPaymentMethod);

			var response = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });

			Assert.AreEqual(expectedPaymentMethod.ToString(), response.Values["CashoutPaymentMethod"].Single());
		}


		private void UpdateDbCashoutPaymentMethodForAccountPreference(Guid accountId, PaymentMethodEnum paymentMethod)
		{
			//DB payment method starts in 1
			Drive.Data.Payments.Db.AccountPreferences.UpdateByAccountId(AccountId: accountId, CashoutPaymentMethodId: (int)(paymentMethod) + 1);
		}

	}
}