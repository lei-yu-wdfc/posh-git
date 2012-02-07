using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class BusinessAccountDetailsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _password;
        private readonly IWebElement _passwordConfirm;
        private readonly IWebElement _secretQuestion;
        private readonly IWebElement _secretAnswer;
        private readonly IWebElement _next;

        public String Password { set { _password.SendValue(value); } }
        public String PasswordConfirm { set { _passwordConfirm.SendValue(value); } }
        public String SecretQuestion { set { _secretQuestion.SendValue(value); } }
        public String SecretAnswer { set { _secretAnswer.SendValue(value); } }

        public BusinessAccountDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-account-setup-form"));

            _password = _form.FindElement(By.Name("password"));
            _passwordConfirm = _form.FindElement(By.Name("password_confirm"));
            _secretQuestion = _form.FindElement(By.Name("secret_question"));
            _secretAnswer = _form.FindElement(By.Name("secret_answer"));
            _next = _form.FindElement(By.Name("next"));
        }

        public BankAccountDetailsPage Next()
        {
            _next.Click();
            return new BankAccountDetailsPage(Client);
        }
    }
}
