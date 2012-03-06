using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class LoginPage : BasePage
    {

        private readonly IWebElement _username;
        private readonly IWebElement _password;
        private readonly IWebElement _buttonLogin;


        public LoginPage(UiClient client)
            : base(client)
        {

            _username = Content.FindElement(By.CssSelector(Elements.Get.LoginPage.Username));
            _password = Content.FindElement(By.CssSelector(Elements.Get.LoginPage.Password));
            _buttonLogin = Content.FindElement(By.CssSelector(Elements.Get.LoginPage.LoginButton));

        }

        public MySummary LoginAs(string email)
        {

            _username.SendKeys(email);
            _password.SendKeys(Data.GetPassword());
            _buttonLogin.Click();
            return new MySummary(Client);
        }
    }
}
