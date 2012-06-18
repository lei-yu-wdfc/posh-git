using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Mappings.Ui.Pages
{
    class ProcessingPageMobile : BasePageMobile
    {
        public ProcessingPageMobile(MobileUiClient client)
            : base(client)
        {
            //var processing = Client.Driver.FindElement(By.Id(UiMap.Get.ProcessingPage.FormId));
            //var img = processing.FindElement(By.CssSelector(UiMap.Get.ProcessingPage.ProcessingImageTag));
            //Assert.That(img.GetAttribute(UiMap.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(UiMap.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public IDecisionPage WaitFor<T>() where T : IDecisionPage
        {
            //NOTE: Is this working??
            //Do.Until(() => !(Source.Contains(UiMap.Get.ProcessingPage.ProcessingText)), TimeSpan.FromMinutes(5));

            if (typeof(T) == typeof(AcceptedPageMobile))
                return Do.With.Timeout(2).Until(() => new AcceptedPageMobile(Client));

            if (typeof(T) == typeof(ApplyTermsPageMobile))
                return Do.With.Timeout(2).Until(() => new ApplyTermsPageMobile(Client));

            if (typeof(T) == typeof(DeclinedPageMobile))
                return Do.With.Timeout(2).Until(() => new DeclinedPageMobile(Client));
            throw new NotImplementedException();
        }
    }
}
