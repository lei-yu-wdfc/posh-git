using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Elements;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class MyPersonalDetailsPageMobile : BasePageMobile
    {

        private readonly IWebElement _addressLink;
        private readonly IWebElement _passwordLink;
        private readonly IWebElement _phoneLink;
        private readonly IWebElement _communicationLink;

        public IWebElement Address { get; set; }
        public IWebElement Phone { get; set; }
        public IWebElement Communication { get; set; }

        public TabsElementMobile TabsElementMobile { get; set; }
        
        public MyPersonalDetailsPageMobile(MobileUiClient client) : base(client)
        {
            Address = Content.FindElement(By.CssSelector("#address"));
            Phone = Content.FindElement(By.CssSelector("#phone"));
            Communication = Content.FindElement(By.CssSelector("#communication"));
            _addressLink = Address.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.AddressLink));
            _passwordLink = Content.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.PasswordLink));
            _phoneLink = Phone.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.PhoneLink));
            _communicationLink = Communication.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.CommunicationLink));
            TabsElementMobile = new TabsElementMobile(this);
        }

        public MyPersonalDetailsPageMobile EditPassword(string currentPassword, string newPassword)
        {
            _passwordLink.Click();
            var editPasswordPopUp = Do.Until(() => new EditPasswordPopUpElement(this));
            return editPasswordPopUp.EditPassword(currentPassword, newPassword);
        }

        public MyPersonalDetailsPageMobile EditAddress()
        {
            _addressLink.Click();
            var editAddressPopUp = Do.Until(() => new AddressPopUpElement(this));
            return editAddressPopUp.EditAddress();
        }

        public MyPersonalDetailsPageMobile EditHomeTelephoneNumber(string number)
        {
            _phoneLink.Click();
            var editPhonePopUp = Do.Until(() => new EditPhoneNumberPopUpElement(this));
            return editPhonePopUp.UpdateHomePhoneNumber(number);
        }

        public MyPersonalDetailsPageMobile EditMobileTelephoneNumber(string number)
        {
            _phoneLink.Click();
            var editPhonePopUp = Do.Until(() => new EditPhoneNumberPopUpElement(this));
            return editPhonePopUp.UpdateMobilePhoneNumber(number);
        }
    }
}
