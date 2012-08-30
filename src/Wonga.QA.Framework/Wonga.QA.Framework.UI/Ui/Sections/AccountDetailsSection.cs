﻿using System;
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

        public String Password
        {
            set
            {
                _password.SendValue(value);
                _password.LostFocus();
            }
        }
        public String PasswordConfirm { set { _passwordConfirm.SendValue(value); } }
        public String SecretQuestion { set { _secretQuestion.SendValue(value); } }
        public String SecretAnswer { set { _secretAnswer.SendValue(value); } }

        public AccountDetailsSection(BasePage page)
            : base(UiMap.Get.AccountDetailsSection.Fieldset, page)
        {
            _password = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.Password));
            _passwordConfirm = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.PasswordConfirm));
            _secretQuestion = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.SecretQuestion));
            _secretAnswer = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.SecretAnswer));
        }
        public bool IsPasswordMismatchWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.PasswordConfirmErrorForm));
            return passwordWarning.Text.Equals(ContentMap.Get.AccountDetailsSection.PasswordMismatchWarning);
        }
        public bool IsPasswordInvalidFormatWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.PasswordErrorForm));
            return passwordWarning.Text.Equals(ContentMap.Get.AccountDetailsSection.PasswordInvalidFormatWarning);
        }
        public bool IsPasswordEqualsEmailWarningOccured()
        {
            var passwordWarning = Section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.PasswordMeterMessage));
            return passwordWarning.Text.Equals(ContentMap.Get.AccountDetailsSection.PasswordEqualsEmailWarning);
        }
    }
}
