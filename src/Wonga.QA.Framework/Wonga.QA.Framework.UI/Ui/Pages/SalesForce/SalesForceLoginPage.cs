using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.SalesForce
{
    public class SalesForceLoginPage : BaseSfPage
    {
        private readonly IWebElement _username;
        private readonly IWebElement _password;
        private readonly IWebElement _buttonLogin;

        public SalesForceLoginPage(UiClient client)
            : base(client)
        {
            _username = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceLoginPage.Username));
            _password = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceLoginPage.Password));
            _buttonLogin = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceLoginPage.LoginButton));
        }

        public SalesForceHomePage LoginAs(string username, string password)
        {
            _username.SendKeys(username);
            _password.SendKeys(password);
            _buttonLogin.Click();
            return new SalesForceHomePage(Client);
        }
    }
}