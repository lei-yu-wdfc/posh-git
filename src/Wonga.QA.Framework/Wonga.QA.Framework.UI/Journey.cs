using System;
using System.Diagnostics;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    public class Journey
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public BasePage CurrentPage { get; set; }

        public Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
        }
        public Journey ApplyForLoan(int amount, int duration)
        {

            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                case AUT.Uk:
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
            var email = Get.RandomEmail();
            string employerName = employerNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    personalDetailsPage.YourName.FirstName = FirstName;
                    personalDetailsPage.YourName.LastName = LastName;
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

                    personalDetailsPage.YourName.FirstName = FirstName;
                    personalDetailsPage.YourName.MiddleName = Get.GetMiddleName();
                    personalDetailsPage.YourName.LastName = LastName;
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
                case AUT.Uk:
                    personalDetailsPage.YourName.FirstName = FirstName;
                    personalDetailsPage.YourName.MiddleName = "";
                    personalDetailsPage.YourName.LastName = LastName;
                    personalDetailsPage.YourName.Title = "Mr";
                    personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
                    personalDetailsPage.YourDetails.Gender = "Male";
                    personalDetailsPage.YourDetails.HomeStatus = "Tenant furnished";
                    personalDetailsPage.YourDetails.MaritalStatus = "Single";
                    personalDetailsPage.YourDetails.NumberOfDependants = "0";
                    personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed - full time";
                    personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
                    personalDetailsPage.EmploymentDetails.EmployerName = employerName;
                    personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
                    personalDetailsPage.EmploymentDetails.EmploymentPosition = "Engineering";
                    personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
                    personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "0";
                    personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd/MMM/yyyy");
                    personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPage.EmploymentDetails.WorkPhone = "02087111222";
                    personalDetailsPage.ContactingYou.CellPhoneNumber = "07707111222";
                    personalDetailsPage.ContactingYou.EmailAddress = email;
                    personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
                    personalDetailsPage.PrivacyPolicy = true;
                    personalDetailsPage.CanContact = "Yes";
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
                case AUT.Uk:
                    addressPage.PostCodeLookup = "SW6 6PN";
                    addressPage.LookupByPostCode();
                    addressPage.GetAddressesDropDown();
                    Do.Until(() => addressPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
                    Do.Until(() => addressPage.FlatNumber = "666");
                    addressPage.AddressPeriod = "3 to 4 years";
                    CurrentPage = addressPage.Next();
                    break;
            }
            return this;
        }

        public Journey FillAccountDetails()
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    accountDetailsPage.AccountDetailsSection.Password = Get.GetPassword();
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = Get.GetPassword();
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
                    accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
                    CurrentPage = accountDetailsPage.Next();//returns PersonalBankAccountPage
                    break;
                case AUT.Ca:
                    accountDetailsPage.AccountDetailsSection.Password = Get.GetPassword();
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = Get.GetPassword();
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
                    accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
                    CurrentPage = accountDetailsPage.Next() as PersonalBankAccountPage;
                    break;
                case AUT.Uk:
                    accountDetailsPage.AccountDetailsSection.Password = Get.GetPassword();
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = Get.GetPassword();
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
                    accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
                    CurrentPage = accountDetailsPage.Next() as PersonalBankAccountPage;
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
                case AUT.Uk:
                    bankDetailsPage.BankAccountSection.BankName = "AIB";
                    bankDetailsPage.BankAccountSection.SortCode = "13-40-20";
                    bankDetailsPage.BankAccountSection.AccountNumber = "63849203";
                    bankDetailsPage.BankAccountSection.BankPeriod = "3 to 4 years";
                    CurrentPage = bankDetailsPage.Next();
                    break;
            }
            return this;
        }

        public Journey FillCardDetails()
        {
            var personalDebitCardPage = CurrentPage as PersonalDebitCardPage;

            personalDebitCardPage.DebitCardSection.CardName = FirstName;
            personalDebitCardPage.DebitCardSection.CardNumber = "4444333322221111";
            personalDebitCardPage.DebitCardSection.CardSecurity = "666";
            personalDebitCardPage.DebitCardSection.CardType = "Visa Debit";
            personalDebitCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            personalDebitCardPage.DebitCardSection.StartDate = "Jan/2007";
            personalDebitCardPage.MobilePinVerification.Pin = "0000";
            CurrentPage = personalDebitCardPage.Next() as ProcessingPage;

            return this;
        }


        public Journey WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }
        public Journey WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }
        public Journey FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            string date = String.Format("{0:d MMM yyyy}", DateTime.Today);
            switch (Config.AUT)
            {
                case AUT.Ca:
                    acceptedPage.SignConfirmCA(date, FirstName, LastName);
                    CurrentPage = acceptedPage.Submit() as DealDonePage;
                    break;
                case AUT.Za:
                    acceptedPage.SignConfirmZA();
                    CurrentPage = acceptedPage.Submit() as DealDonePage;
                    break;
            }
            return this;
        }
        public Journey GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }


    }
}
