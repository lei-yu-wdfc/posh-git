using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MyPaymentsPage : BasePage
    {
        public MyAccountNavigationElement Navigation { get; set; }

        private IWebElement _addBankAccountButton;
        private IWebElement _popupBankName;
        private IWebElement _popupBankAccountType;
        private IWebElement _popupAccountNumber;
        private IWebElement _popupLengthOfTime;
        private IWebElement _popupAddBankAccountButton;
        private IWebElement _popupExeption;
        private IWebElement _popupBankAccountException;


        public MyPaymentsPage(UiClient client)
            : base(client)
        {

            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:  //TODO find out what xpath for button on Ca
                    Navigation = new MyAccountNavigationElement(this);
                    break;
            }
        }
        public bool IsAddBankAccountButtonExists()
        {
            try
            {
                switch (Config.AUT)
                {
                    case AUT.Za:
                    case AUT.Ca:  //TODO find out what xpath for button on Ca
                        _addBankAccountButton =
                            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.AddBankAccountButton));
                        break;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public void AddBankAccountButtonClick()
        {
            _addBankAccountButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.AddBankAccountButton));
            _addBankAccountButton.Click();
        }

        public void AddBankAccount(string bankName, string bankType, string accountNumber, string lenghtOfTime)
        {
            _popupBankName = Do.With.Timeout(3).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupBankName)));
            _popupBankAccountType = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupBankAccountType)));
            _popupAccountNumber = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupAccountNumber)));
            _popupLengthOfTime = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupLengthOfTime)));
            _popupAddBankAccountButton = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupAddBankAccountButton)));

            _popupBankName.SelectOption(bankName);
            _popupBankAccountType.SelectOption(bankType);
            _popupAccountNumber.SendKeys(accountNumber);
            _popupLengthOfTime.SelectOption(lenghtOfTime);
            _popupAddBankAccountButton.Click();
            
        }

        public void ClickCloseButton()
        {
            WaitForSuccessPopup();
            _popupAddBankAccountButton = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupAddBankAccountButton)));
            _popupAddBankAccountButton.Click();
            Do.While(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupSuccessTitle)).Displayed);
        }

        private void WaitForSuccessPopup()
        {
            Do.With.Timeout(10).Until(
                () =>
                Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupSuccessTitle)).Displayed);
        }

        public bool IfHasAnExeption()
        {
            try
            {
                _popupExeption = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupExeption));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string DefaultAccountNumber
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.AccountNumber)).Text.Remove(0, 3); }
        }

        public bool IsChengedBankAccountHasException()
        {
            try
            {
                _popupBankAccountException =
                    Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyPaymentsPage.PopupBankAccountException));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}