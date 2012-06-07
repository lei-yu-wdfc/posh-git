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
    public class RepayEarlyPartpaySuccessPage : BasePage, IRepayPaymentPage
    {
        private IWebElement _header;
        private IWebElement _bodyContent;

        public RepayEarlyPartpaySuccessPage(UiClient client) : base(client)
        {
            //Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.RepayEarlyPartpaySuccessPage.HeaderText));
            _header = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPartpaySuccessPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.RepayEarlyPartpaySuccessPage.ContentArea));
        }

        public bool IsRepayEarlyPartpaySuccessPageAmountTokenNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-interest-saved]");
            bool tokenResult2 = Content.Driver().PageSource.Contains("[repay-amount]");
            bool tokenResult3 = Content.Driver().PageSource.Contains("[repay-new-repayable]");
            return amountResult | tokenResult | tokenResult2 | tokenResult3;
        }

        public bool IsRepayEarlyPartpaySuccessPageDateTokenNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[repay-agreement-date]");
            bool tokenResult2 = Content.Driver().PageSource.Contains("[repay-promise-date]");
            return tokenResult | tokenResult2;
        }

        public String ContentArea()
        {
            return _bodyContent.Text;
        }

    }
}
