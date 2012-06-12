using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Journey
{
    public class WbL0Journey
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }

        public BasePage CurrentPage { get; set; }

        public WbL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
            EmailAddress = Get.RandomEmail(); 
        }

        public WbL0Journey ApplyForLoan(int amount, int duration)
        {

            var homePage = CurrentPage as HomePage;
            homePage.CloseWbWelcomePopup();
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as EligibilityQuestionsPage;
            return this;
        }

        public WbL0Journey AnswerEligibilityQuestions(bool activeCompany = true, bool director = true, bool guarantee = true, bool resident = true, bool debitCard = true, bool submit = true)
        {
            var eligibilityQuestionsPage = CurrentPage as EligibilityQuestionsPage;
            eligibilityQuestionsPage.CheckActiveCompany = activeCompany;
            eligibilityQuestionsPage.CheckDirector = director;
            eligibilityQuestionsPage.CheckGuarantee = guarantee;
            eligibilityQuestionsPage.CheckResident = resident;
            eligibilityQuestionsPage.CheckDebitCard = debitCard;
            if (submit)
            {
                CurrentPage = eligibilityQuestionsPage.Submit();
            }
            return this;
        }

        public WbL0Journey FillPersonalDetails(string middleNameMask = null,string email = null, string phoneNumber = null, bool submit = true)
        {
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;

            personalDetailsPage.YourName.FirstName = FirstName;
            personalDetailsPage.YourName.MiddleName =  middleNameMask ?? Get.RandomString(3, 15);
            personalDetailsPage.YourName.LastName = LastName;
            personalDetailsPage.YourName.Title = "Mr";

            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1990";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";

            personalDetailsPage.ContactingYou.HomePhoneNumber = "02071111234";
            personalDetailsPage.ContactingYou.CellPhoneNumber = phoneNumber ?? Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = email ?? EmailAddress;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email ?? EmailAddress;

            personalDetailsPage.CanContact = "No";
            personalDetailsPage.PrivacyPolicy = true;
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        public WbL0Journey FillAddressDetails(string posteCode = null, string addressPeriod = null, bool submit = true)
        {
            var addressDetailsPage = CurrentPage as AddressDetailsPage;
            addressDetailsPage.PostCode = posteCode ?? "SW6 6PN";
            addressDetailsPage.LookupByPostCode();
            addressDetailsPage.GetAddressesDropDown();
            Do.Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressDetailsPage.AddressPeriod = addressPeriod ?? "3 to 4 years");
            addressDetailsPage.HouseNumber = "1";
            addressDetailsPage.District = "Central";
            addressDetailsPage.County = "South Wales";
            if (submit)
            {
                switch (addressPeriod)
                {
                    case ("Less than 4 months"):
                    case ("Between 4 months and 2 years"):
                        CurrentPage = addressDetailsPage.NextAddressLessThan2() as AddressDetailsPage;
                        break;
                    case ("2 to 3 years"):
                    case ("3 to 4 years"):
                    case ("More than 4 years"):
                        CurrentPage = addressDetailsPage.Next() as AccountDetailsPage;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return this;
        }

        public WbL0Journey EnterAdditionalAddressDetails(string posteCode = null, string addressPeriod = null, bool submit = true)
        {
            var addressDetailsPage2NdEntry = CurrentPage as AddressDetailsPage;
            addressDetailsPage2NdEntry.PostCode = posteCode ?? "SW6 6PN";
            addressDetailsPage2NdEntry.LookupByPostCode();
            addressDetailsPage2NdEntry.GetAddressesDropDown();
            Do.Until(() => addressDetailsPage2NdEntry.SelectedAddress = "101 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressDetailsPage2NdEntry.AddressPeriod = addressPeriod ?? "2 to 3 years");
            if (submit)
            {
                CurrentPage = addressDetailsPage2NdEntry.Next() as AccountDetailsPage;
            }
            return this;
        }

        public WbL0Journey FillAccountDetails(string password = null, bool submit = true)
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = password ?? Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = password ?? Get.GetPassword();
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "How deep the rabbit hole goes?";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Very";
            if (submit)
            {
                CurrentPage = accountDetailsPage.Next();
            }
            return this;
        }

        public WbL0Journey FillBankDetails(string accountNumber = null, string bankPeriod = null, bool submit = true)
        {
            var personalBankAccountPage = CurrentPage as PersonalBankAccountPage;
            personalBankAccountPage.BankAccountSection.BankName = "AIB";
            personalBankAccountPage.BankAccountSection.SortCode = "13-40-20";
            personalBankAccountPage.BankAccountSection.AccountNumber = accountNumber ?? "63849203";
            personalBankAccountPage.BankAccountSection.BankPeriod = bankPeriod ?? "More than 4 years";
            Thread.Sleep(3000);
            if (submit)
            {
                CurrentPage = personalBankAccountPage.Next() as PersonalDebitCardPage;
            }
            return this;
        }

        public WbL0Journey FillCardDetails(string cardNumber = null, string cardSecurity = null, string cardType = null, string expiryDate = null, string startDate = null, string pin = null, bool submit = true)
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
                CurrentPage = personalDebitCardPage.Next() as BusinessDetailsPage;
            }
            return this;
        }

        public WbL0Journey EnterBusinessDetails()
        {
            var businessDetailsPage = CurrentPage as BusinessDetailsPage;
            businessDetailsPage.BusinessName = Get.RandomString(3, 15);
            businessDetailsPage.BusinessNumber = Get.RandomInt(9999999).ToString();
            CurrentPage = businessDetailsPage.Next();
            return this;
        }

        public WbL0Journey DeclineAddAdditionalDirector()
        {
            var additionalDirectorsPage = CurrentPage as AdditionalDirectorsPage;
            CurrentPage = additionalDirectorsPage.Next();
            return this;
        }

        public WbL0Journey AddAdditionalDirector(string firstName = null, string lastName = null, string email = null, bool submit = true)
        {
            var additionalDirectorsPage = CurrentPage as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = firstName ?? Get.RandomString(3, 15);
            addAdditionalDirectorPage.LastName = lastName ?? Get.RandomString(3, 15);
            addAdditionalDirectorPage.EmailAddress = email ?? additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = email ?? additionalDirectorEmail;
            if (submit)
            {
                CurrentPage = addAdditionalDirectorPage.Done();
            }
            return this;
        }

        public WbL0Journey EnterBusinessBankAccountDetails(string accountNumber = null, string bankPeriod = null, bool submit = true)
        {
            var businessBankAccountPage = CurrentPage as BusinessBankAccountPage;
            businessBankAccountPage.BankAccountSection.BankName = "Bank of Scotland Business Banking";
            Do.Until(() => businessBankAccountPage.BankAccountSection.SortCode = "93-86-11");
            businessBankAccountPage.BankAccountSection.AccountNumber = accountNumber ?? "07806039";
            Thread.Sleep(2000);
            businessBankAccountPage.BankAccountSection.BankPeriod = bankPeriod ?? "2 to 3 years";
            if (submit)
            {
                CurrentPage = businessBankAccountPage.Next();
            }
            return this;
        }

        public WbL0Journey EnterBusinessDebitCardDetails(string cardNumber = null, string cardSecurity = null, string cardType = null, string expiryDate = null, string startDate = null, string postCode = null, bool submit = true)
        {
            var businessPaymentCardPage = CurrentPage as BusinessDebitCardPage;
            businessPaymentCardPage.DebitCardSection.CardName = FirstName;
            businessPaymentCardPage.DebitCardSection.CardNumber = cardNumber ?? "4444333322221111";
            businessPaymentCardPage.DebitCardSection.CardSecurity = cardSecurity ?? "666";
            businessPaymentCardPage.DebitCardSection.CardType = cardType ?? "Visa Debit";
            businessPaymentCardPage.DebitCardSection.ExpiryDate = expiryDate ?? "Jan/2015";
            businessPaymentCardPage.DebitCardSection.StartDate = startDate ?? "Jan/2007";

            businessPaymentCardPage.AddressDetailsSection.PostCode = postCode ?? "SW6 6PN";
            businessPaymentCardPage.AddressDetailsSection.LookupByPostCode();
            businessPaymentCardPage.AddressDetailsSection.GetAddressesDropDown();
            Do.Until(() => businessPaymentCardPage.AddressDetailsSection.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => businessPaymentCardPage.AddressDetailsSection.FlatNumber = "15");
            businessPaymentCardPage.AddressDetailsSection.District = "London";
            businessPaymentCardPage.AddressDetailsSection.County = "Camden";
            if (submit)
            {
                CurrentPage = businessPaymentCardPage.Next();
            }
            return this;

        }

        public WbL0Journey WaitForApplyTermsPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<ApplyTermsPage>() as ApplyTermsPage;
            return this;
        }

        public WbL0Journey UpdateLoanDuration()
        {
            var applyTermsPage = CurrentPage as ApplyTermsPage;
            applyTermsPage.EditDurationOfLoan("15");
            return this;
        }

        public WbL0Journey ApplyTerms()
        {
            var applyTermsPage = CurrentPage as ApplyTermsPage;
            CurrentPage = applyTermsPage.Next();
            return this;
        }

        public WbL0Journey WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        public WbL0Journey FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
            CurrentPage = acceptedPage.Submit() as ReferPage;
            return this;

        }

        public WbL0Journey GoHomePage()
        {
            var referPage = CurrentPage as ReferPage;
            CurrentPage = referPage.GoHome();
            return this;
        }


    }
}
