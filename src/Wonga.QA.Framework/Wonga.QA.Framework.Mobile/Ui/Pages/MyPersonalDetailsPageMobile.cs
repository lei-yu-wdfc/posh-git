using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class MyPersonalDetailsPageMobile : BasePageMobile
    {

        private readonly IWebElement _addressLink;
        private readonly IWebElement _passwordLink;
        private readonly IWebElement _phoneLink;
        private readonly IWebElement _communicationLink;
        private IWebElement _popUp;

        private IWebElement _currentPassword;
        private IWebElement _editPassword;
        private IWebElement _editPasswordConfirm;
        private IWebElement _submit;

        private IWebElement _popUpTitle;
        
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
            _popUp = Do.Until(() => Client.Driver.FindElement(By.CssSelector("div#fancybox-content")));
            _popUpTitle = Do.Until(() => _popUp.FindElement(By.CssSelector(".modal h1")));
            Assert.IsTrue(_popUpTitle.Text.Equals("Change your password"));
            _currentPassword = _popUp.FindElement(By.CssSelector("#edit-current-password"));
            _editPassword = _popUp.FindElement(By.CssSelector("#edit-password"));
            _editPasswordConfirm = _popUp.FindElement(By.CssSelector("#edit-password-confirm"));
            _currentPassword.SendKeys(currentPassword);
            _editPassword.SendKeys(newPassword);
            _editPasswordConfirm.SendKeys(newPassword);
            _submit = _popUp.FindElement(By.CssSelector("#edit-submit"));
            _submit.Click();
            Do.Until(() => Client.Driver.FindElement(By.CssSelector(".modal h1")).Text.Equals("Success"));
            Client.Driver.FindElement(By.CssSelector("div#fancybox-wrap a#fancybox-close")).Click();
        }
    }
}
