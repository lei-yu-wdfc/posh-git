using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class AccountDetailsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;

        public Sections.AccountDetailsSection AccountDetailsSection { get; set; }

        public AccountDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.FormId));
            _next = Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.NextButton));
            AccountDetailsSection = new Sections.AccountDetailsSection(this);
        }

        public PersonalBankAccountPage Next()
        {
            _next.Click();
            return new PersonalBankAccountPage(Client);
        }

        public AccountDetailsPage NextClick()
        {
            _next.Click();
            return new AccountDetailsPage(Client);
        }
    }
}
