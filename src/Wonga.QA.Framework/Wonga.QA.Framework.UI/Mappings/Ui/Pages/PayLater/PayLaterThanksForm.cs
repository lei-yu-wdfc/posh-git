using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Mappings.Pages.PayLater
{
    public  class PayLaterThanksForm : BasePage
    {
        private readonly IWebElement _thanksText;

        public PayLaterThanksForm(UiClient client)
            : base(client)
        {
            _thanksText = Client.Driver.FindElement(By.CssSelector(Ui.Get.SubmitionPage.ThanksText));
        }

        public Dictionary<string, string> InspectElementsThanks()
        {
            var elements = new Dictionary<string, string>
                               {
                                   {"ThanksText", _thanksText.Text},
                               };

            return elements;
        }
    }
}
