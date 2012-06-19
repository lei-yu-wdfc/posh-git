using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Journey;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Journey
{
    class ZaMobileL0Journey : BaseL0Journey
    {
        public ZaMobileL0Journey(BasePageMobile homePage)
        {
            CurrentPage = homePage as HomePageMobile;

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
            _gender = GenderEnum.Female;
            _nationalId = Get.GetNationalNumber(_dateOfBirth, _gender == GenderEnum.Female);

            _postCode = Get.GetPostcode();
            _addressPeriod = "2 to 3 years";

            _password = Get.GetPassword();

            _accountNumber = "1234567";
            _bankPeriod = "2 to 3 years";
            _pin = "0000";

            journey.Add(typeof(HomePageMobile), ApplyForLoan);
            journey.Add(typeof(PersonalDetailsPageMobile), FillPersonalDetails);
            journey.Add(typeof(AddressDetailsPageMobile), FillAddressDetails);
            journey.Add(typeof(AccountDetailsPageMobile), FillAccountDetails);
            journey.Add(typeof(PersonalBankAccountPageMobile), FillBankDetails);
            journey.Add(typeof(ProcessingPageMobile), WaitForAcceptedPage);
            journey.Add(typeof(AcceptedPageMobile), FillAcceptedPage);
            journey.Add(typeof(DealDonePage), GoToMySummaryPage);
        }

        protected override BaseL0Journey ApplyForLoan(bool submit = true)
        {
            var homePage = CurrentPage as HomePageMobile;
            homePage.Sliders.HowMuch = _amount.ToString();
            homePage.Sliders.HowLong = _duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPageMobile;
            return this;
        }

        protected override BaseL0Journey FillPersonalDetails(bool submit = true)
        {
            var personalDetailsPage = CurrentPage as PersonalDetailsPageMobile;
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
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPageMobile;
            }
            return this;
        }

        protected override BaseL0Journey FillAddressDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPageMobile;
            addressPage.HouseNumber = "25";
            addressPage.Street = "high road";
            addressPage.Town = "Kuku";
            addressPage.County = "Province";
            addressPage.PostCode = _postCode;
            addressPage.AddressPeriod = _addressPeriod;
            if (submit)
            {
                CurrentPage = addressPage.Next() as AccountDetailsPageMobile;
            }
            return this;
        }

        protected override BaseL0Journey FillAccountDetails(bool submit = true)
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPageMobile;
            accountDetailsPage.AccountDetailsSection.Password = _password;
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = _password;
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            if (submit)
            {
                CurrentPage = accountDetailsPage.Next();
            }
            return this;
        }

        protected override BaseL0Journey FillBankDetails(bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPageMobile;
            bankDetailsPage.BankAccountSection.BankName = "Capitec";
            bankDetailsPage.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage.BankAccountSection.AccountNumber = _accountNumber;
            bankDetailsPage.BankAccountSection.BankPeriod = _bankPeriod;
            bankDetailsPage.PinVerificationSection.Pin = _pin;
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next() as ProcessingPageMobile;
            }
            return this;
        }

        protected override BaseL0Journey WaitForAcceptedPage(bool submit = true)
        {
            var processingPage = CurrentPage as ProcessingPageMobile;
            CurrentPage = processingPage.WaitFor<AcceptedPageMobile>() as AcceptedPageMobile;
            return this;
        }


        protected override BaseL0Journey WaitForDeclinedPage(bool submit = true)
        {
            var processingPage = CurrentPage as ProcessingPageMobile;
            CurrentPage = processingPage.WaitFor<DeclinedPageMobile>() as DeclinedPageMobile;
            return this;
        }

        protected override BaseL0Journey FillAcceptedPage(bool submit = true)
        {
            throw new NotImplementedException();
        }

        protected override BaseL0Journey GoToMySummaryPage(bool submit = true)
        {
            throw new NotImplementedException();
        }

        public override BaseL0Journey WithNationalId(string nationalId)
        {
            _nationalId = nationalId;
            return this;
        }
    }
}
