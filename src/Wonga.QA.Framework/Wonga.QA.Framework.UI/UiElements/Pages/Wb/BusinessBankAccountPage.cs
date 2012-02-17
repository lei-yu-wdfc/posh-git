using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class BusinessBankAccountPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;

        public Sections.BankAccountSection BankAccountSection { get; set; }

        public BusinessBankAccountPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.Id(Elements.Get.BankAccountPage.FormId));
            _next = _form.FindElement(By.Name(Elements.Get.BankAccountPage.NextButton));
            BankAccountSection = new Sections.BankAccountSection(this);
        }

        public Wb.BusinessDebitCardPage Next()
        {
            _next.Click();
            return new Wb.BusinessDebitCardPage(Client);
        }
    }
}
