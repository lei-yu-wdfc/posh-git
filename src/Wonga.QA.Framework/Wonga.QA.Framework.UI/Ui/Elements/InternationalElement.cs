using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;



namespace Wonga.QA.Framework.UI.Elements
{
    public class InternationalElement : BaseElement
    {
        private readonly IWebElement _internationalTrigger;
        private readonly IWebElement _internationalPanelZa;
        private readonly IWebElement _internationalPanelCa;
        private readonly IWebElement _internationalPanelUk;



        public InternationalElement(BasePage page)
            : base(page)
        {
            _internationalTrigger =
                Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.InternationalElement.InternationalTrigger));
            switch (Config.AUT)
            {
                case (AUT.Ca):
                    _internationalPanelZa = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.InternationalElement.InternationalPanelZa));
                    _internationalPanelUk = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.InternationalElement.InternationalPanelUk));
                    break;
                case (AUT.Za):
                    _internationalPanelCa = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.InternationalElement.InternationalPanelCa));
                    _internationalPanelUk = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.InternationalElement.InternationalPanelUk));
                    break;
            }
        }

        public void InternationalTriggerClick()
        {
            _internationalTrigger.Click();
        }

        public void InternationalPanelZaClick()
        {
            _internationalPanelZa.Click();
        }

        public void InternationalPanelCaClick()
        {
            _internationalPanelCa.Click();
        }

        public void InternationalPanelUkClick()
        {
            _internationalPanelUk.Click();
        }
    }
}
