using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.UI.Elements;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.UI.Elements
{
    public class TabsElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _home;
        private readonly IWebElement _howItWorks;
        private readonly IWebElement _aboutUs;
        private readonly IWebElement _advice;
        private readonly IWebElement _myAccount;

        public TabsElement(BasePageMobile page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.TabsElement.TabsForm));
            _home = _form.FindElement(By.CssSelector(UiMapMobile.Get.TabsElement.Home));
            _howItWorks = _form.FindElement(By.CssSelector(UiMapMobile.Get.TabsElement.HowItWorks));
            _aboutUs = _form.FindElement(By.CssSelector(UiMapMobile.Get.TabsElement.AboutUs));
            
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                    _advice = _form.FindElement(By.CssSelector(UiMapMobile.Get.TabsElement.Advice));
                    _myAccount = _form.FindElement(By.CssSelector(UiMapMobile.Get.TabsElement.MyAccount));
                    break;
            }

        }

        public IApplyPage GoHome()
        {
            _home.Click();
            return new HomePageMobile(Page.Client);
        }

        //TODO add methods for all buttons

        public IApplyPage GoToMyAccount()
        {
            _myAccount.Click();
            return new MySummaryPageMobile(Page.Client);
        }
    }
}
