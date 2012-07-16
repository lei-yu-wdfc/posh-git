
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.Ui.PayLaterDotCom
{
	[TestFixture, AUT(AUT.Uk), Pending]
	public class WongaDotComReferences
	{
		[Test, JIRA("PAYLATER-222"), Pending]
		public void GivenAnExistingWongaCustomerWithLiveLoans_WhenIViewMyAccountPageOnPayLaterDotCom_ThenISeeWongaLoanAgreement()
		{}

		[Test, JIRA("PAYLATER-222"), Pending]
		public void GivenAnExistingWongaCustomerWithoutLiveLoans_WhenIViewMyAccountPageOnPayLaterDotCom_ThenISeeALinkToMyAccountOnWongaDotCom()
		{ }
	}
}
