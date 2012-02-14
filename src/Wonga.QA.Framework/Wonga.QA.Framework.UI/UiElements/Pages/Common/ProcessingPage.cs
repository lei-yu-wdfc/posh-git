using System;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ProcessingPage : BasePage
    {
        public ProcessingPage(UiClient client) : base(client)
        {
            var processing = Client.Driver.FindElement(By.Id(Elements.Get.ProcessingPage.FormId));
            var img = processing.FindElement(By.TagName(Elements.Get.ProcessingPage.ProcessingImageTag));
            
            Assert.That(img.GetAttribute(Elements.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(Elements.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public IDecisionPage WaitFor<T>() where T : IDecisionPage
        {

            Do.Until(() => !(Source.Contains(Elements.Get.ProcessingPage.Legend)), TimeSpan.FromMinutes(5));

            if (typeof(T) == typeof(AcceptedPage))
                return new AcceptedPage(Client);

            //if (typeof(T) == typeof(DeclinedPage))
            //    return new DeclinedPage(Client);

            throw new NotImplementedException();
        }
    }
}
