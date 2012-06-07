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
            _next.Submit();
            return new AccountDetailsPage(Client);
        }

        public bool IsSecciLinkVisible()
        {
            return Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.SecciLink)).Displayed;
        }

        public bool IsTermsAndConditionsLinkVisible()
        {
            return Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.TermAndConditionsLink)).Displayed;
        }

        public bool IsExplanationLinkVisible()
        {
            return Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.ExplanationLink)).Displayed;
        }

        public void ClickSecciLink()
        {
            Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.SecciLink)).Click();
        }

        public void ClickTermsAndConditionsLink()
        {
            Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.TermAndConditionsLink)).Click();
        }

        public void ClickExplanationLink()
        {
            Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.ExplanationLink)).Click();
        }

        public String SecciPopupWindowContent()
        {
            var frameName = Client.Driver.FindElement(By.CssSelector("#fancybox-frame")).GetAttribute("name");
            var secci = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciContent));
            return secci.Text;
        }

    }
}
