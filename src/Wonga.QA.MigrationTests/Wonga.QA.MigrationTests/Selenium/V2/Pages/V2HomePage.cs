using OpenQA.Selenium;
using Wonga.QA.Framework.UI;
using Wonga.QA.MigrationTests.Selenium.V2.Elements;

namespace Wonga.QA.MigrationTests.Selenium.V2.Pages
{
    public class V2HomePage : V2BasePage
    {
        public V2SlidersElement Sliders { get; set; }
        public IWebElement EmailTextField { get; set; }
        public IWebElement PasswordTextField { get; set; }

        public V2HomePage(UiClient client)
            : base(client)
        {

            Sliders = new V2SlidersElement(this);
            EmailTextField =
                Client.Driver.FindElement(By.CssSelector("INPUT#ctl00_ContentPlaceHolder1_ucTopNavigator_tbxLogin"));
            PasswordTextField =
                Client.Driver.FindElement(By.CssSelector("INPUT#ctl00_ContentPlaceHolder1_ucTopNavigator_tbxPassword"));

        }
    }
}
