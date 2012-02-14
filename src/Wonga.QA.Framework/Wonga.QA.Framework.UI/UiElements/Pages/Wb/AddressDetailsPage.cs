using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class AddressDetailsPage : BasePage,IApplyPage
    {
        private readonly IWebElement _postCode;
        private readonly IWebElement _form;
        private readonly IWebElement _lookup;
        private readonly IWebElement _next;
        private readonly IWebElement _flatNumber;
        private readonly IWebElement _district;
        private readonly IWebElement _county;
        private readonly IWebElement _addressPeriod;
        private IWebElement _addressOptions;

        public String PostCode { set { _postCode.SendValue(value); } }
        public String SelectedAddress { set { _addressOptions.SelectOption(value); } }
        public String FlatNumber { set { _flatNumber.SendValue(value); } }
        public String District { set { _district.SendValue(value); } }
        public String County { set { _county.SendValue(value); } }
        public String AddressPeriod { set { _addressPeriod.SelectOption(value); } }

        public AddressDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id(Elements.Get.WbAddressDetailsPage.FormId));

            _postCode = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.PostCode));
            _lookup = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.LookupButton));

            _flatNumber = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.FlatNumber));
            _district = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.District));
            _county = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.County));
            _addressPeriod = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.AddressPeriod));
            _next = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.NextButton));
        }

        public void LookupByPostCode()
        {
            _lookup.Click();
        }

        public void GetAddressesDropDown()
        {
            _addressOptions = _form.FindElement(By.Name(Elements.Get.WbAddressDetailsPage.AddressOptions));
        }

        public Wb.AccountDetailsPage Next()
        {
            _next.Click();
            return new Wb.AccountDetailsPage(Client);
        }

    }
}
