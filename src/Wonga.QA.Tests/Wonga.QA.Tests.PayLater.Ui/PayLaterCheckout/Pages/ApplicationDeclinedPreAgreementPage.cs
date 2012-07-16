using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.Ui.PayLaterCheckout.Pages
{
	[TestFixture, AUT(AUT.Uk), Pending]
	public class ApplicationDeclinedPreAgreementPage
	{
		[Test, JIRA("PAYLATER-380"), Pending]
		public void GivenACustomerWithAPayLaterApplicationInArrears_WhenICompleteTheCheckoutJourney_ISeeADeclineMessage()
		{
		}

		[Test, JIRA("PAYLATER-380"), Pending]
		public void GivenACustomerWithAWongaLoanInArrears_WhenICompleteTheCheckoutJourney_ISeeADeclineMessage()
		{
		}
	}
}
