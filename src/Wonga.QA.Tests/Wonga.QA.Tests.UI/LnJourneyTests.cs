using System;
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
    class LnJourneyTests : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("QA-196")]
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

        [Test, AUT(AUT.Za), JIRA("QA-198")]
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

        [Test, AUT(AUT.Za), Pending("Example of ZA Ln journey")]
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

        [Test, AUT(AUT.Uk), JIRA("UK-1533", "UK-1902"), Pending("Fails due to bug UK-1902")]
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

        [Test, AUT(AUT.Uk), JIRA("UK-1533"), Pending("In Development")]
        public void UkFullLnJourneyTest2()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journeyL0.ApplyForLoan(200, 10)
                .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage();
                //.GoToMySummaryPage().CurrentPage as MySummaryPage;

            // pay
            Application application = customer.GetApplications()[0];
            application.RepayOnDueDate();

            // Ln journey
            //loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetLnJourney(Client.Home());
            var page = journeyLn.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }
        

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-199"), Pending("There is bug with resend pin on Za, and there is no option to change mobile phone in LN Journey on Ca")]
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
                 .WithMobileNumber("075" + Get.RandomLong(1000000, 9999999).ToString())
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
                    pageZA.SetNewMobilePhone = "075" + phone;
                    pageZA.ResendPinClick();
                    var smsZa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber("2775" + phone));
                    foreach (var sms in smsZa)
                    {
                        Console.WriteLine(sms.MessageText + "/" + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.com."));
                    }
                    Assert.AreEqual(2, smsZa.Count());
                    break;
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetLnJourney(Client.Home());
                    var pageCa = journeyCa.ApplyForLoan(200, 15)
                                   .SetName(name, surname).CurrentPage as ApplyPage;
                    //there is no option to change mobile phone number               
                    break;
            }
        }
    }
}
