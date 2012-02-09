using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class AddressDetailsPage : BasePage
    {
        private readonly IWebElement _postCode;
        private readonly IWebElement _form;
        private readonly IWebElement _lookup;
        private readonly IWebElement _next;
        private readonly IWebElement _flatNumber;
        private readonly IWebElement _disctrict;
        private readonly IWebElement _county;
        private readonly IWebElement _addressPeriod;
        private IWebElement _addressOptions;

        public String PostCode { set { _postCode.SendValue(value); } }
        public String SelectedAddress { set { _addressOptions.SelectOption(value); } }
        public String FlatNumber { set { _flatNumber.SendValue(value); } }
        public String District { set { _disctrict.SendValue(value); } }
        public String County { set { _county.SendValue(value); } }
        public String AddressPeriod { set { _addressPeriod.SelectOption(value); } }

        public AddressDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-address-form"));

            _postCode = _form.FindElement(By.Name("postcode_lookup_uk"));
            _lookup = _form.FindElement(By.Name("op"));

            _flatNumber = _form.FindElement(By.Name("flat"));
            _disctrict = _form.FindElement(By.Name("district"));
            _county = _form.FindElement(By.Name("county"));
            _addressPeriod = _form.FindElement(By.Name("address_period"));
            _next = _form.FindElement(By.Name("next"));
        }

        public void LookupByPostCode()
        {
            _lookup.Click();
        }

        public void GetAddressesDropDown()
        {
            _addressOptions = _form.FindElement(By.Name("address_options"));
        }

        public BusinessAccountDetailsPage Next()
        {
            _next.Click();
            return new BusinessAccountDetailsPage(Client);
        }

    }
}
