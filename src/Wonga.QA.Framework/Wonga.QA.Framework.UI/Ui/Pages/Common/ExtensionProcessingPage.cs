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
    public class ExtensionProcessingPage : BasePage
    {
        private IWebElement _staticLink;

        public ExtensionProcessingPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.ExtensionProcessingPage.HeaderText));
            _staticLink = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionProcessingPage.ProcessingStaticLink));
        }

        public IExtensionPaymentPage WaitFor<T>() where T : IExtensionPaymentPage
        {
            if (typeof(T) == typeof(ExtensionAgreementPage))
                return Do.With.Timeout(2).Until(() => new ExtensionAgreementPage(Client));

            if (typeof(T) == typeof(ExtensionPaymentFailedPage))
                return Do.With.Timeout(2).Until(() => new ExtensionPaymentFailedPage(Client));

            if (typeof(T) == typeof(ExtensionErrorPage))
                return Do.With.Timeout(2).Until(() => new ExtensionErrorPage(Client));
            
            throw new NotImplementedException();
        }
    }
}
