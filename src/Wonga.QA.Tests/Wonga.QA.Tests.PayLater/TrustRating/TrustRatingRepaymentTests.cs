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
		public void GivenAnApplication_WhenAFullRepaymentIsMade_TheAccountTrustRatingIsIncreased()
		{
			var account = AccountBuilder.PayLater.New().Build();
			var originalTrustRating = AccountQueries.PayLater.GetTrustRating(account.Id);

			var purchaseValue = 100;
			var application = ApplicationBuilder.PayLater.New(account);
		}

		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnApplication_WhenAPartialRepaymentIsMade_TheAccountTrustRatingIsIncreased()
		{
			
		}

		[Test, JIRA("PAYLATER-337"), Pending]
		public void GivenAnAccountWithMultipleApplications_WhenOneApplicationIsPartiallyRepaid_TheAccountTrustRatingIsIncreased()
		{
			
		}
	}
}
