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
        private DateTime _dateOfBirth;
        private GenderEnum _gender;

        private String _postCode;
        private String _addresPeriod;

        private String _password;

        private String _accountNumber;
        private String _bankPeriod;
        private String _pin;

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
            _dateOfBirth = new DateTime(1957, 10, 30);
            _gender = GenderEnum.Male;

            _postCode = Get.GetPostcode();
            _addresPeriod = "2 to 3 years";

            _password = Get.GetPassword();

            _accountNumber = "3023423";
            _bankPeriod = "More than 4 years";
            _pin = "0000";
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
            personalDetailsPage.ProvinceSection.Province = "British Columbia";
            Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = _middleName;
            personalDetailsPage.YourName.LastName = _lastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = "123213126";
            personalDetailsPage.YourDetails.DateOfBirth = _dateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = _gender.ToString();
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
            personalDetailsPage.EmploymentDetails.EmployerName = _employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Professional (finance, accounting, legal, HR)";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "0";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = _mobilePhone;
            personalDetailsPage.ContactingYou.EmailAddress = _email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = _email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = true;
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillAddressDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "1403";
            addressPage.Street = "Edward";
            addressPage.Town = "Hearst";
            addressPage.PostCode = _postCode;
            addressPage.AddressPeriod = _addresPeriod;
            addressPage.PostOfficeBox = "C12345";
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.AccountDetailsSection.Password = _password;
            addressPage.AccountDetailsSection.PasswordConfirm = _password;
            addressPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            addressPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            if (submit)
            {
                CurrentPage = addressPage.Next() as PersonalBankAccountPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillBankDetails(bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
            bankDetailsPage.BankAccountSection.BranchNumber = "00011";
            bankDetailsPage.BankAccountSection.AccountNumber = _accountNumber;
            bankDetailsPage.BankAccountSection.BankPeriod = _bankPeriod;
            bankDetailsPage.PinVerificationSection.Pin = _pin;
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next() as ProcessingPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillCardDetails(bool submit = true)
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
            acceptedPage.SignConfirmCaL0(date, _firstName, _lastName);
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
            throw new NotImplementedException(message: "Don't used on Ca");
        }

        public IL0ConsumerJourney WithMotherMaidenName(string motherMaidenName)
        {
            throw new NotImplementedException(message: "Don't use on Ca");
        }

        public IL0ConsumerJourney WithPosteCode(string postCode)
        {
            _postCode = postCode;
            return this;
        }

        public IL0ConsumerJourney WithAddresPeriod(string addresPeriod)
        {
            _addresPeriod = addresPeriod;
            return this;
        }

        public IL0ConsumerJourney WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public IL0ConsumerJourney WithAccountNumber(string accountNumber)
        {
            _accountNumber = accountNumber;
            return this;
        }

        public IL0ConsumerJourney WithBankPeriod(string bankPeriod)
        {
            _bankPeriod = bankPeriod;
            return this;
        }

        public IL0ConsumerJourney WithPin(string pin)
        {
            _pin = pin;
            return this;
        }

        public IL0ConsumerJourney WithCardNumber(string cardNumber)
        {
            throw new NotImplementedException(message: "Don't use on Ca");
        }
        public IL0ConsumerJourney WithCardSecurity(string cardSecurity)
        {
            throw new NotImplementedException(message: "Don't use on Ca");
        }

        public IL0ConsumerJourney WithCardType(string cardType)
        {
            throw new NotImplementedException(message: "Don't use on Ca");
        }

        public IL0ConsumerJourney WithExpiryDate(string expiryDate)
        {
            throw new NotImplementedException(message: "Don't use on Ca");
        }

        public IL0ConsumerJourney WithStartDate(string startDate)
        {
            throw new NotImplementedException(message: "Don't use on Ca");
        }
        #endregion
    }
}
