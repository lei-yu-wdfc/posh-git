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
    public class TopupDealDonePage : BasePage, IApplyPage
    {
        private IWebElement _accountLink;

        public TopupDealDonePage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(ContentMap.Get.TopupDealDonePage.SuccessMessage));
            _accountLink = Content.FindElement(By.CssSelector(UiMap.Get.TopupDealDonePage.AccountLink));

        }

        public bool IsDealDonePageTopupAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[topup-amount]");
            return amountResult | tokenResult ;
        }

        public bool IsDealDonePageDateNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[topup-repayment-date]");
            return tokenResult ;
        }

        public bool IsDealDonePageJiffyNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("jiffy");
            return !tokenResult;
        }

        public IApplyPage ContinueToMyAccount()
        {
            _accountLink.Click();
            return new MySummaryPage(Client);
        }

        public string SucessMessage
        {
            get
            {
                return Content.FindElement(By.CssSelector(UiMap.Get.TopupDealDonePage.SuccessMessage1)).Text +
                       Content.FindElement(By.CssSelector(UiMap.Get.TopupDealDonePage.SuccessMessage2)).Text;
            }
        }
    }
}
