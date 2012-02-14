using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class PersonalBankAccountPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;

        public Sections.BankAccountSection BankAccountSection { get; set; }

        public PersonalBankAccountPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id(Elements.Get.WbPersonalBankAccountPage.FormId));
            _next = _form.FindElement(By.Name(Elements.Get.WbPersonalBankAccountPage.NextButton));
            BankAccountSection = new Sections.BankAccountSection(this);
        }

        public Wb.PersonalDebitCardPage Next()
        {
            _next.Click();
            return new Wb.PersonalDebitCardPage(Client);
        }
    }
}
