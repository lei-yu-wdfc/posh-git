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
    public class RepayErrorPage : BasePage, IRepayPaymentPage
    {
        private IWebElement _header;
        private IWebElement _bodyContent;

        public RepayErrorPage(UiClient client) : base(client)
        {
            //Assert.That(Headers, Has.Item(Wonga.QA.Framework.UI.ContentMap.Get.RepayDueFullpaySuccessPage.HeaderText));
            _header = Content.FindElement(By.CssSelector(UiMap.Get.RepayErrorPage.Header));
            _bodyContent = Content.FindElement(By.CssSelector(UiMap.Get.RepayErrorPage.ContentArea));
        }
    }
}
