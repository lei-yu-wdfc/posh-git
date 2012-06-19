using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class DebitOrderPage : BasePage
    {

        public DebitOrderPage(UiClient client) : base(client)
        {
        }

        public DebitOrderSuccessPage Submit()
        {
            var button = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.DebitOrderPage.SubmitButton)));
            button.Click();
            return new DebitOrderSuccessPage(Client);
        }
    }
}
