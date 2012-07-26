using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.UiTests.Web.Facebook
{
    [Parallelizable(TestScope.All)]
    class FacebookL0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Uk), Pending("Waiting on Facebook Environment Setup - AUT.UkFb")]
        public void FBL0Journey()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var mySummary = journey.Teleport<DealDonePage>() as DealDonePage;
        }

        [Test, AUT(AUT.Uk), JIRA("UK-969", "UKWEB-250"), MultipleAsserts, Pending("Waiting on Facebook Environment Setup - AUT.UkFb")]
        public void FBL0PreAgreementPartonAccountSetupPageTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
           var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
            Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
            Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

            //string baseWindowHdl = Client.Driver.CurrentWindowHandle;

            //Check SECCI popup window
            accountSetupPage.ClickSecciLink();
            // TBD: check header and values and close the pop-up
            Assert.Contains(accountSetupPage.SecciPopupWindowContent(), "150");
            // end of TBD: check header and values and close the pop-up
            accountSetupPage.ClosePopupWindow();

            Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), ContentMap.Get.AccountSetupPage.LoanConditionText);
            accountSetupPage.ClosePopupWindow();

            Assert.Contains(accountSetupPage.GetExplanationTitle(), ContentMap.Get.AccountSetupPage.ImportantInformationText);
            accountSetupPage.ClosePopupWindow();

            // Manually check that loan agreement and SECCI emails are sent
            Console.WriteLine("Manually check that that loan agreement and SECCI emails are sent for user={0}", email);
        }

    }
}
