using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepayOverduePaymentFailedPage : BasePage, IRepayPaymentPage
    {
        private IWebElement _header;
        private IWebElement _bodyContent;
        private IWebElement _totalNewRepayable;
        private IWebElement _repayRetry;
        private IWebElement _addCard;
        
        public RepayOverduePaymentFailedPage(UiClient client) : base(client)
        {
            _header = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.ContentArea));
            _totalNewRepayable = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.TotalNewRepayable));
            _repayRetry = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.RepayRetry));
            _addCard = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.AddCard));
        }

        public bool IsPaymentFailedAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-new-repayable]");
            return amountResult | tokenResult;
        }
    }
}
