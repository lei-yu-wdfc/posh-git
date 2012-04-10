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
    class BehaviourAndAdvertTracking : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("ZA-2115")]
        public void L0VerifyDoubleclickTagInsertedInPage()
        {
            ////////////////////////////////////////////////////////////////
            var doubleClickTokensList = new List<KeyValuePair<string, string>>();
            var doubleClickTokensBlackList = new List<KeyValuePair<string, string>>();

            // Load the homepage:
            var page = Client.Home();

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_ho244"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "homepage"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u1", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "<front>"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "home"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "homepages/*"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "Homepage"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_Homepage"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995820"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(page.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            ////////////////////////////////////////////////////////////////
            // Application pages

            // Create a journey:
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            // Go to the first page:
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0571"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-details"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_PersonalDetails"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995827"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the second page:
            var addressDetailsPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask)).CurrentPage as AddressDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0642"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-address"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Address"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995828"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the third page:
            var accountDetailsPage = journey.FillAddressDetails().CurrentPage as AccountDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0281"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-account"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Account"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "995832"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the fourth page:
            var personalBankAccountPage = journey.FillAccountDetails().CurrentPage as PersonalBankAccountPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0346"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
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
            var waitForAcceptedPage = journey.FillBankDetails().CurrentPage as ProcessingPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0882"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "processing"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Processing"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996616"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains( doubleClickTokensList);

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Go to the sixth (accepted) page:
            var acceptedPage = journey.WaitForAcceptedPage().CurrentPage as AcceptedPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(acceptedPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0817"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "apply-accept"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_Accept"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996618"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);

            // Complete the accept page:
            var dealDonePage = journey.FillAcceptedPage().CurrentPage as DealDonePage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(dealDonePage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            doubleClickTokensList.Clear();
            doubleClickTokensList.Add(new KeyValuePair<string, string>("src", "3567941"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("cat", "za_l0643"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("type", "za_l0_s"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "deal-done"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_L0_Sale"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_L0_DealDone"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996620"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

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
            doubleClickTokensList.Add(new KeyValuePair<string, string>("u2", "1"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("paths", "my-account"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("group_name", "ZA_Ln"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_name", "ZA_Ln_MyAccount"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("activity_id", "996622"));
            doubleClickTokensList.Add(new KeyValuePair<string, string>("html_override", ""));
            SourceContains(doubleClickTokensList);

            // Check that we don't have an empty u1 value (e.g. "u1: []"):
            doubleClickTokensBlackList.Clear();
            doubleClickTokensBlackList.Add(new KeyValuePair<string, string>("u1", ""));
            SourceDoesNotContain(doubleClickTokensBlackList);
        }
    }
}
