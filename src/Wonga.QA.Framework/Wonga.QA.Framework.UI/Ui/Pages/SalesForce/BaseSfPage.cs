using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.UI.UiElements.Pages.SalesForce
{
    public abstract class BaseSfPage
    {
        public String Source { get { return Client.Driver.PageSource; } }
        public UiClient Client;

        protected BaseSfPage(UiClient client)
        {
            Client = client;
            Do.Until(() => Source);
        }
    }
}