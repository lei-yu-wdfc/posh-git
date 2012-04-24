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
    class ExtensionErrorPage : BasePage, IExtensionPaymentPage
    {
        public ExtensionErrorPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item("Ouch! We're sorry"));
        }

        public bool IsErrorPageSorryNotPresent()
        {
            bool tokenResult = Content.Driver().PageSource.Contains("We're sorry");
            return !tokenResult;
        }

    }
}
