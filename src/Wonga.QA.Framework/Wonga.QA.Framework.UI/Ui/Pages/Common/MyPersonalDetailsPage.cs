using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MyPersonalDetailsPage : BasePage
    {
        public MyAccountNavigationElement Navigation { get; set; }
        public LoginElement Login { get; set; }
        public string SetCommunicationPrefs
        {
            set
            {
                _communicationPrefs.SelectLabel(value);
            }
        }
        private readonly IWebElement _address;
        private readonly IWebElement _password;
        private readonly IWebElement _phone;
        private readonly IWebElement _communication;
        private ReadOnlyCollection<IWebElement> _communicationPrefs;
        private IWebElement _editPhoneHome;
        private IWebElement _editPhoneMobile;
        private IWebElement _editPasswordCurrent;
        private IWebElement _editPasswordNew;
        private IWebElement _editPasswordConfirm;
        private IWebElement _editPasswordErrorMessage;
        private IWebElement _editPasswordPopupErrorMessage;
        private IWebElement _submitButton;

        public MyPersonalDetailsPage(UiClient client) : base(client)
        {
            Navigation = new MyAccountNavigationElement(this);
            Login = new LoginElement(this);

            _address = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Address));
            _password = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Password));
            _phone = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Phone));
            _communication = Content.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.Communication));
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

        public void ChangePhone(string homePhone, string mobilePhone)
        {
            _editPhoneHome = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPhoneHome));
            _editPhoneMobile = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.EditPhoneMobile));
            _submitButton = Client.Driver.FindElement(By.CssSelector(Ui.Get.MyPersonalDetailsPage.SubmitButton));

            _editPhoneHome.SendKeys(homePhone);
            _editPhoneMobile.SendKeys(mobilePhone);
        }
        
        public void Submit()
        {
            _submitButton.Click();
        }

        public void CommunicationClick()
        {
            _communication.Click();
            Thread.Sleep(1000);
            _communicationPrefs = Client.Driver.FindElements(By.CssSelector(Ui.Get.MyPersonalDetailsPage.CommunicationPrefs));
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
            catch(NoSuchElementException)
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
            catch(NoSuchElementException)
            {
                return false;
            }
            
        }

       

    }
}
