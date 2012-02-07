using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Pages.Interfaces;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.UI.Pages
{
    public class ProcessingPage : BasePage
    {
        public ProcessingPage(UiClient client)
            : base(client)
        {
            IWebElement processing = Client.Driver.FindElement(By.Id("wonga-processing"));
            IWebElement img = processing.FindElement(By.TagName("img"));

            Assert.That(img.GetAttribute("alt"), Is.EqualTo("Processing"));
        }

        public IDecisionPage WaitFor<T>() where T : IDecisionPage
        {

            Do.Until(() => !(Source.Contains("We are processing your application") || Source.Contains("We are now processing your application")), TimeSpan.FromMinutes(5));

            if (typeof(T) == typeof(Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage))
                return new Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage(Client);

            //if (typeof(T) == typeof(DeclinedPage))
            //    return new DeclinedPage(Client);

            throw new NotImplementedException();
        }
    }
}
