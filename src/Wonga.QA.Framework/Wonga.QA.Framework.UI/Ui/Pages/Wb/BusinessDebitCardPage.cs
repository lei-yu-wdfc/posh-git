using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Sections;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.Mappings;


namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class BusinessDebitCardPage : BasePage
    {
        private readonly IWebElement _next;
        private readonly IWebElement _form;

        public DebitCardSection DebitCardSection { get; set; }
        public AddressDetailsSection AddressDetailsSection { get; set; }

        public BusinessDebitCardPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Ui.Get.BusinessDebitCardPage.FormId));
            _next = _form.FindElement(By.CssSelector(Ui.Get.BusinessDebitCardPage.NextButton));

            DebitCardSection = new DebitCardSection(this);
            AddressDetailsSection = new AddressDetailsSection(this);
        }

        public ProcessingPage Next()
        {
            _next.Click();
            return new ProcessingPage(Client);
        }

    }
}
