using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    internal class LnJourneyTests : UiTest
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

        [Test, AUT(AUT.Uk), JIRA("UK-1533", "UK-1902")]
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

            var customer =
                new Customer(
                    Guid.Parse(
                        Drive.Api.Queries.Post(new GetAccountQuery { Login = email, Password = Get.GetPassword() }).Values
                            ["AccountId"].Single()));
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
            Assert.IsTrue(applyPage.IsPhoneNumberNotChangedMessageVisible(),
                          "Message that mobile phone number has not changed is not dispalyed");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1533")]
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

            var customer =
                new Customer(
                    Guid.Parse(
                        Drive.Api.Queries.Post(new GetAccountQuery { Login = email, Password = Get.GetPassword() }).Values
                            ["AccountId"].Single()));
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


        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-199"), Pending("ZA-2510")]
        public void LoggedCustomerWithoutLoanAppliesNewLoanChangesMobilePhoneAndClicksResendPinItShouldBeResent()
        {
            string email = Get.RandomEmail();
            string name = Get.RandomString(3, 10);
            string surname = Get.RandomString(3, 10);
            string oldphone = "077009" + Get.RandomLong(1000, 9999).ToString();
            Customer customer = CustomerBuilder
                .New()
                .WithForename(name)
                .WithSurname(surname)
                .WithEmailAddress(email)
                .WithMobileNumber(oldphone)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            string phone = "077009" + Get.RandomLong(1000, 9999).ToString();
            var loginPage = Client.Login();
            loginPage.LoginAs(email);
            switch (Config.AUT)
            {
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetLnJourney(Client.Home());
                    var pageZA = journeyZa.ApplyForLoan(200, 20).CurrentPage as ApplyPage;
                    pageZA.SetNewMobilePhone = phone;
                    pageZA.ResendPinClick();
                    Thread.Sleep(5000);
                    var smsZa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(phone.Replace("077", "2777")));
                    Do.Until(
                        () => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(phone.Replace("077", "2777")));
                    Assert.AreEqual(2, smsZa.Count());
                    Console.WriteLine(smsZa.Count());
                    foreach (var sms in smsZa)
                    {
                        Console.WriteLine(sms.MessageText + " / " + sms.CreatedOn);
                        Assert.IsTrue(
                            sms.MessageText.Contains("You will need it to complete your application back at Wonga.com."));
                    }

                    Console.WriteLine(smsZa.Count());
                    break;
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetLnJourney(Client.Home());
                    var pageCa = journeyCa.ApplyForLoan(200, 25)
                        //.SetName(name, surname)
                                   .CurrentPage as ApplyPage;
                    pageCa.SetNewMobilePhone = phone;
                    pageCa.ResendPinClick();
                    var smsCa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(phone.Replace("077", "177")));
                    foreach (var sms in smsCa)
                    {
                        Console.WriteLine(sms.MessageText + " / " + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.ca."));
                    }
                    Assert.AreEqual(1, smsCa.Count());
                    break;
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-205")]
        public void CheckEmailsWhenLoanTakenOutAsLNCustomer()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();
            Application application1 = ApplicationBuilder
                .New(customer)
                .Build();
            application1.RepayOnDueDate();
            Application application2 = ApplicationBuilder
                .New(customer)
                .Build();

            var mail = Do.Until(() => Drive.Data.QaData.Db.Email.FindAllByEmailAddress(email)).FirstOrDefault();
            Console.WriteLine(mail.EmailId);
            var mailTemplate = Do.Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail.EmailId, Key: "Loan_Agreement"));
            Console.WriteLine(mailTemplate.Value.ToString());
            Assert.IsNotNull(mailTemplate);
            Assert.IsTrue(mailTemplate.value.ToString().Contains("You promise to pay and will make one repayment of"));
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-302")]
        public void LoggedCustomerWithoutLoanAppliesNewLoanChangesMobilePhoneAndClicksResendPinAndGoFarther()
        {
            string email = Get.RandomEmail();
            string name = Get.RandomString(3, 10);
            string surname = Get.RandomString(3, 10);
            string oldphone = "077009" + Get.RandomLong(1000, 9999).ToString();
            Customer customer = CustomerBuilder
                .New()
                .WithForename(name)
                .WithSurname(surname)
                .WithEmailAddress(email)
                .WithMobileNumber(oldphone)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            string phone = "077009" + Get.RandomLong(1000, 9999).ToString();
            var loginPage = Client.Login();
            loginPage.LoginAs(email);
        
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.ApplyForLoan(200, 20).CurrentPage as ApplyPage;
            applyPage.SetNewMobilePhone = phone;
            applyPage.ResendPinClick();
            Thread.Sleep(2000);
            applyPage.CloseResendPinPopup();
            applyPage.ApplicationSection.SetPin = "0000";
            journey.CurrentPage = applyPage.Submit();
            var mySummary = journey.WaitForAcceptedPage() as AcceptedPage;


        }

        [Test, AUT(AUT.Za), Category(TestCategories.Smoke)]
        public void LnVerifyUrlsAreCorrect()
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
            var applyPage = journey.ApplyForLoan(200, 10)
                .SetName(name, surname).CurrentPage as ApplyPage;
            // Check the URL here is /apply-member
            Assert.Contains(Client.Driver.Url, "apply-member", "The apply page URL is not /apply-member.");

            journey.CurrentPage = applyPage.Submit() as ProcessingPage;
            // Check the URL here is /processing-member
            Assert.Contains(Client.Driver.Url, "processing-member", "The processing page URL is not /processing-member.");

            journey.WaitForAcceptedPage();
            // Check the URL here is /apply-accept-member
            Assert.Contains(Client.Driver.Url, "apply-accept-member", "The accept page URL is not /apply-accept-member.");

            journey.FillAcceptedPage();
            // Check the URL here is /deal-done-member
            Assert.Contains(Client.Driver.Url, "deal-done-member", "The deal done page URL is not /deal-done-member.");
        }

    }
}
