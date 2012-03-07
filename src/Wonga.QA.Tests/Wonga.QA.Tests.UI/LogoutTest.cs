using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Ui
{
    class LogoutTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-161")]
        public void LogOutShouldBeRedirectedToHomePage()
        {
            var loginPage = Client.Login();
            string email = Data.GetEmail();
            CustomerBuilder.New().WithEmailAddress(email).Build();
            var page = loginPage.LoginAs(email);

            var homePage = Client.Home();
            homePage.Login.Logout();
            var title = page.Client.Driver.FindElement(By.XPath("//div[@id='block-wonga-1']/h2[1]"));
            switch (Config.AUT)
            {
                case AUT.Za:
                    Assert.AreEqual("Flexible, short-term loans that give you lots of control", title.Text);
                    break;
                case AUT.Ca:
                    Assert.AreEqual("Flexible, short term loans that give you lots of control", title.Text);
                    break;
            }



        }

    }
}
