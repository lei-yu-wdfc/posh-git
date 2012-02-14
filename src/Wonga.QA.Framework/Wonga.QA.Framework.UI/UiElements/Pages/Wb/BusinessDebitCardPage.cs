using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class BusinessDebitCardPage: BasePage
    {
        private readonly IWebElement _next;
        private readonly IWebElement _form;

        public Sections.DebitCardSection DebitCardSection { get; set; }

        public BusinessDebitCardPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.Id(Elements.Get.WbBusinessDebitCardPage.FormId));
            _next = _form.FindElement(By.Name(Elements.Get.WbBusinessDebitCardPage.NextButton));
            DebitCardSection = new Sections.DebitCardSection(this);
        }

        public Common.ProcessingPage Next()
        {
            _next.Click();
            return new Common.ProcessingPage(Client);
        }
    }
}
