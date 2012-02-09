using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class AddAditionalDirectorsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _done;
        private readonly IWebElement _addAnother;

        private readonly IWebElement _title;
        private readonly IWebElement _firstName;
        private readonly IWebElement _lastName;
        private readonly IWebElement _email;
        private readonly IWebElement _emailConfirm;

        public new String Title
        {
            set { _title.SelectOption(value); }
        }

        public String FirstName
        {
            set { _firstName.SendValue(value); }
        }

        public String LastName
        {
            set { _lastName.SendValue(value); }
        }

        public String EmailAddress
        {
            set { _email.SendValue(value); }
        }

        public String ConfirmEmailAddress
        {
            set { _emailConfirm.SendValue(value); }
        }

        public AddAditionalDirectorsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-directors-form"));
            _done = _form.FindElement(By.Name("done"));
            _addAnother = _form.FindElement(By.Name("add"));

            _title = _form.FindElement(By.Name("title"));
            _firstName = _form.FindElement(By.Name("first_name"));
            _lastName = _form.FindElement(By.Name("last_name"));
            _email = _form.FindElement(By.Name("email"));
            _emailConfirm = _form.FindElement(By.Name("email_confirm"));

        }

        public MainBusinessBankAccountPage Done()
        {
            _done.Click();
            return new MainBusinessBankAccountPage(Client);
        }

        public AddAditionalDirectorsPage AddAditionalDirector()
        {
            _addAnother.Click();
            return new AddAditionalDirectorsPage(Client);
        }
    }
}
