using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Elements
{
    //TODO finish this
    public class LoginElement : BaseElement
    {
        private IWebElement _loginTrigger;
        private IWebElement _form;
        private IWebElement _email;
        private IWebElement _password;
        private IWebElement _loginButton;

        public LoginElement(BasePage page)
            : base(page)
        {

        }

        public MySummaryPage LoginAs(string email, string password)
        {
            _loginTrigger = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.LoginElement.LoginTrigger));
            _form = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.LoginElement.FormId));
            _email = _form.FindElement(By.CssSelector(Ui.Get.LoginElement.Email));
            _password = _form.FindElement(By.CssSelector(Ui.Get.LoginElement.Password));
            _loginButton = _form.FindElement(By.CssSelector(Ui.Get.LoginElement.LoginButton));
            //Mouse to _loginTrigger to make email and pass fields visible
            _email.SendKeys(email);
            _password.SendKeys(password);
            _loginButton.Click();
            return new MySummaryPage(Page.Client);
        }

        public HomePage Logout()
        {
            Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.LoginElement.LogoutTrigger)).Click();
            return new HomePage(Page.Client);
        }

    }
}
