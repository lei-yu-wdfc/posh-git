using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.Ui.PayLaterCheckout.Pages
{
	[TestFixture, AUT(AUT.Uk), Pending]
	public class MobilePinVerificationPage
	{
		[Test, JIRA("PAYLATER-196"), Pending]
		public void GivenANewUser_WhenIEnterTheCorrectPin_ICanContinueTheApplication()
		{
		}

		[Test, JIRA("PAYLATER-196"), Pending]
		public void GivenANewUser_WhenIEnterAnIncorrectPin_ICannotContinueTheApplication()
		{
		}

		[Test, JIRA("PAYLATER-197"), Pending]
		public void GivenANewUser_WhenIRequestThePinToBeResent_ISeeAMessageTellingMeItWasResent()
		{
		}

		[Test, JIRA("PAYLATER-197"), DependsOn("GivenANewUser_WhenIRequestThePinToBeResent_ISeeAMessageTellingMeItWasResent"), Pending]
		public void GivenANewUser_WhenIRequestThePinToBeResent_AnSmsContainingThePinIsSentToMyMobilePhone()
		{
		}

		[Test, JIRA("PAYLATER-188"), Pending]
		public void GivenANewUser_WhenIEnterNoPin_IWantVisualErrorCuesToBeDisplayed()
		{
		}
	}
}
