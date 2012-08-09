using System;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class TimeoutTestPage : BasePage
    {
        public TimeoutTestPage(UiClient client) : base(client)
        {
            if (Content.Text.Contains(ContentMap.Get.TimeoutTestPage.ContentText) == false)
                throw new SystemException("TimeoutTest page not displayed.");
        }
    }
}
