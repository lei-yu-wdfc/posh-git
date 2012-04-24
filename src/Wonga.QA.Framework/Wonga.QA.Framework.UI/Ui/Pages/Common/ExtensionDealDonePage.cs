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
    public class ExtensionDealDonePage : BasePage, IApplyPage
    {
        private IWebElement _accountLink;

        public ExtensionDealDonePage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item("Success! Your new promise date has been approved."));
            _accountLink = Content.FindElement(By.CssSelector(Ui.Get.ExtensionDealDonePage.AccountLink));
        }

        public bool IsDealDonePageExtensionAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[extension-amount]");
            return amountResult | tokenResult ;
        }

        public bool IsDealDonePageDateNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[extension-repayment-date]");
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
    }
}
