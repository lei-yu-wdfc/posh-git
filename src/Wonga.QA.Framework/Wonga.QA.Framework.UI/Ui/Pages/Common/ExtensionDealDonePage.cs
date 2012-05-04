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
        private IWebElement _header;
        private IWebElement _bodyContent;

        public ExtensionDealDonePage(UiClient client) : base(client)
        {
            _header = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionDealDonePage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionDealDonePage.ContentArea));
        }

        public bool IsDealDonePageExtensionAmountNotPresent()
        {
            bool amountResult = Content.Driver().PageSource.Contains("£0.00");
            bool tokenResult = Content.Driver().PageSource.Contains("[extension-amount]");
            return amountResult | tokenResult ;
        }

        public bool IsDealDonePageDateTokenPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("[extension-repayment-date]");
            return tokenResult ;
        }

        public bool IsDealDonePageSorryNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("sorry");
            return !tokenResult;
        }
    }
}
