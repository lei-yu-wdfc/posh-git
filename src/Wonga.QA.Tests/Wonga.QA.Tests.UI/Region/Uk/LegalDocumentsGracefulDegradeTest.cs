using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Ui.Region.Uk
{
    [Parallelizable(TestScope.All)]
    public class LegalDocumentsGracefulDegradeTest : UiTest
    {

        /* Check L0 account setup link toggle is clicked and SECCI, T&C or explanation links are broken 
         * and a prescribed error message is displayed and that progression in the application is stopped
         * Returning to the start of a journey will clear errors and alow progression
         */
        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), IgnorePageErrors, Owner(Owner.OrizuNwokeji)]
        public void L0LegalDocumentErrorIsStoppedAndMessaged()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithEmail(email);

            var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            FillInAccountFields(accountSetupPage);

            //Turns off SECCI, T&C, Explanation to simulate content error
            var secciLink = accountSetupPage.GetSecciToggleElement();
            secciLink.SecciToggleButtonClick();

            Assert.AreEqual("Turn document links back on", secciLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

            Assert.Multiple(() =>
            {
                const String errorMessage =
                    "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

                //Open Secci pop-up page and check correct error message is displayed
                accountSetupPage.ClickSecciLink();
                Assert.Contains(accountSetupPage.SecciPopupWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();
                Thread.Sleep(2000);

                accountSetupPage = accountSetupPage.NextClick();
                Assert.AreEqual(errorMessage, accountSetupPage.GetErrorText());

                //Open TermsAndConditions pop-up and check correct error message is displayed
                accountSetupPage.ClickTermsAndConditionsLink();
                Assert.Contains(accountSetupPage.TermsAndConditionsPopUpWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();
                Thread.Sleep(2000);

                FillInAccountFields(accountSetupPage);

                accountSetupPage = accountSetupPage.NextClick();
                Assert.AreEqual(errorMessage, accountSetupPage.GetErrorText());

                // Open WrittenExplanation pop-up and check the correct error message is displayed
                accountSetupPage.ClickWrittenExplanationLink();
                Assert.Contains(accountSetupPage.WrittenExplanationPopUpWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();
                Thread.Sleep(2000);

                FillInAccountFields(accountSetupPage);

                accountSetupPage = accountSetupPage.NextClick();
                Assert.AreEqual(errorMessage, accountSetupPage.GetErrorText());

                //Turn on SECCI, T&C, Explanation and check Bank account details page is displayed correctly
                secciLink = accountSetupPage.GetSecciToggleElement();
                secciLink.SecciToggleButtonClick();
                Assert.AreEqual("Turn document links off", secciLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

                FillInAccountFields(accountSetupPage);

                var page = accountSetupPage.Next();
            });
        }

        private void FillInAccountFields(AccountDetailsPage accountSetupPage)
        {
            Thread.Sleep(3000);
            accountSetupPage.AccountDetailsSection.Password = "Passw0rd";
            accountSetupPage.AccountDetailsSection.PasswordConfirm = "Passw0rd";
            accountSetupPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountSetupPage.AccountDetailsSection.SecretAnswer = "Secret answer";
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), IgnorePageErrors, Pending("Test is complete and we are waiting for the functionality"), Owner(Owner.PavithranVangiti)]
        public void L0ContinueWithoutOpeningLegalDocumentsApplicationShouldSucceed()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithEmail(email);

            var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            FillInAccountFields(accountSetupPage);

            //Turns off SECCI, T&C, Explanation to simulate content error
            var secciLink = accountSetupPage.GetSecciToggleElement();
            secciLink.SecciToggleButtonClick();

            Assert.AreEqual("Turn document links back on", secciLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

            Assert.Multiple(() =>
            {
                const String errorMessage =
                    "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

                //Open Secci pop-up page and check correct error message is displayed
                accountSetupPage.ClickSecciLink();
                Assert.Contains(accountSetupPage.SecciPopupWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();
                Thread.Sleep(2000);

                accountSetupPage = accountSetupPage.NextClick();
                Assert.AreEqual(errorMessage, accountSetupPage.GetErrorText());
            });

                journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithEmail(email);

                accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

                FillInAccountFields(accountSetupPage);
                var page = accountSetupPage.Next();
        }
    }
}