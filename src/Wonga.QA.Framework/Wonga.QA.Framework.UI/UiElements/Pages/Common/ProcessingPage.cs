using System;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Timeout = Wonga.QA.Framework.Core.Timeout;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ProcessingPage : BasePage
    {
        public ProcessingPage(UiClient client) : base(client)
        {
            //var processing = Client.Driver.FindElement(By.Id(Elements.Get.ProcessingPage.FormId));
            //var img = processing.FindElement(By.CssSelector(Elements.Get.ProcessingPage.ProcessingImageTag));
            //Assert.That(img.GetAttribute(Elements.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(Elements.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public IDecisionPage WaitFor<T>() where T : IDecisionPage
        {
            //NOTE: Is this working??
            //Do.Until(() => !(Source.Contains(Elements.Get.ProcessingPage.ProcessingText)), TimeSpan.FromMinutes(5));
            
            if (typeof(T) == typeof(AcceptedPage))
                return Do.Until(() => new AcceptedPage(Client), Timeout.TwoMinutes);

            if (typeof(T) == typeof(DeclinedPage))
                return Do.Until(() => new DeclinedPage(Client), Timeout.TwoMinutes);
            throw new NotImplementedException();
        }
    }
}
