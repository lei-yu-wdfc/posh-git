using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.UI.Wb
{
    public class SeleniumTests : UiTest
    {
        [Test, AUT(AUT.Wb)]
        public void WbSeleniumL0ApplicationProcessAccepted()
        {
            //var processingPage = SmeL0Application();
            var processingPage = WbL0PathTest("TESTNoCheck");
            var acceptedPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.AcceptedPage)processingPage.WaitFor<Wonga.QA.Framework.UI.UiElements.Pages.Wb.AcceptedPage>();
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
            var dealDonePage = (Wonga.QA.Framework.UI.UiElements.Pages.Common.DealDonePage) acceptedPage.Submit();
            
        }

        //Note: This is not active yet
        //[Test, Explicit]
        public void WbSeleniumL0ApplicationProcessDeclined()
        {
            var processingPage = WbL0PathTest();
            //var acceptedPage = (Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage)processingPage.WaitFor<Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage>();
            //acceptedPage.SignTermsMainApplicant();
            //acceptedPage.SignTermsGuarantor();
        }

        private Wonga.QA.Framework.UI.UiElements.Pages.Common.ProcessingPage WbL0PathTest(String middleNameMask = null)
        {
            //Note:Use Data.Random
            const string graydonCompanyRegistrationNumberMask = "00000086";
            var random = new Random((Int32)DateTime.Now.Ticks);
            var middleName = GenerateRandomString();
            var firstName = GenerateRandomString();
            var emailAddress = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            if (!String.IsNullOrEmpty(middleNameMask))
                middleName = middleNameMask;

            var page = Client.Home();
            page.Sliders.HowMuch = "5550";
            page.Sliders.HowLong = "30";

            var eligibilityQuestionsPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.EligibilityQuestionsPage)page.Sliders.Apply();
            eligibilityQuestionsPage.CheckActiveCompany = true;
            eligibilityQuestionsPage.CheckDirector = true;
            eligibilityQuestionsPage.CheckGuarantee = true;
            eligibilityQuestionsPage.CheckOnlineAccess = true;
            eligibilityQuestionsPage.CheckResident = true;
            eligibilityQuestionsPage.CheckTurnover = true;
            eligibilityQuestionsPage.CheckVat = true;
            eligibilityQuestionsPage.CheckDebitCard = true;

            var personalDetailsPage = (Wonga.QA.Framework.UI.UiElements.Pages.Common.PersonalDetailsPage)eligibilityQuestionsPage.Submit();
            personalDetailsPage.YourName.FirstName = firstName;
            personalDetailsPage.YourName.MiddleName = middleName;
            personalDetailsPage.YourName.LastName = GenerateRandomString();
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

            var addressDetailsPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.AddressDetailsPage)personalDetailsPage.Submit();
            addressDetailsPage.PostCode = "SW6 6PN";
            Thread.Sleep(1000);
            addressDetailsPage.LookupByPostCode();
            Thread.Sleep(4000);
            addressDetailsPage.GetAddressesDropDown();
            addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN";
            Thread.Sleep(2000);
            addressDetailsPage.FlatNumber = "666";
            addressDetailsPage.District = "Central";
            addressDetailsPage.County = "South Wales";
            addressDetailsPage.AddressPeriod = "3 to 4 years";

            var accountDetailsPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.AccountDetailsPage)addressDetailsPage.Next();
            Thread.Sleep(1000);
            accountDetailsPage.AccountDetailsSection.Password = "PassW0rd";
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = "PassW0rd";
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "How deep the rabbit hole goes?";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Very";

            var personalBankAccountPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.PersonalBankAccountPage)accountDetailsPage.Next();
            Thread.Sleep(1000);
            personalBankAccountPage.BankAccountSection.BankName = "AIB";
            personalBankAccountPage.BankAccountSection.SortCode = "13-40-20";
            personalBankAccountPage.BankAccountSection.AccountNumber = "63849203";
            personalBankAccountPage.BankAccountSection.BankPeriod = "3 to 4 years";

            var personalDebitCardPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.PersonalDebitCardPage)personalBankAccountPage.Next();
            Thread.Sleep(1000);
            personalDebitCardPage.DebitCardSection.CardName = firstName;
            personalDebitCardPage.DebitCardSection.CardNumber = "4444333322221111";
            personalDebitCardPage.DebitCardSection.CardSecurity = "666";
            personalDebitCardPage.DebitCardSection.CardType = "Visa Debit";
            personalDebitCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            personalDebitCardPage.DebitCardSection.StartDate = "Jan/2007";
            personalDebitCardPage.MobilePinVerification.Pin = "0000";

            var businessDetailsPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.BusinessDetailsPage)personalDebitCardPage.Next();
            Thread.Sleep(1000);
            businessDetailsPage.BusinessName = GenerateRandomString();
            businessDetailsPage.BusinessNumber = String.Join(null, "123456789".OrderBy(a => random.Next()).Take(7));
            //businessDetailsPage.BusinessNumber = graydonCompanyRegistrationNumberMask;

            var additionalDirectorsPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.AdditionalDirectorsPage)businessDetailsPage.Next();
            Thread.Sleep(1000);

            var addAdditionalDirectorPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.AddAditionalDirectorsPage)additionalDirectorsPage.AddAditionalDirector();
            Thread.Sleep(1000);
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = GenerateRandomString();
            addAdditionalDirectorPage.LastName = GenerateRandomString();
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;

            var businessBankAccountPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.BusinessBankAccountPage)addAdditionalDirectorPage.Done();
            Thread.Sleep(1000);
            businessBankAccountPage.BankAccountSection.BankName = "Bank of Scotland Business Banking";
            businessBankAccountPage.BankAccountSection.SortCode = "93-86-11";
            businessBankAccountPage.BankAccountSection.AccountNumber = "07806039";
            businessBankAccountPage.BankAccountSection.BankPeriod = "2 to 3 years";

            var businessPaymentCardPage = (Wonga.QA.Framework.UI.UiElements.Pages.Wb.BusinessDebitCardPage) businessBankAccountPage.Next();
            businessPaymentCardPage.DebitCardSection.CardName = firstName;
            businessPaymentCardPage.DebitCardSection.CardNumber = "4444333322221111";
            businessPaymentCardPage.DebitCardSection.CardSecurity = "666";
            businessPaymentCardPage.DebitCardSection.CardType = "Visa Debit";
            businessPaymentCardPage.DebitCardSection.ExpiryDate = "Jan/2015";
            businessPaymentCardPage.DebitCardSection.StartDate = "Jan/2007";

            return businessPaymentCardPage.Next();

        }
        private static String GenerateRandomString()
        {
            var random = new Random((Int32)DateTime.Now.Ticks);
            return String.Join(null, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".OrderBy(a => random.Next()).Take(random.Next(3, 15)));
        }
    }

}
