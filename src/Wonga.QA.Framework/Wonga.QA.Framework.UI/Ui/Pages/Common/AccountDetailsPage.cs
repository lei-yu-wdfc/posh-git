using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class AccountDetailsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;
        private IWebElement _loanConditionsTitle;
        private IWebElement _explanationTitle;

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

        public String GetTermsAndConditionsTitle()
        {
            Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.TermAndConditionsLink)).Click();
            Thread.Sleep(4000);

            string currentWindowHdl = Client.Driver.CurrentWindowHandle;

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.PopupContentFrame)).GetAttribute("name");
            _loanConditionsTitle = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.Id("wonga.com-loan-conditions"));

            var title = _loanConditionsTitle.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return title;
        }

        public string GetExplanationTitle()
        {
            Content.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.ExplanationLink)).Click();
            Thread.Sleep(4000);

            string currentWindowHdl = Client.Driver.CurrentWindowHandle;

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.PopupContentFrame)).GetAttribute("name");
            _explanationTitle = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.Id("important-information-about-your-loan"));

            var title = _explanationTitle.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return title;
        }

        public String SecciPopupWindowContent()
        {
            string currentWindowHdl = Client.Driver.CurrentWindowHandle;

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.PopupContentFrame)).GetAttribute("name");
            var secci = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciContent));
            var secci_text = secci.Text;
            
            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return secci_text;
        }

        public void ClosePopupWindow()
        {
            Thread.Sleep(1000);
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.PopupClose)).Click();
        }

        public SecciToggleElement GetSecciToggleElement()
        {
            var SecciTogglelink = new SecciToggleElement(this);
            return SecciTogglelink;
        }

    }
}