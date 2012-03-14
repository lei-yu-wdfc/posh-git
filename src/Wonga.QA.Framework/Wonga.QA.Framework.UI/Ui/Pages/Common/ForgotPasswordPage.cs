using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ForgotPasswordPage : BasePage
    {
        private readonly IWebElement _username;
        private readonly IWebElement _captcha;
        private readonly IWebElement _next;
        public ForgotPasswordPage(UiClient client)
            : base(client)
        {
            _username = Content.FindElement(By.CssSelector(Ui.Get.ForgotPasswordPage.Username));
            _captcha = Content.FindElement(By.CssSelector(Ui.Get.ForgotPasswordPage.Captcha));
            _next = Content.FindElement(By.CssSelector(Ui.Get.ForgotPasswordPage.Next));
        }

        public HomePage EnterEmailAndCaptcha(string email, string captcha)
        {
            _username.SendKeys(email);
            _captcha.SendKeys(captcha);
            _next.Click();
            return new HomePage(Client);
        }
    }
}