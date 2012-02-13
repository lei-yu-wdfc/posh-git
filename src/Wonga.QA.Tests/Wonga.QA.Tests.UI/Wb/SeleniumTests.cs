using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;

namespace Wonga.QA.Tests.UI.Wb
{
    [Category("Wb")]
    public class SeleniumTests : UiTest
    {
        [Test]
        public void SmeL0ApplicationProcessAccepted()
        {
            var processingPage = SmeL0Application();
            var acceptedPage = (Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage)processingPage.WaitFor<Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage>();
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
        }

        [Test]
        public void SmeL0ApplicationProcessDeclined()
        {
            var processingPage = SmeL0Application();
            var acceptedPage = (Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage)processingPage.WaitFor<Wonga.QA.Framework.UI.Pages.Wb.AcceptedPage>();
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
        }

        private Wonga.QA.Framework.UI.Pages.ProcessingPage SmeL0Application(String middleNameMask = null)
        {
            const string graydonCompanyRegistrationNumberMask = "00000086";
            var random = new Random((Int32)DateTime.Now.Ticks);
            var middleName = GenerateRandomString();
            var firstName = GenerateRandomString();
            var emailAddress = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            if (!String.IsNullOrEmpty(middleNameMask))
                middleName = middleNameMask;

            var page = Client.Home();
            page.Sliders.HowMuch = "5000";
            page.Sliders.HowLong = "30";

            var eligibilityQuestionsPage = (Wonga.QA.Framework.UI.Pages.Wb.EligibilityQuestionsPage)page.Sliders.Apply();
            eligibilityQuestionsPage.CheckActiveCompany = true;
            eligibilityQuestionsPage.CheckDirector = true;
            eligibilityQuestionsPage.CheckGuarantee = true;
            eligibilityQuestionsPage.CheckOnlineAccess = true;
            eligibilityQuestionsPage.CheckResident = true;
            eligibilityQuestionsPage.CheckTurnover = true;
            eligibilityQuestionsPage.CheckVat = true;

            var personalDetailsPage = (Wonga.QA.Framework.UI.Pages.Wb.PersonalDetailsPage)eligibilityQuestionsPage.Submit();
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

            var addressDetailsPage = (Wonga.QA.Framework.UI.Pages.Wb.AddressDetailsPage)personalDetailsPage.Submit();
            addressDetailsPage.PostCode = "SW6 6PN";
            Thread.Sleep(1000);
            addressDetailsPage.LookupByPostCode();
            Thread.Sleep(6000);
            addressDetailsPage.GetAddressesDropDown();
            addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN";
            Thread.Sleep(3000);
            addressDetailsPage.FlatNumber = "666";
            addressDetailsPage.District = "Central";
            addressDetailsPage.County = "South Wales";
            addressDetailsPage.AddressPeriod = "3 to 4 years";

            var businessAccountDetailsPage = (Wonga.QA.Framework.UI.Pages.Wb.BusinessAccountDetailsPage)addressDetailsPage.Next();
            Thread.Sleep(1000);
            businessAccountDetailsPage.Password = "PassW0rd";
            businessAccountDetailsPage.PasswordConfirm = "PassW0rd";
            businessAccountDetailsPage.SecretQuestion = "How deep the rabbit hole goes?";
            businessAccountDetailsPage.SecretAnswer = "Very";

            var bankAccountDetailsPage = (Wonga.QA.Framework.UI.Pages.Wb.BankAccountDetailsPage)businessAccountDetailsPage.Next();
            Thread.Sleep(1000);
            bankAccountDetailsPage.BankName = "AIB";
            bankAccountDetailsPage.SortCode = "00-00-00";
            bankAccountDetailsPage.AccountNumber = "00000000";
            bankAccountDetailsPage.BankPeriod = "3 to 4 years";

            var debitCardDetailsPage = (Wonga.QA.Framework.UI.Pages.Wb.DebitCardDetailsPage)bankAccountDetailsPage.Next();
            Thread.Sleep(1000);
            debitCardDetailsPage.CardName = firstName;
            debitCardDetailsPage.CardNumber = "4444333322221111";
            debitCardDetailsPage.CardSecurity = "666";
            debitCardDetailsPage.CardType = "Visa Debit";
            debitCardDetailsPage.ExpiryDate = "Jan/2015";
            debitCardDetailsPage.StartDate = "Jan/2007";
            debitCardDetailsPage.MobilePinVerification.Pin = "0000";

            var businessDetailsPage = (Wonga.QA.Framework.UI.Pages.Wb.BusinessDetailsPage)debitCardDetailsPage.Next();
            Thread.Sleep(1000);

            businessDetailsPage.BusinessName = GenerateRandomString();
            businessDetailsPage.BusinessNumber = String.Join(null, "123456789".OrderBy(a => random.Next()).Take(7));
            //businessDetailsPage.BusinessNumber = graydonCompanyRegistrationNumberMask;

            var additionalDirectorsPage = (Wonga.QA.Framework.UI.Pages.Wb.AdditionalDirectorsPage)businessDetailsPage.Next();
            Thread.Sleep(1000);

            var addAdditionalDirectorPage = (Wonga.QA.Framework.UI.Pages.Wb.AddAditionalDirectorsPage)additionalDirectorsPage.AddAditionalDirector();
            Thread.Sleep(1000);
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = GenerateRandomString();
            addAdditionalDirectorPage.LastName = GenerateRandomString();
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;

            var mainBusinessBankAccountPage = (Wonga.QA.Framework.UI.Pages.Wb.MainBusinessBankAccountPage)addAdditionalDirectorPage.Done();
            Thread.Sleep(1000);
            mainBusinessBankAccountPage.BankName = "Bank of Scotland Business Banking";
            mainBusinessBankAccountPage.SortCode = "00-00-00";
            mainBusinessBankAccountPage.AccountNumber = "00000000";
            mainBusinessBankAccountPage.BankPeriod = "2 to 3 years";

            return (Wonga.QA.Framework.UI.Pages.ProcessingPage)mainBusinessBankAccountPage.Finish();
        }
        private static String GenerateRandomString()
        {
            var random = new Random((Int32)DateTime.Now.Ticks);
            return String.Join(null, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".OrderBy(a => random.Next()).Take(random.Next(3, 15)));
        }
    }
}
