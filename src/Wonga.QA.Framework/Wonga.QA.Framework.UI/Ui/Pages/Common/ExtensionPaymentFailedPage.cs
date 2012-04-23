using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ExtensionPaymentFailedPage : BasePage, IExtensionPaymentPage
    {
        public ExtensionPaymentFailedPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(Ui.Get.ExtensionPaymentFailedPage.HeaderText));
        }

        public bool IsPaymentFailedAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[extension-amount]");
            return amountResult | tokenResult;
        }

        public bool IsPaymentFailedDateNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[extension-repayment-date]");
            return tokenResult;
        }
    }
}
