using System;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Ui.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ExtensionAgreementPage : BasePage, IExtensionPaymentPage
    {
        private IWebElement _nextButton;
        private IWebElement _agreementLegals;
        private IWebElement _secciButton;
        private IWebElement _aeDocumentButton;
        private IWebElement _termsAndConditionsButton;

        public IWebElement secciHeader;
        public IWebElement secciPrint;
        public IWebElement secci;

        public IWebElement _SecciTogglelink;
        
        public ExtensionAgreementPage(UiClient client)
            : base(client)
        {
            _nextButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionAgreementAcceptButton));
            _agreementLegals = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionAgreementscroll));
            _secciButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionSecciButton));
            _termsAndConditionsButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionTermsAndConditionsButton));
            _aeDocumentButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionExplanationButton));
        }

        public bool IsExtensionAgreementPageLegalInfoDisplayed()
        {
            bool legalResult = _agreementLegals.Displayed;
            return legalResult;

        }

        public ExtensionDealDonePage Accept()
        {
            _nextButton.Click();
            return new ExtensionDealDonePage(Client);
        }

        //public ExtensionSecciDocumentSection ClickExtensionSecciLink()
        public void ClickExtensionSecciLink()
        {
            _secciButton.Click();
            Do.Until(InitialiseSecci);
            //return new ExtensionSecciDocumentSection(this);
        }

        public void ClickExplanationLink()
        {
            _aeDocumentButton.Click();
        }

        public void ClickTermsAndConditionsLink()
        {
            _termsAndConditionsButton.Click();
        }

        public Boolean InitialiseSecci()
        {
            try
            {
                secciHeader = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciHeader));
                secciPrint = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciPrint));
                
                Assert.IsTrue(secciPrint.Text.Contains(ContentMap.Get.ExtensionAgreementPage.PrintThisPage));
                Assert.IsTrue(secciHeader.Text.Contains(ContentMap.Get.ExtensionAgreementPage.ReadThis));
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public void DoPrint()
        {
            secciPrint.Click();
            //print event check?
        }

        public String GetSecci()
        {
            return secci.GetValue();
        }

        public SecciToggleElement GetSecciToggleElement()
        {
            var SecciTogglelink = new SecciToggleElement(this);
            return SecciTogglelink;
        }

        public void SecciToggleButtonClick()
        {
            _SecciTogglelink.Click();
        }

        public void ClosePopupWindow()
        {
            Thread.Sleep(1000);
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.PopupCloseLink)).Click();
        }

        public String SecciPopupWindowContent()
        {
            string currentWindowHdl = Client.Driver.CurrentWindowHandle;
            Thread.Sleep(3000);

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.PopupContentFrame)).GetAttribute("name");
            var secci = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciContent));
            var secci_text = secci.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return secci_text;
        }

        public String TermsAndConditionsContent()
        {
            string currentWindowHdl = Client.Driver.CurrentWindowHandle;
            Thread.Sleep(3000);

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.PopupContentFrame)).GetAttribute("name");
            var content = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.TermsAndConditionsContent));
            var contentText = content.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return contentText;
        }

        public String WrittenExplanationContent()
        {
            string currentWindowHdl = Client.Driver.CurrentWindowHandle;
            Thread.Sleep(3000);

            var frameName = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.PopupContentFrame)).GetAttribute("name");
            var content = Client.Driver.SwitchTo().Frame(frameName).FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.WrittenExplanationContent));
            var contentText = content.Text;

            Client.Driver.SwitchTo().Window(currentWindowHdl);

            return contentText;
        }

        public ExtensionAgreementPage ClickAcceptGetError()
        {
            _nextButton.Click();
            return new ExtensionAgreementPage(Client);
        }

        public string GetErrorText()
        {
            return this.Error;
        }
    }
}