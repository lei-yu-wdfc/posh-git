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



namespace Wonga.QA.Framework.UI.UiElements
{
    public class InternationalElements : BaseElement
    {
        private readonly IWebElement _internationalTrigger;
        private readonly IWebElement _internationalPanelZa;
        private readonly IWebElement _internationalPanelUk;


        public InternationalElements(BasePage page)
            : base(page)
        {
            _internationalTrigger = Page.Client.Driver.FindElement(By.XPath(Elements.Get.InternationalSection.InternationalTrigger));
            _internationalPanelZa = Page.Client.Driver.FindElement(By.XPath(Elements.Get.InternationalSection.InternationalPanelZa));
            _internationalPanelUk = Page.Client.Driver.FindElement(By.XPath(Elements.Get.InternationalSection.InternationalPanelUk));
        }

        public void InternationalTriggerClick()
        {
            _internationalTrigger.Click();
        }

        public void InternationalPanelZaClick()
        {
            _internationalPanelZa.Click();
        }

        public void InternationalPanelUkClick()
        {
            _internationalPanelUk.Click();
        }
    }
}
