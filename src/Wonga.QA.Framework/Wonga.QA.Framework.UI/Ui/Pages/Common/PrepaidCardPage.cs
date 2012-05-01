using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class PrepaidCardPage : BasePage
    {
        private readonly IWebElement _getCardButton;
        public PrepaidCardPage(UiClient client) : base(client)
        {
            _getCardButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.GetCardButton));

        }
    }
}
