using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
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

        public AccountDetailsSection(BasePage page) : base(Ui.Get.AccountDetailsSection.Fieldset, page)
        {
            _password = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.Password));
            _passwordConfirm = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.PasswordConfirm));
            _secretQuestion = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.SecretQuestion));
            _secretAnswer = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.SecretAnswer));
        }
        public bool IsPasswordMismatchWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.PasswordConfirmErrorForm));
            return passwordWarning.Text.Equals("Passwords must match");
        }
        public bool IsPasswordInvalidFormatWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.PasswordErrorForm));
            return passwordWarning.Text.Equals("Your password must be 8 or more characters and must include a capital letter and a number.");
        }
        public bool IsPasswordEqualsEmailWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(Ui.Get.AccountDetailsSection.PasswordErrorForm));
            return passwordWarning.Text.Equals("Password must not contain user name.");
       }
    }
}
