using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class PrepaidAdminPage:BasePage
    {
        private readonly IWebElement _disablePremiumCard;
        private readonly IWebElement _saveConfiguration;

        public PrepaidAdminPage(UiClient client) : base(client)
        {
            _disablePremiumCard = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidAdminPage.DisablePremiumCard));
            _saveConfiguration = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidAdminPage.SaveConfiguration));
        }

    }
}
