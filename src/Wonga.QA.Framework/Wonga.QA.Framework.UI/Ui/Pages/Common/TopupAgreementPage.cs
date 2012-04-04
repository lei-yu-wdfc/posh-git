using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class TopUpAgreementPage : BasePage
    {
        private IWebElement _nextButton;
        private IWebElement _agreementLegals;

        public TopUpAgreementPage(UiClient client) : base(client)
        {
            
            _nextButton = Content.FindElement(By.CssSelector(Ui.Get.TopupAgreementPage.TopupAgreementAcceptButton));
            _agreementLegals = Content.FindElement(By.CssSelector(Ui.Get.TopupAgreementPage.TopupAgreementscroll));
        }

        public bool IsTopupAgreementPageTopupAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[topup-amount]");
            return amountResult | tokenResult ;
        }

        public bool IsTopupAgreementPageDateNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[topup-repayment-date]");
            return tokenResult ;
        }

        public bool IsTopupTotalAmountTokenBeingReplaced()
        {
            bool result = Content.Driver().PageSource.Contains("[topup-total-amount]");
            return result;
        }

        public bool IsAgreementPageTopupScrollEmpty()
        {
            bool legalResult2 = _agreementLegals.Displayed;
            bool legalResult = _agreementLegals.Displayed;
            return legalResult2 | legalResult;

        }

    }

}