using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Ui.Pages.Common
{
    public class BusinessSummaryPage : BasePage
    {
        private readonly IWebElement _businessSummary;
        private readonly IWebElement _accountDetails;
        private readonly IWebElement _yourDetails;
        public SlidersElement Sliders { get; set; }

        
        public BusinessSummaryPage(UiClient client) : base(client)
        {
            _businessSummary = Content.FindElement(By.CssSelector(UiMap.Get.BusinessSummaryPage.BusinessSummary));
            _accountDetails = Content.FindElement(By.CssSelector(UiMap.Get.BusinessSummaryPage.AccountDetails));
            _yourDetails = Content.FindElement(By.CssSelector(UiMap.Get.BusinessSummaryPage.YourDetails));
            Sliders = new SlidersElement(this);
        }
    }
}
