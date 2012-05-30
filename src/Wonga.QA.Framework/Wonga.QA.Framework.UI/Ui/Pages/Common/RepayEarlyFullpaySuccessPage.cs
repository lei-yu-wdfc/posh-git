using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepayEarlyFullpaySuccessPage : BasePage, IRepayPaymentPage
    {
        private IWebElement _header;
        private IWebElement _bodyContent;

        public RepayEarlyFullpaySuccessPage(UiClient client) : base(client)
        {
            //Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.RepayEarlyFullpaySuccessPage.HeaderText));
            _header = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyFullpaySuccessPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyFullpaySuccessPage.ContentArea));
        }

        public bool IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-interest-saved]");
            return amountResult | tokenResult;
        }

        public bool IsRepayEarlyFullpaySuccessPageDateTokenNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-agreement-date]");
            return tokenResult;
        }

    }
}
