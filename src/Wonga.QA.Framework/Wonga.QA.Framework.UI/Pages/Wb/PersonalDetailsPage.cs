using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class PersonalDetailsPage : BasePage
    {
        public Wonga.QA.Framework.UI.Elements.Sections.YourNameSection YourName { get; set; }
        public Wonga.QA.Framework.UI.Elements.Sections.YourDetailsSection YourDetails { get; set; }
        public Wonga.QA.Framework.UI.Elements.Sections.ContactingYouSection ContactingYou { get; set; }
        public Boolean PrivacyPolicy { set { _privacy.Toggle(value); } }
        public Object CanContact
        {
            set
            {
                if (value is bool)
                    _contact.Single().Toggle((Boolean)value);
                else
                    _contact.SelectLabel((String)value);
            }
        }

        private readonly IWebElement _form;
        private readonly IWebElement _privacy;
        private readonly ReadOnlyCollection<IWebElement> _contact;
        private readonly IWebElement _next;

        public PersonalDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-personal-form"));

            _privacy = _form.FindElement(By.Name("privacy"));
            _contact = _form.FindElements(By.Name("update_option"));
            _next = _form.FindElement(By.Name("next"));

            YourName = new Wonga.QA.Framework.UI.Elements.Sections.YourNameSection(this);
            YourDetails = new Wonga.QA.Framework.UI.Elements.Sections.YourDetailsSection(this);
            ContactingYou = new Wonga.QA.Framework.UI.Elements.Sections.ContactingYouSection(this);
        }

        public AddressDetailsPage Submit()
        {
            _next.Click();
            return new AddressDetailsPage(Client);
        }
    }
}
