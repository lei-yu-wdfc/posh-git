using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using NHamcrest.Core;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class ReferPage : BasePage, IApplyPage
    {
        private IWebElement _returnToHomepage;

        public ReferPage(UiClient client)
            : base(client)
        {
            Assert.That(Headers, Has.Item(Ui.Get.ReferPage.HeaderText));
            _returnToHomepage = Content.FindElement(By.CssSelector(Ui.Get.ReferPage.ReturnToHomepage));
        }

        public void GoHome()
        {
            _returnToHomepage.Click();
        }
    }
}
