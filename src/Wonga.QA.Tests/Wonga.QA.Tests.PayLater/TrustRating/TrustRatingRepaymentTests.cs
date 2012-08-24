using MbUnit.Framework;
using Wonga.QA.Framework.Account.Queries;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.TrustRating
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class TrustRatingRepaymentTests
	{
		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplication_WhenAFullRepaymentIsMade_TheAccountTrustRatingIncrease()
		{
			var account = AccountBuilder.PayLater.New().Build();
			decimal originalTrustRating = account.TrustRating;

			const int purchaseValue = 150;
			var application = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValue).Build();
			application.Repay();

			var postRepaymentTrustRating = AccountQueries.PayLater.GetTrustRating(account.Id);
			Assert.AreEqual(originalTrustRating, postRepaymentTrustRating);
		}

		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplication_WhenAPartialRepaymentIsMade_TheAccountTrustRatingDoesNotChange()
		{
			var account = AccountBuilder.PayLater.New().Build();

			const int purchaseValue = 150;
			var application = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValue).Build();
			var postPurchaseTrustRating = account.TrustRating;

			const int repaymentAmount = 100;
			application.Repay(repaymentAmount);

			var postRepaymentTrustRating = AccountQueries.PayLater.GetTrustRating(account.Id);
			Assert.AreEqual(postPurchaseTrustRating, postRepaymentTrustRating);
		}

		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnAccountWithMultipleApplications_WhenOneApplicationIsPartiallyRepaid_TheAccountTrustRatingDoesNotChange()
		{
			var account = AccountBuilder.PayLater.New().Build();
			var originalTrustRating = account.TrustRating;

			const int purchaseValueA = 150;
			const int purchaseValueB = 100;
			var applicationA = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValueA).Build();
			var applicationB = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValueB).Build();

			const int repaymentAmountA = 100;
			const int repaymentAmountB = 50;
			applicationA.Repay(repaymentAmountA);
			applicationB.Repay(repaymentAmountB);

			var expectedTrustRating = originalTrustRating - (purchaseValueA + purchaseValueB);
			var postRepaymentTrustRating = account.TrustRating;
			Assert.AreEqual(expectedTrustRating, postRepaymentTrustRating);
		}
	}
}
