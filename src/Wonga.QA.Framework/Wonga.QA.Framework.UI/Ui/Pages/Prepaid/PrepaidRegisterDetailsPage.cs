using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace  Wonga.QA.Framework.UI.UiElements.Pages
{
    public class PrepaidRegisterDetailsPage:BasePage
    {
        private readonly IWebElement _registerDetailsBox;

        public PrepaidRegisterDetailsPage(UiClient client) : base(client)
        {
            _registerDetailsBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidRegisterDetailsPage.RegisterDetailsBox));
        }
    }
}
