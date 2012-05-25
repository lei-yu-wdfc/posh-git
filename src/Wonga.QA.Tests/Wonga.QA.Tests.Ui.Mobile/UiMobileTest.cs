using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    public abstract class UiMobileTest
    {
        public TestLocal<UiClient> _Client = new TestLocal<UiClient>(() => new UiClient());
        private Config.UiConfig.BrowserType? _initialBrowser;

        public UiClient Client
        {
            get { return _Client.Value; }
        }
        [SetUp]
        public void SetUp()
        {
            _initialBrowser = _initialBrowser ?? Config.Ui.Browser;
            Config.Ui.Browser = Config.UiConfig.BrowserType.FirefoxMobile;
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                //reset Browser back to Firefox
                Config.Ui.Browser = _initialBrowser.Value;

                var name = TestContext.CurrentContext.Test.Name;
                if (Client.Driver is CustomRemoteWebDriver)
                    SauceRestClient.UpdateJobPassFailStatus(((CustomRemoteWebDriver) Client.Driver).GetSessionId());
                if (!Config.Ui.RemoteMode)
                    TestLog.EmbedImage(name + ".Screen", Client.Screen());
                TestLog.AttachHtml(name + ".Source", Client.Source());
            }
            finally
            {
                Client.Dispose();   
            }
        }
    }
}
