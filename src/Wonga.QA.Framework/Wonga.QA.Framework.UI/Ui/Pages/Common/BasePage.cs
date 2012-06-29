using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Gallio.Framework;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Testing.Attributes;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public abstract class BasePage
    {
        public String Title { get { return Client.Driver.Title; } }
        public String Source { get { return Client.Driver.PageSource; } }
        public String Url { get { return Client.Driver.Url; } }

        public UiClient Client;
        public String Error;
        public String InvalidFormError;
        public String[] Headers;
        public IWebElement Content;

        protected BasePage(UiClient client)
        {
            
            Client = client;
            Do.Until(() => Source);

            ReadOnlyCollection<IWebElement> errors = Client.Driver.FindElements(By.ClassName("error"));
            Error = errors.Count == 0 ? null : String.Join("\n", errors.Select(error => error.Text));

            List<String> invalidFormErrors = Client.Driver.FindElements(By.ClassName("invalid")).Select(e => e.Text.Trim()).Where(t => !String.IsNullOrEmpty(t)).ToList();
            InvalidFormError = invalidFormErrors.Count == 0 ? null : String.Join("\n", invalidFormErrors);

            List<IWebElement> headers = Client.Driver.FindElements(By.TagName("h1")).ToList();
            headers.AddRange(Client.Driver.FindElements(By.TagName("h2")));
            Headers = headers.Select(header => header.Text.Trim()).Where(header => !String.IsNullOrEmpty(header)).ToArray();

            if (!DoIgnoreErrors())
            {
                Assert.That(Title, Is.Not<String>(Starts.With("Error")));
                Assert.That(Error, Is.Null(), Error);
                Assert.That(InvalidFormError, Is.Null(), InvalidFormError);
                Assert.That(Headers, Is.Not(Has.Item("Access denied")));
                Assert.That(Headers, Is.Not(Has.Item<String>(Starts.With("Error"))));
            }

            Content = Config.Ui.Browser.Equals(Config.UiConfig.BrowserType.FirefoxMobile) ? Do.Until(() => Client.Driver.FindElement(By.Id("content-content"))) : Do.Until(() => Client.Driver.FindElement(By.Id("content-area"))); 
            
        }

        public BasePage WaitForPage<T>() where T : BasePage
        {
            if (typeof (T) == typeof (HomePage))
                return Do.With.Timeout(2).Until(() => new HomePage(Client));
            throw new NotImplementedException();
        }

        public bool IsWarningOccurred(string elementSelector,string warningSelector)
        {
            IWebElement element = this.Client.Driver.FindElement(By.CssSelector(elementSelector));
            element.LostFocus();
            try
            {
              IWebElement  errorElement =
                           this.Client.Driver.FindElement(By.CssSelector(warningSelector));
                string firstNameErrorFormClass = errorElement.GetAttribute("class");
                if (firstNameErrorFormClass.Equals("invalid"))
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Can't find error form");
                return false;
            }
            return false;
        }
 	 	

 	 	
        public bool IsSuccessTickOccured(string elementSelector, string warningSelector)
        {
            IWebElement element = this.Client.Driver.FindElement(By.CssSelector(elementSelector));
            element.LostFocus();
            try
            {
                IWebElement errorElement =
                             this.Client.Driver.FindElement(By.CssSelector(warningSelector));

                string firstNameErrorFormClass = errorElement.GetAttribute("class");

                if (firstNameErrorFormClass.Equals("invalid success"))
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Can't find error form");
                return false;
            }
            return false;
        }
         
        private bool DoIgnoreErrors()
        {
            return
                TestContext.CurrentContext.TestStep.Metadata.Any(x => x.Key == "IgnorePageErrors")
                || TestContext.CurrentContext.TestStep.Parent.Metadata.Any(x => x.Key == "IgnorePageErrors");
        }
    }
}
