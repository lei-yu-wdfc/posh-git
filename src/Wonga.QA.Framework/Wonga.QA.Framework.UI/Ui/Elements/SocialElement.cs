using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Elements
{
    public class SocialElement : BaseElement
    {
        private IWebElement _socialTrigger;

        public SocialElement(BasePage page): base(page)
        {
            _socialTrigger = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SocialElement.SocialTrigger));
        }
    }
}
