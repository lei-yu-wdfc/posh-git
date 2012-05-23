using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class TimeoutTestPage : BasePage
    {
        public TimeoutTestPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(ContentMap.Get.TimeoutTestPage.HeaderText));
        }
    }
}
