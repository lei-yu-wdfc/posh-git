using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Uk
{

    /* Check L0 account setup link toggle is clicked and SECCI, T&C or explanation links are broken 
     * and a prescribed error message is displayed and that progression in the application is stopped
     * Returning to the start of a journey will clear errors and allow progression*/
    [JIRA("UKWEB-365"), Parallelizable(TestScope.All), AUT(AUT.Uk)]
    public class LegalDocumentsTests : UiTest
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

        // 1. Start L0, simulate documents fetching issue, open a legal document, get error.
        // 2. Ensure user cannot proceed to Bank Details page
        [Test, JIRA("UKWEB-365", "UKWEB-1005"), IgnorePageErrors, Owner(Owner.OrizuNwokeji, Owner.PavithranVangiti), Pending("UKWEB-1005: With document links turned off, clicking Next button in Account Details page should show the error message")]
        public void L0AccountDetails_DocumentsLinkIsTurnedOff_DisplayErrorMessage()
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
            });
        }

        // 1. Start L0, simulate documents fetching issue, open a legal document, get error. 
        // 2. Restart L0 and successfully proceed to Bank Details page without opening a legal document.
        [Test, JIRA("UKWEB-365", "UKWEB-1005"), IgnorePageErrors, Pending("UKWEB-1005: With document links turned off, clicking Next button in Account Details page should show the error message"), Owner(Owner.PavithranVangiti)]
        public void L0AccountDetailsRestartL0AndContinueWithoutOpeningLegalDocumentsL0ShouldSucceed()
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

        /* Start L0, simulate documents fetching issue, and successfully proceed to the Bank Details page 
        without opening a legal document */
        [Test, JIRA("UKWEB-365"), IgnorePageErrors, Owner(Owner.StanDesyatnikov)]
        public void L0AccountDetailsContinueWithoutOpeningLegalDocumentsL0ShouldSucceed()
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

        #region L0 Accept page

        // 1. Start L0, simulate documents fetching issue in Accept page, open a legal document, get error.
        // 2. Ensure user cannot proceed to Deal Done page
        [Test, JIRA("UKWEB-365"), IgnorePageErrors, Owner(Owner.StanDesyatnikov)]
        public void L0AcceptPageLegalDocumentsErrorTest()
        {
            const String errorMessage = "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home()).WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var acceptPage = journeyL0.Teleport<AcceptedPage>() as AcceptedPage;

            // Turns Off SECCI, T&C, Explanation to simulate content error
            var secciLink = acceptPage.GetSecciToggleElement();
            secciLink.SecciToggleButtonClick();

            Do.With.Message("Document fetching is NOT switched OFF").Until(() => secciLink.GetSecciToggleButtonText().Contains("Turn document links back on"));

            Assert.Multiple(() =>
            {
                //Open Secci pop-up page and check correct error message is displayed
                acceptPage.ClickSecciLink();
                Assert.Contains(acceptPage.SecciPopupWindowContent(), errorMessage);
                acceptPage.ClosePopupWindow();
                Thread.Sleep(2000);

                //Click on 'I Accept' button and ensure you stay on the same page
                acceptPage = acceptPage.ClickAcceptGetError();
                Assert.AreEqual(errorMessage, acceptPage.GetErrorText());

                //Open Written Explanation pop-up page and check correct error message is displayed
                acceptPage.ClickWrittenExplanationLink();
                Assert.Contains(acceptPage.WrittenExplanationContent(), errorMessage);
                acceptPage.ClosePopupWindow();
                Thread.Sleep(2000);

                //Click on 'I Accept' button and ensure you stay on the same page
                acceptPage = acceptPage.ClickAcceptGetError();
                Assert.AreEqual(errorMessage, acceptPage.GetErrorText());
            });
        }

        /* Start L0, simulate documents fetching issue in Accept page, and successfully proceed to the
        Deal Done without opening a legal document*/
        [Test, JIRA("UKWEB-365"), IgnorePageErrors, Owner(Owner.PavithranVangiti)]
        public void L0AcceptPageContinueWithoutOpeningLegalDocumentsL0ShouldSucceed()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home()).WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var acceptPage = journeyL0.Teleport<AcceptedPage>() as AcceptedPage;

            // Turns Off SECCI, T&C, Explanation to simulate content error
            var secciLink = acceptPage.GetSecciToggleElement();
            secciLink.SecciToggleButtonClick();

            Do.With.Message("Document fetching is NOT switched OFF").Until(() => secciLink.GetSecciToggleButtonText().Contains("Turn document links back on"));

            //Click 'I Accept' button
            acceptPage.Submit();

            // The Deal done page should open
            try
            {
                var dealDonePage = new DealDonePage(this.Client);
            }
            catch (Exception)
            {
                Assert.Fail("Deal done page didn't open");
                throw;
            }
        }

        #endregion

        #region Extension Agreement page

        // 1. Start Extension, simulate documents fetching issue in Extension Agreement page, open a legal document, get error.
        // 2. Ensure user cannot proceed to Deal Done page
        [Test, JIRA("UKWEB-365"), IgnorePageErrors, Owner(Owner.PavithranVangiti)]
        public void ExtensionAgreementPageLegalDocumentsErrorTest()
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

            Do.With.Message("Document fetching is NOT switched OFF").Until(() => secciToggleLink.GetSecciToggleButtonText().Contains("Turn document links back on"));

            const string errorMessage = "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

            // Check popups
            agreementPage.ClickExtensionSecciLink();
            Assert.Contains(agreementPage.SecciPopupWindowContent(), errorMessage);
            agreementPage.ClosePopupWindow();
            Thread.Sleep(2000);

            agreementPage = agreementPage.ClickAcceptGetError();
            Assert.AreEqual(errorMessage, agreementPage.GetErrorText());

            agreementPage.ClickTermsAndConditionsLink();
            Thread.Sleep(4000);
            Assert.Contains(agreementPage.TermsAndConditionsContent(), errorMessage);
            agreementPage.ClosePopupWindow();
            Thread.Sleep(2000);

            agreementPage = agreementPage.ClickAcceptGetError();
            Assert.AreEqual(errorMessage, agreementPage.GetErrorText());

            agreementPage.ClickExplanationLink();
            Thread.Sleep(4000);
            Assert.Contains(agreementPage.WrittenExplanationContent(), errorMessage);
            agreementPage.ClosePopupWindow();
            Thread.Sleep(2000);

            agreementPage = agreementPage.ClickAcceptGetError();
            Assert.AreEqual(errorMessage, agreementPage.GetErrorText());
        }

        // 1. Start Extension, simulate documents fetching issue in Extension Agreement page, open a legal document, get error. 
        // 2. Restart Extension and successfully proceed to Deal Done page without opening a legal document.
        [Test, JIRA("UKWEB-365"), MultipleAsserts, IgnorePageErrors, Owner(Owner.PavithranVangiti)]
        public void ExtensionRestartExtensionAndContinueWithoutOpeningLegalDocumentsShouldSucceed()
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

            Do.With.Message("Document fetching is NOT switched OFF").Until(() => secciToggleLink.GetSecciToggleButtonText().Contains("Turn document links back on"));

            const string errorMessage = "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109.";

            //Check pop-ups
            agreementPage.ClickExtensionSecciLink();
            Assert.Contains(agreementPage.SecciPopupWindowContent(), errorMessage);
            agreementPage.ClosePopupWindow();
            Thread.Sleep(2000);

            agreementPage = agreementPage.ClickAcceptGetError();
            Assert.AreEqual(errorMessage, agreementPage.GetErrorText());

            agreementPage.ClickTermsAndConditionsLink();
            Thread.Sleep(2000);
            Assert.Contains(agreementPage.TermsAndConditionsContent(), errorMessage);
            agreementPage.ClosePopupWindow();
            Thread.Sleep(2000);

            agreementPage = agreementPage.ClickAcceptGetError();
            Assert.AreEqual(errorMessage, agreementPage.GetErrorText());

            agreementPage.ClickExplanationLink();
            Thread.Sleep(2000);
            Assert.Contains(agreementPage.WrittenExplanationContent(), errorMessage);
            agreementPage.ClosePopupWindow();
            Thread.Sleep(2000);

            agreementPage = agreementPage.ClickAcceptGetError();
            Assert.AreEqual(errorMessage, agreementPage.GetErrorText());

            //Go to My Summary page and continue with extension process
            Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "/my-account");
            mySummaryPage = new MySummaryPage(this.Client);
            mySummaryPage.ChangePromiseDateButtonClick();
            requestPage = new ExtensionRequestPage(this.Client);

            requestPage.SetExtendDays(extensionDays.ToString("#"));
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            //Select Accept button with turning off the document links and ensure extension successful page is displayed
            agreementPage.Accept();

            try
            {
                var dealDonePage = new ExtensionDealDonePage(this.Client);
                Assert.IsFalse(dealDonePage.IsDealDonePageExtensionAmountNotPresent());
                Assert.IsFalse(dealDonePage.IsDealDonePageDateTokenPresent());
            }
            catch(Exception)
            {
                Assert.Fail("Deal done page didn't open");
                throw;
            }
        }

        /* Start Extension, simulate documents fetching issue in Extension Agreement page, and successfully proceed to the
        Deal Done without opening a legal document*/
        [Test, JIRA("UKWEB-365"), MultipleAsserts, IgnorePageErrors, Owner(Owner.PavithranVangiti)]
        public void ExtensionContinueWithoutOpeningLegalDocumentsShouldSucceed()
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

            Do.With.Message("Document fetching is NOT switched OFF").Until(() => secciToggleLink.GetSecciToggleButtonText().Contains("Turn document links back on"));

            //Select Accept button with turning off the document links and ensure extension successful page is displayed
            agreementPage.Accept();

            try
            {
                var dealDonePage = new ExtensionDealDonePage(this.Client);
                Assert.IsFalse(dealDonePage.IsDealDonePageExtensionAmountNotPresent());
                Assert.IsFalse(dealDonePage.IsDealDonePageDateTokenPresent());
            }
            catch (Exception)
            {
                Assert.Fail("Deal done page didn't open");
                throw;
            }
        }

        #endregion

    }
}