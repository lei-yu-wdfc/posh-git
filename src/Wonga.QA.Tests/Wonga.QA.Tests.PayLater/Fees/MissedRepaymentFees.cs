using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.Fees
{
	[TestFixture, AUT(AUT.Uk), JIRA("PAYLTRST-166"), Parallelizable(TestScope.All)]
	public class MissedRepaymentFees
	{
		[Test, Pending]
		public void GivenACustomerInArrears_WhenInArrearsAfter29Days_ThenArrearsChargeNotApplied()
		{
		}

		[Test, Pending]
		public void GivenACustomerInArrears_WhenInArrearsAfter30Days_ThenArrearsChargeApplied()
		{
		}

		[Test, Pending]
		public void GivenACustomerInArrears_WhenInArrearsAfter90Days_ThenArrearsChargeNotApplied()
		{
		}

		[Test, Pending]
		public void GivenACustomerInArrears_WhenInArrearsAfter120Days_ThenArrearsChargeNotApplied()
		{
		}
	}
}
