using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class LoginPageMobile : BasePageMobile
    {
        private readonly IWebElement _emailAddress;
        private readonly IWebElement _password;
        private readonly IWebElement _loginButton;
        private readonly IWebElement _forgotPassword;
        
        public LoginPageMobile(MobileUiClient client) : base(client)
        {
            _emailAddress = Content.FindElement(By.CssSelector(UiMapMobile.Get.LoginPageMobile.EmailAddress));
            _password = Content.FindElement(By.CssSelector(UiMapMobile.Get.LoginPageMobile.Password));
            _loginButton = Content.FindElement(By.CssSelector(UiMapMobile.Get.LoginPageMobile.LoginButton));
            _forgotPassword = Content.FindElement(By.CssSelector(UiMapMobile.Get.LoginPageMobile.ForgotPassword));
        }

        public MySummaryPageMobile LoginAs(string email, string password)
        {
            _emailAddress.SendKeys(email);
            _password.SendKeys(password);
            _loginButton.Click();
            return new MySummaryPageMobile(Client);
        }
    }
}
