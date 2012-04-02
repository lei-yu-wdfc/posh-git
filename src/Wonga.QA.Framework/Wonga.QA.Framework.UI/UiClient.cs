using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.SalesForce;

namespace Wonga.QA.Framework.UI
{
    public class UiClient : IDisposable
    {
        public IWebDriver Driver;

        [Obsolete]
        public UiClient(String profileDir)
        {
            var firefoxProfile = new FirefoxProfile(profileDir);
            Driver = new FirefoxDriver(firefoxProfile);
        }

        public UiClient()
        {
            var capabillities = GetDesiredCapabilities();
            Driver = GetWebDriver(capabillities);
            
        }

        private IWebDriver GetWebDriver(DesiredCapabilities capabilities)
        {
            if(Config.Ui.ExternalAccess)
                return new RemoteWebDriver(Config.Ui.RemoteUri, capabilities);
            else
                switch (Config.Ui.Browser)
                {
                        case(Config.UiConfig.BrowserType.Chrome):
                        return new ChromeDriver();
                        break;
                    case(Config.UiConfig.BrowserType.Firefox):
                        return new FirefoxDriver();
                        break;
                    case(Config.UiConfig.BrowserType.InternetExplorer):
                        var ieOps = new InternetExplorerOptions();
                        ieOps.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                        return new InternetExplorerDriver(ieOps);
                        break;
                    case(Config.UiConfig.BrowserType.Opera):
                        throw new NotImplementedException("Safari is not supported yet");
                        break;
                    case(Config.UiConfig.BrowserType.Safari):
                        throw new NotImplementedException("Safari is not supported by WebDriver");
                    default:
                        throw new ArgumentException("Please select a Browser Type via the BrowserType user session variable");    
                }
        }

        private DesiredCapabilities GetDesiredCapabilities()
        {
            DesiredCapabilities capabilities;
            switch (Config.Ui.Browser)
            {
                case(Config.UiConfig.BrowserType.Chrome):
                    capabilities = DesiredCapabilities.Chrome();
                    break;
                case(Config.UiConfig.BrowserType.Firefox):
                    capabilities = DesiredCapabilities.Firefox();
                    break;
                case(Config.UiConfig.BrowserType.InternetExplorer):
                    capabilities = DesiredCapabilities.InternetExplorer();
                    break;
                case(Config.UiConfig.BrowserType.Opera):
                    capabilities = DesiredCapabilities.Opera();
                    break;
                case(Config.UiConfig.BrowserType.Safari):
                    throw new NotImplementedException("Safari is not supported yet");
                default:
                    throw new ArgumentException("Please select a Browser Type via the BrowserType user session variable");
            }
            capabilities.SetCapability(CapabilityType.Version, Config.Ui.BrowserVersion);
            capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.XP));
            capabilities.SetCapability("name", "Testing Selenium 2 with C# on Sauce");
            capabilities.SetCapability("username", Config.Ui.RemoteUsername);
            capabilities.SetCapability("accessKey", Config.Ui.RemoteApiKey);
            return capabilities;
        }

        public HomePage Home()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home);
            return new HomePage(this);
        }

        public LoginPage Login()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "/login");
            return new LoginPage(this);
        }

        public MyPaymentsPage Payments()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "/my-account/details");
            return new MyPaymentsPage(this);
        }
        
        public AboutUsPage About()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "/about");
            return new AboutUsPage(this);
        }

        public Image Screen()
        {
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
            Do.Until(() => { Driver.Quit(); return true; });
        }

        public SalesForceLoginPage SalesForceStart()
        {
            Driver.Navigate().GoToUrl(Config.SalesforceUi.Home);
            return new SalesForceLoginPage(this);
        }

    }
}
