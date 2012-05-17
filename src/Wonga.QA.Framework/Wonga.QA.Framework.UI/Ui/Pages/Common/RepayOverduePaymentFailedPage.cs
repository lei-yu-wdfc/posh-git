﻿using NHamcrest.Core;
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
        
        public RepayOverduePaymentFailedPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.RepayOverduePaymentFailedPage.HeaderText));
            _header = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.RepayOverduePaymentFailedPage.ContentArea));
        }

        public bool IsPaymentFailedAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-new-repayable]");
            return amountResult | tokenResult;
        }
    }
}