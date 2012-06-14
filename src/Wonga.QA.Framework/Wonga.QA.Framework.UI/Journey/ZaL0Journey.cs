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
    class ZaL0Journey : IL0ConsumerJourney
    {
        private String _firstName;
        private String _lastName;
        private String _middleName;
        private String _email;
        private String _employerName;
        private String _mobilePhone;
        private String _nationalId;
        private DateTime _dateOfBirth;
        private GenderEnum _gender;
        public BasePage CurrentPage { get; set; }


        public ZaL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            _firstName = Get.GetName();
            _lastName = Get.RandomString(10);
            _middleName = Get.RandomString(10);
            _employerName = Get.RandomString(10);
            _email = Get.RandomEmail();
            _mobilePhone = Get.GetMobilePhone();
            _dateOfBirth = new DateTime(1957, 10, 30);
            _gender = GenderEnum.Female;
            _nationalId = Get.GetNationalNumber(_dateOfBirth, _gender == GenderEnum.Female);
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
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = _middleName;
            personalDetailsPage.YourName.LastName = _lastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = _nationalId.ToString();//"5710300020087";
            personalDetailsPage.YourDetails.DateOfBirth = _dateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = _gender.ToString();
            personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPage.YourDetails.HomeLanguage = "English";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "3000";
            personalDetailsPage.EmploymentDetails.EmployerName = _employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Accountancy";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Administration";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "9";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "5";
            personalDetailsPage.EmploymentDetails.WorkPhone = "0123456789";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = _mobilePhone;
            personalDetailsPage.ContactingYou.EmailAddress = _email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = _email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            personalDetailsPage.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillAddressDetails(string postcode = null, string addresPeriod = null, bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "25";
            addressPage.Street = "high road";
            addressPage.Town = "Kuku";
            addressPage.County = "Province";
            addressPage.PostCode = postcode ?? Get.GetPostcode();
            addressPage.AddressPeriod = addresPeriod ?? "2 to 3 years";
            if (submit)
            {
                CurrentPage = addressPage.Next() as AccountDetailsPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails(string password = null, bool submit = true)
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = password ?? Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = password ?? Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            if (submit)
            {
                CurrentPage = accountDetailsPage.Next();//returns PersonalBankAccountPage
            }
            return this;
        }

        public IL0ConsumerJourney FillBankDetails(string accountNumber = null, string bankPeriod = null, string pin = null, bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Capitec";
            bankDetailsPage.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage.BankAccountSection.AccountNumber = accountNumber ?? "1234567";
            bankDetailsPage.BankAccountSection.BankPeriod = bankPeriod ?? "2 to 3 years";
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
            acceptedPage.SignConfirmZA();
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
            var acceptedPage = CurrentPage as AcceptedPage;
            acceptedPage.Client.Driver.Navigate().GoToUrl(Config.Ui.Home);
            var homePage = acceptedPage.WaitForPage<HomePage>() as HomePage;
            CurrentPage = homePage.Login.LoginAs(_email, Get.GetPassword());

            return this;
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
