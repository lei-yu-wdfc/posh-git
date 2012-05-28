using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MySummaryPageMobile : BasePage
    {
        private readonly IWebElement _myPaymentDetailsButton;
        private readonly IWebElement _mySummaryButton;
        private readonly IWebElement _myPersonalDetailsButton;

        public MySummaryPageMobile(UiClient client) : base(client)
        {
            _mySummaryButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPageMobile.MySummaryButton));
            _myPersonalDetailsButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPageMobile.MyPersonalDetailsButton));
            _myPaymentDetailsButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPageMobile.MyPaymentDetailsButton));
        }
    }
}
