using OpenQA.Selenium;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class BehaviourAndAdvertTracking : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("ZA-2115", "QA-301"), MultipleAsserts]
        public void L0VerifyDoubleclickTagsActiveOnL0Journey()
        {
            ////////////////////////////////////////////////////////////////
            var doubleClickTokensList = new List<KeyValuePair<string, string>>();
            var doubleClickTokensBlackList = new List<KeyValuePair<string, string>>();

            // Load the homepage:
            var page = Client.Home();

            // Look for the opening and closing DC comments:
            Assert.IsTrue(page.Client.Source().Contains("Start of DoubleClick Floodlight Tag: Please do not remove."), "Could not find 'Start of DoubleClick Floodlight Tag: Please do not remove.' - please verify DC tags are being rendered correctly.");
            Assert.IsTrue(page.Client.Source().Contains("End of DoubleClick Floodlight Tag: Please do not remove"), "Could not find 'End of DoubleClick Floodlight Tag: Please do not remove' - please verify DC tags are being rendered correctly.");

            // Check for Google Ad Services code:
            Assert.IsTrue(page.Client.Source().Contains("<!-- Google Code for Conversion Pages -->"));
            Assert.IsTrue(page.Client.Source().Contains("https://www.googleadservices.com/pagead/conversion/1017715892/?value=0&label=RSVMCNSNnQIQtLmk5QM&guid=ON&script=0"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_ho244"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "homepage"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "<front>"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "home"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "homepages/*"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "Homepage"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_Homepage"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995820"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u1: [");
            SourceDoesNotContain("u1: []");
            SourceContains("u2: [");

            // Check that the Google Analytics urchin is present on the homepage:
            Assert.IsTrue(page.Client.Source().Contains("UA-4700273-21"), "Couldn't find GA urchin ID 'UA-4700273-21' - verify that Google Analytics tag is being rendered correctly.");
            Assert.IsTrue(page.Client.Source().Contains("google-analytics.com/ga.js"), "Couldn't find 'google-analytics.com/ga.js' - verify that Google Analytics tag is being rendered correctly.");
            
            // Check that the wonga mobile tools module is doing its thing:
            Assert.IsTrue(page.Client.Source().Contains("[desktop]"), "[desktop] (device type detection from wonga_mobile module) not found in page source: verify that the wonga_mobile module is enabled.");
            Assert.IsFalse(page.Client.Source().Contains("[DeviceType]"), "[DeviceType] found in page source: verify that the wonga_mobile module is enabled.");
            Assert.IsFalse(page.Client.Source().Contains("[DeviceGroup]"), "[DeviceType] found in page source: verify that the wonga_mobile module is enabled.");

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(page.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            ////////////////////////////////////////////////////////////////
            // Application pages

            // Create a journey:
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));

            // Go to the first page:
            var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0571"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-details"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_PersonalDetails"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995827"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u2: [");
            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the second page:
            var addressDetailsPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0642"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-address"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Address"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995828"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u2: [");

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the third page:
            var accountDetailsPage = journey.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0281"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-account"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Account"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995832"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u2: [");
            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the fourth page:
            var personalBankAccountPage = journey.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0346"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-bank"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Bank"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996615"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the fifth (processing) page:
            var waitForAcceptedPage = journey.Teleport<ProcessingPage>() as ProcessingPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0882"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "processing"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Processing"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996616"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains( doubleClickTokensList);
            SourceContains("u2: [");
            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the sixth (accepted) page:
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(acceptedPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0817"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-accept"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Accept"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996618"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u2: [");
            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Complete the accept page:
            var dealDonePage = journey.Teleport<DeclinedPage>() as DealDonePage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(dealDonePage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0643"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0_s"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "deal-done"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0_Sale"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_DealDone"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996620"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u2: [");
            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the eigth (my-account) page:
            Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "my-account");

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(page.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_ln060"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_ln"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "my-account"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_Ln"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_Ln_MyAccount"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996622"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);
            SourceContains("u2: [");
            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);
        }
    }
}
