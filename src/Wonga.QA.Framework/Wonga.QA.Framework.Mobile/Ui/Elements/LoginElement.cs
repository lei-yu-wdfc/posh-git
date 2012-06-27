using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
{
    //TODO finish this
    public class LoginElement : BaseElement
    {
        private IWebElement _loginTrigger;
        private IWebElement _form;
        private IWebElement _email;
        private IWebElement _password;
        private IWebElement _loginButton;

        public LoginElement(BasePageMobile page)
            : base(page)
        {

        }

        public MySummaryPageMobile LoginAs(string email, string password)
        {
            _loginTrigger = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.LoginElement.LoginTrigger));
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.LoginElement.FormId));
            _email = _form.FindElement(By.CssSelector(UiMapMobile.Get.LoginElement.Email));
            _password = _form.FindElement(By.CssSelector(UiMapMobile.Get.LoginElement.Password));
            _loginButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.LoginElement.LoginButton));
            _loginTrigger.Click();
            _email.SendKeys(email);
            _password.SendKeys(password);
            _loginButton.Click();
            return new MySummaryPageMobile(Page.Client);
        }

        public HomePageMobile Logout()
        {
            Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.LoginElement.LogoutTrigger)).Click();
            return new HomePageMobile(Page.Client);
        }

    }
}
