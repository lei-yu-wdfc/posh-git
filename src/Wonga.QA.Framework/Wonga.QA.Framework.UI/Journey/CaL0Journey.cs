using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class CaL0Journey : IL0ConsumerJourney
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
		public string NationalId { get; set; } //Not used yet
    	public DateTime DateOfBirth { get; set; } //not used yet

        public BasePage CurrentPage { get; set; }
		

         public CaL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
        }
        public IL0ConsumerJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillPersonalDetails(string employerNameMask = null)
        {
            var email = Get.RandomEmail();
            string employerName = employerNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ProvinceSection.Province = "British Columbia";
            Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

            personalDetailsPage.YourName.FirstName = FirstName;
            personalDetailsPage.YourName.MiddleName = Get.GetMiddleName();
            personalDetailsPage.YourName.LastName = LastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = "123213126";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
            personalDetailsPage.YourDetails.Gender = "Male";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
            personalDetailsPage.EmploymentDetails.EmployerName = employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Professional (finance, accounting, legal, HR)";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "0";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "9876543210";
            personalDetailsPage.ContactingYou.EmailAddress = email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = true;
            CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillPersonalDetailsWithEmail(string employerNameMask = null, string email = null)
        {
            string employerName = employerNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ProvinceSection.Province = "British Columbia";
            Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

            personalDetailsPage.YourName.FirstName = FirstName;
            personalDetailsPage.YourName.MiddleName = Get.GetMiddleName();
            personalDetailsPage.YourName.LastName = LastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = "123213126";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
            personalDetailsPage.YourDetails.Gender = "Male";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
            personalDetailsPage.EmploymentDetails.EmployerName = employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Professional (finance, accounting, legal, HR)";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "0";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "9876543210";
            personalDetailsPage.ContactingYou.EmailAddress = email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = true;
            CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillAddressDetails()
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "1403";
            addressPage.Street = "Edward";
            addressPage.Town = "Hearst";
            addressPage.PostCode = "V4F3A9";
            addressPage.AddressPeriod = "2 to 3 years";
            addressPage.PostOfficeBox = "C12345";
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails()
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.AccountDetailsSection.Password = Get.GetPassword();
            addressPage.AccountDetailsSection.PasswordConfirm = Get.GetPassword();
            addressPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            addressPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            CurrentPage = addressPage.Next() as PersonalBankAccountPage;
            return this;
        }

        public IL0ConsumerJourney FillBankDetails()
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
            bankDetailsPage.BankAccountSection.BranchNumber = "00011";
            bankDetailsPage.BankAccountSection.AccountNumber = "3023423";
            bankDetailsPage.BankAccountSection.BankPeriod = "More than 4 years";
            bankDetailsPage.PinVerificationSection.Pin = "0000";
            CurrentPage = bankDetailsPage.Next() as ProcessingPage;
            return this;
        }

        public IL0ConsumerJourney FillCardDetails()
        {
            throw new NotImplementedException();
        }

        public IL0ConsumerJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }

        public IL0ConsumerJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        public IL0ConsumerJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            string date = String.Format("{0:d MMM yyyy}", DateTime.Today);
            acceptedPage.SignConfirmCaL0(date, FirstName, LastName);
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        public IL0ConsumerJourney GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }

        public IL0ConsumerJourney IgnoreAcceptingLoanAndReturnToHomePageAndLogin()
        {
            throw new NotImplementedException();
        }
    }
}
