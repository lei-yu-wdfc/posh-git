using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class PrepaidCardMenuElement : BaseElement
    {
        private readonly IWebElement _summary;
        private readonly IWebElement _transaction;
        private readonly IWebElement _topUp;
        private readonly IWebElement _rewards;
        private readonly IWebElement _forgottenPin;

        private IWebElement _budjet;
        private IWebElement _cashback;

        public PrepaidCardMenuElement(BasePage page)
            : base(page)
        {
            _summary = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.Summary));
            _transaction = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.Transaction));
            _topUp = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.TopUp));
            _rewards = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.Rewards));
            _forgottenPin = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.ForgottenPin));
        }

        public void DisplayPremiumMenu()
        {
            _budjet = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.Budjet));
            _cashback = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardMenuElement.Cashback));
        }
    }
}
