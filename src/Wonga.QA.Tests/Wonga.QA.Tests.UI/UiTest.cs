using System;
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
            //Note - I will take these out for now since I dont know HOW to see the attachement
            var name = TestContext.CurrentContext.Test.Name;
            //TestLog.EmbedImage(name + ".Screen", Client.Screen());
            TestLog.AttachHtml(name + ".Source", Client.Source());
            Client.Dispose();
        }

        protected ProcessingPage WbL0Path(String middleNameMask = null)
        {
            //const string specialCompanyRegistrationNumberMask = "00000086";
            var middleName = middleNameMask ?? Data.RandomString(3, 15);
            var firstName = Data.RandomString(3, 15);
            var emailAddress = Data.GetEmail();

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

        protected ProcessingPage ZaL0Path(string employerNameMask = null)
        {
            var email = Data.GetEmail();
            string employerName = employerNameMask ?? Data.GetMiddleName();

            var homePage = Client.Home();

            var page = Client.Home();
            page.Sliders.HowMuch = "1000";
            page.Sliders.HowLong = "20";

            var personalDetailsPage = page.Sliders.Apply() as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.LastName = _lastName;
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

            var addressPage = personalDetailsPage.Submit() as AddressDetailsPage;
            addressPage.FlatNumber = "25";
            addressPage.Street = "high road";
            addressPage.Town = "Kuku";
            addressPage.County = "Province";
            addressPage.PostCode = "1234";
            addressPage.AddressPeriod = "2 to 3 years";

            var accountDetailsPage = addressPage.Next() as AccountDetailsPage;
            accountDetailsPage.AccountDetailsSection.Password = Data.GetPassword();
            accountDetailsPage.AccountDetailsSection.PasswordConfirm = Data.GetPassword();
            accountDetailsPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountDetailsPage.AccountDetailsSection.SecretAnswer = "Secret answer";

            var bankDetailsPage = accountDetailsPage.Next();
            bankDetailsPage.BankAccountSection.BankName = "Capitec";
            bankDetailsPage.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage.BankAccountSection.AccountNumber = "1234567";
            bankDetailsPage.BankAccountSection.BankPeriod = "2 to 3 years";
            bankDetailsPage.PinVerificationSection.Pin = "0000";

            return bankDetailsPage.Next() as ProcessingPage;
        }

        protected ProcessingPage CaL0Path(string employerNameMask = null)
        {
            var email = Data.GetEmail();
            string employerName = employerNameMask ?? Data.GetMiddleName();

            var page = Client.Home();
            page.Sliders.HowMuch = "111";
            page.Sliders.HowLong = "10"; 

            var personalDetailsPage = page.Sliders.Apply() as PersonalDetailsPage;
            personalDetailsPage.ProvinceSection.Province = "British Columbia";
            Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

            personalDetailsPage.YourName.FirstName = _firstName;
            personalDetailsPage.YourName.MiddleName = Data.GetMiddleName();
            personalDetailsPage.YourName.LastName = _lastName;
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

            var addressPage = personalDetailsPage.Submit() as AddressDetailsPage;
            addressPage.FlatNumber = "1403";
            addressPage.Street = "Edward";
            addressPage.Town = "Hearst";
            addressPage.PostCode = "V4F3A9";
            addressPage.AddressPeriod = "2 to 3 years";
            addressPage.PostOfficeBox = "C12345";

            addressPage.AccountDetailsSection.Password = Data.GetPassword();
            addressPage.AccountDetailsSection.PasswordConfirm = Data.GetPassword();
            addressPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            addressPage.AccountDetailsSection.SecretAnswer = "Secret answer";

            var bankDetailsPage = addressPage.Next() as PersonalBankAccountPage;
            bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
            bankDetailsPage.BankAccountSection.BranchNumber = "00011";
            bankDetailsPage.BankAccountSection.AccountNumber = "3023423";
            bankDetailsPage.BankAccountSection.BankPeriod = "More than 4 years";
            bankDetailsPage.PinVerificationSection.Pin = "0000";

            return bankDetailsPage.Next() as ProcessingPage;
        }
    }
}
