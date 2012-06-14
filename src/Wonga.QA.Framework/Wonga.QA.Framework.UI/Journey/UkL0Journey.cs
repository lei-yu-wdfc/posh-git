using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class UkL0Journey : IL0ConsumerJourney
    {

        private String _firstName;
        private String _lastName;
        private String _middleName;
        private String _email;
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

        private String _cardNumber;
        private String _cardSecurity;
        private String _cardType;
        private String _expiryDate;
        private String _startDate;

        public BasePage CurrentPage { get; set; }

        public UkL0Journey(BasePage homePage)
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
            _addresPeriod = "3 to 4 years";

            _password = Get.GetPassword();

            _accountNumber = "63849203";
            _bankPeriod = "3 to 4 years";
            _pin = "0000";

            _cardNumber = "4444333322221111";
            _cardSecurity = "666";
            _cardType = "Visa Debit";
            _expiryDate = "Jan/2015";
            _startDate = "Jan/2007";
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
            personalDetailsPage.YourDetails.DateOfBirth = _dateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = _gender.ToString();
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
            personalDetailsPage.EmploymentDetails.EmployerName = _employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Engineering";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "0";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.EmploymentDetails.WorkPhone = "02087111222";
            personalDetailsPage.ContactingYou.CellPhoneNumber = _mobilePhone;
            personalDetailsPage.ContactingYou.EmailAddress = _email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = _email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillAddressDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.PostCodeLookup = _postCode;
            addressPage.LookupByPostCode();
            addressPage.GetAddressesDropDown();
            Do.Until(() => addressPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressPage.HouseNumber = "666");
            addressPage.AddressPeriod = _addresPeriod;
            if (submit)
            {
                CurrentPage = addressPage.Next();
            }
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails(bool submit = true)
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = _password;
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = _password;
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            if (submit)
            {
                CurrentPage = accountDetailsPage.Next() as PersonalBankAccountPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillBankDetails(bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "AIB";
            bankDetailsPage.BankAccountSection.SortCode = "13-40-20";
            bankDetailsPage.BankAccountSection.AccountNumber = _accountNumber;
            bankDetailsPage.BankAccountSection.BankPeriod = _bankPeriod;
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next();
            }
            return this;
        }

        public IL0ConsumerJourney FillCardDetails(bool submit = true)
        {
            var personalDebitCardPage = CurrentPage as PersonalDebitCardPage;

            personalDebitCardPage.DebitCardSection.CardName = _firstName;
            personalDebitCardPage.DebitCardSection.CardNumber = _cardNumber;
            personalDebitCardPage.DebitCardSection.CardSecurity = _cardSecurity;
            personalDebitCardPage.DebitCardSection.CardType = _cardType;
            personalDebitCardPage.DebitCardSection.ExpiryDate = _expiryDate;
            personalDebitCardPage.DebitCardSection.StartDate = _startDate;
            personalDebitCardPage.MobilePinVerification.Pin = _pin;
            if (submit)
            {
                CurrentPage = personalDebitCardPage.Next() as ProcessingPage;
            }
            return this;
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
            throw new NotImplementedException(message: "Don't used on Uk");
        }

        public IL0ConsumerJourney WithMotherMaidenName(string motherMaidenName)
        {
            throw new NotImplementedException(message: "Don't use on Uk");
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
            _cardNumber = cardNumber;
            return this;
        }
        public IL0ConsumerJourney WithCardSecurity(string cardSecurity)
        {
            _cardSecurity = cardSecurity;
            return this;
        }

        public IL0ConsumerJourney WithCardType(string cardType)
        {
            _cardType = cardType;
            return this;
        }

        public IL0ConsumerJourney WithExpiryDate(string expiryDate)
        {
            _expiryDate = expiryDate;
            return this;
        }

        public IL0ConsumerJourney WithStartDate(string startDate)
        {
            _startDate = startDate;
            return this;
        }

        #endregion
    }
}
