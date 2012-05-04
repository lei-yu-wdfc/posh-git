﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Journey
{
    class ZaMobileL0Journey : IL0ConsumerJourney
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public BasePage CurrentPage { get; set; }
        public String Email { get; set; }

        public ZaMobileL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePageMobile;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
            DateOfBirth = new DateTime(1957, 10, 30);
            NationalId = Get.GetNIN(DateOfBirth, true);
            Email = Get.RandomEmail();
        }

        public IL0ConsumerJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePageMobile;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillPersonalDetails(string employerNameMask = null)
        {
            string employerName = employerNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = FirstName;
            personalDetailsPage.YourName.LastName = LastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = NationalId.ToString();//"5710300020087";
            personalDetailsPage.YourDetails.DateOfBirth = DateOfBirth.ToString("d/MMM/yyyy");
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
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = Email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = Email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            personalDetailsPage.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillPersonalDetailsWithEmail(string employerNameMask = null, string email = null)
        {
            throw new NotImplementedException();
        }

        public IL0ConsumerJourney FillAddressDetails()
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "25";
            addressPage.Street = "high road";
            addressPage.Town = "Kuku";
            addressPage.County = "Province";
            addressPage.PostCode = Get.GetPostcode();
            addressPage.AddressPeriod = "2 to 3 years";
            CurrentPage = addressPage.Next() as AccountDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails()
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            CurrentPage = accountDetailsPage.Next();
            return this;
        }

        public IL0ConsumerJourney FillBankDetails()
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Capitec";
            bankDetailsPage.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage.BankAccountSection.AccountNumber = "1234567";
            bankDetailsPage.BankAccountSection.BankPeriod = "2 to 3 years";
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
            throw new NotImplementedException();
        }

        public IL0ConsumerJourney GoToMySummaryPage()
        {
            throw new NotImplementedException();
        }

        public IL0ConsumerJourney IgnoreAcceptingLoanAndReturnToHomePageAndLogin()
        {
            throw new NotImplementedException();
        }
    }
}