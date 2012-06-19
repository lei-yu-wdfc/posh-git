using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
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

        public AccountDetailsSection(BasePageMobile page)
            : base(UiMapMobile.Get.AccountDetailsSection.Fieldset, page)
        {
            _password = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.Password));
            _passwordConfirm = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.PasswordConfirm));
            _secretQuestion = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.SecretQuestion));
            _secretAnswer = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.SecretAnswer));
        }
        public bool IsPasswordMismatchWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.PasswordConfirmErrorForm));
            return passwordWarning.Text.Equals(ContentMapMobile.Get.AccountDetailsSection.PasswordMismatchWarning);
        }
        public bool IsPasswordInvalidFormatWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.PasswordErrorForm));
            return passwordWarning.Text.Equals(ContentMapMobile.Get.AccountDetailsSection.PasswordInvalidFormatWarning);
        }
        public bool IsPasswordEqualsEmailWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsSection.PasswordErrorForm));
            return passwordWarning.Text.Equals(ContentMapMobile.Get.AccountDetailsSection.PasswordEqualsEmailWarning);
        }
    }
}
