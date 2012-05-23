using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepaymentOptionsPage : BasePage
    {
        private readonly IWebElement _easypayNumber;
        private readonly IWebElement _easypayPrintButton;

        
        public RepaymentOptionsPage(UiClient client) : base(client)
        {
            _easypayNumber = Content.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.EasypayNumber));
            _easypayPrintButton = Content.FindElement(By.CssSelector(UiMap.Get.RepaymentOptionsPage.EasypayPrintButton));
        }
    }
}
