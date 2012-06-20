using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Journey;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class ZaL0Journey : BaseL0Journey
    {


        public ZaL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;

            _submit = true;

            _amount = 200;
            _duration = 10;

            _firstName = Get.GetName();
            _lastName = Get.RandomString(10);
            _middleName = Get.RandomString(10);
            _title = "Mr";
            _employerName = Get.RandomString(10);
            _email = Get.RandomEmail();
            _mobilePhone = Get.GetMobilePhone();
            _dateOfBirth = new DateTime(1957, 10, 30);
            _gender = GenderEnum.Female;
            _nationalId = Get.GetNationalNumber(_dateOfBirth, _gender == GenderEnum.Female);

            _postCode = Get.GetPostcode();
            _addressPeriod = "2 to 3 years";

            _password = Get.GetPassword();

            _bankName = "Capitec";
            _bankAccountType = "Current";
            _accountNumber = "1234567";
            _bankPeriod = "2 to 3 years";
            _pin = "0000";

            journey.Add(typeof(HomePage), ApplyForLoan);
            journey.Add(typeof(PersonalDetailsPage), FillPersonalDetails);
            journey.Add(typeof(AddressDetailsPage), FillAddressDetails);
            journey.Add(typeof(AccountDetailsPage), FillAccountDetails);
            journey.Add(typeof(PersonalBankAccountPage), FillBankDetails);
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

        protected override BaseL0Journey FillAddressDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "25";
            addressPage.Street = "high road";
            addressPage.Town = "Kuku";
            addressPage.County = "Province";
            addressPage.PostCode = _postCode;
            addressPage.AddressPeriod = _addressPeriod;
            if (submit)
            {
                CurrentPage = addressPage.Next() as AccountDetailsPage;
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
                CurrentPage = accountDetailsPage.Next();//returns PersonalBankAccountPage
            }
            return this;
        }

        protected override BaseL0Journey FillBankDetails(bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = _bankName;
            bankDetailsPage.BankAccountSection.BankAccountType = _bankAccountType;
            bankDetailsPage.BankAccountSection.AccountNumber = _accountNumber;
            bankDetailsPage.BankAccountSection.BankPeriod = _bankPeriod;
            bankDetailsPage.PinVerificationSection.Pin = _pin;
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next() as ProcessingPage;
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
            acceptedPage.SignConfirmZA();
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        protected override BaseL0Journey GoToMySummaryPage(bool submit = true)
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }

        public override BaseL0Journey IgnoreAcceptingLoanAndReturnToHomePageAndLogin(bool submit = true)
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            acceptedPage.Client.Driver.Navigate().GoToUrl(Config.Ui.Home);
            var homePage = acceptedPage.WaitForPage<HomePage>() as HomePage;
            CurrentPage = homePage.Login.LoginAs(_email, Get.GetPassword());
            return this;
        }

        public override BaseL0Journey WithNationalId(string nationalId)
        {
            _nationalId = nationalId;
            return this;
        }

        public override BaseL0Journey WithBankAccountType(string bankAccountType)
        {
            _bankAccountType = bankAccountType;
            return this;
        }
    }
}
