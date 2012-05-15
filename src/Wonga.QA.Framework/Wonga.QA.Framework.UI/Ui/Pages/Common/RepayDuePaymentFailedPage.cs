using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepayDuePaymentFailedPage : BasePage, IRepayPaymentPage
    {
        public RepayDuePaymentFailedPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(UiMap.Get.RepayDuePaymentFailedPage.HeaderText));
        }

        public bool IsPaymentFailedAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-new-repayable]");
            return amountResult | tokenResult;
        }
    }
}
