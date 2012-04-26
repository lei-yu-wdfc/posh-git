using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Admin
{
    public abstract class AdminBasePage
    {
        public String Title { get { return Client.Driver.Title; } }
        public String Source { get { return Client.Driver.PageSource; } }
        public String Url { get { return Client.Driver.Url; } }

        public UiClient Client;
        public String Error;
        public String InvalidFormError;
        public String[] Headers;
        public IWebElement Content;

        protected AdminBasePage(UiClient client)
        {

            Client = client;
            Do.Until(() => Source);
            Assert.That(Title, Is.Not<String>(Starts.With("Error")));

            ReadOnlyCollection<IWebElement> errors = Client.Driver.FindElements(By.ClassName("error"));
            Error = errors.Count == 0 ? null : String.Join("\n", errors.Select(error => error.Text));
            Assert.That(Error, Is.Null(), Error);

            List<String> invalidFormErrors = Client.Driver.FindElements(By.ClassName("invalid")).Select(e => e.Text.Trim()).Where(t => !String.IsNullOrEmpty(t)).ToList();
            InvalidFormError = invalidFormErrors.Count == 0 ? null : String.Join("\n", invalidFormErrors);
            Assert.That(InvalidFormError, Is.Null(), InvalidFormError);

            List<IWebElement> headers = Client.Driver.FindElements(By.TagName("h1")).ToList();
            headers.AddRange(Client.Driver.FindElements(By.TagName("h2")));
            Headers = headers.Select(header => header.Text.Trim()).Where(header => !String.IsNullOrEmpty(header)).ToArray();
            Assert.That(Headers, Is.Not(Has.Item("Access denied")));
            Assert.That(Headers, Is.Not(Has.Item<String>(Starts.With("Error"))));
        }
    }
}
