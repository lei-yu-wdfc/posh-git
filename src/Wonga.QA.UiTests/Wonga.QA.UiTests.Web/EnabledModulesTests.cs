using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class EnabledModulesTests : UiTest
    {
        // This file contains tests which check the page source to
        // verify that certain modules are enabled, e.g. the Wonga
        // Click module will add a JS tag which has a specific string
        // present on all URLs; the L0 ZA module adds a certain type
        // of insert into L0 journey pages, and so on.

        [Test, AUT(AUT.Za), JIRA("ZA-2604")]
        public void VerifyWongaClickEnabled()
        {
            Client.Home();
            Assert.IsTrue(Client.Driver.PageSource.Contains("wonga_click"), "Could not find 'wonga_click' in the page source code. Please verify the Wonga Click module is enabled.");
        }

        [Test, AUT(AUT.Za)]
        public void VerifyGoogleAnalyticsTagInserted()
        {
            Client.Home();
            Assert.IsTrue(Client.Driver.PageSource.Contains(".google-analytics.com/ga.js"), "Could not find '.google-analytics.com/ga.js' in the page source code. Please verify the Google Analytics tag is being inserted correctly.");
        }

        [Test, AUT(AUT.Za)]
        public void VerifyWongaDoubleClickEnabled()
        {
            Client.Home();
            Assert.IsTrue(Client.Driver.PageSource.Contains("Start of DoubleClick Floodlight Tag: Please do not remove."), "Could not find 'Start of DoubleClick Floodlight Tag: Please do not remove.' in the page source code. Please verify the Wonga DoubleClick module is enabled.");
        }

        [Test, AUT(AUT.Za)]
        public void VerifyWongaXsdFapiEnabled()
        {
            Client.Home();
            Assert.IsTrue(Client.Driver.PageSource.Contains("XSD FAPI"), "Could not find 'XSD FAPI' in the page source code. Please verify the Wonga XSD FAPI module is enabled.");
        }

        [Test, AUT(AUT.Za)]
        public void VerifyGooglePageAdServicesTagInsertedInPage()
        {
            Client.Home();
            Assert.IsTrue(Client.Driver.PageSource.Contains("https://www.googleadservices.com/pagead/conversion.js"), "Could not find 'https://www.googleadservices.com/pagead/conversion.js' in the page source code. Please verify the Google Ad Services tracking code is inserted in the page.");
        }
    }
}
