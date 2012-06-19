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
        private readonly IWebElement _balanceToday;
        private readonly IWebElement _easypayNumber;
        private readonly IWebElement _easypayPrintButton;
        private readonly IWebElement _howToUseEasyPayLink;
        private readonly IWebElement _debitOrderButton;
        private readonly IWebElement _manualRepaymentButton;
        
        public RepaymentOptionsPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(ContentMap.Get.RepaymentOptionsPage.HeaderText));
            _repaymentOptionsContainer = Content.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.RepaymentOptionsContainer));
            _balanceToday = Content.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.BalanceToday));
            _easypayNumber = _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.EasypayNumber));
            _easypayPrintButton = _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.EasypayPrintButton));
            _howToUseEasyPayLink =
                _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.HowToUseEasyPayLink));
            _debitOrderButton =
                _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.DebitOrderButton));
            _manualRepaymentButton = _repaymentOptionsContainer.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.ManualRepaymentButton));
        }

        public String BalanceToday
        {
            get { return _balanceToday.Text; }
        }

        public String EasypayNumber
        {
            get { return _easypayNumber.Text; }
        }

        public IWebDriver EasyPayPrintButtonClick()
        {
            _easypayPrintButton.Click();
            if(!Client.Driver.WindowHandles.Count.Equals(2))
                Thread.Sleep(2000);
                _easypayPrintButton.Click();
            Assert.IsTrue(Client.Driver.WindowHandles.Count.Equals(2));
            var printWindow = Client.Driver.SwitchTo().Window(Client.Driver.WindowHandles[1]);
            return printWindow;
        }

        public IWebElement HowToUseEasyPayLink
        {
            get { return _howToUseEasyPayLink; }
        }

        public DebitOrderPage DebitOrderButtonClick()
        {
            var button = Do.Until(()=>Client.Driver.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.DebitOrderButton)));
            button.Click();
            return new DebitOrderPage(Client);
        }

        
        public String ManualRepaymentButtonClick
        {
            get
            {
                _manualRepaymentButton.Click();
                return null;
            }
        }

    }
}
