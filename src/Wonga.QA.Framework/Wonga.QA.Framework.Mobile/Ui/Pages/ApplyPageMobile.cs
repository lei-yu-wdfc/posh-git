using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Sections;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class ApplyPageMobile : BasePageMobile
    {
        private readonly IWebElement _phoneNumber;
        private readonly IWebElement _bankAccount;
        private readonly IWebElement _applyNow;
        private IWebElement _popupCloseButton;

        private IWebElement _editMobileNumber;
        
        public ApplicationSection ApplicationSection { get; set; }

        
        public ApplyPageMobile(MobileUiClient client) : base(client)
        {
            _phoneNumber = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.PhoneNumber));
            _bankAccount = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.BankAccount));
            _applyNow    = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.ApplyNowButton));

            ApplicationSection = new ApplicationSection(this);
        }

        public ProcessingPageMobile ClickApplyNowButton()
        {
            _applyNow.Click();
            return new ProcessingPageMobile(Client);
        }

        public string SetNewMobilePhone
        {
            set
            {
                ApplicationSection.ClickChangeMobileButton();
                Do.Until(IsEditMobileNumberDisplayed);
                _editMobileNumber.SendValue(value);
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.PopupSaveButton)).Click();
                Do.Until(IsPopupCloseButtonDisplayed);
                _popupCloseButton.Click();
                Do.While(IsPopupCloseButtonDisplayed);
            }
        }

        private bool IsEditMobileNumberDisplayed()
        {
            _editMobileNumber = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.EditMobileNumber));
            return _editMobileNumber.Displayed;
        }

        public bool IsMobilePhonePopupSaveButtonEnabled()
        {
            return Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.PopupSaveButton)).Enabled;
        }

        private bool IsPopupCloseButtonDisplayed()
        {
            _popupCloseButton = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.ApplyPageMobile.PopupCloseButton));
            return _popupCloseButton.Displayed;
        }
    }
}
