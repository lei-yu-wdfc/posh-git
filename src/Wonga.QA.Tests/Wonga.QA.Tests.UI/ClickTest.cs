using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    [TestFixture, Parallelizable(TestScope.All)]
    class ClickTest : UiTest
    {

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("CA-2326"), SmokeTest]
        public void VerifyIsClickCodeLoaded()
        {
            var homePage = Client.Home();

            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"http.*click.*.js", RegexOptions.IgnoreCase));

        }

        [Test, AUT(AUT.Ca, AUT.Za), SmokeTest]
        public void VerifyClickExternalJsFileUrlIsPresentInScriptTag()
        {
            var homePage = Client.Home();

            // Set the JS file URL without the protocol, and initialise the protocol var:
            string wongaClickJsFileUrlWithoutProtocol = "click.wonga.com/scripts/click_min.js";

            // @TODO: get the protocol programmatically:
            string protolol = "";

            // Get the right (assumed) protocol:
            switch (Config.SUT)
            {
                case SUT.Live:
                    {
                        // Set the protocol to HTTPS:
                        protolol = "https";
                    }
                    break;

                default:
                    {
                        // Set the protocol to HTTP:
                        protolol = "http";

                    }
                    break;
            }

            var fullClickUrl = protolol + "://" + wongaClickJsFileUrlWithoutProtocol;
            Assert.IsTrue(homePage.Source.Contains(fullClickUrl), "Unable to find mention of the Wonga Click JS file's URL " + fullClickUrl + ". Please verify it is on the page. If the URL of the file has changed, please update this test.");
        }

        public void VerifyIsClickParamsSet()
        {
            var homePage = Client.Home();
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['tids'", RegexOptions.IgnoreCase));
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['region'", RegexOptions.IgnoreCase));
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['devicetype'", RegexOptions.IgnoreCase));
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['devicegroup'", RegexOptions.IgnoreCase));

        }

    }
}