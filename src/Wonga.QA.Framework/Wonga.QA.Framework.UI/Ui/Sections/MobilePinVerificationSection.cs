﻿using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class MobilePinVerificationSection : BaseSection
    {
        private readonly IWebElement _pin;

        public MobilePinVerificationSection(BasePage page)
            : base(Ui.Get.MobilePinVerificationSection.Fieldset, page)
        {
            _pin = Section.FindElement(By.CssSelector(Ui.Get.MobilePinVerificationSection.Pin));
        }
        public String Pin { set { _pin.SendValue(value); } }
    }
}