using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Ca;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.UiTests.Web.Region.Wb.Journey
{
    [Parallelizable(TestScope.All), AUT(AUT.Wb)]
    public class L0JourneyTests : UiTest
    {
        [Test, JIRA("QA-251")]
        public void WbFrontendLoadsCorrectly()
        {
            var homePage = Client.Home();
            homePage.AssertThatIsWbHomePage();
        }

        [Test, JIRA("QA-181")]
        public void L0JourneyCustomerOnCurrentAddressPageDoesNotEnterSomeRequiredFieldsWarningMessageDisplayedWb()
        {
            var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName("TESTNoCheck");
            var addressDetailsPage = journeyWb.Teleport<AddressDetailsPage>() as AddressDetailsPage;
            addressDetailsPage.PostCode = "SW6 6PN";
            addressDetailsPage.LookupByPostCode();
            addressDetailsPage.GetAddressesDropDown();
            Do.With.Message("There is no Adress field on AddresDetails Page").Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.With.Message("There is no addres period field on AddresDetails Page").Until(() => addressDetailsPage.AddressPeriod = "2 to 3 years");
            addressDetailsPage.HouseNumber = "";
            Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
            addressDetailsPage.HouseNumber = "1";
            addressDetailsPage.Street = "";
            Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
            addressDetailsPage.Street = "Harbord Street";
            addressDetailsPage.Town = "";
            Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
            addressDetailsPage.Town = "LONDON";
            addressDetailsPage.AddressPeriod = "--- Please select ---";
            Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
            addressDetailsPage.PostcodeInForm = "";
            Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
        }

        [Test, JIRA("QA-256")]
        public void EnsureCustomerCanAddGuarantorsToL0()
        {
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var firstName = Get.RandomString(3, 15);
            var lastName = Get.RandomString(3, 15);
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName("TESTNoCheck")
                .WithAddresPeriod("More than 4 years")
                .WithAdditionalDirrector()
                .WithAdditionalDirectorName(firstName)
                .WithAdditionalDirectorSurName(lastName)
                .WithAdditionalDirectorEmail(additionalDirectorEmail)
                .FillAndStop();
            var additionalDirectorsPage = journey.Teleport<AdditionalDirectorsPage>() as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            string directors = additionalDirectorsPage.GetDirectors();
            Assert.IsTrue(directors.Contains(firstName + " " + lastName));
        }

        [Test, JIRA("QA-256")]
        public void EnsureAllGurantorsReceiveTheUnsignedGuarantorEmail()
        {
            var email = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName("TESTNoCheck")
                .WithEmail(email)
                .WithAddresPeriod("More than 4 years")
                .WithAdditionalDirrector()
                .WithAdditionalDirectorEmail(additionalDirectorEmail);
            var homePage = journey.Teleport<HomePage>() as HomePage;

            var mail = Do.With.Message("There is no sought-for email in DB").Until(() => Drive.Data.QaData.Db.Emails.FindByEmailAddress(email));
            var mailTemplate = Do.With.Message("There is no sought-for email token in DB").Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail.EmailId, Key: "Html_body"));
            Assert.IsNotNull(mailTemplate);
            Assert.IsTrue(mailTemplate.value.Contains("Good news"));

            var mail2 = Do.With.Message("There is no sought-for email in DB").Until(() => Drive.Data.QaData.Db.Emails.FindByEmailAddress(additionalDirectorEmail));
            var mailTemplate2 = Do.With.Message("There is no sought-for email token in DB").Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail2.EmailId, Key: "Html_body"));
            Assert.IsNotNull(mailTemplate2);
            Assert.IsTrue(mailTemplate2.value.Contains("Good news"));
        }

        [Test, JIRA("QA-256")]
        public void EnsureWhenL0LandsOnMyAccountsThatTheProgressOfLoanIsAllThatDisplayedAndNotLoanDetails()
        {
            var email = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName("TESTNoCheck")
                .WithEmail(email)
                .WithAddresPeriod("More than 4 years")
                .WithAdditionalDirrector()
                .WithAdditionalDirectorEmail(additionalDirectorEmail);
            var homePage = journey.Teleport<HomePage>() as HomePage;
            var myPayments = Client.Payments();
            var mySummary = myPayments.Navigation.MySummaryButtonClick();
            Assert.IsTrue(mySummary.GetMyAccountStatus().Contains(ContentMap.Get.MySummaryPage.AccountStatusMessage));
        }

        [Test, JIRA("QA-287"), Category(TestCategories.SmokeTest)]
        public void WbL0JourneyShouldNotBeAbleToProceedWithoutAcceptingAllEligibilityQuestions()
        {
            int getRandomNumber = Get.RandomInt(0, 4);
            bool[] checkBox = new bool[5] { true, true, true, true, true };
            checkBox[getRandomNumber] = false;

            var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
                .WithEligibilityQuestions(checkBox[0], checkBox[1], checkBox[2], checkBox[3], checkBox[4])
                .FillAndStop();
            var eligibilityQuestionsPage = journeyWb.Teleport<EligibilityQuestionsPage>() as EligibilityQuestionsPage;

            var URLbefore = Client.Driver.Url;
            eligibilityQuestionsPage.ClickNextButton();
            Thread.Sleep(2000);
            var URLafter = Client.Driver.Url;

            Assert.AreEqual(URLbefore, URLafter);
            //Assert.IsTrue(e.Message.Contains("was Box must be ticked to proceed"));
        }

        [Test, JIRA("QA-258"), Category(TestCategories.SmokeTest)]
        public void TheWongaBusinessPolicyHaveNoReferenceToZaCaUk()
        {
            string ca = "wonga.ca";
            string za = "wonga.co.za";
            string uk = "wonga.com";
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personaltDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
            personaltDetailsPage.PrivacyPolicyClick();
            List<string> hrefs = personaltDetailsPage.GetHrefsOfLinksOnPrivacyPopup();
            foreach (var href in hrefs)
            {
                Console.WriteLine(href);
                Assert.IsFalse(href.Contains(ca));
                Assert.IsFalse(href.Contains(za));
                Assert.IsFalse(href.Contains(uk));
            }
        }
    }
}
