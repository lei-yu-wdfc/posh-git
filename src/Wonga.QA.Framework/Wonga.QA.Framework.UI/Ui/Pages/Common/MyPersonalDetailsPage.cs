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

        private readonly IWebElement _address;
        private readonly IWebElement _password;
        private readonly IWebElement _phone;
        private readonly IWebElement _communication;
        private ReadOnlyCollection<IWebElement> _communicationPrefs;

        public MyPersonalDetailsPage(UiClient client) : base(client)
        {
            Navigation = new MyAccountNavigationElement(this);
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

        public void PasswordClick()
        {
            _password.Click();
            Thread.Sleep(1000);
        }

        public void PhoneClick()
        {
            _phone.Click();
            Thread.Sleep(1000);
        }

        public void CommunicationClick()
        {
            _communication.Click();
            Thread.Sleep(1000);
        }

        public string SetCommunicationPrefs
        {
            set
            {
                _communicationPrefs = Content.FindElements(By.CssSelector(Ui.Get.MyPersonalDetailsPage.CommunicationPrefs));
                _communicationPrefs.SelectLabel(value);
            }
        }

    }
}
