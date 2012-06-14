using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class CaL0Journey : IL0ConsumerJourney
    {
        private String _firstName;
        private String _lastName;
        private String _middleName;
        private String _employerName;
        private String _mobilePhone;
        public BasePage CurrentPage { get; set; }

        public CaL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            _firstName = Get.GetName();
            _lastName = Get.RandomString(10);
            _middleName = Get.RandomString(10);
            _employerName = Get.RandomString(10);
            _email = Get.RandomEmail();
            _mobilePhone = Get.GetMobilePhone();
        }
        public IL0ConsumerJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillPersonalDetails(bool submit = true)
        {
            string employerName = employerNameMask ?? Get.GetMiddleName();
            string middleName = middleNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ProvinceSection.Province = "British Columbia";
            Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

            personalDetailsPage.YourName.FirstName = firstName ?? FirstName;
            personalDetailsPage.YourName.MiddleName = middleName;
            personalDetailsPage.YourName.LastName = firstName ?? LastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = "123213126";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
            personalDetailsPage.YourDetails.Gender = gender ?? "Male";
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
            personalDetailsPage.ContactingYou.CellPhoneNumber = mobilePhone ?? Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = email ?? Email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email ?? Email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = true;
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillAddressDetails(string postcode = null, string addresPeriod = null, bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "1403";
            addressPage.Street = "Edward";
            addressPage.Town = "Hearst";
            addressPage.PostCode = postcode ?? "V4F3A9";
            addressPage.AddressPeriod = addresPeriod ?? "2 to 3 years";
            addressPage.PostOfficeBox = "C12345";
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails(string password = null, bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.AccountDetailsSection.Password = password ?? Get.GetPassword();
            addressPage.AccountDetailsSection.PasswordConfirm = password ?? Get.GetPassword();
            addressPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            addressPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            if (submit)
            {
                CurrentPage = addressPage.Next() as PersonalBankAccountPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillBankDetails(string accountNumber = null, string bankPeriod = null, string pin = null, bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
            bankDetailsPage.BankAccountSection.BranchNumber = "00011";
            bankDetailsPage.BankAccountSection.AccountNumber = accountNumber ?? "3023423";
            bankDetailsPage.BankAccountSection.BankPeriod = bankPeriod ?? "More than 4 years";
            bankDetailsPage.PinVerificationSection.Pin = pin ?? "0000";
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next() as ProcessingPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillCardDetails(string cardNumber = null, string cardSecurity = null, string cardType = null, string expiryDate = null, string startDate = null, string pin = null, bool submit = true)
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

        #region Builder
        public IL0ConsumerJourney WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public IL0ConsumerJourney WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public IL0ConsumerJourney WithMiddleName(string middleName)
        {
            _middleName = middleName;
            return this;
        }

        public IL0ConsumerJourney WithEmployerName(string employerName)
        {
            _employerName = employerName;
            return this;
        }

        public IL0ConsumerJourney WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public IL0ConsumerJourney WithMobilePhone(string mobilePhone)
        {
            _mobilePhone = mobilePhone;
            return this;
        }
        public IL0ConsumerJourney WithGender(GenderEnum gender)
        {
            _gender = gender;
            return this;
        }

        public IL0ConsumerJourney WithDateOfBirth(DateTime dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
            return this;
        }

        public IL0ConsumerJourney WithNationalId(string nationalId)
        {
            _nationalId = nationalId;
            return this;
        }

        #endregion
    }
}
