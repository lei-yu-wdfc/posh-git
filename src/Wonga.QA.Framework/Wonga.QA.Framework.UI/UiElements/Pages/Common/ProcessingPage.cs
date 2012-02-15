using System;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ProcessingPage : BasePage
    {
        public ProcessingPage(UiClient client)
            : base(client)
        {
            var processing = Client.Driver.FindElement(By.Id(Elements.Get.ProcessingPage.FormId));
            var img = processing.FindElement(By.TagName(Elements.Get.ProcessingPage.ProcessingImageTag));

            Assert.That(img.GetAttribute(Elements.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(Elements.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public IDecisionPage WaitFor<T>() where T : IDecisionPage
        {
            Do.Until(() => !(Source.Contains(Elements.Get.ProcessingPage.Legend)), TimeSpan.FromMinutes(5));
            switch (Config.AUT)
            {
                case (AUT.Wb):
                    {
                        if (typeof(T) == typeof(Pages.Wb.AcceptedPage))
                            return new Pages.Wb.AcceptedPage(Client);

                        if (typeof(T) == typeof(Pages.Common.DeclinedPage))
                            return new Pages.Common.DeclinedPage(Client);
                    }
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
