using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI;
using Wonga.QA.MigrationTests.V2Selenium.Elements;

namespace Wonga.QA.MigrationTests.V2Selenium.Pages
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
