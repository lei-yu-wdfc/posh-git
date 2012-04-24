using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ExtensionAgreementPage : BasePage, IExtensionPaymentPage
    {
        private IWebElement _nextButton;
        private IWebElement _agreementLegals;

        public ExtensionAgreementPage(UiClient client) : base(client)
        {
            
            _nextButton = Content.FindElement(By.CssSelector(Ui.Get.ExtensionAgreementPage.ExtensionAgreementAcceptButton));
            _agreementLegals = Content.FindElement(By.CssSelector(Ui.Get.ExtensionAgreementPage.ExtensionAgreementscroll));
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
        

    }

}