using MbUnit.Framework;
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
		}
	}
}
