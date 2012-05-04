using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui.Mobile
{
    public abstract class UiMobileTest
    {
        public UiClient Client;

        [SetUp]
        public void SetUp()
        {
            Config.Ui.Browser = Config.UiConfig.BrowserType.FirefoxMobile;
            Client = new UiClient();
        }

        [TearDown]
        public void TearDown()
        {
            //reset Browser back to Firefox
            Config.Ui.Browser = Config.UiConfig.BrowserType.Firefox;

            var name = TestContext.CurrentContext.Test.Name;
            if (Client.Driver is CustomRemoteWebDriver)
                SauceRestClient.UpdateJobPassFailStatus(((CustomRemoteWebDriver)Client.Driver).GetSessionId());
            if (!Config.Ui.RemoteMode)
                TestLog.EmbedImage(name + ".Screen", Client.Screen());
            TestLog.AttachHtml(name + ".Source", Client.Source());
            Client.Dispose();
        }
    }
}
