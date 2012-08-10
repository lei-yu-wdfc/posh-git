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
        private IWebElement _secureCodeError;
        private IWebElement _minCashError;
        public ApplicationSection ApplicationSection { get; set; }
        public ApplyPage(UiClient client)
            : base(client)
        {
            ApplicationSection = new ApplicationSection(this);
        }

        public BasePage Submit()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.Submit)).Click();
            switch (Config.AUT)
            {
                case AUT.Wb:
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
                Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.PopupSaveButton)).Click();
                Do.Until(IsPopupCloseButtonDisplayed);
                _popupCloseButton.Click();
                Do.While(IsPopupCloseButtonDisplayed);
            }
        }

        public string SetIncorrectMobilePhone
        {
            set
            {
                ApplicationSection.ClickChangeMobileButton();
                Do.Until(IsEditMobileNumberDisplayed);
                _editMobileNumber.SendValue(value);
                Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.PopupSaveButton)).Click();
                Do.Until(IsPhoneNumberNotChangedMessageVisible);
            }
        }

        public string SetSecureCode
        {
            set
            {
                _secureCodeError = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.SecurityCode));
                _secureCodeError.SendValue(value);
            }
        }

        public bool IsPhoneNumberNotChangedMessageVisible()
        {
            return Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.PopupErrorMessage)).Displayed;
        }

        public bool IsMobilePhonePopupCancelButtonEnabled()
        {
            return Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.PopupCancelButton)).Enabled;
        }

        public bool IsMobilePhonePopupSaveButtonEnabled()
        {
            return Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.PopupSaveButton)).Enabled;
        }
        public string GetCurrentBankAccount
        {
            get
            {
                var currentBankAccount = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.BankAccount));
                return currentBankAccount.GetValue().Remove(0, 3);
            }
        }
        private bool IsEditMobileNumberDisplayed()
        {
            _editMobileNumber = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.EditMobileNumber));
            return _editMobileNumber.Displayed;
        }
        private bool IsPopupCloseButtonDisplayed()
        {
            _popupCloseButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.PopupCloseButton));
            return _popupCloseButton.Displayed;
        }

        public void ResendPinClick()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.ResendPin)).Click();
        }

        public String GetResendPinPopupText()
        {
            return Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.ResendPinPopupText)).Text);
        }

        public void CloseResendPinPopup()
        {
            IWebElement close = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.ResendPinPopupClose)));
            close.Click();
        }

        public bool IsSecurecodeWarningOccurred()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.Submit)).Click();
            try
            {
                _secureCodeError =
                            Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.SecurcodeError)));
                string secureCodeErrorClass = _secureCodeError.GetAttribute("class");

                if (!secureCodeErrorClass.Contains("success"))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't find error form" + "\n\n");
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
        }

        public bool IsMinCashWarningOccurred()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplyPage.Submit)).Click();
            try
            {
                _minCashError =
                            Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.MinCashError)));
                string minCashErrorclass = _minCashError.GetAttribute("class");

                if (!minCashErrorclass.Contains("success"))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't find error form" + "\n\n");
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
        }
    }
}
