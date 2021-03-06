﻿using System;
using System.Threading;
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
        public ProcessingPage(UiClient client) : base(client)
        {
            //var processing = Client.Driver.FindElement(By.Id(UiMap.Get.ProcessingPage.FormId));
            //var img = processing.FindElement(By.CssSelector(UiMap.Get.ProcessingPage.ProcessingImageTag));
            //Assert.That(img.GetAttribute(UiMap.Get.ProcessingPage.ProcessingImageAttributeName), Is.EqualTo(UiMap.Get.ProcessingPage.ProcessingImageAttributeText));
        }

        public IDecisionPage WaitFor<T>() where T : IDecisionPage
        {
            //NOTE: Is this working??
            //Do.Until(() => !(Source.Contains(UiMap.Get.ProcessingPage.ProcessingText)), TimeSpan.FromMinutes(5));
            
            if (typeof(T) == typeof(AcceptedPage))
                return Do.With.Timeout(2).Until(() => new AcceptedPage(Client));

            if (typeof(T) == typeof(ApplyTermsPage))
                return Do.With.Timeout(2).Until(() => new ApplyTermsPage(Client));

            if (typeof(T) == typeof(DeclinedPage))
                return Do.With.Timeout(2).Until(() => new DeclinedPage(Client));
            throw new NotImplementedException();
        }
    }
}
