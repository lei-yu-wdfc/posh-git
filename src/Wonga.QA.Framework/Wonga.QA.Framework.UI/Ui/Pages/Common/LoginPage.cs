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

            _username = Content.FindElement(By.CssSelector(UiMap.Get.LoginPage.Username));
            _password = Content.FindElement(By.CssSelector(UiMap.Get.LoginPage.Password));
            _buttonLogin = Content.FindElement(By.CssSelector(UiMap.Get.LoginPage.LoginButton));
            _forgotPassword = Content.FindElement(By.CssSelector(UiMap.Get.LoginPage.ForgotPassword));
        }

        public MySummaryPage LoginAs(string email)
        {
            _username.SendKeys(email);
            _password.SendKeys(Get.GetPassword());
            _buttonLogin.Click();
            return new MySummaryPage(Client);
        }

        public MySummaryPageMobile LoginAsMobile(string email)
        {

            _username.SendKeys(email);
            _password.SendKeys(Get.GetPassword());
            _buttonLogin.Click();
            return new MySummaryPageMobile(Client);
        }

        public void LoginRedirectAs(string email)
        {
            _username.SendKeys(email);
            _password.SendKeys(Get.GetPassword());
            _buttonLogin.Click();
        }
        
        public PrepaidAdminPage LoginAs()
        {
            _username.SendKeys(Config.PrepaidAdminUI.User);
            _password.SendKeys(Config.PrepaidAdminUI.Pwd);
            _buttonLogin.Click();

            Client.Driver.Navigate().GoToUrl(Config.PrepaidAdminUI.Home);
            return new PrepaidAdminPage(Client);
        }
        
        public ForgotPasswordPage ForgotPasswordClick()
        {
            _forgotPassword.Click();
            return new ForgotPasswordPage(Client);
        }
    }
}
