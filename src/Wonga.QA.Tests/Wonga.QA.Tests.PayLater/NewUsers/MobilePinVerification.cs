using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater
{
	[TestFixture, AUT(AUT.Uk), Pending]
	public class MobilePinVerification
	{
		[Test, JIRA("PAYLATER-196"), Pending]
		public void GivenANewUser_WhenIEnterTheCorrectPin_MyMobilePhoneIsVerified()
		{
		}

		[Test, JIRA("PAYLATER-196"), Pending]
		public void GivenANewUser_WhenIEnterTheCorrectPin_MyMobilePhoneIsNotVerified()
		{
		}

		[Test, JIRA("PAYLATER-197"), Pending]
		public void GivenANewUser_WhenIRequestThePinToBeResent_AnSmsIsSentToMyMobilePhoneNumber()
		{
		}
	}
}
