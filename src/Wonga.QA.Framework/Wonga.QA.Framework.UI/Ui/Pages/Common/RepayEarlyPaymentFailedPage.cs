using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepayEarlyPaymentFailedPage : BasePage, IRepayPaymentPage
    {
        private IWebElement _header;
        private IWebElement _bodyContent;
        private IWebElement _totalNewRepayable;
        private IWebElement _repayRetry;
        private IWebElement _addCard;

        public RepayEarlyPaymentFailedPage(UiClient client)
            : base(client)
        {          
            _header = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPaymentFailedPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPaymentFailedPage.ContentArea));
            _totalNewRepayable = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPaymentFailedPage.TotalNewRepayable));
            _repayRetry = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPaymentFailedPage.RepayRetry));
            _addCard = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPaymentFailedPage.AddCard));
        }

        public bool IsPaymentFailedAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[total-new-repayable]");
            return amountResult | tokenResult;
        }

        public bool IsPaymentFailedDateNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-promise-date]");
            return tokenResult;
        }
    }
}
