using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class MySummary : UiTest
    {

        [Test, AUT(AUT.Ca)]
        public void IsRepaymentWarningAvailableForLn()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            CustomerBuilder.New().WithEmailAddress(email).Build();
            var summary = loginPage.LoginAs(email);

            summary.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "/repay-canada");
            var xpath = summary.Client.Driver.FindElement(By.CssSelector("#content-area p:nth-child(1)"));
            Assert.IsTrue(xpath.Text.Contains("via online banking"));
        }

        [Test, AUT(AUT.Ca)]
        public void ThisIsTestSoBuggerOff()
        {
            
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-953")]
        // Check the my Summary page after we click My Summary buton
        // UKWEB-953: Web elements on My Summary and Repay pages are shifted
        public void ClickMySummaryButton()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();

            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
        }
    }
}
