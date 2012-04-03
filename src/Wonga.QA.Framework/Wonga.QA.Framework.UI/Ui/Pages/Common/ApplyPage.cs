using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.UiElements.Sections;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ApplyPage : BasePage, IApplyPage
    {
        private IWebElement _editMobileNumber;
        private IWebElement _popupCloseButton;
        public ApplicationSection ApplicationSection { get; set; }
        public ApplyPage(UiClient client)
            : base(client)
        {
            ApplicationSection = new ApplicationSection(this);
        }

        public BasePage Submit()
        {
            Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplyPage.Submit)).Click();
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                case AUT.Uk:
                    return new ProcessingPage(Client);
                default:
                    throw new NotImplementedException();
            }
        }

        public string SetNewMobilePhone
        {
            set
            { 
                ApplicationSection.ClickChangeMobileButton();
                Do.Until(IsEditMobileNumberDisplayed);
                _editMobileNumber.SendValue(value);
                Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplyPage.PopupSaveButton)).Click();
                Do.Until(IsPopupCloseButtonDisplayed);
                _popupCloseButton.Click();
                Do.While(IsPopupCloseButtonDisplayed);
            }
        }
        private bool IsEditMobileNumberDisplayed()
        {
            _editMobileNumber = Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplyPage.EditMobileNumber));
            return _editMobileNumber.Displayed;
        }
        private bool IsPopupCloseButtonDisplayed()
        {
            _popupCloseButton = Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplyPage.PopupCloseButton));
            return _popupCloseButton.Displayed;
        }
    }
}
