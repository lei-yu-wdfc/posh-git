using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class ApplyPageMobile : BasePageMobile
    {
        private readonly IWebElement _phoneNumber;
        private readonly IWebElement _bankAccount;
        private readonly IWebElement _applyNow;
        
        public ApplyPageMobile(MobileUiClient client) : base(client)
        {
            _phoneNumber = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.PhoneNumber));
            _bankAccount = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.BankAccount));
            _applyNow    = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.ApplyNowButton));
        }

        public ProcessingPageMobile ClickApplyNowButton()
        {
            _applyNow.Click();
            return new ProcessingPageMobile(Client);
        }
    }
}
