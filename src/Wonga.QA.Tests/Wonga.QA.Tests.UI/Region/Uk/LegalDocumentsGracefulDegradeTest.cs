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
        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), IgnorePageErrors, Pending("Development")]
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

            Thread.Sleep(3000);

            Assert.AreEqual("Turn document links back on", secciLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

            Assert.Multiple(() =>
            {
                const String errorMessage =
                    "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

                //Check Broken Secci
                accountSetupPage.ClickSecciLink();
                Assert.Contains(accountSetupPage.SecciPopupWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();

                accountSetupPage = accountSetupPage.NextClick();
                Assert.AreEqual(errorMessage, accountSetupPage.GetErrorText());

                // TBD: click TermsAndConditions
                // Check an error is dispalyed
                //Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
                accountSetupPage.ClickTermsAndConditionsLink();
                Thread.Sleep(3000);
                Assert.Contains(accountSetupPage.TermsAndConditionsPopUpWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();
                Thread.Sleep(1000);

                FillInAccountFields(accountSetupPage);

                accountSetupPage = accountSetupPage.NextClick();
                // TBD: Check an error appeared in the top

                // TBD: click Explanation
                // Check an error is dispalyed
                //Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
                accountSetupPage.ClickWrittenExplanationLink();
                Thread.Sleep(3000);
                Assert.Contains(accountSetupPage.WrittenExplanationPopUpWindowContent(), errorMessage);
                accountSetupPage.ClosePopupWindow();
                Thread.Sleep(1000);

                FillInAccountFields(accountSetupPage);

                accountSetupPage = accountSetupPage.NextClick();
                // TBD: Check an error appeared in the top

                //Turns on SECCI, T&C, Explanation to simulate content error
                secciLink = accountSetupPage.GetSecciToggleElement();
                secciLink.SecciToggleButtonClick();
                Thread.Sleep(3000);
                Assert.AreEqual("Turn document links off", secciLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

                FillInAccountFields(accountSetupPage);

                var page = accountSetupPage.NextClick();
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
    }
}