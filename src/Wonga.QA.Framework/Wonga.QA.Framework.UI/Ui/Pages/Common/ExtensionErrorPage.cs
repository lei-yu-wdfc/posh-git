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

namespace Wonga.QA.Framework.UI.Ui.Pages.Common
{
    class ExtensionErrorPage
    {
        private IWebElement _accountLink;

        public ExtensionErrorPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item("Ouch! We're sorry"));
            _accountLink = Content.FindElement(By.CssSelector(Ui.Get.ExtensionErrorPage.AccountLink));
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
