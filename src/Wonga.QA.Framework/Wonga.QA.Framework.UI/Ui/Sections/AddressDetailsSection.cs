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
    public class AddressDetailsSection : BaseSection
    {
        private readonly IWebElement _postCode;
        private readonly IWebElement _lookup;

        private IWebElement _flatNumber;
        private IWebElement _district;
        private IWebElement _county;
        private IWebElement _city;
        private IWebElement _street;
        private IWebElement _addressOptions;
        private IWebElement _postCodeErrorForm;
        private IWebElement _addressOptionsWrapper;
        private IWebElement _postcodeValid;

        public String PostCode { set { _postCode.SendValue(value); } }
        public String SelectedAddress { set { _addressOptions.SelectOption(value); } }
        public String FlatNumber { set { _flatNumber.SendValue(value); } }
        public String District { set { _district.SendValue(value); } }
        public String County { set { _county.SendValue(value); } }
        public String Town { set { _city.SendValue(value); } }
        public String Street { set { _street.SendValue(value); } }



        public AddressDetailsSection(BasePage page)
            : base(Ui.Get.AddressDetailsSection.Fieldset, page)
        {
            _postCode = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.Postcode));
            _lookup = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.LookupButton));
        }


        public void LookupByPostCode()
        {
            _addressOptionsWrapper = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.AddressOptionsWrapper));
            Do.With.Interval(1).Until(ClickLookupAddress);
            Do.Until(() => _addressOptionsWrapper.Displayed);
            _district = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.District));
            _county = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.County));
            _city = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.City));
            _street = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.Street));

            switch(Config.AUT)
            {
                case AUT.Wb:
                    _flatNumber = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.FlatNumber));
                    break;
            }
        }

        private IWebElement ClickLookupAddress()
        {
            _lookup.Click();
            _postcodeValid = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.PostcodeValid));
            return _postcodeValid.FindElement(By.CssSelector(".success"));
        }

        public void GetAddressesDropDown()
        {
            _addressOptions = Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.AddressOptions));
        }

        public bool IsPostcodeWarningOccurred()
        {
            try
            {
                _postCodeErrorForm =
                           Section.FindElement(By.CssSelector(Ui.Get.AddressDetailsSection.PostcodeErrorForm));
                string postCodeErrorFormClass = _postCodeErrorForm.GetAttribute("class");

                if (postCodeErrorFormClass.Equals("invalid"))
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return false;

        }
    }
}
