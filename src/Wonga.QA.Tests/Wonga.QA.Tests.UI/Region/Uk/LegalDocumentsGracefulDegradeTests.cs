﻿using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Ui.Region.Uk
{
    [JIRA("UKWEB-365"), Parallelizable(TestScope.All)]
    public class LegalDocumentsGracefulDegradeTests : UiTest
    {
        #region L0 Account Details page

        private void FillInAccountFields(AccountDetailsPage accountSetupPage)
        {
            Thread.Sleep(3000);
            accountSetupPage.AccountDetailsSection.Password = "Passw0rd";
            accountSetupPage.AccountDetailsSection.PasswordConfirm = "Passw0rd";
            accountSetupPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountSetupPage.AccountDetailsSection.SecretAnswer = "Secret answer";
        }

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

        // 1. Start L0, simulate documents fetching issue, open a legal document, get error. 
        // 2. Restart L0 and successfully proceed to next page without opening a legal document.
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

            // The Personal Bank Account page should open
            try
            {
                var page = accountSetupPage.Next();
            }
            catch (Exception)
            {
                Assert.Fail("Personal Bank Account page didn't open");
                throw;
            }
        }

        // Start L0, simulate documents fetching issue, and successfully proceed to the next page without opening a legal document
        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), IgnorePageErrors, Pending("Test is complete and we are waiting for the functionality"), Owner(Owner.StanDesyatnikov)]
        public void L0ContinueWithoutOpeningLegalDocumentsApplicationShouldSucceed2()
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

            FillInAccountFields(accountSetupPage);

            // The Personal Bank Account page should open
            try
            {
                var page = accountSetupPage.Next();
            }
            catch (Exception)
            {
                Assert.Fail("Personal Bank Account page didn't open");
                throw;
            }

        }
        
        #endregion

        #region Extension Agreement page

        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), MultipleAsserts, IgnorePageErrors, Pending("Test is complete and we are waiting for the functionality"), Owner(Owner.PavithranVangiti)]
        public void ExtensionLegalDocumentErrorIsStoppedAndMessaged()
        {
            string email = Get.RandomEmail();
            const int loanAmount = 150;
            const int extensionDays = 7;
            const int loanTerm = 7;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            requestPage.SetExtendDays(extensionDays.ToString("#"));
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            //Turns off SECCI, T&C, Explanation to simulate content error
            var secciToggleLink = agreementPage.GetSecciToggleElement();
            secciToggleLink.SecciToggleButtonClick();
            Assert.AreEqual("Turn document links back on", secciToggleLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

            const string errorMessage = "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

           /* agreementPage.ClickExtensionSecciLink(); // so far no error message displayed
            Assert.Contains(agreementPage.SecciPopupWindowContent(), errorMessage);
            agreementPage.ClosePopupWindow();*/

            agreementPage.ClickExplanationLink();
            Assert.Contains(agreementPage.TermsAndConditionsContent(), errorMessage);
            agreementPage.ClosePopupWindow();

            agreementPage.ClickTermsAndConditionsLink();
            Assert.Contains(agreementPage.WrittenExplanationContent(), errorMessage);
            agreementPage.ClosePopupWindow();
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), MultipleAsserts, IgnorePageErrors, Pending("Test is complete and we are waiting for the functionality"), Owner(Owner.PavithranVangiti)]
        public void ExtensionShouldSucceedWhenContinuedWithoutOpeningLegalDocuments()
        {
            string email = Get.RandomEmail();
            const int loanAmount = 150;
            const int extensionDays = 7;
            const int loanTerm = 7;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);

            requestPage.SetExtendDays(extensionDays.ToString("#"));
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            //Turns off SECCI, T&C, Explanation to simulate content error
            var secciToggleLink = agreementPage.GetSecciToggleElement();
            secciToggleLink.SecciToggleButtonClick();
            Assert.AreEqual("Turn document links back on", secciToggleLink.GetSecciToggleButtonText(), "Document fetching is NOT switched OFF");

            const string errorMessage = "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";
            /* agreementPage.ClickExtensionSecciLink(); // so far no error message displayed
             Assert.Contains(agreementPage.SecciPopupWindowContent(), errorMessage);
             agreementPage.ClosePopupWindow();*/

            agreementPage.ClickExplanationLink();
            Assert.Contains(agreementPage.TermsAndConditionsContent(), errorMessage);
            agreementPage.ClosePopupWindow();

            agreementPage.ClickTermsAndConditionsLink();
            Assert.Contains(agreementPage.WrittenExplanationContent(), errorMessage);
            agreementPage.ClosePopupWindow();

            //Select Accept button
            agreementPage.Accept();

            //Check error message is displayed on the page - to do





        }

        #endregion

    }
}