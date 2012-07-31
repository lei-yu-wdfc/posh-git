using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FALoginPage : BasePage
    {
        private readonly IWebElement _username;
        private readonly IWebElement _password;
        private readonly IWebElement _buttonLogin;
        private readonly IWebElement _forgotPassword;

        public FALoginPage(UiClient client)
            : base(client)
        {
            _username = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentLoginPage.Username));
            _password = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentLoginPage.Password));
            _buttonLogin = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentLoginPage.LoginButton));
            _forgotPassword = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentLoginPage.ForgotPassword));
        }

        public BasePage LoginAs(string email, string password)
        {
            _username.Clear();
            _password.Clear();
            _username.SendKeys(email);
            _password.SendKeys(password);
            _buttonLogin.Click();
            return new FinancialAssessmentPage(Client);
        }
    }
}
