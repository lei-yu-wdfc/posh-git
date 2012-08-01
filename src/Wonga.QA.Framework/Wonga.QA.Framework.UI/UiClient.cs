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
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings.Pages.PayLater;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Admin;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Framework.UI.UiElements.Pages.SalesForce;
using SubmitionPage = Wonga.QA.Framework.UI.UiElements.Pages.SubmitionPage;

namespace Wonga.QA.Framework.UI
{
    public class UiClient : IDisposable 
    {
        private IWebDriver _iWebDriver;
        public IWebDriver Driver
        {
            get { return _iWebDriver; }
        }

        
        public IList<BrowserCapability> BrowserCapabilities;

        public UiClient()
        {
            InitializeBrowserCapabilities();
            _iWebDriver = GetWebDriver();
            if (Driver is CustomRemoteWebDriver)
                TestContext.CurrentContext.AddMetadata("JobURL", string.Format("https://saucelabs.com/jobs/{0}", ((CustomRemoteWebDriver)Driver).GetSessionId()));
        }

        private IWebDriver GetWebDriver()
        {
            var capabilities = GetDesiredCapabilities();
            if(Config.Ui.RemoteMode)
                return new CustomRemoteWebDriver(Config.Ui.RemoteUri, capabilities);
            switch (Config.Ui.Browser)
            {
                case(Config.UiConfig.BrowserType.Chrome):
                    return new ChromeDriver();
                case(Config.UiConfig.BrowserType.Firefox):
                    return new FirefoxDriver();
                case (Config.UiConfig.BrowserType.FirefoxMobile):
                    var firefoxProfile = new FirefoxProfile();
                    firefoxProfile.SetPreference("network.http.accept.default", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8,text/vnd.wap.wml;q=0.6");
                    return new FirefoxDriver(firefoxProfile);
                case(Config.UiConfig.BrowserType.InternetExplorer):
                    var ieOps = new InternetExplorerOptions();
                    ieOps.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    return new InternetExplorerDriver(ieOps);
                case(Config.UiConfig.BrowserType.Opera):
                    throw new NotImplementedException("Opera is not supported yet");
                case(Config.UiConfig.BrowserType.Safari):
                    return new SafariDriver();
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
                case(Config.UiConfig.BrowserType.Chrome):
                    capabilities = DesiredCapabilities.Chrome();
                    break;
                case(Config.UiConfig.BrowserType.Firefox):
                    capabilities = DesiredCapabilities.Firefox();
                    break;
                case (Config.UiConfig.BrowserType.FirefoxMobile):
                    capabilities = DesiredCapabilities.Firefox();
                    break;
                case(Config.UiConfig.BrowserType.InternetExplorer):
                    capabilities = DesiredCapabilities.InternetExplorer();
                    break;
                case(Config.UiConfig.BrowserType.Opera):
                    capabilities = DesiredCapabilities.Opera();
                    break;
                case(Config.UiConfig.BrowserType.Safari):
                    capabilities = DesiredCapabilities.Safari() as DesiredCapabilities;
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

        public HomePage Home()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home);
            return new HomePage(this);
        }

        public FinancialAssessmentPage FinancialAssessment()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "financial-assessment");
            return new FinancialAssessmentPage(this);
        }

        public FALoginPage FinancialAssessmentLogin()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "financial-assessment/login");
            return new FALoginPage(this);
        } 

        public HomePage DoubleclickCookiesHome()
        {
            Driver.Navigate().GoToUrl(Config.Ui.DoubleClickCookiesHome);
            return new HomePage(this);
        }

        public HomePageMobile MobileHome()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home);
            return new HomePageMobile(this);
        }

        public LoginPage Login()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "/login");
            return new LoginPage(this);
        }

        public LoginPage LoginPrepaidAdmin()
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
            switch(Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                    Driver.Navigate().GoToUrl(Config.Ui.Home + "/about");
                    break;
                case AUT.Wb:
                    Driver.Navigate().GoToUrl(Config.Ui.Home + "/about-us");
                    break;
            }
            
            return new AboutUsPage(this);
        }

        public HowItWorksPage HowItWorks()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "/how-it-works");
            return new HowItWorksPage(this);
        }

        public PaymentCardsPage PaymentCards()
        {
            Driver.Navigate().GoToUrl(Config.Admin.Home + "/PaymentCards/GetList/00000000-0000-0000-0000-000000000000");
            return new PaymentCardsPage(this);
        }

        public AccountingPage Accounting()
        {
            Driver.Navigate().GoToUrl(Config.Admin.Home + "/Accounting");
            return new AccountingPage(this);
        }

        public FAQPage Faq()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "/frequently-asked-questions");
            return new FAQPage(this);
        }

        public PrepaidCardPage PrepaidCardPage()
        {
            Driver.Navigate().GoToUrl(Config.Ui.Home + "my-account/prepaid/");
            Driver.Url = Config.Ui.Home + "my-account/prepaid/";
            return new PrepaidCardPage(this);
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

        ~UiClient()
        {
            Dispose(false);
        }

        public SalesForceLoginPage SalesForceStart()
        {
            Driver.Navigate().GoToUrl(Config.Salesforce.Home);
            return new SalesForceLoginPage(this);
        }

        #region PayLater

        public PayLaterLoginPage PayLaterStart()
        {
            Driver.Navigate().GoToUrl(Config.PayLater.Home);
            return new PayLaterLoginPage(this);
        }

        public SubmitionPage PayLaterSubmition()
        {
            Driver.Navigate().GoToUrl(Config.PayLater.Home);
            return new SubmitionPage(this);
        }

        public PayLaterThanksForm PayLaterThanks()
        {
            Driver.Navigate().GoToUrl(Config.PayLater.Home);
            return new PayLaterThanksForm(this);
        }

        #endregion

        private void InitializeBrowserCapabilities()
        {
            BrowserCapabilities = new List<BrowserCapability>();
            //FF
            BrowserCapabilities.Add(new BrowserCapability {BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "3.0"});
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "3.5" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "3.6" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "4" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "5" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "6" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "7" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "8" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "9" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "10" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Firefox, Platform = PlatformType.XP, Version = "11" });

            //IE
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.InternetExplorer, Platform = PlatformType.XP, Version = "6" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.InternetExplorer, Platform = PlatformType.XP, Version = "7" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.InternetExplorer, Platform = PlatformType.XP, Version = "8" });
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.InternetExplorer, Platform = PlatformType.XP, Version = "9" });

            //Chrome
            BrowserCapabilities.Add(new BrowserCapability { BrowserType = Config.UiConfig.BrowserType.Chrome, Platform = PlatformType.Vista, Version = null });
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
