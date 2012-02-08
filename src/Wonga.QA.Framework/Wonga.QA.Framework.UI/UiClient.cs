﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Pages;

namespace Wonga.QA.Framework.UI
{
    [Obsolete]
    public class UiClient : IDisposable
    {
        public IWebDriver Driver;

        public UiClient(String profileDir)
        {
            var firefoxProfile = new FirefoxProfile(profileDir);
            Driver = new FirefoxDriver(firefoxProfile);
        }

        public UiClient()
        {
            //Driver = new FirefoxDriver();
            //Driver = new InternetExplorerDriver();
            Driver = new ChromeDriver();
        }

        public HomePage Home()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home);
            return new HomePage(this);
        }

        public Image Screen()
        {
            Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            MemoryStream stream = new MemoryStream(screenshot.AsByteArray);
            return Image.FromStream(stream);
        }

        public String Source()
        {
            return Driver.PageSource;
        }

        public void Dispose()
        {
            Do.Until(() => { Driver.Quit(); return true; });
        }
    }
}
