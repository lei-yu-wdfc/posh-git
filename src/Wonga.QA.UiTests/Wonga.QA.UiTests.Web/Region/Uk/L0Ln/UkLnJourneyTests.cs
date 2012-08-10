﻿using System;
using System.Linq;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Uk.L0Ln
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    public class UkLnJourneyTests : UiTest
    {
        [Test, JIRA("UK-1533", "UK-1902", "UKWEB-914"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Description("UI Ln Journey after L0 is created via API. Mobile phone number is updated during Ln")]
        public void LnNewMobilePhonePassed()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetLnJourney(Client.Home()).WithNewMobilePhone();
            var page = journeyLn.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, JIRA("UK-1533"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Description("UI Ln Journey after L0 is created via API")]
        public void LnWithoutChangingMobilePassed()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetLnJourney(Client.Home());
            var page = journeyLn.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, JIRA("UK-886"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void LnExistingMobilePhoneNotAccepted()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();

            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;

            var customer =
                new Customer(
                    Guid.Parse(
                        Drive.Api.Queries.Post(new GetAccountQuery { Login = email, Password = Get.GetPassword() }).Values
                            ["AccountId"].Single()));
            var application = customer.GetApplication();

            var mobileNumber = customer.GetCustomerMobileNumber();


            // Repay
            application.RepayOnDueDate();

            // Ln journey
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;
            applyPage.SetIncorrectMobilePhone = mobileNumber;

            Assert.IsTrue(applyPage.IsMobilePhonePopupCancelButtonEnabled(), "Cancel button is not enabled");
            Assert.IsTrue(applyPage.IsMobilePhonePopupSaveButtonEnabled(), "Save button is not disabled");
            Assert.IsTrue(applyPage.IsPhoneNumberNotChangedMessageVisible(),
                          "Message that mobile phone number has not changed is not dispalyed");
        }

        [Test, MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnUrlsAreCorrect()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();

            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();

            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();

            var mySummaryPageAfterLogin = loginPage.LoginAs(email);
            var homePage = Client.Home();
            var journey = JourneyFactory.GetLnJourney(homePage);
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;

            // Check the URL contains /applyln
            Assert.Contains(Client.Driver.Url, "/applyln", "The apply page URL does not contain '/applyln'");
            var processingPage = journey.Teleport<ProcessingPage>() as ProcessingPage;

            // Check the URL ends with /processing-page
            Assert.EndsWith(Client.Driver.Url, "/processing-page", "The processing page URL is not /processing-page.");
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

            // Check the URL ends with /accept
            Assert.EndsWith(Client.Driver.Url, "/accept", "The accept page URL is not /accept.");
            var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;

            // Check the URL ends with /dealdoneLN
            Assert.EndsWith(Client.Driver.Url, "/dealdoneLN", "The deal done page URL is not /dealdoneLN.");
        }

        [Test, AUT(AUT.Uk), JIRA("QA-335")]
        public void LnCustomerChangesMobilePhoneAndEntersInvalidPinShouldNotBeAbleToTakeLoan()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;

            applyPage.SetNewMobilePhone = Get.GetMobilePhone();
            applyPage.ApplicationSection.SetPin = "1111";
            try
            {
                applyPage.Submit();
            }
            catch (AssertionFailureException exception)
            {
                Assert.IsTrue(exception.Message.Contains(ContentMap.Get.ApplyPage.PinErrorMessage));
            }
        }

        [Test, AUT(AUT.Uk), JIRA("QA-347"), Pending("Functional is not ready")]
        public void LnCustomerWithoutLiveLoanPassLnWithNewCard()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPayments = mySummaryPage.Navigation.MyPaymentDetailsButtonClick();
            myPayments.AddVisaElectronCard("4222 2222 2222 2222");
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.Teleport<MySummaryPage>() as MySummaryPage;

        }

        [Test, AUT(AUT.Uk), JIRA("QA-344"), Owner(Owner.PetrTarasenko)]
        public void LnSecretCodeValidation()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;
            applyPage.SetSecureCode = "abc";
            Assert.IsTrue(applyPage.IsSecurecodeWarningOccurred());
            applyPage.SetSecureCode = "12a";
            Assert.IsTrue(applyPage.IsSecurecodeWarningOccurred());
            applyPage.SetSecureCode = "";
            Assert.IsTrue(applyPage.IsSecurecodeWarningOccurred());

        }

        [Test, AUT(AUT.Uk), JIRA("QA-345"), Owner(Owner.PetrTarasenko)]
        public void LnCashFieldValidation()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;
            applyPage.ApplicationSection.SetMinCash = "ab";
            Assert.IsTrue(applyPage.IsMinCashWarningOccurred());
            applyPage.ApplicationSection.SetMinCash = "@#";
            Assert.IsTrue(applyPage.IsMinCashWarningOccurred());
            applyPage.ApplicationSection.SetMinCash = "";
            Assert.IsTrue(applyPage.IsMinCashWarningOccurred());

        }

    }
}
