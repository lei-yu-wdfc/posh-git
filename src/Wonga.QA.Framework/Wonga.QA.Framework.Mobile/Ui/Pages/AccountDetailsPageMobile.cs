using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class AccountDetailsPageMobile : BasePageMobile
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;
        private IWebElement _loanConditionsTitle;
        private IWebElement _explanationTitle;

        public Sections.AccountDetailsSection AccountDetailsSection { get; set; }

        public AccountDetailsPageMobile(MobileUiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.FormId));
            _next = Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.NextButton));
            AccountDetailsSection = new Sections.AccountDetailsSection(this);
        }

        public PersonalBankAccountPageMobile Next()
        {
            _next.Click();
            return new PersonalBankAccountPageMobile(Client);
        }

        public AccountDetailsPageMobile NextClick()
        {
            _next.Submit();
            return new AccountDetailsPageMobile(Client);
        }

        public bool IsSecciLinkVisible()
        {
            return Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.SecciLink)).Displayed;
        }

        public bool IsTermsAndConditionsLinkVisible()
        {
            return Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.TermAndConditionsLink)).Displayed;
        }

        public bool IsExplanationLinkVisible()
        {
            return Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.ExplanationLink)).Displayed;
        }

        public void ClickSecciLink()
        {
            Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.SecciLink)).Click();
        }

        public String GetTermsAndConditionsTitle()
        {
            Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.TermAndConditionsLink)).Click();
            Thread.Sleep(4000);

            string currentWindowHdl = Client.Driver.CurrentWindowHandle;

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.PopupContentFrame)).GetAttribute("name");
            _loanConditionsTitle = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.Id("wonga.com-loan-conditions"));

            var title = _loanConditionsTitle.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return title;
        }

        public string GetExplanationTitle()
        {
            Content.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.ExplanationLink)).Click();
            Thread.Sleep(4000);

            string currentWindowHdl = Client.Driver.CurrentWindowHandle;

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.PopupContentFrame)).GetAttribute("name");
            _explanationTitle = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.Id("important-information-about-your-loan"));

            var title = _explanationTitle.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return title;
        }

        public String SecciPopupWindowContent()
        {
            string currentWindowHdl = Client.Driver.CurrentWindowHandle;

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.PopupContentFrame)).GetAttribute("name");
            var secci = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.CssSelector(UiMapMobile.Get.ExtensionAgreementPageMobile.SecciContent));
            var secci_text = secci.Text;
            
            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return secci_text;
        }

        public void ClosePopupWindow()
        {
            Thread.Sleep(1000);
            Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.AccountDetailsPageMobile.PopupClose)).Click();
        }
    }
}
