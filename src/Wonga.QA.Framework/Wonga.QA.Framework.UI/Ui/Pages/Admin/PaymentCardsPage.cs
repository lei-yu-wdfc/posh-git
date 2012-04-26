using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Admin
{
    public class PaymentCardsPage : AdminBasePage
    {
        private readonly IWebElement _addCard;

        public PaymentCardsPage(UiClient client) : base(client)
        {
            _addCard = Client.Driver.FindElement(By.CssSelector(Mappings.Ui.Get.PaymentCardsPage.AddCardLink));
        }

        public AddCardPage AddCard()
        {
            _addCard.Click();
            return new AddCardPage(Client);
        }
    }
}
