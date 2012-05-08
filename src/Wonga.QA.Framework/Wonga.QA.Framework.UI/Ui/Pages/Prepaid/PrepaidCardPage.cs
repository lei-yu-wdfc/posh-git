using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Elements.Prepaid;


namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class PrepaidCardPage : BasePage
    {
        private  IWebElement _applyCardButton;
        private IWebElement _premiumCardButton;
        private PrepaidCardMenuElement _menu;
        private ChooseCardTypeMenuElement _cardMenu;
        private IWebElement _faqLink;
        private IWebElement _tsLink;
        private IWebElement _tsInFeesLink;
        private IWebElement _premiumRewardsLink;

        public PrepaidCardPage(UiClient client) : base(client)
        {
            
        }

        public void ApplyPrepaidCardButtonClick()
        {
            _applyCardButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.ApplyCardButton));
            _applyCardButton.Click();
        }

        public PrepaidCardPage PremiumCardButtonClick()
        {
            _premiumCardButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChooseCardTypeMenuElement.PremiumCard));
            _premiumCardButton.Click();    
            //Client.Driver.Navigate().GoToUrl("http://dev.wonga.com/my-account/prepaid/premium");
            //Client.Driver.Url = "http://dev.wonga.com/my-account/prepaid/premium";
            return new PrepaidCardPage(Client);

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

        public void ShowCardTypeMenu()
        {
            _cardMenu = new ChooseCardTypeMenuElement(this);
        }

        public void FindFAQAndTSLinks()
        {
            _faqLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.FAQLink));
            _tsLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.TSLink));
        }

        public void FindTSInFeesLink()
        {
            _tsInFeesLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.TSInFeesLink));
        }

        public void FindPremiumRewardsLink()
        {
            _premiumRewardsLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.PremiumRewardsLink));
        }
    }
}
