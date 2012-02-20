using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class AccountDetailsSection : BaseSection
    {
        private readonly IWebElement _password;
        private readonly IWebElement _passwordConfirm;
        private readonly IWebElement _secretQuestion;
        private readonly IWebElement _secretAnswer;

        public String Password { set { _password.SendValue(value); } }
        public String PasswordConfirm { set { _passwordConfirm.SendValue(value); } }
        public String SecretQuestion { set { _secretQuestion.SendValue(value); } }
        public String SecretAnswer { set { _secretAnswer.SendValue(value); } }

        public AccountDetailsSection(BasePage page) : base(Elements.Get.AccountDetailsElement.Legend, page)
        {
            _password = Section.FindElement(By.CssSelector(Elements.Get.AccountDetailsElement.Password));
            _passwordConfirm = Section.FindElement(By.CssSelector(Elements.Get.AccountDetailsElement.PasswordConfirm));
            _secretQuestion = Section.FindElement(By.CssSelector(Elements.Get.AccountDetailsElement.SecretQuestion));
            _secretAnswer = Section.FindElement(By.CssSelector(Elements.Get.AccountDetailsElement.SecretAnswer));
        }
    }
}
