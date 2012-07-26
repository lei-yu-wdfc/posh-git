using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class TimeoutTest : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-794", "UKWEB-238")]
        public void AutologoutRedirectTest()
        {
            string email = Get.RandomEmail();

            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();

            var mySummaryPage = Client.Login().LoginAs(email);

            mySummaryPage.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "timeout-test");
            var timeoutPage = new TimeoutTestPage(this.Client);

            System.Threading.Thread.Sleep(20000); // already logged out

            var loginPage2 = new LoginPage(this.Client);

            loginPage2.LoginRedirectAs(email);

            var timeoutPageReopened = new TimeoutTestPage(this.Client); // redirected to test page. 
        }

        [Test, AUT(AUT.Uk), JIRA("UK-794"), Pending("UKWEB-948: After timeout and login, the My Summary page does not open")]
        public void AutologoutDoesNotRedirectTest()
        {
            string email = Get.RandomEmail();

            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();

            var mySummaryPage = Client.Login().LoginAs(email);

            mySummaryPage.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "timeout-test-no-redirect");
            var timeoutPage = new TimeoutTestPage(this.Client);

            System.Threading.Thread.Sleep(20000); // already logged out

            var loginPage2 = new LoginPage(this.Client);

            loginPage2.LoginAs(email); // not redirected to test page. My Summary page opens.
        }
    }
}
