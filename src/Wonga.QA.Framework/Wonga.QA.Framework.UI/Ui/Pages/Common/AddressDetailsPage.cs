using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Sections;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class AddressDetailsPage : BasePage, IApplyPage
    {
        public AccountDetailsSection AccountDetailsSection { get; set; }
        private readonly IWebElement _postCode;
        private readonly IWebElement _postCodeLookup;
        private readonly IWebElement _form;
        private readonly IWebElement _lookup;
        private readonly IWebElement _next;
        private readonly IWebElement _flatNumber;
        private readonly IWebElement _district;
        private readonly IWebElement _county;
        private readonly IWebElement _town;
        private readonly IWebElement _street;
        private readonly IWebElement _addressPeriod;
        private readonly IWebElement _postOfficeBox;

        private IWebElement _addressOptions;
        private IWebElement _postCodeErrorForm;
        private IWebElement _addressOptionsWrapper;
        private IWebElement _postcodeValid;

        public String PostCodeLookup { set { _postCodeLookup.SendValue(value); } }
        public String PostCode { set { _postCode.SendValue(value); } }
        public String SelectedAddress { set { _addressOptions.SelectOption(value); } }
        public String FlatNumber { set { _flatNumber.SendValue(value); } }
        public String District { set { _district.SendValue(value); } }
        public String County { set { _county.SendValue(value); } }
        public String Town { set { _town.SendValue(value); } }
        public String Street { set { _street.SendValue(value); } }
        public String AddressPeriod { set { _addressPeriod.SelectOption(value); } }
        public String PostOfficeBox { set { _postOfficeBox.SendValue(value); } }

        public AddressDetailsPage(UiClient client)
            : base(client)
        {

            _form = Content.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.FormId));
            _postCode = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.Postcode));
            _flatNumber = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.FlatNumber));
            _addressPeriod = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.AddressPeriod));
            _next = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.NextButton));
            _addressOptionsWrapper = _form.FindElement(By.CssSelector(Ui.Get.AddressDetailsPage.AddressOptionsWrapper));
            _postcodeValid = _form.FindElement(By.CssSelector(Ui.Get.AddressDetailsPage.PostcodeValid));

            switch (Config.AUT)
            {
                case (AUT.Wb):
                    _county = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.County));
                    _district = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.District));
                    _lookup = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.LookupButton));
                    break;
                case (AUT.Za):
                    _county = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.County));
                    _district = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.District));
                    _street = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.Street));
                    _town = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.Town));
                    break;
                case (AUT.Ca):
                    _street = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.Street));
                    _town = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.Town));
                    _postOfficeBox = _form.FindElement(By.CssSelector(Ui.Get.AddressDetailsPage.PostOfficeBox));
                    AccountDetailsSection = new AccountDetailsSection(this);
                    break;
                case (AUT.Uk):
                    _postCodeLookup = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.PostcodeLookup));
                    _lookup = _form.FirstOrDefaultElement(By.CssSelector(Ui.Get.AddressDetailsPage.LookupButton));
                    break;
            }
        }

        public void LookupByPostCode()
        {
            Do.With.Interval(1).Until(ClickLookupAddress);
            Do.Until(() => _addressOptionsWrapper.Displayed);
        }

        private IWebElement ClickLookupAddress()
        {
            _lookup.Click();
            return _postcodeValid.FindElement(By.CssSelector(".success"));
        }

        public void GetAddressesDropDown()
        {
            _addressOptions = _form.FindElement(By.CssSelector(Ui.Get.AddressDetailsPage.AddressOptions));
        }

        public BasePage Next()
        {
            _next.Click();
            switch (Config.AUT)
            {
                case (AUT.Ca):
                    return new PersonalBankAccountPage(Client);
                default:
                    return new AccountDetailsPage(Client);

            }
        }
        public bool IsPostcodeWarningOccurred()
        {
            _postCodeErrorForm =
                       _form.FindElement(By.CssSelector(Ui.Get.AddressDetailsPage.PostcodeErrorForm));
            string postCodeErrorFormClass = _postCodeErrorForm.GetAttribute("class");

            if (postCodeErrorFormClass.Equals("invalid"))
            {
                return true;
            }

            return false;

        }

    }
}
