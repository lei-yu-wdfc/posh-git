using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class HomePageWelcomePopup : BaseElement
    {
        private readonly IWebElement _frame;
        private readonly IWebElement _close;

        public HomePageWelcomePopup(HomePage page)
            : base(page)
        {
            _frame = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePageWelcomePopup.Frame));
            _close = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePageWelcomePopup.Close)); ;

        }
    }
}
