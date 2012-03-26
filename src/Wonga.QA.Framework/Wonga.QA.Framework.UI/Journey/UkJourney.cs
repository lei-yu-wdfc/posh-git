using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class UkJourney : ICustomerJourney
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BasePage CurrentPage { get; set; }

        public UkJourney(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
        }
        public ICustomerJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            return this;
        }

        public ICustomerJourney FillPersonalDetails(string employerNameMask = null)
        {
            var email = Get.RandomEmail();
            string employerName = employerNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourName.FirstName = FirstName;
            personalDetailsPage.YourName.MiddleName = Get.GetMiddleName();
            personalDetailsPage.YourName.LastName = LastName;
            personalDetailsPage.YourDetails.Gender = "Male";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
            personalDetailsPage.YourDetails.HomeStatus = "Owner occupier";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed - full time";
            personalDetailsPage.EmploymentDetails.EmployerName = employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Administration";
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d MMM yyyy");
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "5";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "5";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "5000";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.WorkPhone = "01605741258";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "07200000000";
            personalDetailsPage.ContactingYou.EmailAddress = email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPage.PrivacyPolicy = true;
            CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            return this;

        }

        public ICustomerJourney FillAddressDetails()
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.PostCode = "se3 0sw";
            addressPage.LookupByPostCode();

            Thread.Sleep(10000);
            addressPage.GetAddressesDropDown();
            addressPage.SelectedAddress = "52 Ryculff Square, LONDON SE3 0SW";

            Thread.Sleep(10000);
            addressPage.GetAddressFieldsUK();
            addressPage.AddressPeriod = "3 to 4 years";
            CurrentPage = addressPage.Next() as AccountDetailsPage;
            return this;
        }

        public ICustomerJourney FillAccountDetails()
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.AccountDetailsSection.Password = Get.GetPassword();
            addressPage.AccountDetailsSection.PasswordConfirm = Get.GetPassword();
            addressPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            addressPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            CurrentPage = addressPage.Next() as PersonalBankAccountPage;
            return this;
        }

        public ICustomerJourney FillBankDetails()
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Barclays";
            bankDetailsPage.BankAccountSection.SortCode = "13-40-20";
            bankDetailsPage.BankAccountSection.AccountNumber = "63849203";
            bankDetailsPage.BankAccountSection.BankPeriod = "3 to 4 years";
            CurrentPage = bankDetailsPage.Next() as PersonalDebitCardPage;
            return this;
        }

        public ICustomerJourney FillDebitCardPage()
        {
            var cardDetailsPage = CurrentPage as PersonalDebitCardPage;
            cardDetailsPage.DebitCardSection.CardType = "Visa Electron";
            cardDetailsPage.DebitCardSection.CardNumber = "4444333322221111";
            cardDetailsPage.DebitCardSection.CardName = "mr " + LastName;
            cardDetailsPage.DebitCardSection.StartDate = "Jan/2010";
            cardDetailsPage.DebitCardSection.ExpiryDate = "Jan/2014";
            cardDetailsPage.DebitCardSection.CardSecurity = "000";
            cardDetailsPage.MobilePinVerification.Pin = "0000";
            CurrentPage = cardDetailsPage.Next() as ProcessingPage;
            return this;
        }

        public ICustomerJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }

        public ICustomerJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        public ICustomerJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        public ICustomerJourney GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }
    }
}
