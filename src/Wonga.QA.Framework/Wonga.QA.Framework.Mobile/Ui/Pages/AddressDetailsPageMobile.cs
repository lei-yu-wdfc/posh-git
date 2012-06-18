using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class AddressDetailsPageMobile : BasePageMobile, IApplyPage
    {
       // public AccountDetailsSection AccountDetailsSection { get; set; }
        private readonly IWebElement _postCode;
        //private readonly IWebElement _postCodeInForm;
        //private readonly IWebElement _postCodeLookup;
        private readonly IWebElement _form;
        //private readonly IWebElement _lookup;
        private readonly IWebElement _next;
        private readonly IWebElement _district;
        private readonly IWebElement _county;
        private readonly IWebElement _town;
        private readonly IWebElement _street;
        private readonly IWebElement _addressPeriod;
        //private readonly IWebElement _postOfficeBox;

        private IWebElement _houseNumber;
        private IWebElement _addressOptions;
        private IWebElement _postCodeErrorForm;
        private IWebElement _houseNumberErrorForm;
        private IWebElement _streetErrorForm;
        private IWebElement _townErrorForm;
        private IWebElement _countyErrorForm;
        private IWebElement _addressPeriodErrorForm;
        //private IWebElement _addressOptionsWrapper;
        //private IWebElement _postcodeValid;

        //public String PostCodeLookup { set { _postCodeLookup.SendValue(value); } }
        public String PostCode { set { _postCode.SendValue(value); } }
        //public String PostcodeInForm { set { _postCodeInForm.SendValue(value); } }
        public String SelectedAddress { set { _addressOptions.SelectOption(value); } }
        public String HouseNumber
        {
            set
            {
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.HouseNumber)).SendValue(value);
            }
        }
        public String District { set { _district.SendValue(value); } }
        public String County { set { _county.SendValue(value); } }
        public String Town { set { _town.SendValue(value); } }
        public String Street { set { _street.SendValue(value); } }
        public String AddressPeriod
        {
            set
            {
                _addressPeriod.SelectOption(value);
            }
        }
        //public String PostOfficeBox { set { _postOfficeBox.SendValue(value); } }

        public AddressDetailsPageMobile(MobileUiClient client)
            : base(client)
        {

            _form = Content.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.FormId));
            _postCode = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Postcode));
            _houseNumber = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.HouseNumber));
            _addressPeriod = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.AddressPeriod));
            _next = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.NextButton));

            switch (Config.AUT)
            {
                //case (AUT.Wb):
                //    _county = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.County));
                //    _district = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.District));
                //    _lookup = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.LookupButton));
                //    _street = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Street));
                //    _town = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Town));
                //    _postCodeInForm = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.PostcodeInForm));
                //    break;
                case (AUT.Za):
                    _county = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.County));
                    _district = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.District));
                    _street = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Street));
                    _town = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Town));
                    break;
                //case (AUT.Ca):
                //    _street = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Street));
                //    _town = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Town));
                //    _postOfficeBox = _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.PostOfficeBox));
                //    AccountDetailsSection = new AccountDetailsSection(this);

                //    break;
                //case (AUT.Uk):
                //    _postCodeInForm = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.PostcodeInForm));
                //    _street = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Street));
                //    _town = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.Town));
                //    _postCodeLookup = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.PostcodeLookup));
                //    _lookup = _form.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.LookupButton));
                //    break;
            }
        }

        //public void LookupByPostCode()
        //{
        //    _addressOptionsWrapper = _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.AddressOptionsWrapper));
        //    Do.With.Interval(1).Until(ClickLookupAddress);
        //    Do.Until(() => _addressOptionsWrapper.Displayed);
        //}

        //private IWebElement ClickLookupAddress()
        //{
        //    _lookup.Click();
        //    _postcodeValid = _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.PostcodeValid));
        //    return _postcodeValid.FindElement(By.CssSelector(".success"));
        //}

        public void GetAddressesDropDown()
        {
            _addressOptions = _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.AddressOptions));
        }

        public BasePageMobile Next()
        {
            _next.Click();
            switch (Config.AUT)
            {
                //case (AUT.Ca):
                //    return new PersonalBankAccountPage(Client);
                default:
                    return new AccountDetailsPageMobile(Client);

            }
        }

        public AddressDetailsPageMobile NextClick()
        {
            _next.Click();
            return new AddressDetailsPageMobile(Client);
        }

        public BasePageMobile NextAddressLessThan2()
        {
            _next.Click();
            return new AddressDetailsPageMobile(Client);

        }
        public bool IsPostcodeWarningOccurred()
        {
            _next.Click();
            try
            {
                _postCodeErrorForm =
                           Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.PostcodeErrorForm)));
                string postCodeErrorFormClass = _postCodeErrorForm.GetAttribute("class");

                if (postCodeErrorFormClass.Contains("invalid"))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't find error form"+"\n\n");
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
        }
        public bool IsHouseNumberWarningOccurred()
        {
            _next.Click();
            try
            {
                _houseNumberErrorForm = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.HouseNumberErrorForm));
                string houseNumberErrorFormClass = _houseNumberErrorForm.GetAttribute("class");

                if (houseNumberErrorFormClass.Equals("invalid"))
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Can't find error form");
                return false;
            }
            return false;
        }
        public bool IsStreetWarningOccurred()
        {
            _next.Click();
            try
            {
                _streetErrorForm =
                           _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.StreetErrorForm));
                string streetErrorFormClass = _streetErrorForm.GetAttribute("class");

                if (streetErrorFormClass.Contains("invalid"))
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Can't find error form");
                return false;
            }
            return false;
        }
        public bool IsTownWarningOccurred()
        {
            _next.Click();
            try
            {
                _townErrorForm =
                           _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.TownErrorForm));
                string townErrorFormClass = _townErrorForm.GetAttribute("class");

                if (townErrorFormClass.Contains("invalid"))
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
        public bool IsCountyWarningOccurred()
        {
            _next.Click();
            try
            {
                _countyErrorForm =
                           _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.CountyErrorForm));
                string countyErrorFormClass = _countyErrorForm.GetAttribute("class");

                if (countyErrorFormClass.Contains("invalid"))
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
        public bool IsAddressPeriodWarningOccurred()
        {
            _next.Click();
            try
            {
                _addressPeriodErrorForm =
                           _form.FindElement(By.CssSelector(UiMapMobile.Get.AddressDetailsPage.AddressPeriodErrorForm));
                string addressPeriodErrorFormClass = _addressPeriodErrorForm.GetAttribute("class");

                if (addressPeriodErrorFormClass.Contains("invalid"))
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
