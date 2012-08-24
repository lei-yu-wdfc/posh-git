using MbUnit.Framework;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.TrustRating
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class TrustRatingMissedPaymentFeeTests
	{
		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplicationWithAMissedPaymentCharge_WhenAFullRepaymentIsMade_TheAccountTrustRatingIsIncreased()
		{
			var account = AccountBuilder.PayLater.New().Build();
			var originalTrustRating = account.TrustRating;

			const int purchaseValue = 150;
			var application = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValue).Build();

			application.PutFirstInstallmentIntoArrears();
			application.Repay();

			var expectedTrustRating = originalTrustRating;
			var postRepaymentTrustRating = account.TrustRating;
			Assert.AreEqual(expectedTrustRating, postRepaymentTrustRating);
		}
	}
}
