﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Journey;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class CaL0Journey : BaseL0Journey
    {
        public CaL0Journey(BasePage homePage)
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
            _addressPeriod = "2 to 3 years";

            _password = Get.GetPassword();

            _bankName = "Bank of Montreal";
            _branchNumber = "00011";
            _accountNumber = "3023423";
            _bankPeriod = "More than 4 years";
            _pin = "0000";

            journey.Add(typeof(HomePage), ApplyForLoan);
            journey.Add(typeof(PersonalDetailsPage), FillPersonalDetails);
            journey.Add(typeof(AddressDetailsPage), FillAddressDetails);
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
            personalDetailsPage.ProvinceSection.Province = "British Columbia";
            //Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = _middleName;
            personalDetailsPage.YourName.LastName = _lastName;
            personalDetailsPage.YourName.Title = _title;
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

        protected override BaseL0Journey FillAddressDetails(bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "1403";
            addressPage.Street = "Edward";
            addressPage.Town = "Hearst";
            addressPage.PostCode = _postCode;
            addressPage.AddressPeriod = _addressPeriod;
            addressPage.PostOfficeBox = "C12345";

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

        protected override BaseL0Journey FillAccountDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "There is no AccountDetailsPage on Ca");
        }

        protected override BaseL0Journey FillBankDetails(bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = _bankName;
            bankDetailsPage.BankAccountSection.BranchNumber = _branchNumber;
            bankDetailsPage.BankAccountSection.AccountNumber = _accountNumber;
            bankDetailsPage.BankAccountSection.BankPeriod = _bankPeriod;
            bankDetailsPage.PinVerificationSection.Pin = _pin;
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next() as ProcessingPage;
            }
            return this;
        }

        protected override BaseL0Journey FillCardDetails(bool submit = true)
        {
            throw new NotImplementedException();
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
            string date = String.Format("{0:d MMM yyyy}", DateTime.Today);
            acceptedPage.SignConfirmCaL0(date, _firstName, _lastName);
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        protected override BaseL0Journey GoToMySummaryPage(bool submit = true)
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }

        public override BaseL0Journey WithBranchNumber(string branchNumber)
        {
            _branchNumber = branchNumber;
            return this;
        }

    }
}
