using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class LogoutTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-161"), SmokeTest]
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
