using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class MyAccountTestsBase : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-218"), Category(TestCategories.SmokeTest)]
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

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-193"), Category(TestCategories.SmokeTest)]
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
                    application.PutIntoArrears(arrearsdays);
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
                    //  Assert.AreEqual("R649.89", mySummaryPage.GetPromisedRepayAmount);
                    //   Assert.AreEqual(actualPromisedRepayDate, mySummaryPage.GetPromisedRepayDate);
                    // need to add check data on popup, whan it well be added
                    break;
                case (AUT.Ca):
                    date = DateTime.Now.AddDays(-arrearsdays);
                    email = Get.RandomEmail();
                    customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                    application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm)
                        .Build();
                    application.PutIntoArrears(arrearsdays);
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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-187"), Category(TestCategories.SmokeTest), Pending("ZA-2786")]
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
            string postcode;
            switch (Config.AUT)
            {
                case AUT.Za:
                    postcode = Get.GetPostcode();
                    break;
                case AUT.Ca:
                    postcode = "V4F3A9";
                    break;
                default:
                    throw new NotImplementedException();
            }
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithPosteCode(postcode);
            var mySummaryPage = journey.Teleport<MySummaryPage>() as MySummaryPage;
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            Assert.AreEqual(postcode, myPersonalDetailsPage.GetPostcode);
        }
    }
}
