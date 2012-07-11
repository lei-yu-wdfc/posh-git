using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.Elements
{
    public class TabsElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _home;
        private readonly IWebElement _howItWorks;
        private readonly IWebElement _aboutUs;
        private readonly IWebElement _advice;
        //private readonly IWebElement _myAccount;

        public TabsElement(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.TabsElement.TabsForm));
            _home = _form.FindElement(By.CssSelector(UiMap.Get.TabsElement.Home));
            _howItWorks = _form.FindElement(By.CssSelector(UiMap.Get.TabsElement.HowItWorks));
            _aboutUs = _form.FindElement(By.CssSelector(UiMap.Get.TabsElement.AboutUs));
            
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                    _advice = _form.FindElement(By.CssSelector(UiMap.Get.TabsElement.Advice));
                    break;
            }

        }

        public IApplyPage GoHome()
        {
            _home.Click();
            return new HomePage(Page.Client);
        }

        //TODO add methods for all buttons

        
    }
}
