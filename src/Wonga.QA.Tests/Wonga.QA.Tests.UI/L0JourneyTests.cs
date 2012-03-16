using System;
using System.Globalization;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    class L0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-180")]
        public void L0JourneyInvalidPostcodeShouldCauseWarningMessage()
        {
            var journey = new Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails("test:EmployedMask")
                                      .CurrentPage as AddressDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    addressPage.PostCode = "qqqq";
                    break;
                case AUT.Ca:
                    addressPage.PostCode = "111111";
                    break;
            }
            addressPage.Street = "Asd"; //to lost focus
            Assert.IsTrue(addressPage.IsPostcodeWarningOccurred());

        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-189")]
        public void L0JourneyInvalidPINShouldCauseWarningMessageOnNextPage()
        {
            var journey = new Journey(Client.Home());
            var bankDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails("test:EmployedMask")
                                      .FillAddressDetails()
                                      .FillAccountDetails()
                                      .CurrentPage as PersonalBankAccountPage;

            switch (Config.AUT)
            {
                case AUT.Za:
                    bankDetailsPage.BankAccountSection.BankName = "Capitec";
                    bankDetailsPage.BankAccountSection.BankAccountType = "Current";
                    bankDetailsPage.BankAccountSection.AccountNumber = "1234567";
                    bankDetailsPage.BankAccountSection.BankPeriod = "2 to 3 years";
                    break;
                case AUT.Ca:
                    bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
                    bankDetailsPage.BankAccountSection.BranchNumber = "00011";
                    bankDetailsPage.BankAccountSection.AccountNumber = "3023423";
                    bankDetailsPage.BankAccountSection.BankPeriod = "More than 4 years";
                    break;
            }
            bankDetailsPage.PinVerificationSection.Pin = "9999";
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage.Next(); });

        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-177")]
        public void ChangeLoanAmountAndDurationOnPersonalDetailsViaPlusMinusOptions()
        {
            var journey = new Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            personalDetailsPage.ClickAmountPlusButton();
            personalDetailsPage.ClickDurationMinusButton();
            string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
            string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

            var acceptedPage = journey.FillPersonalDetails("test:EmployedMask")
                                     .FillAddressDetails()
                                     .FillAccountDetails()
                                     .FillBankDetails()
                                     .WaitForAcceptedPage().CurrentPage as AcceptedPage;

            string actualTotalToRepay = acceptedPage.GetTotalToRepay;
            string actualRepaymentDate = acceptedPage.GetRepaymentDate;

            Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
            Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);
            var dealDonePage = journey.FillAcceptedPage().CurrentPage as DealDonePage;

            actualTotalToRepay = dealDonePage.GetRapaymentAmount();

            var date = DateTime.ParseExact(dealDonePage.GetRepaymentDate(), "dd MMMM yyyy", null);

            switch (date.Day % 10)
            {
                case 1:
                    actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                : String.Format("{0:dddd d\\s\\t MMM yyyy}", date);
                    break;
                case 2:
                    actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                : String.Format("{0:dddd d\\n\\d MMM yyyy}", date);
                    break;
                case 3:
                    actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                : String.Format("{0:dddd d\\r\\d MMM yyyy}", date);
                    break;
                default:
                    actualRepaymentDate = String.Format("{0:dddd d\\t\\h MMM yyyy}", date);
                    break;

            }

            Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
            Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);

            switch (Config.AUT)
            {
                case AUT.Ca:
                    var mySummaryPage = journey.GoToMySummaryPage().CurrentPage as MySummaryPage;

                    actualTotalToRepay = mySummaryPage.GetTotalToRepay;

                    Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
                    //TODO add the dates comparison
                    break;
                //TODO case AUT.Za:
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-176")]
        public void ChangeLoanAmountAndDurationOnPersonalDetailsViaTypingToTheFields()
        {
            var journey = new Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            personalDetailsPage.ChangeAmount = "195";
            personalDetailsPage.ChangeDuration = "5";
            string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
            string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

            var acceptedPage = journey.FillPersonalDetails("test:EmployedMask")
                                     .FillAddressDetails()
                                     .FillAccountDetails()
                                     .FillBankDetails()
                                     .WaitForAcceptedPage().CurrentPage as AcceptedPage;

            string actualTotalToRepay = acceptedPage.GetTotalToRepay;
            string actualRepaymentDate = acceptedPage.GetRepaymentDate;

            Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
            Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);
            var dealDonePage = journey.FillAcceptedPage().CurrentPage as DealDonePage;

            actualTotalToRepay = dealDonePage.GetRapaymentAmount();

            var date = DateTime.ParseExact(dealDonePage.GetRepaymentDate(), "dd MMMM yyyy", null);

            switch (date.Day % 10)
            {
                case 1:
                    actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                : String.Format("{0:dddd d\\s\\t MMM yyyy}", date);
                    break;
                case 2:
                    actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                : String.Format("{0:dddd d\\n\\d MMM yyyy}", date);
                    break;
                case 3:
                    actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                : String.Format("{0:dddd d\\r\\d MMM yyyy}", date);
                    break;
                default:
                    actualRepaymentDate = String.Format("{0:dddd d\\t\\h MMM yyyy}", date);
                    break;

            }

            Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
            Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);

            switch (Config.AUT)
            {
                case AUT.Ca:
                    var mySummaryPage = journey.GoToMySummaryPage().CurrentPage as MySummaryPage;

                    actualTotalToRepay = mySummaryPage.GetTotalToRepay;

                    Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
                    //TODO add the dates comparison
                    break;
                //TODO case AUT.Za:
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-183")]
        public void EnterDifferentPasswordsAtAccountDetailsPageShouldCauseWarningMessage()
        {
            var journey = new Journey(Client.Home());
            switch (Config.AUT)
            {
                case AUT.Za:
                    var accountDetailsPage = journey.ApplyForLoan(200, 10)
                                       .FillPersonalDetails("test:EmployedMask")
                                       .FillAddressDetails().CurrentPage as AccountDetailsPage;
                    accountDetailsPage.AccountDetailsSection.Password = "Passw0rd";
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = "qweqweqwe";
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "123124";//to lost focus
                    Assert.IsTrue(accountDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
                    break;
                case AUT.Ca:
                    var addressDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails("test:EmployedMask")
                                      .FillAddressDetails().CurrentPage as AddressDetailsPage;
                    addressDetailsPage.AccountDetailsSection.Password = "Passw0rd";
                    addressDetailsPage.AccountDetailsSection.PasswordConfirm = "qweqweqwe";
                    addressDetailsPage.AccountDetailsSection.SecretQuestion = "12312"; //to lost focus
                    Assert.IsTrue(addressDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
                    break;

            }

        }

    }
}
