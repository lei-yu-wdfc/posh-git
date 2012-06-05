using System;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class TopupProcessingPage : BasePage
    {
        private IWebElement _staticLink;

        public TopupProcessingPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.ExtensionProcessingPage.HeaderText));
            _staticLink = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionProcessingPage.ProcessingStaticLink));
        }

        public TopupAgreementPage WaitForAgreementPage(UiClient client)
        {
            Thread.Sleep(9000);
            return new TopupAgreementPage(Client);
        }
    }
}
