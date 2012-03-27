using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    class MyAccountTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-218")]
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

        [Test, AUT(AUT.Za), JIRA("QA-203")]
        public void L0JourneyInvalidAccountNumberShouldCauseWarningMessageOnNextPage()
        {
            var journey1 = new Journey(Client.Home());
            var bankDetailsPage1 = journey1.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .FillAddressDetails()
                                      .FillAccountDetails()
                                      .CurrentPage as PersonalBankAccountPage;

            bankDetailsPage1.BankAccountSection.BankName = "Capitec";
            bankDetailsPage1.BankAccountSection.BankAccountType = "Current";
            bankDetailsPage1.BankAccountSection.AccountNumber = "7434567";
            bankDetailsPage1.BankAccountSection.BankPeriod = "2 to 3 years";
            bankDetailsPage1.PinVerificationSection.Pin = "0000";
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage1.Next(); });

            var journey2 = new Journey(Client.Home());
            var bankDetailsPage2 = journey2.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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

        [Test, AUT(AUT.Za), JIRA("QA-202")]
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

                Thread.Sleep(2000); // Wait some time to load popup

                var paymentPage = payment1.AddBankAccount("Capitec", "Current", "7434567", "2 to 3 years");
                Thread.Sleep(2000); // Wait some time before assert
                Assert.IsTrue(paymentPage.IfHasAnExeption());
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

                var paymentPage = payment1.AddBankAccount("Capitec", "Current", "7534567", "2 to 3 years");
                Thread.Sleep(2000); // Wait some time before assert
                Assert.IsTrue(paymentPage.IfHasAnExeption());
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }

        [Test, AUT(AUT.Za), JIRA("QA-201")]
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

                Thread.Sleep(2000); // Wait some time to load popup

                var paymentPage = payment.AddBankAccount("Capitec", "Current", accountNumber, "2 to 3 years");
                Thread.Sleep(3000);
                paymentPage.CloseButtonClick();
                payment = Client.Payments();
                Assert.IsTrue(payment.IsAccountNumberRight(accountNumber));
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }

        [Test, AUT(AUT.Za), JIRA("QA-214"), Pending("Not completed")]
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
            myPersonalDetailsPage.SetCommunicationPrefs =
                "I am happy to receive updates and other communications from Wonga via email and SMS.";

            myPersonalDetailsPage.Submit();
            Thread.Sleep(10000);
            myPersonalDetailsPage.Submit();
            Thread.Sleep(10000);

            Assert.AreEqual("You are happy to receive updates and other communications from Wonga via email and SMS.", myPersonalDetailsPage.GetCommunicationText);


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
            Thread.Sleep(10000);
            myPersonalDetailsPage.ChangePhone("0123000000", "0212571908", "0000");

            myPersonalDetailsPage.Submit();
            Thread.Sleep(10000);
            myPersonalDetailsPage.Submit();

            Thread.Sleep(10000);
            var homePhone = Drive.Db.Comms.CustomerDetails.FirstOrDefault(c => c.Email == email).HomePhone;

            Assert.AreEqual("0123000000", myPersonalDetailsPage.GetHomePhone);
            Assert.AreEqual("0123000000", homePhone);
            //TODO check SF
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-193"), Pending("need refinement")]
        public void ArrearsCustomerCheckData()
        {
            int arrearsdays = 10;
            string actualPromisedRepayDate;
            DateTime date = DateTime.Now.AddDays(-arrearsdays);
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.PutApplicationIntoArrears(arrearsdays);
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            switch (Config.AUT)
            {
                case (AUT.Za):
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
                    break;
                case (AUT.Ca):
                    #region DateFormat
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
                    Assert.AreEqual("$129.45", mySummaryPage.GetTotalToRepay);
                    Assert.AreEqual("$129.00", mySummaryPage.GetPromisedRepayAmount);
                    Thread.Sleep(10000);
                    Assert.AreEqual(actualPromisedRepayDate, mySummaryPage.GetPromisedRepayDate);
                    mySummaryPage.RepayButtonClick();
                    Thread.Sleep(10000);
                    Assert.AreEqual("$129.18", mySummaryPage.GetTotalToRepayAmountPopup);
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

        }
    }
}
