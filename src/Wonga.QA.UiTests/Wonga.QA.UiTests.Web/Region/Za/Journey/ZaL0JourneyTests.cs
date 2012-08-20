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

namespace Wonga.QA.UiTests.Web.Region.Za.Journey
{
    [Parallelizable(TestScope.All), AUT(AUT.Za)]
    class ZaL0JourneyTests : UiTest
    {
        [Test, JIRA("ZA-2108"), Pending("Broken")]
        public void L0VerifyWongaLzeroZaModuleSignatureInsertedInPage()
        {
            // Checks for the presence of "<!-- Output from wonga_lzero_za/<$_GET['q']> -->" in page source.
            // This test complements the normal ZA L0 tests since the L0 journey should be functionally the
            // same as before the refactor.

            // Create a journey:
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));

            // Go to the first page:
            var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-details -->"));

            // Go to the second page:
            var addressDetailsPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-address -->"));

            // Go to the third page:
            var accountDetailsPage = journey.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-account -->"));

            // Go to the fourth page:
            var personalBankAccountPage = journey.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-bank -->"));
        }

        [Test, JIRA("QA-170")] //Removed from smoke because of the problem with sliders update
        public void CustomerOnHowItWorksPageShouldBeAbleUseSlidersProperly()
        {
            //CA is out due to new wonga sliders being implemented on homepage only 
            //soon it will be on "my account" and in other regions

            var howItWorks = Client.HowItWorks();
            var personalDetailsPage = howItWorks.ApplyForLoan(200, 10);
            Assert.IsTrue(personalDetailsPage is PersonalDetailsPage);
        }

        [Test, JIRA("QA-179"), Category(TestCategories.SmokeTest)]
        public void L0JourneyCustomerIdNumberShouldBeAlignedWithDOBAndGender()
        {
            var email = Get.RandomEmail();
            var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPageZa = journeyZa.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
            personalDetailsPageZa.YourName.FirstName = Get.RandomString(3, 10);
            personalDetailsPageZa.YourName.LastName = Get.RandomString(3, 10);
            personalDetailsPageZa.YourName.Title = "Mr";
            personalDetailsPageZa.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPageZa.YourDetails.HomeLanguage = "English";
            personalDetailsPageZa.YourDetails.NumberOfDependants = "0";
            personalDetailsPageZa.YourDetails.MaritalStatus = "Single";
            personalDetailsPageZa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPageZa.EmploymentDetails.MonthlyIncome = "3000";
            personalDetailsPageZa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
            personalDetailsPageZa.EmploymentDetails.EmployerIndustry = "Accountancy";
            personalDetailsPageZa.EmploymentDetails.EmploymentPosition = "Administration";
            personalDetailsPageZa.EmploymentDetails.TimeWithEmployerYears = "9";
            personalDetailsPageZa.EmploymentDetails.TimeWithEmployerMonths = "5";
            personalDetailsPageZa.EmploymentDetails.WorkPhone = "0123456789";
            personalDetailsPageZa.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPageZa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPageZa.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPageZa.ContactingYou.CellPhoneNumber = "0770090000";
            personalDetailsPageZa.ContactingYou.EmailAddress = email;
            personalDetailsPageZa.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPageZa.PrivacyPolicy = true;
            personalDetailsPageZa.CanContact = "Yes";
            personalDetailsPageZa.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            personalDetailsPageZa.YourDetails.Number = Get.GetNationalNumber(new DateTime(1957, 3, 10), true);
            personalDetailsPageZa.YourDetails.Gender = "Male";
            personalDetailsPageZa.YourDetails.DateOfBirth = "9/Mar/1957";
            personalDetailsPageZa.YourDetails.Gender = "Female";
            personalDetailsPageZa.YourDetails.DateOfBirth = "10/Mar/1957";
            journeyZa.CurrentPage = personalDetailsPageZa.Submit() as AddressDetailsPage;
        }

        [Test, JIRA("QA-275"), Pending("ZA-1952, Za-2489")]
        public void PasswordThatEqualToTheEmailWithUpperLastSimbolAddressWarningMessageShouldDisplayed()
        {
            var email = Get.RandomEmail();
            var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmail(email)
                .WithPassword(email.Remove(email.Length - 1, 1) + "M")
                .FillAndStop();
            var accountDetailsPageZa = journeyZa.Teleport<AccountDetailsPage>() as AccountDetailsPage;
            try
            {
                accountDetailsPageZa = accountDetailsPageZa.NextClick();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(ContentMap.Get.ProblemProcessingDetailsMessage));
                IWebElement section = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.Fieldset));
                IWebElement password = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.Password));
                IWebElement passwordConfirm = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.PasswordConfirm));
                IWebElement secretQuestion = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.SecretQuestion));
                IWebElement secretAnswer = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.SecretAnswer));
                IWebElement next = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.NextButton));
                password.SendValue("Passw0rd");
                passwordConfirm.SendValue("Passw0rd");
                secretQuestion.SendValue("Secret question'-.");
                secretAnswer.SendValue("Secret answer");
                next.Click();
                try
                {
                    var page = new HomePage(Client);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains(ContentMap.Get.ProblemProcessingDetailsMessage));
                }
            }
        }

        [Test, Category(TestCategories.SmokeTest), JIRA("QA-277")]
        public void L0JourneyInvalidPostcodeShouldCauseWarningMessageValidPostcodeShouldDimissWarning()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithPosteCode("12.5")
                .FillAndStop();
            var addressPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;
            try
            {
                addressPage = addressPage.NextClick();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(ContentMap.Get.AddressDeatailsPage.PostcodeError));
                IWebElement form = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AddressDetailsPage.FormId));
                IWebElement postCode = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.Postcode));
                IWebElement houseNumber = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.HouseNumber));
                IWebElement addressPeriod = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.AddressPeriod));
                IWebElement next = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.NextButton));
                IWebElement county = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.County));
                IWebElement street = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.Street));
                IWebElement town = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.Town));
                postCode.SendValue("1234");
                houseNumber.SendValue("25");
                street.SendValue("high road");
                town.SendValue("Kuku");
                county.SendValue("Province");
                addressPeriod.SelectOption("2 to 3 years");
                next.Click();
                try
                {
                    var page = new AccountDetailsPage(Client);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains(ContentMap.Get.AddressDeatailsPage.PostcodeError));
                }
            }
        }

        [Test, Category(TestCategories.SmokeTest), JIRA("QA-276")]
        public void CustomerUsesExistingIdNumberShouldBeAbleToProceed()
        {
            var customer = Do.With.Message("There is no customer in DB").Until(() => Drive.Data.Comms.Db.CustomerDetails.FindAllByGender(2).FirstOrDefault());
            Console.WriteLine(customer.NationalNumber.ToString() + "  /  " + customer.DateOfBirth.ToString().Replace(" 00:00:00", ""));


            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithNationalId(customer.NationalNumber.ToString())
                .WithDateOfBirth(customer.DateOfBirth);
            var processingPage = journey.Teleport<ProcessingPage>() as ProcessingPage;
        }

        [Test, Pending("Test is yet to be complete. Author: Ben Ifie")]
        public void L0DropOff()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;

        }

        [Test, JIRA("QA-247")]
        [Row(100, 37)]
        [Row(100, 31)]
        [Row(131, 34)]
        [Row(153, 37)]
        public void VerifyThatInduplumNeverBrokenAndTotalToRepayIsSmalestThenTwoLoanAmount(int _loanAmount, int _duration)
        {
            int controlSum = _loanAmount * 2;
            double totalToRepay;

            var HomePage = Client.Home();

            HomePage.Sliders.HowMuch = _loanAmount.ToString();
            HomePage.Sliders.HowLong = _duration.ToString();

            totalToRepay = Convert.ToDouble(HomePage.Sliders.GetTotalToRepay.Remove(0, 1));
            Assert.IsTrue((int)totalToRepay <= controlSum);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAmount(_loanAmount).WithDuration(_loanAmount);
            var personalDetails = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

            totalToRepay = Convert.ToDouble(personalDetails.GetTotalToRepay.Remove(0, 1));
            Assert.IsTrue((int)totalToRepay <= controlSum);

            var SummaryPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            totalToRepay = Convert.ToDouble(SummaryPage.GetTotalToRepay.Remove(0, 1));
            Assert.IsTrue((int)totalToRepay <= controlSum);
        }

        [Test, JIRA("QA-308")]
        public void ShouldPossibleToCompleteAnL0WithRetiredStatus()
        {
            string firstName = Get.RandomString(3, 10);
            string lastName = Get.RandomString(3, 10);
            string Email = Get.RandomEmail();
            DateTime DateOfBirth = new DateTime(1957, 10, 30);
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithFirstName(firstName).WithLastName(lastName);
            var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
            string employerName = Get.EnumToString(RiskMask.TESTEmployedMask);

            string NationalId = Get.GetNationalNumber(DateOfBirth, true);
            personalDetailsPage.YourName.FirstName = firstName;
            personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
            personalDetailsPage.YourName.LastName = lastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = NationalId.ToString();
            personalDetailsPage.YourDetails.DateOfBirth = DateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPage.YourDetails.HomeLanguage = "English";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Retired";
            personalDetailsPage.EmploymentDetails.SelfEmployedMonthlyIncome = "3000";
            personalDetailsPage.ContactingYou.HomePhoneNumber = "0123456789";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "0123456789";
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = Email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = Email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            personalDetailsPage.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";

            journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            var processingPageZa = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, JIRA("QA-178"), AUT(AUT.Za, AUT.Ca)]
        public void ShouldPossibleToCompleteAnL0WhenChoosingEmployedFullAndThenRetiredStatus()
        {
            string firstName = Get.RandomString(3, 10);
            string lastName = Get.RandomString(3, 10);
            string Email = Get.RandomEmail();
            DateTime DateOfBirth = new DateTime(1957, 10, 30);
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithFirstName(firstName).WithLastName(lastName);
            var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
            string employerName = Get.EnumToString(RiskMask.TESTEmployedMask);
            string NationalId = Get.GetNationalNumber(DateOfBirth, true);
            personalDetailsPage.YourName.FirstName = firstName;
            personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
            personalDetailsPage.YourName.LastName = lastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = NationalId.ToString();
            personalDetailsPage.YourDetails.DateOfBirth = DateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPage.YourDetails.HomeLanguage = "English";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.EmployerName = "Valera";
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Education";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Teacher";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "7";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "3000";
            personalDetailsPage.ContactingYou.HomePhoneNumber = "0123456789";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "0123456789";
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = Email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = Email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            personalDetailsPage.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Retired";
            journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
        }


    }
}
