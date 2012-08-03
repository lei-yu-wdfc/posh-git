using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssessmentAboutYouTest : UiTest
    {
        public Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            return customer;
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void PrepopulatedAgreementReferenceAndEmailCheck()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.Teleport<FAAboutYouPage>() as FAAboutYouPage;

            var payments = Drive.Data.Payments.Db;
            var appId = customer.GetApplication().Id;
            var data = payments.Applications.FindByExternalId(appId).ApplicationReference;

            Assert.AreEqual(customer.Email, faaboutyoupage.GetPrepopulatedEmailAddress(), "Prepopulated Email check");
            Assert.AreEqual(data, faaboutyoupage.GetPrepopulatedAgreementReference(), "Prepopulated AgreementReference check");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithValidDataAndCheckDataAfterGoBack()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.FillAndStop().WithEmployer(Get.GetEmployerName()).Teleport<FAAboutYouPage>() as FAAboutYouPage;

            var firstName = faaboutyoupage.FirstName;
            var lastName = faaboutyoupage.LastName;
            var dateOfBirth = faaboutyoupage.DateOfBirth;
            var houseNumber = faaboutyoupage.HouseNumber;
            var postCode = faaboutyoupage.PostCode;
            var employer = faaboutyoupage.Employer;
            var yourHousehold = faaboutyoupage.AdultsInHousehold;
            var childrenInHousehold = faaboutyoupage.ChidrenInHousehold;
            var numberOfVehiles = faaboutyoupage.NumberOfVehiles;

            var faincomepage = faaboutyoupage.NextClick() as FAIncomePage;
            Assert.Contains(faincomepage.Url, "/income", "Pass to income page, with right data in fields");

            faaboutyoupage = faincomepage.PreviousClick() as FAAboutYouPage;

            Assert.AreEqual(faaboutyoupage.FirstName, firstName, "Firstname check after go back");
            Assert.AreEqual(faaboutyoupage.LastName, lastName, "Lastname check after go back");
            Assert.AreEqual(faaboutyoupage.DateOfBirth, dateOfBirth, "DOB check after go back");
            Assert.AreEqual(faaboutyoupage.HouseNumber, houseNumber, "HouseNumber check after go back");
            Assert.AreEqual(faaboutyoupage.PostCode, postCode, "PostCode check after go back");
            Assert.AreEqual(faaboutyoupage.Employer, employer, "Employer check after go back");
            Assert.AreEqual(faaboutyoupage.AdultsInHousehold, yourHousehold, "Adults in Household check after go back");
            Assert.AreEqual(faaboutyoupage.ChidrenInHousehold, childrenInHousehold, "Children in Household check after go back");
            Assert.AreEqual(faaboutyoupage.NumberOfVehiles, numberOfVehiles, "Vehiles number check after go back");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void InvalidPostcodeWarningMessageCheck()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.FillAndStop().Teleport<FAAboutYouPage>() as FAAboutYouPage;

            faaboutyoupage.PostCode = Get.GetName();
            faaboutyoupage.NextClick(error: true);
            Assert.IsTrue(faaboutyoupage.PostCodeErrorPresent());
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithInvalidDataFields()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);
            BasePage basepage;

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.FillAndStop().Teleport<FAAboutYouPage>() as FAAboutYouPage;

            var fieldmem = faaboutyoupage.FirstName;
            faaboutyoupage.FirstName = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free ForeName field");
            if (faaboutyoupage.Url.Contains("/income")) { faaboutyoupage = (basepage as FAIncomePage).PreviousClick() as FAAboutYouPage; }
            faaboutyoupage.FirstName = fieldmem;

            fieldmem = faaboutyoupage.LastName;
            faaboutyoupage.LastName = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free LastName field");
            if (faaboutyoupage.Url.Contains("/income")) { faaboutyoupage = (basepage as FAIncomePage).PreviousClick() as FAAboutYouPage; }
            faaboutyoupage.LastName = fieldmem;

            fieldmem = faaboutyoupage.HouseNumber;
            faaboutyoupage.HouseNumber = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free HouseNumber field");
            if (faaboutyoupage.Url.Contains("/income")) { faaboutyoupage = (basepage as FAIncomePage).PreviousClick() as FAAboutYouPage; }
            faaboutyoupage.HouseNumber = fieldmem;

            fieldmem = faaboutyoupage.PostCode;
            faaboutyoupage.PostCode = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free PostCode field");
            if (faaboutyoupage.Url.Contains("/income")) { faaboutyoupage = (basepage as FAIncomePage).PreviousClick() as FAAboutYouPage; }
            faaboutyoupage.PostCode = fieldmem;

            fieldmem = faaboutyoupage.AdultsInHousehold;
            faaboutyoupage.AdultsInHousehold = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free AdultsInHousehold field");
            if (faaboutyoupage.Url.Contains("/income")) { faaboutyoupage = (basepage as FAIncomePage).PreviousClick() as FAAboutYouPage; }
            faaboutyoupage.AdultsInHousehold = fieldmem;

            fieldmem = faaboutyoupage.ChidrenInHousehold;
            faaboutyoupage.ChidrenInHousehold = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free ChidrenInHousehold field");
            if (faaboutyoupage.Url.Contains("/income")) { faaboutyoupage = (basepage as FAIncomePage).PreviousClick() as FAAboutYouPage; }
            faaboutyoupage.ChidrenInHousehold = fieldmem;

            fieldmem = faaboutyoupage.NumberOfVehiles;
            faaboutyoupage.NumberOfVehiles = "";
            try { basepage = faaboutyoupage.NextClick(error: true); } catch { basepage = new FAIncomePage(Client); }
            Assert.Contains(faaboutyoupage.Url, "/about-you", "Pass to income page, with free NumberOfVehiles field");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithInvalidDobDayField()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.Teleport<FAAboutYouPage>() as FAAboutYouPage;

            faaboutyoupage.FirstName = Get.GetName();
            faaboutyoupage.LastName = Get.RandomString(10);
            faaboutyoupage.HouseNumber = Get.RandomString(10);
            faaboutyoupage.DateOfBirth = "1977-10-00";
            faaboutyoupage.PostCode = Get.GetPostcode();
            faaboutyoupage.AdultsInHousehold = Get.RandomInt(1, 5).ToString();
            faaboutyoupage.ChidrenInHousehold = Get.RandomInt(1, 5).ToString();
            faaboutyoupage.NumberOfVehiles = Get.RandomInt(1, 5).ToString();

            try { faaboutyoupage.NextClick(error: true); } catch {}
            Assert.Contains(faaboutyoupage.Url, "/about-you");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithInvalidDobMonthField()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.Teleport<FAAboutYouPage>() as FAAboutYouPage;

            faaboutyoupage.FirstName = Get.GetName();
            faaboutyoupage.LastName = Get.RandomString(10);
            faaboutyoupage.HouseNumber = Get.RandomString(10);
            faaboutyoupage.DateOfBirth = "1977-00-21";
            faaboutyoupage.PostCode = Get.GetPostcode();
            faaboutyoupage.AdultsInHousehold = Get.RandomInt(1, 5).ToString();
            faaboutyoupage.ChidrenInHousehold = Get.RandomInt(1, 5).ToString();
            faaboutyoupage.NumberOfVehiles = Get.RandomInt(1, 5).ToString();

            try { faaboutyoupage.NextClick(error: true); } catch { }
            Assert.Contains(faaboutyoupage.Url, "/about-you");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithInvalidDobYearField()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.Teleport<FAAboutYouPage>() as FAAboutYouPage;

            faaboutyoupage.FirstName = Get.GetName();
            faaboutyoupage.LastName = Get.RandomString(10);
            faaboutyoupage.HouseNumber = Get.RandomString(10);
            faaboutyoupage.DateOfBirth = "00-10-21";
            faaboutyoupage.PostCode = Get.GetPostcode();
            faaboutyoupage.AdultsInHousehold = Get.RandomInt(1, 5).ToString();
            faaboutyoupage.ChidrenInHousehold = Get.RandomInt(1, 5).ToString();
            faaboutyoupage.NumberOfVehiles = Get.RandomInt(1, 5).ToString();

            try { faaboutyoupage.NextClick(error: true); } catch { }
            Assert.Contains(faaboutyoupage.Url, "/about-you");
        }
    }
}
