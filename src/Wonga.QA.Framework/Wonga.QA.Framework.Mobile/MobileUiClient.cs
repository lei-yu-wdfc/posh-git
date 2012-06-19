using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using Gallio.Framework;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Wonga.QA.Framework.Core;


namespace Wonga.QA.Framework.Mobile
{
    public class MobileUiClient : IDisposable
    {
        private IWebDriver _iWebDriver;
        public IWebDriver Driver
        {
            get { return _iWebDriver; }
        }

        public MobileUiClient()
        {
            _iWebDriver = GetWebDriver();
        }

        private IWebDriver GetWebDriver()
        {
            switch (Config.Ui.Browser)
            {
                case (Config.UiConfig.BrowserType.FirefoxMobile):
                    var firefoxProfile = new FirefoxProfile();
                    firefoxProfile.SetPreference("network.http.accept.default", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8,text/vnd.wap.wml;q=0.6");
                    return new FirefoxDriver(firefoxProfile);
                case (Config.UiConfig.BrowserType.InternetExplorer):
                    return new AndroidDriver("http://127.0.0.1:8080/wd/hub"); //Default android selenium server address
                default:
                    throw new ArgumentException("Please select a Browser Type via the QAFBrowser environment variable");
            }
        }
        

        public Image Screen()
        {
            if (!(Driver is ITakesScreenshot))
                return null;
            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            var stream = new MemoryStream(screenshot.AsByteArray);
            return Image.FromStream(stream);
        }

        public String Source()
        {
            return Driver.PageSource;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Do.Until(() =>
                         {
                             Driver.Quit();
                             return true;
                         });
        }

        ~MobileUiClient()
        {
            Dispose(false);
        }
        
    }
}
