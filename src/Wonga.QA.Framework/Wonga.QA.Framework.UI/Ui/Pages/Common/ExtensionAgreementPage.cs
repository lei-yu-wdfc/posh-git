using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Sections;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ExtensionAgreementPage : BasePage, IExtensionPaymentPage
    {
        private IWebElement _nextButton;
        private IWebElement _agreementLegals;
        private IWebElement _secciButton;
        private IWebElement _aeDocumentButton;

        public IWebElement secciHeader;
        public IWebElement secciPrint;
        public IWebElement secci;
        
        public ExtensionAgreementPage(UiClient client)
            : base(client)
        {
            _nextButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionAgreementAcceptButton));
            _agreementLegals = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionAgreementscroll));
            _secciButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.ExtensionSecciButton));
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

        public void clickExplanationLink()
        {
            _aeDocumentButton.Click();
        }


        public Boolean InitialiseSecci()
        {
            try
            {
                secciHeader = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciHeader));
                secciPrint = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciPrint));
                secci = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionAgreementPage.SecciContent));
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
    }
}