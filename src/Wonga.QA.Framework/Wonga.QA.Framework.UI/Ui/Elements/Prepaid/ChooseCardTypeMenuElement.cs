using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Ui.Elements.Prepaid
{
    public class ChooseCardTypeMenuElement : BaseElement
    {
        private readonly IWebElement _standardCard;
        private readonly IWebElement _premiumCard;

        public ChooseCardTypeMenuElement(BasePage page)
            : base(page)
        {
            _standardCard = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChooseCardTypeMenuElement.StandardCard));
            _premiumCard = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChooseCardTypeMenuElement.PremiumCard));
        }
    }
}
