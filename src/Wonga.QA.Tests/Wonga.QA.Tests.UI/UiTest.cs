﻿using System;
using System.Threading;
using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Tests.Ui
{
    public abstract class UiTest
    {
        public UiClient Client;
        protected String _firstName;
        protected String _lastName;

        [SetUp]
        public void SetUp()
        {
            Client = new UiClient();
            _firstName = Data.GetName();
            _lastName = Data.RandomString(10);
        }

        [TearDown]
        public void TearDown()
        {
            var name = TestContext.CurrentContext.Test.Name;
            TestLog.EmbedImage(name + ".Screen", Client.Screen());
            TestLog.AttachHtml(name + ".Source", Client.Source());
            Client.Dispose();
        }

        protected ProcessingPage WbL0Path(String middleNameMask = null)
        {
            //const string specialCompanyRegistrationNumberMask = "00000086";
            var middleName = middleNameMask ?? Data.RandomString(3, 15);
            var firstName = Data.RandomString(3, 15);
            var emailAddress = Data.RandomEmail();

            var page = Client.Home();
            page.Sliders.HowMuch = "5550";
            page.Sliders.HowLong = "30";

            var eligibilityQuestionsPage = page.Sliders.Apply() as EligibilityQuestionsPage;
            eligibilityQuestionsPage.CheckActiveCompany = true;
            eligibilityQuestionsPage.CheckDirector = true;
            eligibilityQuestionsPage.CheckGuarantee = true;
            eligibilityQuestionsPage.CheckOnlineAccess = true;
            eligibilityQuestionsPage.CheckResident = true;
            eligibilityQuestionsPage.CheckTurnover = true;
            eligibilityQuestionsPage.CheckVat = true;
            eligibilityQuestionsPage.CheckDebitCard = true;

            var personalDetailsPage = eligibilityQuestionsPage.Submit();
            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = middleName;
            personalDetailsPage.YourName.LastName = _lastName;
            personalDetailsPage.YourName.Title = "Mr";

            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1990";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";

            personalDetailsPage.ContactingYou.HomePhoneNumber = "02071111234";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "07712345678";
            personalDetailsPage.ContactingYou.EmailAddress = emailAddress;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = emailAddress;

            personalDetailsPage.CanContact = "No";
            personalDetailsPage.PrivacyPolicy = true;

            var addressDetailsPage = personalDetailsPage.Submit() as AddressDetailsPage;
            addressDetailsPage.PostCode = "SW6 6PN";
            Thread.Sleep(1000);
            addressDetailsPage.LookupByPostCode();
            Thread.Sleep(4000);
            addressDetailsPage.GetAddressesDropDown();
            Do.Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressDetailsPage.FlatNumber = "666");
            addressDetailsPage.District = "Central";
            addressDetailsPage.County = "South Wales";
            addressDetailsPage.AddressPeriod = "3 to 4 years";

            var accountDetailsPage = addressDetailsPage.Next() as AccountDetailsPage;
            Thread.Sleep(1000);
            accountDetailsPage.AccountDetailsSection.Password = "PassW0rd";
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = "PassW0rd";
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "How deep the rabbit hole goes?";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Very";

            var personalBankAccountPage = accountDetailsPage.Next();
            Thread.Sleep(1000);
            personalBankAccountPage.BankAccountSection.BankName = "AIB";
            personalBankAccountPage.BankAccountSection.SortCode = "13-40-20";
            personalBankAccountPage.BankAccountSection.AccountNumber = "63849203";
            personalBankAccountPage.BankAccountSection.BankPeriod = "3 to 4 years";

            var personalDebitCardPage = personalBankAccountPage.Next() as PersonalDebitCardPage;
            Thread.Sleep(1000);
            personalDebitCardPage.DebitCardSection.CardName = firstName;
            personalDebitCardPage.DebitCardSection.CardNumber = "4444333322221111";
            personalDebitCardPage.DebitCardSection.CardSecurity = "666";
            personalDebitCardPage.DebitCardSection.CardType = "Visa Debit";
            personalDebitCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            personalDebitCardPage.DebitCardSection.StartDate = "Jan/2007";
            personalDebitCardPage.MobilePinVerification.Pin = "0000";

            var businessDetailsPage = personalDebitCardPage.Next() as BusinessDetailsPage;
            Thread.Sleep(1000);
            businessDetailsPage.BusinessName = Data.RandomString(3, 15);
            businessDetailsPage.BusinessNumber = Data.RandomInt(9999999).ToString();
            //businessDetailsPage.BusinessNumber = graydonCompanyRegistrationNumberMask;

            var additionalDirectorsPage = businessDetailsPage.Next();
            Thread.Sleep(1000);

            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            Thread.Sleep(1000);
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = Data.RandomString(3, 15);
            addAdditionalDirectorPage.LastName = Data.RandomString(3, 15);
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;

            var businessBankAccountPage = addAdditionalDirectorPage.Done();
            Thread.Sleep(1000);
            businessBankAccountPage.BankAccountSection.BankName = "Bank of Scotland Business Banking";
            businessBankAccountPage.BankAccountSection.SortCode = "93-86-11";
            businessBankAccountPage.BankAccountSection.AccountNumber = "07806039";
            businessBankAccountPage.BankAccountSection.BankPeriod = "2 to 3 years";

            var businessPaymentCardPage = businessBankAccountPage.Next();
            businessPaymentCardPage.DebitCardSection.CardName = firstName;
            businessPaymentCardPage.DebitCardSection.CardNumber = "4444333322221111";
            businessPaymentCardPage.DebitCardSection.CardSecurity = "666";
            businessPaymentCardPage.DebitCardSection.CardType = "Visa Debit";
            businessPaymentCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            businessPaymentCardPage.DebitCardSection.StartDate = "Jan/2007";

            return businessPaymentCardPage.Next();

        }

    }
}
