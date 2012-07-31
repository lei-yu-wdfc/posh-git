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
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture]
    public class FinancialAssessmentAboutYouTest : UiTest
    {
        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), MultipleAsserts, Pending("Financial Assessment")]
        public void PrepopulatedAgreementReferenceAndEmailCheck()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.Teleport<FAAboutYouPage>() as FAAboutYouPage;

            Assert.AreEqual(customer.Email, faaboutyoupage.GetPrepopulatedEmailAddress(), "Prepopulated Email check");
            Assert.AreEqual(/*customer.GetCcin()/*var t = */customer.GetApplication().Id.ToString(), faaboutyoupage.GetPrepopulatedAgreementReference(), "Prepopulated AgreementReference check");
        }

        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), MultipleAsserts, Pending("Financial Assessment")]
        public void NextClickWithValidDataAndCheckDataAfterGoBack()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.FillAndStop().WithEmployer(Get.GetName()).Teleport<FAIncomePage>() as FAAboutYouPage;

            var firstName = faaboutyoupage.FirstName;
            var lastName = faaboutyoupage.LastName;
            //var dateOfBirth = new DateTime(1977, 10, 11);
            var houseNumber = faaboutyoupage.HouseNumber;
            var postCode = faaboutyoupage.PostCode;
            var employer = faaboutyoupage.Employer;
            var yourHousehold = faaboutyoupage.AdultsInHousehold;
            var childrenInHousehold = faaboutyoupage.ChidrenInHousehold;
            var numberOfVehiles = faaboutyoupage.NumberOfVehiles;

            var financialassessmentincomepage = faaboutyoupage.NextClick() as FAIncomePage;
            Assert.Contains(financialassessmentincomepage.Url, "/income", "Pass to income page, with right data in fields");

            faaboutyoupage = financialassessmentincomepage.PreviousClick() as FAAboutYouPage;

            Assert.AreEqual(faaboutyoupage.FirstName, firstName, "Firstname check after go back");
            Assert.AreEqual(faaboutyoupage.LastName, lastName, "Lastname check after go back");
            Assert.AreEqual(faaboutyoupage.DateOfBirth, "1977-10-10", "DOB check after go back");
            Assert.AreEqual(faaboutyoupage.HouseNumber, houseNumber, "HouseNumber check after go back");
            Assert.AreEqual(faaboutyoupage.PostCode, postCode, "PostCode check after go back");
            Assert.AreEqual(faaboutyoupage.Employer, employer, "Employer check after go back");
            Assert.AreEqual(faaboutyoupage.AdultsInHousehold, yourHousehold, "Adults in Household check after go back");
            Assert.AreEqual(faaboutyoupage.ChidrenInHousehold, childrenInHousehold, "Children in Household check after go back");
            Assert.AreEqual(faaboutyoupage.NumberOfVehiles, numberOfVehiles, "Vehiles number check after go back");
        }

        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), Pending("Financial Assessment")]
        public void InvalidPotcodeWarningMessageCheck()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faaboutyoupage = journeyLn.Teleport<FAIncomePage>() as FAAboutYouPage;

            //var financialassessmentaboutyoupage = financialassessmentincomepage.PreviousClick() as FinancialAssessmentAboutYouPage;

            faaboutyoupage.PostCode = Get.GetName();
            faaboutyoupage.NextClick(error: true);
            Assert.IsTrue(faaboutyoupage.PostCodeErrorPresent());
        }

        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), Pending("Financial Assessment")]
        public void NextClickWithInvalidData()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var financialassessmentincomepage = journeyLn.Teleport<FAIncomePage>() as FAIncomePage;

            var faaboutyoupage = financialassessmentincomepage.PreviousClick() as FAAboutYouPage;

            var mem = faaboutyoupage.FirstName;
            faaboutyoupage.FirstName = "";
            var page = faaboutyoupage.NextClick(error: true);
            //Assert.Throws<AssertionFailureException>(() => financialassessmentaboutyoupage.NextClick());

            faaboutyoupage.FirstName = mem;
        }
    }
}
