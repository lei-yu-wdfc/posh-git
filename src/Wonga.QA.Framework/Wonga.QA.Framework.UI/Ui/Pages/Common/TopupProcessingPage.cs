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
            if (Config.AUT == AUT.Uk)
            {
                Do.With.Message("Topup Processing page does not show the wait message").Timeout(new TimeSpan(0, 0, 8))
                    .Until(() => Content.Text.Contains(ContentMap.Get.TopupProcessingPage.WaitMessage));
                _staticLink = Content.FindElement(By.CssSelector(UiMap.Get.TopupProcessingPage.ProcessingStaticLink));
            }
            else
            {
                Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.ExtensionProcessingPage.HeaderText));
                _staticLink = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionProcessingPage.ProcessingStaticLink));
            }
        }

        public TopupAgreementPage WaitForAgreementPage(UiClient client)
        {
            Thread.Sleep(9000);
            return new TopupAgreementPage(Client);
        }
    }
}
