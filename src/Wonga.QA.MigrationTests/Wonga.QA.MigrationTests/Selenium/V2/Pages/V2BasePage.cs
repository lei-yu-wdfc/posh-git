using Wonga.QA.Framework.UI;

namespace Wonga.QA.MigrationTests.Selenium.V2.Pages
{
    public abstract class V2BasePage
    {
        public UiClient Client;

        protected V2BasePage(UiClient client)
        {
            Client = client;
        }
    }
}
