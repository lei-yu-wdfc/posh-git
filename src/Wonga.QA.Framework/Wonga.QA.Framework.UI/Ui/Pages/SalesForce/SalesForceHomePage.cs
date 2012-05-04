﻿using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.SalesForce
{
    public class SalesForceHomePage : BaseSfPage
    {
        private readonly IWebElement _searchbox;
        private readonly IWebElement _searchbutton;

        public SalesForceHomePage(UiClient client)
            : base(client)
        {
            _searchbox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceHomePage.SearchBox));
            _searchbutton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceHomePage.SearchButton));
        }

        public SalesForceSearchResultPage FindCustomerByMail(string mail)
        {
            _searchbox.SendKeys(mail);
            _searchbutton.Click();
            _searchbutton.Click(); // I don't know why, but it's work only with two click
           return new SalesForceSearchResultPage(Client);
        }
    }
}