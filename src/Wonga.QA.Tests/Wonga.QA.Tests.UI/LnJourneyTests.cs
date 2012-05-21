﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
	[TestFixture, Parallelizable(TestScope.All)]
    class LnJourneyTests : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("QA-196"), Pending("ZA-2510"), Category(TestCategories.Smoke)]
        public void LnCustomerTakesNewLoanAndChangesTheMobilePhoneThenChangesShouldBeReflected()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            var oldMobilePhone = myPersonalDetails.GetMobilePhone;
            var homePage = Client.Home();

            var journey = JourneyFactory.GetLnJourney(homePage);
            var applyPage = journey.ApplyForLoan(200, 10)
                .SetName(name, surname).CurrentPage as ApplyPage;
            applyPage.SetNewMobilePhone = "0111111111";
            applyPage.ApplicationSection.SetPin = "0000";
            journey.CurrentPage = applyPage.Submit() as ProcessingPage;
            mySummaryPage = journey.WaitForAcceptedPage()
                                .FillAcceptedPage()
                                .GoToMySummaryPage()
                                .CurrentPage as MySummaryPage;
            myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            Assert.AreNotEqual(oldMobilePhone, myPersonalDetails.GetMobilePhone);
            Assert.AreEqual("0111111111", myPersonalDetails.GetMobilePhone);
            var mobileFromDb =
                Drive.Data.Comms.Db.CustomerDetails.FindAllBy(AccountId: customer.Id).FirstOrDefault().MobilePhone;

            Assert.AreEqual("0111111111", mobileFromDb);
        }

        [Test, AUT(AUT.Za), JIRA("QA-198"), Pending()]
        public void LnCustomerChangesMobilePhoneAndEntersInvalidPinShouldNotBeAbleToTakeLoan()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.ApplyForLoan(200, 10).CurrentPage as ApplyPage;

            applyPage.SetNewMobilePhone = "0111111111";
            applyPage.ApplicationSection.SetPin = "1111";
            try
            {
                applyPage.Submit();
            }
            catch (AssertionFailureException exception)
            {
                Assert.IsTrue(exception.Message.Contains("The SMS PIN you entered was incorrect"));
            }
        }

        [Test, AUT(AUT.Ca), Pending("Example of CA Ln journey")]
        public void CaFullLnJourneyTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.ApplyForLoan(200, 10)
                           .SetName(name, surname)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;


        }

        [Test, AUT(AUT.Za)]
        public void ZaFullLnJourneyTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1533", "UK-1902"), Pending("Disabled as failing during build testing. To be checked.")]
        public void FullLnJourneyTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetLnJourney(Client.Home());
            var page = ((UkLnJourney)journeyLn.ApplyForLoan(200, 10))
                           .FillApplicationDetailsWithNewMobilePhone()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), JIRA("UK-886"), MultipleAsserts]
        public void ExistingMobilePhoneNumberNotAccepted()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journeyL0.ApplyForLoan(200, 10)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage();

            var customer = new Customer(Guid.Parse(Drive.Api.Queries.Post(new GetAccountQuery { Login = email, Password = Get.GetPassword() }).Values["AccountId"].Single()));
            var application = customer.GetApplication();
            
            var mobileNumber = customer.GetCustomerMobileNumber();
            

            // Repay
            application.RepayOnDueDate();

           // Ln journey
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.ApplyForLoan(200, 10);

            var applyPage = page.CurrentPage as ApplyPage;
            applyPage.SetIncorrectMobilePhone = mobileNumber;

            Assert.IsTrue(applyPage.IsMobilePhonePopupCancelButtonEnabled(), "Cancel button is not enabled");
            Assert.IsTrue(applyPage.IsMobilePhonePopupSaveButtonEnabled(), "Save button is not disabled");
            Assert.IsTrue(applyPage.IsPhoneNumberNotChangedMessageVisible(), "Message that mobile phone number has not changed is not dispalyed");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1533"), Pending("Disabled as failing during build testing. To be checked.")]
        public void L0LnJourneyTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journeyL0.ApplyForLoan(200, 10)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage();
            
            var customer = new Customer(Guid.Parse(Drive.Api.Queries.Post(new GetAccountQuery { Login = email, Password = Get.GetPassword() }).Values["AccountId"].Single()));
            var application = customer.GetApplication();

            // Repay
            application.RepayOnDueDate();

            // Ln journey
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }


        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-199"), Pending("ZA-2510"), Category(TestCategories.Smoke)]
        public void LoggedCustomerWithoutLoanAppliesNewLoanChangesMobilePhoneAndClicksResendPinItShouldBeResent()
        {
            string email = Get.RandomEmail();
            string name = Get.RandomString(3, 10);
            string surname = Get.RandomString(3, 10);
            string phone = Get.RandomLong(1000000, 9999999).ToString();
            Customer customer = CustomerBuilder
                 .New()
                 .WithForename(name)
                 .WithSurname(surname)
                 .WithEmailAddress(email)
                 .WithMobileNumber("077009" + Get.RandomLong(10000, 99999))
                 .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
           var loginPage = Client.Login();
            loginPage.LoginAs(email);
            switch (Config.AUT)
            {
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetLnJourney(Client.Home());
                    var pageZA = journeyZa.ApplyForLoan(200, 20).CurrentPage as ApplyPage;
                    pageZA.SetNewMobilePhone = "077009" + Get.RandomLong(10000, 99999);
                    pageZA.ResendPinClick();
                    var smsZa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber("2775" + phone));
                    foreach (var sms in smsZa)
                    {
                        Console.WriteLine(sms.MessageText + "/" + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.com."));
                    }
                   // Assert.AreEqual(2, smsZa.Count());
                    break;
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetLnJourney(Client.Home());
                    var pageCa = journeyCa.ApplyForLoan(200, 25)
                                   .SetName(name, surname).CurrentPage as ApplyPage;
                    pageCa.SetNewMobilePhone = "077009" + Get.RandomLong(10000, 99999);
                    pageCa.ResendPinClick();
                    var smsCa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber("175" + phone));
                    foreach (var sms in smsCa)
                    {
                        Console.WriteLine(sms.MessageText + "/" + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.ca."));
                    }
                    Assert.AreEqual(2, smsCa.Count());            
                    break;
            }
        }
    }
}
