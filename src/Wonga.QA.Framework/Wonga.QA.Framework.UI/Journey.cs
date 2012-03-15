﻿using System;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    public class Journey
    {
        protected String _firstName;
        protected String _lastName;

        public BasePage CurrentPage { get; set; }

        public Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            _firstName = Data.GetName();
            _lastName = Data.RandomString(10);
        }
        public Journey ApplyForLoan(int amount, int duration)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                    var homePage = CurrentPage as HomePage;
                    homePage.Sliders.HowMuch = amount.ToString();
                    homePage.Sliders.HowLong = duration.ToString();
                    CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
                    break;
            }
            return this;
        }

        public Journey FillPersonalDetails(string employerNameMask = null)
        {
            var email = Data.RandomEmail();
            string employerName = employerNameMask ?? Data.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    personalDetailsPage.YourName.FirstName = _firstName;
                    personalDetailsPage.YourName.LastName = _lastName;
                    personalDetailsPage.YourName.Title = "Mr";
                    personalDetailsPage.YourDetails.Number = "5710300020087";
                    personalDetailsPage.YourDetails.DateOfBirth = "30/Oct/1957";
                    personalDetailsPage.YourDetails.Gender = "Female";
                    personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
                    personalDetailsPage.YourDetails.HomeLanguage = "English";
                    personalDetailsPage.YourDetails.NumberOfDependants = "0";
                    personalDetailsPage.YourDetails.MaritalStatus = "Single";
                    personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPage.EmploymentDetails.MonthlyIncome = "3000";
                    personalDetailsPage.EmploymentDetails.EmployerName = employerName;
                    personalDetailsPage.EmploymentDetails.EmployerIndustry = "Accountancy";
                    personalDetailsPage.EmploymentDetails.EmploymentPosition = "Administration";
                    personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "9";
                    personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "5";
                    personalDetailsPage.EmploymentDetails.WorkPhone = "0123456789";
                    personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
                    personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPage.ContactingYou.CellPhoneNumber = "0720000098";
                    personalDetailsPage.ContactingYou.EmailAddress = email;
                    personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
                    personalDetailsPage.PrivacyPolicy = true;
                    personalDetailsPage.CanContact = "Yes";
                    personalDetailsPage.MarriedInCommunityProperty =
                        "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
                    CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
                    break;
                case AUT.Ca:
                    personalDetailsPage.ProvinceSection.Province = "British Columbia";
                    Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

                    personalDetailsPage.YourName.FirstName = _firstName;
                    personalDetailsPage.YourName.MiddleName = Data.GetMiddleName();
                    personalDetailsPage.YourName.LastName = _lastName;
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
                    break;
            }
            return this;
        }

        public Journey FillAddressDetails()
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    addressPage.FlatNumber = "25";
                    addressPage.Street = "high road";
                    addressPage.Town = "Kuku";
                    addressPage.County = "Province";
                    addressPage.PostCode = "1234";
                    addressPage.AddressPeriod = "2 to 3 years";
                    CurrentPage = addressPage.Next() as AccountDetailsPage;
                    break;
                case AUT.Ca:
                    addressPage.FlatNumber = "1403";
                    addressPage.Street = "Edward";
                    addressPage.Town = "Hearst";
                    addressPage.PostCode = "V4F3A9";
                    addressPage.AddressPeriod = "2 to 3 years";
                    addressPage.PostOfficeBox = "C12345";
                    break;
            }
            return this;
        }

        public Journey FillAccountDetails()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    var accountDetailsPage = CurrentPage as AccountDetailsPage;
                    accountDetailsPage.AccountDetailsSection.Password = Data.GetPassword();
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = Data.GetPassword();
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
                    accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
                    CurrentPage = accountDetailsPage.Next();//returns PersonalBankAccountPage
                    break;
                case AUT.Ca:
                    var addressPage = CurrentPage as AddressDetailsPage;
                    addressPage.AccountDetailsSection.Password = Data.GetPassword();
                    addressPage.AccountDetailsSection.PasswordConfirm = Data.GetPassword();
                    addressPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
                    addressPage.AccountDetailsSection.SecretAnswer = "Secret answer";
                    CurrentPage = addressPage.Next() as PersonalBankAccountPage;
                    break;
            }
            return this;
        }

        public Journey FillBankDetails()
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    bankDetailsPage.BankAccountSection.BankName = "Capitec";
                    bankDetailsPage.BankAccountSection.BankAccountType = "Current";
                    bankDetailsPage.BankAccountSection.AccountNumber = "1234567";
                    bankDetailsPage.BankAccountSection.BankPeriod = "2 to 3 years";
                    bankDetailsPage.PinVerificationSection.Pin = "0000";
                    CurrentPage = bankDetailsPage.Next() as ProcessingPage;
                    break;
                case AUT.Ca:
                    bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
                    bankDetailsPage.BankAccountSection.BranchNumber = "00011";
                    bankDetailsPage.BankAccountSection.AccountNumber = "3023423";
                    bankDetailsPage.BankAccountSection.BankPeriod = "More than 4 years";
                    bankDetailsPage.PinVerificationSection.Pin = "0000";
                    CurrentPage = bankDetailsPage.Next() as ProcessingPage;
                    break;
            }
            return this;
        }
    }
}