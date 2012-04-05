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
    public class TopupProcessingPage : BasePage
    {
        public TopupProcessingPage(UiClient client) : base(client)
        {
            //Assert.That(img.GetAttribute(Ui.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(Ui.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public ITopupDecisionPage WaitFor<T>() where T : ITopupDecisionPage
        {
            //NOTE: Is this working??
            //Do.Until(() => !(Source.Contains(Ui.Get.ProcessingPage.ProcessingText)), TimeSpan.FromMinutes(5));
            
            if (typeof(T) == typeof(TopupAgreementPage))
                return Do.With.Timeout(1).Until(() => new TopupAgreementPage(Client));

            throw new NotImplementedException();
        }
    }
}
