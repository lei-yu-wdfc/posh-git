﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Elements;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class MySummaryPageMobile : BasePageMobile, IApplyPage
    {
        private readonly IWebElement _myPaymentDetailsButton;
        private readonly IWebElement _mySummaryButton;
        private readonly IWebElement _myPersonalDetailsButton;

        private SlidersElement SlidersElement { get; set; }

        public MySummaryPageMobile(MobileUiClient client) : base(client)
        {
            _mySummaryButton = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.MySummaryButton));
            _myPersonalDetailsButton =
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.MyPersonalDetailsButton));
            _myPaymentDetailsButton =
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.MyPaymentDetailsButton));
        }

        public ApplyPageMobile ApplyForLoan(string howlong, string howmuch)
        {
            SlidersElement = new SlidersElement(this);
            SlidersElement.HowMuch = howlong;
            SlidersElement.HowLong = howmuch;
            SlidersElement.Submit.Click();
            return new ApplyPageMobile(Client);
        }
    }
}
