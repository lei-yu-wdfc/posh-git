using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Wonga.QA.Framework.UI
{
    public class UiDriver
    {
        public IWebDriver WebDriver;

        public UiDriver()
        {
            WebDriver = new ChromeDriver();
            //WebDriver = new FirefoxDriver();
        }
    }
}
