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
    public class TabsElementMobile : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _login;
        private readonly IWebElement _applyNow;
        private readonly IWebElement _howItWorks;
        private readonly IWebElement _aboutUs;
        private readonly IWebElement _advice;
        private readonly IWebElement _myAccount;
        private readonly IWebElement _news;
        private readonly IWebElement _help;
        private readonly IWebElement _contactUs;

        public TabsElementMobile(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.TabsElementMobile.TabsForm));
            _login = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.Login));
            _applyNow = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.ApplyNow));
            _howItWorks = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.HowItWorks));
            _aboutUs = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.AboutUs));
            _advice = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.Advice));
            _myAccount = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.MyAccount));
            _news = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.News));
            _help = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.Help));
            _contactUs = _form.FindElement(By.LinkText(ContentMap.Get.TabsElementMobile.ContactUs));
        }

    }
}

