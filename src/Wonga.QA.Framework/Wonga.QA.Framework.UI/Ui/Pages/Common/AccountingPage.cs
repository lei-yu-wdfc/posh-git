using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class AccountingPage : AdminBasePage
    {
        private readonly IWebElement _addFile;
        private readonly IWebElement _cashOut;

        public AccountingPage(UiClient client) : base(client)
        {
            _addFile = Client.Driver.FindElement(By.CssSelector(Ui.Get.AccountingPage.AddFileLink));
            _cashOut = Client.Driver.FindElement(By.CssSelector(Ui.Get.AccountingPage.CashOutLink));
        }

        public AddFilePage AddFile()
        {
            _addFile.Click();
            return new AddFilePage(Client);
        }

        public CashOutPage CashOut()
        {
            _cashOut.Click();
            return new CashOutPage(Client);
        }
    }
}
