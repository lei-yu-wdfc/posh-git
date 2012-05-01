using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;


namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class PrepaidCardPage : BasePage
    {
        private readonly IWebElement _applyCardButton;

        public PrepaidCardPage(UiClient client) : base(client)
        {
            _applyCardButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.ApplyCardButton));

        }

        public void ApplyCardButtonClick()
        {
            _applyCardButton.Click();
        }
    }
}
