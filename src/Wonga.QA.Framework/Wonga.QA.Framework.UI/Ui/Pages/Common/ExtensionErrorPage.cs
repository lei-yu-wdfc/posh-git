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
    public class ExtensionErrorPage : BasePage, IExtensionPaymentPage
    {
        private IWebElement _header;
        private IWebElement _bodyContent;

        public ExtensionErrorPage(UiClient client) : base(client)
        {
            Content.Driver().PageSource.Contains("Ouch! We're very sorry");
            _header = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionErrorPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionErrorPage.BodyText));
        }

        public bool IsErrorPageSorryNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("We're sorry");
            return !tokenResult;
        }

    }
}
