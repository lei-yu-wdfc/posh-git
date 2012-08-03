using Wonga.QA.MigrationTests.Selenium.V2.Pages;

namespace Wonga.QA.MigrationTests.Selenium.V2.Elements
{
    public class V2BaseElement
    {
        public V2BasePage Page;

        protected V2BaseElement(V2BasePage page)
        {
            Page = page;
        }
    }
}
