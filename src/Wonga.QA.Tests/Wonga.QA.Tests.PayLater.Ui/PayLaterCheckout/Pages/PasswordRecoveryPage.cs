using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.PayLater.Ui.PayLaterCheckout.Pages
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class PasswordRecoveryPage : UiTest
	{
		[Test, JIRA("PAYLTRST-164"), Pending]
		public void GivenAnExistingUser_WhenTheyClickForgottenPasswordLink_TheForgotPasswordPageIsDisplayed()
		{
		}

		[Test, JIRA("PAYLTRST-164"), Pending]
		public void GivenAnExistingUser_WhenTheyAnswerSecurityQuestions_ForgotPasswordEmailIsSent()
		{
		}

		[Test, JIRA("PAYLATER-188"), Pending]
		public void GivenANewUser_WhenIEnterNoSecurityAnswer_IWantVisualErrorCuesToBeDisplayed()
		{
		}
	}
}
