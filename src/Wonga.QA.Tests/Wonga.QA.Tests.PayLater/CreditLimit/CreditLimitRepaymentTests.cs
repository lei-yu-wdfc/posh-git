using MbUnit.Framework;
using Wonga.QA.Framework.Account.Queries;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.TrustRating
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class CreditLimitRepaymentTests
	{
		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplication_WhenAFullRepaymentIsMade_ThePayLaterCreditLimitIncrease()
		{
			var account = AccountBuilder.PayLater.New().Build();
			decimal originalTrustRating = account.PayLaterCredit;

			const int purchaseValue = 150;
			var application = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValue).Build();
			application.Repay();

			var postRepaymentCreditLimit = account.PayLaterCredit;
			Assert.AreEqual(originalTrustRating, postRepaymentCreditLimit);
		}

		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplication_WhenAPartialRepaymentIsMade_ThePayLaterCreditLimitDoesNotChange()
		{
			var account = AccountBuilder.PayLater.New().Build();

			const int purchaseValue = 150;
			var application = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValue).Build();
			var postPurchaseCreditLimit = account.PayLaterCredit;

			const int repaymentAmount = 100;
			application.Repay(repaymentAmount);

			var postRepaymentCreditLimit = account.PayLaterCredit;
			Assert.AreEqual(postPurchaseCreditLimit, postRepaymentCreditLimit);
		}

		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnAccountWithMultipleApplications_WhenOneApplicationIsPartiallyRepaid_ThePayLaterCreditLimitDoesNotChange()
		{
			var account = AccountBuilder.PayLater.New().Build();
			var originalCredit = account.PayLaterCredit;

			const int purchaseValueA = 150;
			const int purchaseValueB = 100;
			var applicationA = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValueA).Build();
			var applicationB = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValueB).Build();

			const int repaymentAmountA = 100;
			const int repaymentAmountB = 50;
			applicationA.Repay(repaymentAmountA);
			applicationB.Repay(repaymentAmountB);

			var expectedCreditLimit = originalCredit - (purchaseValueA + purchaseValueB);
			var postRepaymentCreditLimit = account.PayLaterCredit;
			Assert.AreEqual(expectedCreditLimit, postRepaymentCreditLimit);
		}
	}
}
