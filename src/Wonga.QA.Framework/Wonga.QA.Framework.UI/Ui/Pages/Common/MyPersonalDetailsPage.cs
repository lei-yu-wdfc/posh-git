using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MyPersonalDetailsPage : BasePage
    {
        public MyAccountNavigationElement Navigation { get; set; }
        public LoginElement Login { get; set; }
        public TabsElement Tabs { get; set; }
        public string SetCommunicationPrefs
        {
            set
            {
                _communicationPrefs.SelectLabel(value);
            }
        }
        public String GetPopupErrorMessage
        {
            get { return Do.Until(()=>Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.PopupErrorMessage)).Text);  }
        }
        public string GetPostcode
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Postcode)).Text; }
        }
        public string GetHomePhone
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.HomePhone)).Text; }
        }
        public string GetMobilePhone
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.MobilePhone)).Text; }
        }
        public string GetCommunicationText
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.CommunicationText)).Text; }
        }

        private readonly IWebElement _address;
        private readonly IWebElement _password;
        private readonly IWebElement _phone;
        private readonly IWebElement _communication;
        private ReadOnlyCollection<IWebElement> _communicationPrefs;
        private IWebElement _editPhoneHome;
        private IWebElement _editPhoneMobile;
        private IWebElement _editPhonePin;
        private IWebElement _editPasswordCurrent;
        private IWebElement _editPasswordNew;
        private IWebElement _editPasswordConfirm;
        private IWebElement _editPasswordErrorMessage;
        private IWebElement _editPasswordPopupErrorMessage;
        private IWebElement _submitButton;

        public MyPersonalDetailsPage(UiClient client)
            : base(client)
        {
            Navigation = new MyAccountNavigationElement(this);
            Login = new LoginElement(this);
            switch (Config.AUT)
            {
                case AUT.Za:
                    _address = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Address));
                    _password = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Password));
                    _phone = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Phone));
                    _communication = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Communication));
                    break; 
                case AUT.Ca:
                    Tabs = new TabsElement(this);
                    break;
            }

        }

        public void AddressClick()
        {
            _address.Click();
            Thread.Sleep(1000); //Wait for a pop-up
        }

        public void ChangePassword(string currentPass, string newPass, string confirmPass)
        {
            _editPasswordCurrent =
               Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPasswordCurrent));
            _editPasswordNew = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPasswordNew));
            _editPasswordConfirm =
                Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPasswordConfirm));
            _submitButton = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.SubmitButton));

            _editPasswordCurrent.SendKeys(currentPass);
            _editPasswordNew.SendKeys(newPass);
            _editPasswordConfirm.SendKeys(confirmPass);
        }

        public bool ChangePhone(string homePhone, string mobilePhone, string pin)
        {
            _editPhoneHome = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPhoneHome));
            _editPhoneMobile = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPhoneMobile));
            _submitButton = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.SubmitButton));

            _editPhoneHome.Clear();
            _editPhoneHome.SendKeys(homePhone);
            _editPhoneMobile.Clear();
            _editPhoneMobile.SendKeys(mobilePhone);

            _submitButton.Click();
            Do.Until(LookForEditPinField);
            _editPhonePin.SendKeys(pin);
            return true;
        }
        public bool DontChangePhone()
        {
            Thread.Sleep(5000);
            _submitButton = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.SubmitButton));
            _submitButton.Click();
            try
            {
                Do.Until(() => Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPhoneErrorMessage)));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        private bool LookForEditPinField()
        {
            _editPhonePin = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPhonePin));
            return _editPhonePin.Displayed;
        }

        public void WaitForSuccessPopup()
        {
            Do.With.Timeout(10).Until(
                () =>
                Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.PopupSuccessTitle)).Displayed);
        }
        public bool Submit()
        {
            _submitButton = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.SubmitButton));
            _submitButton.Click();
            return true;
        }

        public bool CommunicationClick()
        {
            _communication.Click();
            Thread.Sleep(1000);
            _communicationPrefs = Client.Driver.FindElements(By.CssSelector(Ui.Get.MyPersonalDetailsPage.CommunicationPrefs));
            return true;
        }

        public void PasswordClick()
        {
            _password.Click();
        }

        public void PhoneClick()
        {
            _phone.Click();
        }

        public bool IsPasswordWarningMessageOccurs()
        {
            try
            {
                _editPasswordErrorMessage =
                    Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPasswordErrorMessage));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public bool IsPasswordPopupHasErrorMessage()
        {
            try
            {
                _editPasswordPopupErrorMessage =
                    Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPasswordPopupErrorMessage));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

        }



    }
}
