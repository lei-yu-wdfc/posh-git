using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class DebitOrderSuccessPage : BasePage
    {
        public DebitOrderSuccessPage(UiClient client) : base(client)
        {
        }

        public MySummaryPage BackToYourAccountButtonClick()
        {
            var button = Do.Until(()=>Client.Driver.FindElement(By.CssSelector(UiMap.Get.DebitOrderSuccessPage.BackToYourAccountButton)));
            button.Click();
            return new MySummaryPage(Client);
        }
    }
}
