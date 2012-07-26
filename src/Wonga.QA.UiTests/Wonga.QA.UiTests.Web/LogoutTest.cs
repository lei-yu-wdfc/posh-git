using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class LogoutTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-161"), Category(TestCategories.SmokeTest)]
        public void LogOutShouldBeRedirectedToHomePage()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            CustomerBuilder.New().WithEmailAddress(email).Build();
            var page = loginPage.LoginAs(email);

            var homePage = Client.Home();
            homePage.Login.Logout();
            var title = page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.Title));
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                    Assert.AreEqual(UiMap.Get.HomePage.TitleText, title.Text);
                    break;
            }



        }

    }
}
