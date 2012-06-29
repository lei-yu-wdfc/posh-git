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
        
        public MyPersonalDetailsPageMobile(MobileUiClient client) : base(client)
        {
            _addressLink = Content.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.AddressLink));
            _passwordLink = Content.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.PasswordLink));
            _phoneLink = Content.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.PhoneLink));
            _communicationLink = Content.FindElement(By.CssSelector(UiMapMobile.Get.MyPersonalDetailsPageMobile.CommunicationLink));
        }

        public void EditPassword(string currentPassword, string newPassword)
        {
            _passwordLink.Click();
            var editPasswordPopUp = Do.Until(() => new EditPasswordPopUpElement(this));
            editPasswordPopUp.EditPassword(currentPassword, newPassword);
        }
    }
}
