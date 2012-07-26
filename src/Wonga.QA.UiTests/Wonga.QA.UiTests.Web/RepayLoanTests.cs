using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
	public class RepayLoanTests : UiTest
	{
		[Test, AUT(AUT.Za), JIRA("ZA-1972"), Pending]
		public void EasyPayNumberDisplayed()
		{
			var customer = CustomerBuilder.New().Build();
			ApplicationBuilder.New(customer).Build();

			var loginPage = Client.Login();
			var myAccountPage = loginPage.LoginAs(customer.Email);
			myAccountPage.RepayButtonClick();
		}
	}
}
