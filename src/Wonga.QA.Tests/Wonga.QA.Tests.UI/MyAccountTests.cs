using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class MyAccountTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-218"), SmokeTest]
        public void CustomerWithLiveLoanShouldNotBeAbleToAddBankAccount()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPaymentDetailsPage = mySummaryPage.Navigation.MyPaymentDetailsButtonClick();

            Assert.IsFalse(myPaymentDetailsPage.IsAddBankAccountButtonExists());
        }

        [Test, AUT(AUT.Za), JIRA("QA-203"), SmokeTest]
        public void L0JourneyInvalidAccountNumberShouldCauseWarningMessageOnNextPage()
        {
            var journey1 = JourneyFactory.GetL0Journey(Client.Home());
            var bankDetailsPage1 = journey1.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(employerNameMask: Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .FillAddressDetails()
                                      .FillAccountDetails()
                                      .CurrentPage as PersonalBankAccountPage;

            bankDetailsPage1.BankAccountSection.BankName = "Capitec";
            bankDetailsPage1.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage1.BankAccountSection.AccountNumber = "7434567";
            bankDetailsPage1.BankAccountSection.BankPeriod = "2 to 3 years";
            bankDetailsPage1.PinVerificationSection.Pin = "0000";
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage1.Next(); });

            var journey2 = JourneyFactory.GetL0Journey(Client.Home());
            var bankDetailsPage2 = journey2.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(employerNameMask: Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .FillAddressDetails()
                                      .FillAccountDetails()
                                      .CurrentPage as PersonalBankAccountPage;

            bankDetailsPage2.BankAccountSection.BankName = "Capitec";
            bankDetailsPage2.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage2.BankAccountSection.AccountNumber = "7534567";
            bankDetailsPage2.BankAccountSection.BankPeriod = "2 to 3 years";
            bankDetailsPage2.PinVerificationSection.Pin = "0000";
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage2.Next(); });
        }

        [Test, AUT(AUT.Za), JIRA("QA-202"), SmokeTest]
        public void LNJourneyInvalidAccountNumberShouldCauseWarningMessageOnNextPage()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();  // to take LN status

            var page = loginPage.LoginAs(email);
            var payment1 = Client.Payments();

            if (payment1.IsAddBankAccountButtonExists())
            {
                payment1.AddBankAccountButtonClick();


                payment1.AddBankAccount("Capitec", "Current", "7434567", "2 to 3 years");
                Do.With.Timeout(2).Until(payment1.IfHasAnExeption);
                Assert.IsTrue(payment1.IfHasAnExeption());
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
            var home = Client.Home();
            var payment2 = Client.Payments();

            if (payment2.IsAddBankAccountButtonExists())
            {
                payment2.AddBankAccountButtonClick();

                Thread.Sleep(2000); // Wait some time to load popup

                payment1.AddBankAccount("Capitec", "Current", "7534567", "2 to 3 years");
                Do.With.Timeout(2).Until(payment2.IfHasAnExeption);
                Assert.IsTrue(payment2.IfHasAnExeption());
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }

        [Test, AUT(AUT.Za), JIRA("QA-201"), SmokeTest]
        public void WhenLoggedCustomerWithoutLiveLoanAddsNewBankAccountItShouldBecomePrimary()
        {
            string accountNumber = "1234567";
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();

            var page = loginPage.LoginAs(email);
            var payment = Client.Payments();

            if (payment.IsAddBankAccountButtonExists())
            {
                payment.AddBankAccountButtonClick();
                payment.AddBankAccount("Capitec", "Current", accountNumber, "2 to 3 years");
                payment.ClickCloseButton();

                payment = Client.Payments();
                int whileCount = 0;
                while (accountNumber.Remove(0, 3) != payment.DefaultAccountNumber && whileCount < 50)
                {
                    whileCount++;
                    payment = Client.Payments();
                }
                Console.WriteLine(whileCount);
                Assert.AreEqual(accountNumber.Remove(0, 3), payment.DefaultAccountNumber);
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }

        [Test, AUT(AUT.Za), JIRA("QA-214"), SmokeTest]
        public void CustomerOnMyPersonalDetailsShouldBeAbleToChangeCommunicationPrefs()
        {


            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);

            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            myPersonalDetailsPage.CommunicationClick();
            Thread.Sleep(10000);
            switch (myPersonalDetailsPage.GetCommunicationText)
            {


                case ("You are not happy to receive updates and other communications from Wonga via email and SMS."):
                    {
                        myPersonalDetailsPage.SetCommunicationPrefs =
                            "I am happy to receive updates and other communications from Wonga via email and SMS.";

                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);
                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);

                        var happy = Drive.Data.Comms.Db.ContactPreferences.FindAllBy(AccountId: customer.Id).FirstOrDefault().AcceptMarketingContact;
                        Assert.IsTrue(happy);
                        Assert.AreEqual(
                            "You are happy to receive updates and other communications from Wonga via email and SMS.",
                            myPersonalDetailsPage.GetCommunicationText);
                        break;
                    }
                case ("You are happy to receive updates and other communications from Wonga via email and SMS."):
                    {
                        myPersonalDetailsPage.SetCommunicationPrefs =
                            "I am not happy to receive updates and other communications from Wonga via email and SMS.";

                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);
                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);

                        var happy = Drive.Data.Comms.Db.ContactPreferences.FindAllBy(AccountId: customer.Id).FirstOrDefault().AcceptMarketingContact;
                        Assert.IsFalse(happy);
                        Assert.AreEqual(
                            "You are not happy to receive updates and other communications from Wonga via email and SMS.",
                            myPersonalDetailsPage.GetCommunicationText);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }

        }

        [Test, AUT(AUT.Za), JIRA("QA-216"), Pending("need refinement")]
        public void CustomerShouldBeAbleToChangePassword()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PasswordClick();
            Thread.Sleep(10000); //here and below - waiting for a pop-up
            myPersonalDetailsPage.ChangePassword("Passw0rd", "Passw0rd", "Passw0rd");
            myPersonalDetailsPage.Submit();

            Thread.Sleep(10000);
            Assert.IsTrue(myPersonalDetailsPage.IsPasswordPopupHasErrorMessage());
            myPersonalDetailsPage.PasswordClick();
            myPersonalDetailsPage.PasswordClick();
            Thread.Sleep(10000);
            myPersonalDetailsPage.ChangePassword("Passw0rd", "Pass", "Pass");

            Thread.Sleep(10000);
            Assert.IsTrue(myPersonalDetailsPage.IsPasswordWarningMessageOccurs());

            myPersonalDetailsPage.PasswordClick();
            myPersonalDetailsPage.PasswordClick();
            Thread.Sleep(10000);
            myPersonalDetailsPage.ChangePassword("Passw0rd", "QWEasd12", "QWEasd12");
            myPersonalDetailsPage.Submit();
            Thread.Sleep(10000);

            var homePage = myPersonalDetailsPage.Login.Logout();
            var mySummary = homePage.Login.LoginAs(email, "QWEasd12");
        }

        [Test, AUT(AUT.Za), JIRA("QA-209")]
        public void CustomerShouldBeAbleToChangePhoneNumber()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();

            Do.Until(() => myPersonalDetailsPage.ChangePhone("0123000000", "0212571908", "0000"));

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            Do.Until(() => Drive.Db.Comms.CustomerDetails.Single(c => c.Email == email).HomePhone != "0210000000");
            var homePhone = Drive.Db.Comms.CustomerDetails.FirstOrDefault(c => c.Email == email).HomePhone;

            Assert.AreEqual("0123000000", myPersonalDetailsPage.GetHomePhone);
            Assert.AreEqual("0123000000", homePhone);
            //TODO check SF
        }

        [Test, AUT(AUT.Za), JIRA("QA-212"), SmokeTest]
        public void CustomerShouldBeAbleToChangeMobileNumber()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();

            Do.Until(() => myPersonalDetailsPage.ChangePhone("0210000000", "0213456789", "0000"));

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            Do.Until(() => Drive.Db.Comms.CustomerDetails.Single(c => c.Email == email).MobilePhone != "0212571908");
            var mobilePhone = Drive.Db.Comms.CustomerDetails.FirstOrDefault(c => c.Email == email).MobilePhone;

            Assert.AreEqual("0213456789", myPersonalDetailsPage.GetMobilePhone);
            Assert.AreEqual("0213456789", mobilePhone);
            //TODO check SF
        }

        [Test, AUT(AUT.Za), JIRA("QA-211"), SmokeTest]
        public void ChangingPhoneNumberWithWrongPinShouldCauseWarningMessage()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();

            Do.Until(() => myPersonalDetailsPage.ChangePhone("0210000000", "0211234567", "1111"));
            myPersonalDetailsPage.Submit();
            Assert.IsTrue(myPersonalDetailsPage.GetPopupErrorMessage.Equals("The SMS PIN you entered was incorrect."));



        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-193"), SmokeTest]
        public void ArrearsCustomerCheckDataOnMySummaryAndSF()
        {
            int loanTerm = 10;
            uint arrearsdays = 5;
            string actualPromisedRepayDate;
            DateTime date;
            string email;
            Customer customer;
            Application application;
            LoginPage loginPage;
            MySummaryPage mySummaryPage;

            switch (Config.AUT)
            {
                case (AUT.Za):
                    date = DateTime.Now.AddDays(-arrearsdays);
                    email = Get.RandomEmail();
                    customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                    application = ApplicationBuilder.New(customer)
                        .Build();
                    application.PutApplicationIntoArrears(arrearsdays);
                    loginPage = Client.Login();
                    mySummaryPage = loginPage.LoginAs(email);
                    #region DateFormat
                    switch (date.Day % 10)
                    {
                        case 1:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:dddd d\\s\\t MMM yyyy}", date);
                            break;
                        case 2:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:dddd d\\n\\d MMM yyyy}", date);
                            break;
                        case 3:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:dddd d\\r\\d MMM yyyy}", date);
                            break;
                        default:
                            actualPromisedRepayDate = String.Format("{0:dddd d\\t\\h MMM yyyy}", date);
                            break;
                    }
                    #endregion
                    Assert.AreEqual("R655.23", mySummaryPage.GetTotalToRepay);
                    Assert.AreEqual("R649.89", mySummaryPage.GetPromisedRepayAmount);
                    Assert.AreEqual(actualPromisedRepayDate, mySummaryPage.GetPromisedRepayDate);
                    // need to add check data on popup, whan it well be added
                    break;
                case (AUT.Ca):
                    date = DateTime.Now.AddDays(-arrearsdays);
                    email = Get.RandomEmail();
                    customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                    application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm)
                        .Build();
                    application.PutApplicationIntoArrears(arrearsdays);
                    loginPage = Client.Login();
                    mySummaryPage = loginPage.LoginAs(email);
                    #region DateFormat

                    DateTime now = DateTime.Now;
                    int daysTillStartOfLoan = Drive.Db.GetNumberOfDaysUntilStartOfLoan(now);
                    DateTime promiseDate = now.Date.AddDays(daysTillStartOfLoan + loanTerm);
                    DateTime dueDate = Drive.Db.GetNextWorkingDay(new Date(promiseDate));
                    double dueDateOffsetInDays = dueDate.Subtract(promiseDate).TotalDays;
                    date = now.AddDays(-(arrearsdays + dueDateOffsetInDays));

                    switch (date.Day % 10)
                    {
                        case 1:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:ddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:ddd d\\s\\t MMM yyyy}", date);
                            break;
                        case 2:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:ddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:ddd d\\n\\d MMM yyyy}", date);
                            break;
                        case 3:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:ddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:ddd d\\r\\d MMM yyyy}", date);
                            break;
                        default:
                            actualPromisedRepayDate = String.Format("{0:ddd d\\t\\h MMM yyyy}", date);
                            break;
                    }

                    #endregion
                    Assert.AreEqual("$130.45", mySummaryPage.GetTotalToRepay); //must be $130.45 it's bug, well change whan it's well be resolved 
                    Assert.AreEqual("$130.00", mySummaryPage.GetPromisedRepayAmount);
                    Assert.AreEqual(actualPromisedRepayDate, mySummaryPage.GetPromisedRepayDate);
                    mySummaryPage.RepayButtonClick();
                    Thread.Sleep(10000);
                    Assert.AreEqual("$130.45", mySummaryPage.GetTotalToRepayAmountPopup);
                    #region DateFormat
                    switch (date.Day % 10)
                    {
                        case 1:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:d\\t\\h MMMM yyyy}", date)
                                                        : String.Format("{0:d\\s\\t MMMM yyyy}", date);
                            break;
                        case 2:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:d\\t\\h MMMM yyyy}", date)
                                                        : String.Format("{0:d\\n\\d MMMM yyyy}", date);
                            break;
                        case 3:
                            actualPromisedRepayDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:d\\t\\h MMMM yyyy}", date)
                                                        : String.Format("{0:d\\r\\d MMMM yyyy}", date);
                            break;
                        default:
                            actualPromisedRepayDate = String.Format("{0:d\\t\\h MMMM yyyy}", date);
                            break;
                    }
                    #endregion
                    Assert.AreEqual(actualPromisedRepayDate, mySummaryPage.GetPromisedRepayDatePopup);
                    break;
            }
            // need to add check data in SF whan it well be ready for this
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-187"), SmokeTest]
        public void CustomerEntersInvalidBankAccountWarningMessageShouldBeDisplayed()
        {
            var accounts = new List<string> { "dfgsfgfgsdf", "123 342", "123f445", "+135-6887" };
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Za:
                case AUT.Uk:
                    Application application1 = ApplicationBuilder.New(customer)
                .Build();
                    application1.RepayOnDueDate(); // to take LN status
                    break;

                case AUT.Wb:
                    Application application2 = BusinessApplicationBuilder.New(customer)
                .Build();
                    application2.RepayOnDueDate(); // to take LN status
                    break;
            }
            var page = loginPage.LoginAs(email);
            foreach (string account in accounts)
            {
                var payment = Client.Payments();
                switch (Config.AUT)
                {
                    #region case Za
                    case (AUT.Za):
                        if (payment.IsAddBankAccountButtonExists())
                        {
                            payment.AddBankAccountButtonClick();

                            Thread.Sleep(2000); // Wait some time to load popup
                            payment.AddBankAccount("Capitec", "Current", account, "2 to 3 years");
                            Assert.IsTrue(payment.IsInvalidBankAccountCauseWarning());
                        }
                        else
                        {
                            throw new NullReferenceException("Add bank account button not found");
                        }
                        break;
                    #endregion
                    #region case Ca
                    case (AUT.Ca):
                        // there is no addaccount button on Ca
                        break;
                    #endregion
                    #region case Uk
                    case (AUT.Uk):
                        if (payment.IsAddBankAccountButtonExists())
                        {
                            payment.AddBankAccountButtonClick();

                            Thread.Sleep(2000); // Wait some time to load popup
                            try
                            {
                                //  button add bank account is broken
                                //  payment.AddBankAccount("Capitec", "Current", account, "2 to 3 years");
                                //  throw new Exception("Invalid bank account was pass: " + account);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Assert.IsTrue(e.Message.Contains("Please enter a valid bank account number"));
                            }
                        }
                        else
                        {
                            throw new NullReferenceException("Add bank account button not found");
                        }
                        break;
                    #endregion
                    #region case Wb
                    case (AUT.Wb):
                        if (payment.IsAddBankAccountButtonExists())
                        {
                            payment.AddBankAccountButtonClick();

                            Thread.Sleep(2000); // Wait some time to load popup
                            try
                            {
                                payment.AddBankAccount("Capitec", "Current", account, "2 to 3 years");
                                throw new Exception("Invalid bank account was pass: " + account);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Assert.IsTrue(e.Message.Contains("Please enter a valid bank account number"));
                            }
                        }
                        else
                        {
                            throw new NullReferenceException("Add bank account button not found");
                        }
                        break;
                    #endregion
                }
                var home = Client.Home();
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-215")] //Removed from smoke because of selenium problem with new sliders
        public void MyAccountPostcodeMustBeTheSameAsUserEntered()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                         .FillPersonalDetails(employerNameMask: Get.EnumToString(RiskMask.TESTEmployedMask))
                                         .CurrentPage as AddressDetailsPage;
            string postcode;
            switch (Config.AUT)
            {
                case AUT.Za:
                    postcode = Get.GetPostcode();
                    addressPage.HouseNumber = "25";
                    addressPage.Street = "high road";
                    addressPage.Town = "Kuku";
                    addressPage.County = "Province";
                    addressPage.PostCode = postcode;
                    addressPage.AddressPeriod = "2 to 3 years";
                    journey.CurrentPage = addressPage.Next() as AccountDetailsPage;
                    break;

                case AUT.Ca:
                    postcode = "V4F3A9";
                    addressPage.HouseNumber = "1403";
                    addressPage.Street = "Edward";
                    addressPage.Town = "Hearst";
                    addressPage.PostCode = postcode;
                    addressPage.AddressPeriod = "2 to 3 years";
                    addressPage.PostOfficeBox = "C12345";
                    break;

                default:
                    throw new NotImplementedException();

            }
            var mySummaryPage = journey.FillAccountDetails()
                                    .FillBankDetails()
                                    .WaitForAcceptedPage()
                                    .FillAcceptedPage()
                                    .GoToMySummaryPage()
                                    .CurrentPage as MySummaryPage;
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            Assert.AreEqual(postcode, myPersonalDetailsPage.GetPostcode);
        }

        [Test, AUT(AUT.Wb), JIRA("QA-250")]
        public void WbFrontendMyAccountPageLoadsCorrectly()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Organisation organisation = OrganisationBuilder.New(customer).Build();
            Application application = ApplicationBuilder
                .New(customer, organisation)
                .Build();

            var mySummaryPage = loginPage.LoginAs(email);
        }

        [Test, AUT(AUT.Za), JIRA("QA-208"), SmokeTest]
        public void LoanOlderThanThreeDaysThenViewLoanDetailsLinkShouldBeDisplayedAndCorrect()
        {

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.DaysFromStart(5);
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ClickViewLoanDetailsButton();
            mySummaryPage.WaitForMySummaryPopup();
            Assert.IsTrue(mySummaryPage.IsPopupContainsSummaryDetailsTable());
        }

        [Test, AUT(AUT.Za), JIRA("QA-213"), SmokeTest]
        public void CustomerUpdatesPhoneNumbersAndDoesntMakeChangesShouldSeeMessageOnTopWindow()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            myPersonalDetails.PhoneClick();
            Assert.IsTrue(myPersonalDetails.DontChangePhone());
        }

        [Test, AUT(AUT.Za), JIRA("QA-217"), SmokeTest]
        public void CustomerChangesAddressWithNotValidDataThenWarningMessageShouldOccur()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            var oldTown = myPersonalDetailsPage.GetTown; //to check if changes in db occured

            myPersonalDetailsPage.AddressClick();
            Do.Until(myPersonalDetailsPage.ChangeMyAddressElement.IsChangeMyAddressTitleDisplayed);

            var newFlat = Get.RandomInt(100).ToString();
            var newStreet = Get.RandomString(3, 10);
            var newHouseNumber = Get.RandomString(3, 10);
            var newDistrict = Get.RandomString(3, 10);
            var newTown = Get.RandomString(3, 10);
            var newPostcode = Get.GetPostcode();

            myPersonalDetailsPage.ChangeMyAddressElement.Postcode = "123";
            myPersonalDetailsPage.ChangeMyAddressElement.Flat = newFlat;
            Assert.IsTrue(myPersonalDetailsPage.ChangeMyAddressElement.IsPostcodeWarningOccurred());

            myPersonalDetailsPage.ChangeMyAddressElement.Postcode = newPostcode;
            myPersonalDetailsPage.ChangeMyAddressElement.Street = newStreet;
            myPersonalDetailsPage.ChangeMyAddressElement.HouseNumber = newHouseNumber;
            myPersonalDetailsPage.ChangeMyAddressElement.District = newDistrict;
            myPersonalDetailsPage.ChangeMyAddressElement.Town = newTown;
            myPersonalDetailsPage.ChangeMyAddressElement.AddressPeriod = "2 to 3 years";
            Thread.Sleep(2000);

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            var addresses = Drive.Data.Comms.Db.Addresses;
            
            Do.Until(() => addresses.FindByAccountId(customer.Id).Town != oldTown);
            var currentAddress = addresses.FindByAccountId(customer.Id);
            //Check changes in DB
            Assert.AreEqual(currentAddress.Flat, newFlat);
            Assert.AreEqual(currentAddress.Street, newStreet);
            Assert.AreEqual(currentAddress.HouseNumber, newHouseNumber);
            Assert.AreEqual(currentAddress.District, newDistrict);
            Assert.AreEqual(currentAddress.Town, newTown);
            Assert.AreEqual(currentAddress.PostCode, newPostcode);

            //Check changes in UI
            Assert.AreEqual(myPersonalDetailsPage.GetHouseNumberAndStreet, newHouseNumber + " " + newStreet);
            Assert.AreEqual(myPersonalDetailsPage.GetTown, newTown);
            Assert.AreEqual(myPersonalDetailsPage.GetPostcode, newPostcode);


        }

        [Test, AUT(AUT.Za), JIRA("QA-219"), SmokeTest]
        public void CustomerShouldBeAbleToAddBankAccount()
        {
            var accountPreferences = Drive.Data.Payments.Db.AccountPreferences;
            var bankAccountsBase = Drive.Data.Payments.Db.BankAccountsBase;
            string accountNumber = "1234567";

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();

            loginPage.LoginAs(email);
            var firstMyPaymentsPage = Client.Payments();

            Do.Until(firstMyPaymentsPage.IsAddBankAccountButtonExists);
            firstMyPaymentsPage.AddBankAccountButtonClick();
            firstMyPaymentsPage.AddBankAccount("Capitec", "Current", accountNumber, "2 to 3 years");
            firstMyPaymentsPage.ClickCloseButton();
            var newMyPaymentsPage = Client.Payments();

            Assert.AreEqual(accountNumber.Remove(0, 3), newMyPaymentsPage.DefaultAccountNumber);

            var primaryBankAccountId = accountPreferences.FindByAccountId(customer.Id).PrimaryBankAccountId;
            var bankAccount = bankAccountsBase.FindByBankAccountId(primaryBankAccountId);

            Assert.AreEqual("Capitec", bankAccount.BankName);
            Assert.AreEqual("Current", bankAccount.AccountType);
            Assert.AreEqual(accountNumber, bankAccount.AccountNumber);

            var journey = JourneyFactory.GetLnJourney(Client.Home());

            var applyPage = journey.ApplyForLoan(200, 10)
                                .CurrentPage as ApplyPage;

            Assert.AreEqual(accountNumber.Remove(0, 3), applyPage.GetCurrentBankAccount);

        }

        [Test, AUT(AUT.Za), SmokeTest, JIRA("QA-279")]
        public void LNCustomerChangesMobilePhoneNumberToTheSameOneButUsingSeparators()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string phone = "0211000000";
            List<string> invalidPhones = new List<string> { "021-1000000", "021.1000000", "021,1000000", "021/1000000" };
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).WithMobileNumber(phone).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummary = loginPage.LoginAs(email);
            var myPersonals = mySummary.Navigation.MyPersonalDetailsButtonClick();
            foreach (var invaliPhone in invalidPhones)
            {
                myPersonals.PhoneClick();
                myPersonals.ChangeMobilePhone(invaliPhone, "0000");
            }
        }

        [Test, AUT(AUT.Za), JIRA("QA-210"), SmokeTest]
        public void CustomerChangeTelephonFieldsCheckHomePhone()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();

            var mySummary = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummary.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();
            myPersonalDetailsPage.ChangeHomePhone("");

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            var homePhoneUI = myPersonalDetailsPage.GetHomePhone;
            Console.WriteLine(customer.Id.ToString());
            var homePhoneDB = Do.Until(() => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(customer.Id).HomePhone);

            Assert.AreEqual("", homePhoneUI);
            Assert.AreEqual("", homePhoneDB);
        }

        [Test, AUT(AUT.Za), JIRA("QA-304")]
        public void CustomerClicksOnRepayButtonBeforeDueDate()
        {
            var _applications = Drive.Data.Payments.Db.Applications;
            var _scheduledPayments = Drive.Data.Payments.Db.ScheduledPayments;

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var appId = _applications.FindByExternalId(application.Id).ApplicationId;
            var naedo = _scheduledPayments.FindByApplicationId(appId);
            Assert.IsNull(naedo);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(customer.Email);

            var repaymentOptionsPage = myAccountPage.RepayClick();
        }

        [Test, AUT(AUT.Za), JIRA("QA-306")]
        public void ThereIsNoRepayButtonWhenNAEDOTrackingInPlace()
        {
            var _applications = Drive.Data.Payments.Db.Applications;
            var _scheduledPayments = Drive.Data.Payments.Db.ScheduledPayments;

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            application.PutApplicationIntoArrears(2);

            var appId = _applications.FindByExternalId(application.Id).ApplicationId;
            var naedo = _scheduledPayments.FindByApplicationId(appId);
            Assert.IsNotNull(naedo);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(customer.Email);

            var tagCloud = myAccountPage.GetTagCloud;
            Assert.AreEqual("", tagCloud);
        }

        [Test, AUT(AUT.Za), JIRA("QA-307")]
        public void CustomerSetsUpDebitOrder()
        {
            var _applications = Drive.Data.Payments.Db.Applications;
            var _scheduledPayments = Drive.Data.Payments.Db.ScheduledPayments;

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var appId = _applications.FindByExternalId(application.Id).ApplicationId;
            var naedo = _scheduledPayments.FindByApplicationId(appId);
            Assert.IsNull(naedo);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(customer.Email);

            var repaymentOptionsPage = myAccountPage.RepayClick();
            var debitOrderPage = repaymentOptionsPage.DebitOrderButtonClick();
            var debitOrderSuccessPage = debitOrderPage.Submit();
            var updatedMyAccountPage = debitOrderSuccessPage.BackToYourAccountButtonClick();
            Assert.IsTrue(updatedMyAccountPage.GetStatusText.Contains(ContentMap.Get.MySummaryPage.DebitOrderSuccesText));
        }
    }
}
