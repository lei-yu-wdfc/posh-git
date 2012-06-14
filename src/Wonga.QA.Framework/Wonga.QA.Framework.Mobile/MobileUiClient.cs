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
    public class MobileUiClient
    {
        private IWebDriver _iWebDriver;
        public IWebDriver Driver
        {
            get { return _iWebDriver; }
        }

         public IList<BrowserCapability> BrowserCapabilities;

        public MobileUiClient()
        {
            InitializeBrowserCapabilities();
            _iWebDriver = GetWebDriver();
            if (Driver is CustomRemoteWebDriver)
                TestContext.CurrentContext.AddMetadata("JobURL", string.Format("https://saucelabs.com/jobs/{0}", ((CustomRemoteWebDriver)Driver).GetSessionId()));
        }

        private IWebDriver GetWebDriver()
        {
            var capabilities = GetDesiredCapabilities();
            if (Config.Ui.RemoteMode)
                return new CustomRemoteWebDriver(Config.Ui.RemoteUri, capabilities);
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

        private DesiredCapabilities GetDesiredCapabilities()
        {
            BrowserCapability browserCapability = GetBrowserCapability();
            DesiredCapabilities capabilities;
            switch (Config.Ui.Browser)
            {
                case (Config.UiConfig.BrowserType.FirefoxMobile):
                    capabilities = DesiredCapabilities.Firefox();
                    break;
                case (Config.UiConfig.BrowserType.Android):
                    capabilities = DesiredCapabilities.Android();
                    break;
                
                default:
                    throw new ArgumentException("Please select a QAFBrowser environment variable.");
            }
            capabilities.SetCapability(CapabilityType.Version, Config.Ui.BrowserVersion);
            capabilities.SetCapability(CapabilityType.Platform, new Platform(browserCapability.Platform));
            capabilities.SetCapability("name", TestContext.CurrentContext.Test.Name);
            capabilities.SetCapability("username", Config.Ui.RemoteUsername);
            capabilities.SetCapability("accessKey", Config.Ui.RemoteApiKey);
            var tags = new List<String> { Config.SUT.ToString(), Config.AUT.ToString() };
            capabilities.SetCapability("tags", tags);
            capabilities.SetCapability("public", true);
            capabilities.SetCapability("idle-timeout", 60);
            capabilities.SetCapability("sauce-advisor", false);
            capabilities.SetCapability("record-screenshots", false);
            return capabilities;
        }

        private void InitializeBrowserCapabilities()
        {
           
        }

        public BrowserCapability GetBrowserCapability()
        {
            if (!Config.Ui.RemoteMode)
                return new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = null };
            return
                BrowserCapabilities.Single(x => x.BrowserType == Config.Ui.Browser &&
                                                (Config.Ui.BrowserVersion == x.Version || Config.Ui.Browser == Config.UiConfig.BrowserType.Chrome));
        }
    }

    public class BrowserCapability
    {
        public Config.UiConfig.BrowserType BrowserType { get; set; }
        public string Version { get; set; }
        public PlatformType Platform { get; set; }
    }
}
