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
        private readonly IWebElement _forgotPassword;

        public LoginPage(UiClient client)
            : base(client)
        {

            _username = Content.FindElement(By.CssSelector(Ui.Get.LoginPage.Username));
            _password = Content.FindElement(By.CssSelector(Ui.Get.LoginPage.Password));
            _buttonLogin = Content.FindElement(By.CssSelector(Ui.Get.LoginPage.LoginButton));
            _forgotPassword = Content.FindElement(By.XPath(Ui.Get.LoginPage.ForgotPassword));
        }

        public MySummaryPage LoginAs(string email)
        {

            _username.SendKeys(email);
            _password.SendKeys(Get.GetPassword());
            _buttonLogin.Click();
            return new MySummaryPage(Client);
        }

        public ForgotPasswordPage ForgotPasswordClick()
        {
            _forgotPassword.Click();
            return new ForgotPasswordPage(Client);
        }
    }
}
