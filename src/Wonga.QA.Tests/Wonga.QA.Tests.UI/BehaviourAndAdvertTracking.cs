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
        [Test, AUT(AUT.Za)]
        public void L0VerifyDoubleclickTagInsertedInPage()
        {
            ////////////////////////////////////////////////////////////////
            // @TODO create a method that we can call to check all these details in one easy shot

            // Load the homepage:
            var page = Client.Home();

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(page.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(page.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(page.Client.Source().Contains("cat: za_ho244"));
            Assert.IsTrue(page.Client.Source().Contains("type: homepage"));
            Assert.IsTrue(page.Client.Source().Contains("u1: 1"));
            Assert.IsTrue(page.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(page.Client.Source().Contains("paths: <front>"));
            Assert.IsTrue(page.Client.Source().Contains("group_name: Homepage"));
            Assert.IsTrue(page.Client.Source().Contains("activity_name: ZA_Homepage"));
            Assert.IsTrue(page.Client.Source().Contains("activity_id: 995820"));
            Assert.IsTrue(page.Client.Source().Contains("html_override: "));

            // Check that the page contains the correct Doubleclick JS code (brittle?):
            //Assert.IsTrue(page.Client.Source().Contains("document.write('<iframe src=\"https://fls.doubleclick.net/activityi;src=3567941;cat=za_ho244;type=homepage;u1=1;u2=1;ord=' + a + '?\" width=\"1\" height=\"1\" frameborder=\"0\" style=\"display:none\"></iframe>');"));

            // Check that the page contains the correct no-JS Doubleclick code (also brittle?):
            //Assert.IsTrue(page.Client.Source().Contains("<iframe src=\"https://fls.doubleclick.net/activityi;src=3567941;cat=za_ho244;type=homepage;u1=1;u2=1;ord=1?\" width=\"1\" height=\"1\" frameborder=\"0\" style=\"display:none\"></iframe>"));

            ////////////////////////////////////////////////////////////////
            // Application pages

            // Create a journey:
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            // Go to the first page:
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("cat: za_l0571"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("type: za_l0"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("u1: ")); // Note u1 is the [ApplicationId] so will change for each app
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("paths: apply-details"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("group_name: ZA_L0"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("activity_name: ZA_L0_PersonalDetails"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("activity_id: 995827"));
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("html_override: "));

            // Go to the second page:
            var addressDetailsPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask)).CurrentPage as AddressDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("cat: za_l0642"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("type: za_l0"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("u1: "));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("paths: apply-address"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("group_name: ZA_L0"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("activity_name: ZA_L0_Address"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("activity_id: 995828"));
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("html_override: "));

            // Go to the third page:
            var accountDetailsPage = journey.FillAddressDetails().CurrentPage as AccountDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("cat: za_l0281"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("type: za_l0"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("u1: "));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("paths: apply-account"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("group_name: ZA_L0"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("activity_name: ZA_L0_Account"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("activity_id: 995832"));
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("html_override: "));

            // Go to the fourth page:
            var personalBankAccountPage = journey.FillAccountDetails().CurrentPage as PersonalBankAccountPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("cat: za_l0346"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("type: za_l0"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("u1: "));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("paths: apply-bank"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("group_name: ZA_L0"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("activity_name: ZA_L0_Bank"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("activity_id: 996615"));
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("html_override: "));

            // Go to the fifth (processing) page:
            var waitForAcceptedPage = journey.FillBankDetails().CurrentPage as ProcessingPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("cat: za_l0882"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("type: za_l0"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("u1: "));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("paths: processing"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("group_name: ZA_L0"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("activity_name: ZA_L0_Processing"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("activity_id: 996616"));
            Assert.IsTrue(waitForAcceptedPage.Client.Source().Contains("html_override: "));

            // Go to the sixth (accepted) page:
            var acceptedPage = journey.WaitForAcceptedPage().CurrentPage as AcceptedPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(acceptedPage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(acceptedPage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("cat: za_l0817"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("type: za_l0"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("u1: "));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("paths: apply-accept"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("group_name: ZA_L0"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("activity_name: ZA_L0_Accept"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("activity_id: 996618"));
            Assert.IsTrue(acceptedPage.Client.Source().Contains("html_override: "));

            // Complete the accept page:
            var dealDonePage = journey.FillAcceptedPage().CurrentPage as DealDonePage;

            // Go to the seventh (deal-done) page:
            //Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "deal-done");
            //var dealDonePage = new DealDonePage(Client);

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(dealDonePage.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(dealDonePage.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("cat: za_l0643"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("type: za_l0_s"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("u1: "));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("paths: deal-done"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("group_name: ZA_L0_Sale"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("activity_name: ZA_L0_DealDone"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("activity_id: 996620"));
            Assert.IsTrue(dealDonePage.Client.Source().Contains("html_override: "));

            // Go to the eigth (my-account) page:
            Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "my-account");

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(page.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick HTML comment identifiers for this AUT:
            Assert.IsTrue(page.Client.Source().Contains("src: 3567941"));
            Assert.IsTrue(page.Client.Source().Contains("cat: za_ln060"));
            Assert.IsTrue(page.Client.Source().Contains("type: za_ln"));
            Assert.IsTrue(page.Client.Source().Contains("u1: "));
            Assert.IsTrue(page.Client.Source().Contains("u2: 1"));
            Assert.IsTrue(page.Client.Source().Contains("paths: my-account"));
            Assert.IsTrue(page.Client.Source().Contains("group_name: ZA_Ln"));
            Assert.IsTrue(page.Client.Source().Contains("activity_name: ZA_Ln_MyAccount"));
            Assert.IsTrue(page.Client.Source().Contains("activity_id: 996622"));
            Assert.IsTrue(page.Client.Source().Contains("html_override: "));

        }
    }
}
