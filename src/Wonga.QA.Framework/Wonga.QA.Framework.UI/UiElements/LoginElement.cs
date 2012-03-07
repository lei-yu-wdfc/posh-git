using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.UiElements
{
    //TODO finish this
    public class LoginElement : BaseElement
    {
        private readonly IWebElement _loginTrigger;
        private readonly IWebElement _form;
        private readonly IWebElement _email;
        private readonly IWebElement _password;
        private readonly IWebElement _loginButton;

        public LoginElement(BasePage page)
            : base(page)
        {
            _loginTrigger = Page.Client.Driver.FindElement(By.CssSelector(Elements.Get.LoginSection.LoginTrigger));
            _form = Page.Client.Driver.FindElement(By.CssSelector(Elements.Get.LoginSection.FormId));
            _email = _form.FindElement(By.CssSelector(Elements.Get.LoginSection.Email));
            _password = _form.FindElement(By.CssSelector(Elements.Get.LoginSection.Password));
            _loginButton = _form.FindElement(By.CssSelector(Elements.Get.LoginSection.LoginButton));
        }

        public MySummary LoginAs(string email, string password)
        {
            //Mouse to _loginTrigger to make email and pass fields visible
            _email.SendKeys(email);
            _password.SendKeys(password);
            _loginButton.Click();
            return new MySummary(Page.Client);
        }
    }
}
