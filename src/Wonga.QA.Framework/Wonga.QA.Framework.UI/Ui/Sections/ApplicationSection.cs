using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.UiElements.Sections
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
                    Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplicationSection.EditPin)));
                Do.Until(()=>pin.Displayed);
                pin.SendValue(value);
            }
        }
        public ApplicationSection(BasePage page)
            : base(Ui.Get.ApplicationSection.FormId, page)
        {
            switch (Config.AUT)
            {
                case AUT.Uk:
                    _securityCode = Section.FindElement(By.CssSelector(Ui.Get.ApplicationSection.SecurityCode));
                    _minCash = Section.FindElement(By.CssSelector(Ui.Get.ApplicationSection.MinCash));
                    break;

            }
        }

        public void ClickChangeMobileButton()
        {
            Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplicationSection.ChangeMobileButton)).Click();
        }
    }
}
