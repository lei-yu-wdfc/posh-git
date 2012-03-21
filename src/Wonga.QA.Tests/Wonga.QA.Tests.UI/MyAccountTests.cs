using System;
using Gallio.Framework.Assertions;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.Mappings;

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
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
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

                var paymentPage = payment.AddBankAccount("Capitec", "Current", "1234567", "2 to 3 years");

            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }
    }
}
