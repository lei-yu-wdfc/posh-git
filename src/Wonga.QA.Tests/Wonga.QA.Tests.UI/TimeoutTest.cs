using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Tests.Ui
{
    class TimeoutTest : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-794")]
        public void AutologoutRedirectTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            mySummaryPage.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "timeout-test");
            var timeoutPage = new TimeoutTestPage(this.Client);
            System.Threading.Thread.Sleep(20000);
            var loginPage2 = new LoginPage(this.Client);
            loginPage2.LoginAs(email);
            var timeoutPage2 = new TimeoutTestPage(this.Client);
        }
    }
}
