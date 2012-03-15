using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Framework.UI.UiElements.Sections;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class PersonalDetailsPage : BasePage, IApplyPage
    {
        public YourNameSection YourName { get; set; }
        public EmploymentDetailsSection EmploymentDetails { get; set; }
        public YourDetailsSection YourDetails { get; set; }
        public ContactingYouSection ContactingYou { get; set; }
        public ProvinceSection ProvinceSection { get; set; }
        public Boolean PrivacyPolicy { set { _privacy.Toggle(value); } }
        public SlidersElement Sliders { get; set; }
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
        public string MarriedInCommunityProperty
        {
            set
            {
                _marriedInCommunityProperty.SelectLabel(value);
            }
        }
        private readonly IWebElement _form;
        private readonly ReadOnlyCollection<IWebElement> _marriedInCommunityProperty;
        private readonly IWebElement _privacy;
        private readonly ReadOnlyCollection<IWebElement> _contact;
        private readonly IWebElement _next;

        public PersonalDetailsPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.FormId));
            _privacy = _form.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.CheckPrivacyPolicy));
            _contact = _form.FindElements(By.CssSelector(Ui.Get.PersonalDetailsPage.CheckCanContact));
            _next = _form.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.NextButton));

            YourName = new YourNameSection(this);
            YourDetails = new YourDetailsSection(this);
            ContactingYou = new ContactingYouSection(this);

            switch(Config.AUT)
            {
                case(AUT.Ca):
                    EmploymentDetails = new EmploymentDetailsSection(this);
                    ProvinceSection = new ProvinceSection(this);
                    break;
                case (AUT.Uk):
                    EmploymentDetails = new EmploymentDetailsSection(this);
                    break;
                case (AUT.Za):
                    EmploymentDetails = new EmploymentDetailsSection(this);
                    _marriedInCommunityProperty =
                        _form.FindElements(By.CssSelector(Ui.Get.PersonalDetailsPage.MarriedInCommunityProperty));
                    break;
            }
        }

        public void ClickSliderToggler()
        {
            var sliderToggler = Client.Driver.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.SliderToggler));
            sliderToggler.Click();
            Sliders = new SlidersElement(this);
        }

        public IApplyPage Submit()
        {
            _next.Click();
            return new AddressDetailsPage(Client);
        }
    }
}
