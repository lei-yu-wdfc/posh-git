using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    public class HappyPath : UiTest
    {
        
        public void ZaHappyPath()
        {
            var email = Data.GetEmail();

            var homePage = Client.Home();

            var page = Client.Home();
            page.Sliders.HowMuch = "1000";
            page.Sliders.HowLong = "20";

            var personalDetailsPage = page.Sliders.Apply() as PersonalDetailsPage;
            Thread.Sleep(5000);
            personalDetailsPage.YourName.FirstName = "Monster";
            personalDetailsPage.YourName.LastName = Data.RandomString(10);
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = "5710300020087";
            personalDetailsPage.YourDetails.DateOfBirth = "30/Oct/1957";
            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.HomeStatus = "Owner occupier";
            personalDetailsPage.YourDetails.HomeLanguage = "English";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Employed - full time";
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "3000";
            personalDetailsPage.EmploymentDetails.EmployerName = "test:EmployedMask";
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Accountancy";
            personalDetailsPage.EmploymentDetails.EmploymentPosition = "Administration";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "9";
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "5";
            personalDetailsPage.EmploymentDetails.WorkPhone = "0123456789";
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.NextPayDate = "29 Feb 2012";
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
            
            var accountDetailsPage = addressPage.Next();
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

            var processingPage =  bankDetailsPage.Next() as ProcessingPage;

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();

            var dealDone = acceptedPage.Submit();
        }
    }
}
