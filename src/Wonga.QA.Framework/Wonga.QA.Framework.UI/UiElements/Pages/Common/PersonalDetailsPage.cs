using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Sections;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class PersonalDetailsPage : BasePage
    {
        public YourNameSection YourName { get; set; }
        public YourDetailsSection YourDetails { get; set; }
        public ContactingYouSection ContactingYou { get; set; }
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
            _form = Content.FindElement(By.Id(Elements.Get.PersonalDetailsPage.FormId));

            _privacy = _form.FindElement(By.Name(Elements.Get.PersonalDetailsPage.CheckPrivacyPolicy));
            _contact = _form.FindElements(By.Name(Elements.Get.PersonalDetailsPage.CheckCanContact));
            _next = _form.FindElement(By.Name(Elements.Get.PersonalDetailsPage.NextButton));

            YourName = new YourNameSection(this);
            YourDetails = new YourDetailsSection(this);
            ContactingYou = new ContactingYouSection(this);
        }

        public IApplyPage Submit()
        {
            _next.Click();
            switch (Config.AUT)
            {
                case (AUT.Wb):
                    return new Wb.AddressDetailsPage(Client);
                    
            }
            throw new NotImplementedException();
        }
    }
}
