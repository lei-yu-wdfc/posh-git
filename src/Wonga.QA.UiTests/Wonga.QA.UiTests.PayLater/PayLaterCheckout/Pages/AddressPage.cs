using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater.Ui.PayLaterCheckout.Pages
{
	[TestFixture, AUT(AUT.Uk), Pending]
	public class AddressPage
	{
		[Test, JIRA("PAYLATER-191", "PAYLATER-190", "PAYLATER-189"), Pending]
		public void GivenANewUser_WhenIEnterAFullyMatchedAddress_MyFullAddressOnlyIsDisplayed()
		{
		}

		[Test, JIRA("PAYLATER-191", "PAYLATER-190", "PAYLATER-189"), Pending]
		public void GivenANewUser_WhenIEnterANonMatchedAddress_AListOfCloseMatchesIsDisplayed()
		{
		}

		[Test, JIRA("PAYLATER-191"), Pending]
		public void GivenANewUser_WhenISelectMyFullAddress_IWantToBeAbleToEditEachField()
		{
		}

		[Test, JIRA("PAYLATER-191"), Pending]
		public void GivenANewUser_WhenIClickTheNextButton_ICanContinueMyApplication()
		{
		}

		[Test, JIRA("PAYLATER-188"), Pending]
		public void GivenANewUser_WhenIEnterAnIncorrectPostcode_IWantVisualErrorCuesToBeDisplayed()
		{
		}

		[Test, JIRA("PAYLATER-188"), Pending]
		public void GivenANewUser_WhenIEnterAnIncorrectHouseNumber_IWantVisualErrorCuesToBeDisplayed()
		{
		}
	}
}
