using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.PayLater.Ui.PayLaterCheckout.Pages
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class EmploymentDetailsPage : UiTest
	{
		[Test, JIRA("PAYLTRST-164", "PAYLATER-193"), Pending]
		public void GivenANewUser_WhenTheyEnterEmploymentStatusAndIncome_ThenOtherFieldsBecomeVisible()
		{
			
		}

		[Test, JIRA("PAYLATER-188"), Pending]
		public void GivenANewUser_WhenIEnterNoEmploymentStatus_IWantVisualErrorCuesToBeDisplayed()
		{
		}

		[Test, JIRA("PAYLATER-188"), Pending]
		public void GivenANewUser_WhenIEnterNoIncomeAmount_IWantVisualErrorCuesToBeDisplayed()
		{
		}
	}
}
