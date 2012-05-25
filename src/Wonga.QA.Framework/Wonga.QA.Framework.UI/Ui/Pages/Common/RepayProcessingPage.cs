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
    public class RepayProcessingPage : BasePage
    {
        public RepayProcessingPage(UiClient client) : base(client)
        {
            //var processing = Client.Driver.FindElement(By.Id(UiMap.Get.ProcessingPage.FormId));
            //var img = processing.FindElement(By.CssSelector(UiMap.Get.ProcessingPage.ProcessingImageTag));
            //Assert.That(img.GetAttribute(UiMap.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(UiMap.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public IRepayPaymentPage WaitFor<T>() where T : IRepayPaymentPage
        {
            
            if (typeof(T) == typeof(RepayEarlyPartpaySuccessPage))
                return Do.With.Timeout(1).Until(() => new RepayEarlyPartpaySuccessPage(Client));

            if (typeof(T) == typeof(RepayEarlyFullpaySuccessPage))
                return Do.With.Timeout(1).Until(() => new RepayEarlyFullpaySuccessPage(Client));

            if (typeof(T) == typeof(RepayEarlyPaymentFailedPage))
                return Do.With.Timeout(1).Until(() => new RepayEarlyPaymentFailedPage(Client));

            if (typeof(T) == typeof(RepayDuePartpaySuccessPage))
                return Do.With.Timeout(1).Until(() => new RepayDuePartpaySuccessPage(Client));

            if (typeof(T) == typeof(RepayDueFullpaySuccessPage))
                return Do.With.Timeout(1).Until(() => new RepayDueFullpaySuccessPage(Client));

            if (typeof(T) == typeof(RepayDuePaymentFailedPage))
                return Do.With.Timeout(1).Until(() => new RepayDuePaymentFailedPage(Client));
            
            if (typeof(T) == typeof(RepayOverduePartpaySuccessPage))
                return Do.With.Timeout(1).Until(() => new RepayOverduePartpaySuccessPage(Client));

            if (typeof(T) == typeof(RepayOverdueFullpaySuccessPage))
                return Do.With.Timeout(1).Until(() => new RepayOverdueFullpaySuccessPage(Client));

            if (typeof(T) == typeof(RepayOverduePaymentFailedPage))
                return Do.With.Timeout(1).Until(() => new RepayOverduePaymentFailedPage(Client));

            throw new NotImplementedException();
        }
    }
}
