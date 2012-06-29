using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Framework.Mobile.Mappings;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
{
    public class TabsElementMobile : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _applyNow;
        private readonly IWebElement _howItWorks;
        private readonly IWebElement _aboutUs;
        private readonly IWebElement _advice;
        private readonly IWebElement _myAccount;
        private readonly IWebElement _news;
        private readonly IWebElement _help;
        private readonly IWebElement _contactUs;

        private IWebElement _logOut;
        private IWebElement _login;

        public TabsElementMobile(BasePageMobile page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.TabsElementMobile.TabsForm));
            _applyNow = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.ApplyNow));
            _howItWorks = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.HowItWorks));
            _aboutUs = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.AboutUs));
            _advice = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.Advice));
            _myAccount = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.MyAccount));
            _news = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.News));
            _help = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.Help));
            _contactUs = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.ContactUs));
        }

        public HomePageMobile LogOut()
        {
            _logOut = _form.FindElement(By.LinkText("logout"));
            _logOut.Click();
            return new HomePageMobile(Page.Client);
        }

        public MySummaryPageMobile LogIn(string email, string password)
        {
            _login = _form.FindElement(By.LinkText(ContentMapMobile.Get.TabsElementMobile.Login));
            _login.Click();
            var loginPage = Do.Until(() => new LoginPageMobile(Page.Client));
            loginPage.LoginAs(email, password);
            return new MySummaryPageMobile(Page.Client);
        }

    }
}

