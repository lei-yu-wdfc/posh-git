using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Journey
{
    class WbL0Journey : BaseL0Journey
    {
        private bool _activeCompany;
        private bool _director;
        private bool _guarantee;
        private bool _resident;
        private bool _debitCard;
        private string _additionalDirectorName;
        private string _additionalDirectorSurName;
        private string _additionalDirectorEmail;
        private string _businessBankAccount;
        private string _businessBankPeriod;
        private string _businessDebitCardNumber;
        private string _businessDebitCardSecurity;
        private string _businessDebitCardType;
        private string _businessDebitExpiryDate;
        private string _businessDebitStartDate;

        public WbL0Journey(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;

            _submit = true;

            _amount = 5500;
            _duration = 30;

            _activeCompany = true;
            _director = true;
            _guarantee = true;
            _resident = true;
            _debitCard = true;

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
            _addressPeriod = "3 to 4 years";

            _password = Get.GetPassword();

            _accountNumber = "63849203";
            _bankPeriod = "More than 4 years";
            _pin = "0000";

            _cardNumber = "4444333322221111";
            _cardSecurity = "666";
            _cardType = "Visa Debit";
            _expiryDate = "Jan/2015";
            _startDate = "Jan/2007";

            _additionalDirectorName = Get.RandomString(3, 15);
            _additionalDirectorSurName = Get.RandomString(3, 15);
            _additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());

            _businessBankAccount = "07806039";
            _businessBankPeriod = "2 to 3 years";

            _businessDebitCardNumber = "4444333322221111";
            _businessDebitCardSecurity = "666";
            _businessDebitCardType = "Visa Debit";
            _businessDebitExpiryDate = "Jan/2015";
            _businessDebitStartDate = "Jan/2007";

            journey.Add(typeof(HomePage), ApplyForLoan);
            journey.Add(typeof(EligibilityQuestionsPage), AnswerEligibilityQuestions);
            journey.Add(typeof(PersonalDetailsPage), FillPersonalDetails);
            journey.Add(typeof(AddressDetailsPage), FillAddressDetails);
            journey.Add(typeof(AccountDetailsPage), FillAccountDetails);
            journey.Add(typeof(PersonalBankAccountPage), FillBankDetails);
            journey.Add(typeof(PersonalDebitCardPage), FillCardDetails);
            journey.Add(typeof(BusinessDetailsPage), EnterBusinessDetails);
            journey.Add(typeof(AdditionalDirectorsPage), DeclineAddAdditionalDirector);
            journey.Add(typeof(BusinessBankAccountPage), EnterBusinessBankAccountDetails);
            journey.Add(typeof(BusinessDebitCardPage), EnterBusinessDebitCardDetails);
            journey.Add(typeof(ProcessingPage), WaitForApplyTermsPage);
            journey.Add(typeof(ApplyTermsPage), ApplyTerms);
            journey.Add(typeof(AcceptedPage), FillAcceptedPage);
            journey.Add(typeof(ReferPage), GoHomePage);
        }

        protected override BaseL0Journey ApplyForLoan(bool submit = true)
        {
            var homePage = CurrentPage as HomePage;
            homePage.CloseWbWelcomePopup();
            homePage.Sliders.HowMuch = _amount.ToString();
            homePage.Sliders.HowLong = _duration.ToString();
            CurrentPage = homePage.Sliders.Apply() as EligibilityQuestionsPage;
            return this;
        }

        protected override BaseL0Journey AnswerEligibilityQuestions(bool submit = true)
        {
            var eligibilityQuestionsPage = CurrentPage as EligibilityQuestionsPage;
            eligibilityQuestionsPage.CheckActiveCompany = _activeCompany;
            eligibilityQuestionsPage.CheckDirector = _director;
            eligibilityQuestionsPage.CheckGuarantee = _guarantee;
            eligibilityQuestionsPage.CheckResident = _resident;
            eligibilityQuestionsPage.CheckDebitCard = _debitCard;
            if (submit)
            {
                CurrentPage = eligibilityQuestionsPage.Submit();
            }
            return this;
        }

        protected override BaseL0Journey FillPersonalDetails(bool submit = true)
        {
            var personalDetailsPage = CurrentPage as PersonalDetailsPage;

            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = _middleName;
            personalDetailsPage.YourName.LastName = _lastName;
            personalDetailsPage.YourName.Title = _title;

            personalDetailsPage.YourDetails.Gender = _gender.ToString();
            personalDetailsPage.YourDetails.DateOfBirth = _dateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";

            personalDetailsPage.ContactingYou.HomePhoneNumber = "02071111234";
            personalDetailsPage.ContactingYou.CellPhoneNumber = _mobilePhone;
            personalDetailsPage.ContactingYou.EmailAddress = _email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = _email;

            personalDetailsPage.CanContact = "No";
            personalDetailsPage.PrivacyPolicy = true;
            if (submit)
            {
                CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            }
            return this;
        }

        protected override BaseL0Journey FillAddressDetails(bool submit = true)
        {
            var addressDetailsPage = CurrentPage as AddressDetailsPage;
            addressDetailsPage.PostCode = _postCode;
            addressDetailsPage.LookupByPostCode();
            addressDetailsPage.GetAddressesDropDown();
            Do.Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressDetailsPage.AddressPeriod = _addressPeriod);
            addressDetailsPage.HouseNumber = "1";
            addressDetailsPage.District = "Central";
            addressDetailsPage.County = "South Wales";
            if (submit)
            {
                switch (_addressPeriod)
                {
                    case ("Less than 4 months"):
                    case ("Between 4 months and 2 years"):
                        CurrentPage = addressDetailsPage.NextAddressLessThan2() as AddressDetailsPage;
                        var addressDetailsPage2NdEntry = CurrentPage as AddressDetailsPage;
                        addressDetailsPage2NdEntry.PostCode = _postCode;
                        addressDetailsPage2NdEntry.LookupByPostCode();
                        addressDetailsPage2NdEntry.GetAddressesDropDown();
                        Do.Until(() => addressDetailsPage2NdEntry.SelectedAddress = "101 Harbord Street, LONDON SW6 6PN");
                        Do.Until(() => addressDetailsPage2NdEntry.AddressPeriod = _addressPeriod);
                        CurrentPage = addressDetailsPage2NdEntry.Next() as AccountDetailsPage;
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

        protected override BaseL0Journey FillAccountDetails(bool submit = true)
        {
            var accountDetailsPage = CurrentPage as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = _password;
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = _password;
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "How deep the rabbit hole goes?";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Very";
            if (submit)
            {
                CurrentPage = accountDetailsPage.Next();
            }
            return this;
        }

        protected override BaseL0Journey FillBankDetails(bool submit = true)
        {
            var personalBankAccountPage = CurrentPage as PersonalBankAccountPage;
            personalBankAccountPage.BankAccountSection.BankName = "AIB";
            personalBankAccountPage.BankAccountSection.SortCode = "13-40-20";
            personalBankAccountPage.BankAccountSection.AccountNumber = _accountNumber;
            personalBankAccountPage.BankAccountSection.BankPeriod = _bankPeriod;
            Thread.Sleep(3000);
            if (submit)
            {
                CurrentPage = personalBankAccountPage.Next() as PersonalDebitCardPage;
            }
            return this;
        }

        protected override BaseL0Journey FillCardDetails(bool submit = true)
        {
            var personalDebitCardPage = CurrentPage as PersonalDebitCardPage;
            personalDebitCardPage.DebitCardSection.CardName = _firstName;
            personalDebitCardPage.DebitCardSection.CardNumber = _cardNumber;
            personalDebitCardPage.DebitCardSection.CardSecurity = _cardSecurity;
            personalDebitCardPage.DebitCardSection.CardType = _cardType;
            personalDebitCardPage.DebitCardSection.ExpiryDate = _expiryDate;
            personalDebitCardPage.DebitCardSection.StartDate = _startDate;
            personalDebitCardPage.MobilePinVerification.Pin = _pin;
            if (submit)
            {
                CurrentPage = personalDebitCardPage.Next() as BusinessDetailsPage;
            }
            return this;
        }

        protected override BaseL0Journey EnterBusinessDetails(bool submit = true)
        {
            var businessDetailsPage = CurrentPage as BusinessDetailsPage;
            businessDetailsPage.BusinessName = Get.RandomString(3, 15);
            businessDetailsPage.BusinessNumber = Get.RandomInt(9999999).ToString();
            CurrentPage = businessDetailsPage.Next();
            return this;
        }

        protected override BaseL0Journey DeclineAddAdditionalDirector(bool submit = true)
        {
            var additionalDirectorsPage = CurrentPage as AdditionalDirectorsPage;
            CurrentPage = additionalDirectorsPage.Next();
            return this;
        }

        protected override BaseL0Journey AddAdditionalDirector(bool submit = true)
        {
            var additionalDirectorsPage = CurrentPage as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = _additionalDirectorName;
            addAdditionalDirectorPage.LastName = _additionalDirectorSurName;
            addAdditionalDirectorPage.EmailAddress = _additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = _additionalDirectorEmail;
            if (submit)
            {
                CurrentPage = addAdditionalDirectorPage.Done();
            }
            return this;
        }

        protected override BaseL0Journey EnterBusinessBankAccountDetails(bool submit = true)
        {
            var businessBankAccountPage = CurrentPage as BusinessBankAccountPage;
            businessBankAccountPage.BankAccountSection.BankName = "Bank of Scotland Business Banking";
            Do.Until(() => businessBankAccountPage.BankAccountSection.SortCode = "93-86-11");
            businessBankAccountPage.BankAccountSection.AccountNumber = _businessBankAccount;
            Thread.Sleep(2000);
            businessBankAccountPage.BankAccountSection.BankPeriod = _businessBankPeriod;
            if (submit)
            {
                CurrentPage = businessBankAccountPage.Next();
            }
            return this;
        }

        protected override BaseL0Journey EnterBusinessDebitCardDetails(bool submit = true)
        {
            var businessPaymentCardPage = CurrentPage as BusinessDebitCardPage;
            businessPaymentCardPage.DebitCardSection.CardName = _firstName;
            businessPaymentCardPage.DebitCardSection.CardNumber = _businessDebitCardNumber;
            businessPaymentCardPage.DebitCardSection.CardSecurity = _businessDebitCardSecurity;
            businessPaymentCardPage.DebitCardSection.CardType = _businessDebitCardType;
            businessPaymentCardPage.DebitCardSection.ExpiryDate = _businessDebitExpiryDate;
            businessPaymentCardPage.DebitCardSection.StartDate = _businessDebitStartDate;

            businessPaymentCardPage.AddressDetailsSection.PostCode = _postCode;
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

        protected override BaseL0Journey WaitForApplyTermsPage(bool submit = true)
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<ApplyTermsPage>() as ApplyTermsPage;
            return this;
        }

        public override BaseL0Journey UpdateLoanDuration(bool submit = true)
        {
            var applyTermsPage = CurrentPage as ApplyTermsPage;
            applyTermsPage.EditDurationOfLoan("15");
            return this;
        }

        protected override BaseL0Journey ApplyTerms(bool submit = true)
        {
            var applyTermsPage = CurrentPage as ApplyTermsPage;
            CurrentPage = applyTermsPage.Next();
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
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
            CurrentPage = acceptedPage.Submit() as ReferPage;
            return this;
        }

        protected override BaseL0Journey GoHomePage(bool submit = true)
        {
            var referPage = CurrentPage as ReferPage;
            CurrentPage = referPage.GoHome();
            return this;
        }

        protected override BaseL0Journey GoToMySummaryPage(bool submit = true)
        {
            throw new NotImplementedException();
        }

        protected override BaseL0Journey WaitForAcceptedPage(bool submit = true)
        {
            throw new NotImplementedException();
        }

        #region Builder
        public override BaseL0Journey WithDeclineDecision()
        {
            journey.Remove(typeof(ProcessingPage));
            journey.Remove(typeof(ApplyTermsPage));
            journey.Remove(typeof(AcceptedPage));
            journey.Remove(typeof(ReferPage));
            journey.Add(typeof(ProcessingPage), WaitForDeclinedPage);
            return this;
        }

        public override BaseL0Journey WithAdditionalDirrector()
        {
            journey.Remove(typeof(AdditionalDirectorsPage));
            journey.Remove(typeof(BusinessBankAccountPage));
            journey.Remove(typeof(BusinessDebitCardPage));
            journey.Remove(typeof(ProcessingPage));
            journey.Remove(typeof(ApplyTermsPage));
            journey.Remove(typeof(AcceptedPage));
            journey.Remove(typeof(ReferPage));
            journey.Add(typeof(AdditionalDirectorsPage), AddAdditionalDirector);
            journey.Add(typeof(BusinessBankAccountPage), EnterBusinessBankAccountDetails);
            journey.Add(typeof(BusinessDebitCardPage), EnterBusinessDebitCardDetails);
            journey.Add(typeof(ProcessingPage), WaitForApplyTermsPage);
            journey.Add(typeof(ApplyTermsPage), ApplyTerms);
            journey.Add(typeof(AcceptedPage), FillAcceptedPage);
            journey.Add(typeof(ReferPage), GoHomePage);
            return this;
        }

        public override BaseL0Journey WithEligibilityQuestions(bool activeCompany = true, bool director = true, bool guarantee = true, bool resident = true, bool debitCard = true)
        {
            _activeCompany = activeCompany;
            _director = director;
            _guarantee = guarantee;
            _resident = resident;
            _debitCard = debitCard;
            return this;
        }

        public override BaseL0Journey WithAdditionalDirectorName(string additionalDirectorName)
        {
            _additionalDirectorName = additionalDirectorName;
            return this;
        }

        public override BaseL0Journey WithAdditionalDirectorSurName(string additionalDirectorSurName)
        {
            _additionalDirectorSurName = additionalDirectorSurName;
            return this;
        }

        public override BaseL0Journey WithAdditionalDirectorEmail(string additionalDirectorEmail)
        {
            _additionalDirectorEmail = additionalDirectorEmail;
            return this;
        }

        public override BaseL0Journey WithBusinessBankAccount(string businessBankAccount)
        {
            _businessBankAccount = businessBankAccount;
            return this;
        }

        public override BaseL0Journey WithBusinessBankPeriod(string businessBankPeriod)
        {
            _businessBankPeriod = businessBankPeriod;
            return this;
        }

        public override BaseL0Journey WithBusinessDebitCardNumber(string businessDebitCardNumber)
        {
            _businessDebitCardNumber = businessDebitCardNumber;
            return this;
        }

        public override BaseL0Journey WithBusinessDebitCardSecurity(string businessDebitCardSecurity)
        {
            _businessDebitCardSecurity = businessDebitCardSecurity;
            return this;
        }

        public override BaseL0Journey WithBusinessDebitCardType(string businessDebitCardType)
        {
            _businessDebitCardType = businessDebitCardType;
            return this;
        }

        public override BaseL0Journey WithBusinessDebitCardExpiryDate(string businessDebitExpiryDate)
        {
            _businessDebitExpiryDate = businessDebitExpiryDate;
            return this;
        }

        public override BaseL0Journey WithBusinessDebitCardStartDate(string businessDebitStartDate)
        {
            _businessDebitStartDate = businessDebitStartDate;
            return this;
        }
        #endregion
    }
}
