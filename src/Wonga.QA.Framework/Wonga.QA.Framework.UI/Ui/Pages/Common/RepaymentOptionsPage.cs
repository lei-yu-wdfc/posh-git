using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepaymentOptionsPage : BasePage
    {
        private readonly IWebElement _repaymentOptionsContainer;
        private readonly IWebElement _easypayNumber;
        private readonly IWebElement _easypayPrintButton;
        private readonly IWebElement _manualRepaymentButton;
        
        public RepaymentOptionsPage(UiClient client) : base(client)
        {
            _repaymentOptionsContainer = Content.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.RepaymentOptionsContainer));
            _easypayNumber = _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.EasypayNumber));
            _easypayPrintButton = _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.EasypayPrintButton));
            _manualRepaymentButton =
                _repaymentOptionsContainer.FindElement(
                    By.CssSelector(UiMap.Get.RepaymentOptionsPage.ManualRepaymentButton));
        }

        public String EasypayNumber
        {
            get { return _easypayNumber.Text; }
        }

        public IWebDriver EasyPayPrintButtonClick()
        {
            _easypayPrintButton.Click();
            Do.Until(() => Client.Driver.WindowHandles.Count.Equals(2));
            var printWindow = Client.Driver.SwitchTo().Window(Client.Driver.WindowHandles[1]);
            return printWindow;
        }
    }
}
