using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
{
    public class ApplicationSection : BaseSection
    {
        private readonly IWebElement _securityCode;
        private readonly IWebElement _minCash;

        public String SetSecurityCode
        {
            set { _securityCode.SendValue(value); }
        }
        public String SetMinCash
        {
            set { _minCash.SendValue(value); }
        }
        public String SetPin
        {
            set
            {
                var pin =
                    Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.ApplicationSection.EditPin)));
                Do.Until(() => pin.Displayed);
                pin.SendValue(value);
            }
        }

        public ApplicationSection(BasePageMobile page)
            : base(UiMapMobile.Get.ApplicationSection.FormId, page)
        {
            switch (Config.AUT)
            {
                case AUT.Uk:
                    _securityCode = Section.FindElement(By.CssSelector(UiMapMobile.Get.ApplicationSection.SecurityCode));
                    _minCash = Section.FindElement(By.CssSelector(UiMapMobile.Get.ApplicationSection.MinCash));
                    break;
            }
        }

        public void ClickChangeMobileButton()
        {
            Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.ApplicationSection.ChangeMobileButton)).Click();
        }
    }
}
