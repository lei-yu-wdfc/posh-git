using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;


namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class PrepaidCardPage : BasePage
    {
        private  IWebElement _applyCardButton;
        private PrepaidCardMenuElement _menu;

        public PrepaidCardPage(UiClient client) : base(client)
        {

        }

        public void ApplyPrepaidCardButtonClick()
        {
            _applyCardButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.ApplyCardButton));
            _applyCardButton.Click();
        }

        public void ShowMenuElementsForStandardCard()
        {
            _menu = new PrepaidCardMenuElement(this);
        }

        public void ShowMenuElementsForPremiumCard()
        {
            _menu = new PrepaidCardMenuElement(this);
            _menu.DisplayPremiumMenu();
        }
    }
}
