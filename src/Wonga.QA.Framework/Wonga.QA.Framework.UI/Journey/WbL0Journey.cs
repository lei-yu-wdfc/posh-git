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

        public WbL0Journey AnswerEligibilityQuestions(bool activeCompany = true, bool director = true, bool guarantee = true, bool onlineAccess = true, bool resident = true, bool turnover = true, bool vat = true, bool debitCard = true, bool submit = true)
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

        public WbL0Journey FillAccountDetails()
        {
            var password = Get.GetPassword();
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = password;
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = password;
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "How deep the rabbit hole goes?";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Very";
            CurrentPage = accountDetailsPage.Next();
            return this;
        }

        public WbL0Journey FillBankDetails()
        {
            var personalBankAccountPage = CurrentPage as PersonalBankAccountPage;
            personalBankAccountPage.BankAccountSection.BankName = "AIB";
            personalBankAccountPage.BankAccountSection.SortCode = "13-40-20";
            personalBankAccountPage.BankAccountSection.AccountNumber = "63849203";
            personalBankAccountPage.BankAccountSection.BankPeriod = "More than 4 years";
            Thread.Sleep(3000);
            CurrentPage = personalBankAccountPage.Next() as PersonalDebitCardPage;
            return this;
        }

        public WbL0Journey FillCardDetails()
        {
            var personalDebitCardPage = CurrentPage as PersonalDebitCardPage;
            personalDebitCardPage.DebitCardSection.CardName = FirstName;
            personalDebitCardPage.DebitCardSection.CardNumber = "4444333322221111";
            personalDebitCardPage.DebitCardSection.CardSecurity = "666";
            personalDebitCardPage.DebitCardSection.CardType = "Visa Debit";
            personalDebitCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            personalDebitCardPage.DebitCardSection.StartDate = "Jan/2007";
            personalDebitCardPage.MobilePinVerification.Pin = "0000";
            CurrentPage = personalDebitCardPage.Next() as BusinessDetailsPage;
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

        public WbL0Journey AddAdditionalDirector()
        {
            var additionalDirectorsPage = CurrentPage as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = Get.RandomString(3, 15);
            addAdditionalDirectorPage.LastName = Get.RandomString(3, 15);
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;
            CurrentPage = addAdditionalDirectorPage.Done();
            return this;
        }

        public WbL0Journey EnterBusinessBankAccountDetails()
        {
            var businessBankAccountPage = CurrentPage as BusinessBankAccountPage;
            businessBankAccountPage.BankAccountSection.BankName = "Bank of Scotland Business Banking";
            Do.Until(() => businessBankAccountPage.BankAccountSection.SortCode = "93-86-11");
            businessBankAccountPage.BankAccountSection.AccountNumber = "07806039";
            Thread.Sleep(2000);
            businessBankAccountPage.BankAccountSection.BankPeriod = "2 to 3 years";
            CurrentPage = businessBankAccountPage.Next();
            return this;
        }

        public WbL0Journey EnterBusinessDebitCardDetails()
        {
            var businessPaymentCardPage = CurrentPage as BusinessDebitCardPage;
            businessPaymentCardPage.DebitCardSection.CardName = FirstName;
            businessPaymentCardPage.DebitCardSection.CardNumber = "4444333322221111";
            businessPaymentCardPage.DebitCardSection.CardSecurity = "666";
            businessPaymentCardPage.DebitCardSection.CardType = "Visa Debit";
            businessPaymentCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            businessPaymentCardPage.DebitCardSection.StartDate = "Jan/2007";

            businessPaymentCardPage.AddressDetailsSection.PostCode = "SW6 6PN";
            businessPaymentCardPage.AddressDetailsSection.LookupByPostCode();
            businessPaymentCardPage.AddressDetailsSection.GetAddressesDropDown();
            Do.Until(() => businessPaymentCardPage.AddressDetailsSection.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => businessPaymentCardPage.AddressDetailsSection.FlatNumber = "15");
            businessPaymentCardPage.AddressDetailsSection.District = "London";
            businessPaymentCardPage.AddressDetailsSection.County = "Camden";

            CurrentPage = businessPaymentCardPage.Next();
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
