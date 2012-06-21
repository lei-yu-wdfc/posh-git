using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Journey;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class UkL0Journey : BaseL0Journey
    {
        public UkL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;

            _submit = true;

            _amount = 100;
            _duration = 20;

            _firstName = Get.GetName();
            _lastName = Get.RandomString(10);
            _middleName = Get.RandomString(10);
            _title = "Mr";
            _employerName = Get.RandomString(10);
            _email = Get.RandomEmail();
            _mobilePhone = Get.GetMobilePhone();
            _dateOfBirth = new DateTime(1957, 10, 30);
            _gender = GenderEnum.Male;

            _postCode = Get.GetPostcode();
            _addressPeriod = "3 to 4 years";

            _password = Get.GetPassword();

            _bankName = "AIB";
            _sortCode = "13-40-20";
            _accountNumber = "63849203";
            _bankPeriod = "3 to 4 years";
            _pin = "0000";

            _cardNumber = "4444333322221111";
            _cardSecurity = "666";
            _cardType = "Visa Debit";
            _expiryDate = "Jan/2015";
            _startDate = "Jan/2007";

            journey.Add(typeof(HomePage), ApplyForLoan);
            journey.Add(typeof(PersonalDetailsPage), FillPersonalDetails);
            journey.Add(typeof(AddressDetailsPage), FillAddressDetails);
            journey.Add(typeof(AccountDetailsPage), FillAccountDetails);
            journey.Add(typeof(PersonalBankAccountPage), FillBankDetails);
            journey.Add(typeof(PersonalDebitCardPage), FillCardDetails);
            journey.Add(typeof(ProcessingPage), WaitForAcceptedPage);
            journey.Add(typeof(AcceptedPage), FillAcceptedPage);
            journey.Add(typeof(DealDonePage), GoToMySummaryPage);
        }

        protected override BaseL0Journey ApplyForLoan(bool submit = true)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = _amount.ToString();
            homePage.Sliders.HowLong = _duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            return this;
        }

        protected override BaseL0Journey FillPersonalDetails(bool submit = true)
        {
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = _middleName;
            personalDetailsPage.YourName.LastName = _lastName;
            personalDetailsPage.YourName.Title = _title;
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

        protected override BaseL0Journey FillAddressDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.PostCodeLookup = _postCode;
            addressPage.LookupByPostCode();
            addressPage.GetAddressesDropDown();
            Do.Until(() => addressPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressPage.HouseNumber = "666");
            addressPage.AddressPeriod = _addressPeriod;
            if (submit)
            {
                CurrentPage = addressPage.Next();
            }
            return this;
        }

        protected override BaseL0Journey FillAccountDetails(bool submit = true)
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

        protected override BaseL0Journey FillBankDetails(bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = _bankName;
            bankDetailsPage.BankAccountSection.SortCode = _sortCode;
            bankDetailsPage.BankAccountSection.AccountNumber = _accountNumber;
            bankDetailsPage.BankAccountSection.BankPeriod = _bankPeriod;
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next();
            }
            return this;
        }

        protected override BaseL0Journey FillCardDetails(bool submit = true)
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

        protected override BaseL0Journey WaitForAcceptedPage(bool submit = true)
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }

        protected override BaseL0Journey WaitForDeclinedPage(bool submit = true)
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        protected override BaseL0Journey FillAcceptedPage(bool submit = true)
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        protected override BaseL0Journey GoToMySummaryPage(bool submit = true)
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }

        #region Builder

        public override BaseL0Journey WithSortCode(string sortCode)
        {
            _sortCode = sortCode;
            return this;
        }

        public override BaseL0Journey WithCardNumber(string cardNumber)
        {
            _cardNumber = cardNumber;
            return this;
        }
        public override BaseL0Journey WithCardSecurity(string cardSecurity)
        {
            _cardSecurity = cardSecurity;
            return this;
        }

        public override BaseL0Journey WithCardType(string cardType)
        {
            _cardType = cardType;
            return this;
        }

        public override BaseL0Journey WithExpiryDate(string expiryDate)
        {
            _expiryDate = expiryDate;
            return this;
        }

        public override BaseL0Journey WithStartDate(string startDate)
        {
            _startDate = startDate;
            return this;
        }

        #endregion
    }
}
