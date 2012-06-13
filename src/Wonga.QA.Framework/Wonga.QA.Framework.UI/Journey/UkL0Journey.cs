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
    class UkL0Journey : IL0ConsumerJourney
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; } //not used yet
        public DateTime DateOfBirth { get; set; } //Not used yet
        public BasePage CurrentPage { get; set; }
        public String Email { get; set; }

        public UkL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
            Email = Get.RandomEmail();
        }
        public IL0ConsumerJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            return this;
        }

        public IL0ConsumerJourney FillPersonalDetails(string firstName = null, string lastName = null, string middleNameMask = null, string employerNameMask = null, string email = null, string mobilePhone = null, bool submit = true)
        {
            string employerName = employerNameMask ?? Get.GetMiddleName();
            string middleName = middleNameMask ?? Get.GetMiddleName();
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = firstName ?? FirstName;
            personalDetailsPage.YourName.MiddleName = middleName;
            personalDetailsPage.YourName.LastName =  lastName ?? LastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
            personalDetailsPage.YourDetails.Gender = "Male";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
            personalDetailsPage.EmploymentDetails.EmployerName = employerName;
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Finance";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Engineering";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "0";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.EmploymentDetails.WorkPhone = "02087111222";
            personalDetailsPage.ContactingYou.CellPhoneNumber = mobilePhone ?? Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = email ?? Email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email ?? Email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillAddressDetails(string postcode = null, string addresPeriod = null, bool submit = true)
        {
            var addressPage = CurrentPage as AddressDetailsPage;
            addressPage.PostCodeLookup = postcode ?? "SW6 6PN";
            addressPage.LookupByPostCode();
            addressPage.GetAddressesDropDown();
            Do.Until(() => addressPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressPage.HouseNumber = "666");
            addressPage.AddressPeriod = addresPeriod ?? "3 to 4 years";
            if (submit)
            {
                CurrentPage = addressPage.Next();
            }
            return this;
        }

        public IL0ConsumerJourney FillAccountDetails(string password = null, bool submit = true)
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = password ?? Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = password ?? Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";
            if (submit)
            {
                CurrentPage = accountDetailsPage.Next() as PersonalBankAccountPage;
            }
            return this;
        }

        public IL0ConsumerJourney FillBankDetails(string accountNumber = null, string bankPeriod = null, string pin = null, bool submit = true)
        {
            var bankDetailsPage = CurrentPage as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "AIB";
            bankDetailsPage.BankAccountSection.SortCode = "13-40-20";
            bankDetailsPage.BankAccountSection.AccountNumber = accountNumber ?? "63849203";
            bankDetailsPage.BankAccountSection.BankPeriod = bankPeriod ?? "3 to 4 years";
            if (submit)
            {
                CurrentPage = bankDetailsPage.Next();
            }
            return this;
        }

        public IL0ConsumerJourney FillCardDetails(string cardNumber = null, string cardSecurity = null, string cardType = null, string expiryDate = null, string startDate = null, string pin = null, bool submit = true)
        {
            var personalDebitCardPage = CurrentPage as PersonalDebitCardPage;

            personalDebitCardPage.DebitCardSection.CardName = FirstName;
            personalDebitCardPage.DebitCardSection.CardNumber = cardNumber ?? "4444333322221111";
            personalDebitCardPage.DebitCardSection.CardSecurity = cardSecurity ?? "666";
            personalDebitCardPage.DebitCardSection.CardType = cardType ?? "Visa Debit";
            personalDebitCardPage.DebitCardSection.ExpiryDate = expiryDate ?? "Jan/2015";
            personalDebitCardPage.DebitCardSection.StartDate = startDate ?? "Jan/2007";
            personalDebitCardPage.MobilePinVerification.Pin = pin ?? "0000";
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
    }
}
