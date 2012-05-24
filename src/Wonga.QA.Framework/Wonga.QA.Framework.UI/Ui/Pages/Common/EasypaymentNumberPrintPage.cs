using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class EasypaymentNumberPrintPage: BasePage
    {
        private readonly IWebElement _yourEasyPayNumber ;
        
        public EasypaymentNumberPrintPage(UiClient client) : base(client)
        {
            _yourEasyPayNumber = Client.Driver.FindElement(By.CssSelector(UiMap.Get.EasypaymentNumberPrintPage.YourEasyPayNumber));
        }

        public String YourEasyPayNumberValue
        {
            get { return _yourEasyPayNumber.Text; }
        }
    }
}
