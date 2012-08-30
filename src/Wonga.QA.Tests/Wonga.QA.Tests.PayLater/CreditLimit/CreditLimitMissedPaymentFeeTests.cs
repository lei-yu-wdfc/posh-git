using MbUnit.Framework;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.CreditLimit
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class CreditLimitMissedPaymentFeeTests
	{
		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplicationWithAMissedPaymentCharge_WhenAFullRepaymentIsMade_ThePayLaterCreditLimitIsIncreased()
		{
			var account = AccountBuilder.PayLater.New().Build();
			var originalTrustRating = account.PayLaterCredit;

			const int purchaseValue = 150;
			var application = ApplicationBuilder.PayLater.New(account).WithTotalAmount(purchaseValue).Build();

			application.PutFirstInstallmentIntoArrears();
			application.Repay();

			var expectedTrustRating = originalTrustRating;
			var availablePayLaterCredit = account.PayLaterCredit;
			Assert.AreEqual(expectedTrustRating, availablePayLaterCredit);
		}
	}
}
