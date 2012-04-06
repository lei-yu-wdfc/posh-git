﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    class L0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-180")]
        public void L0JourneyInvalidPostcodeShouldCauseWarningMessage()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var bankDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;
            personalDetailsPage.ClickAmountPlusButton();
            personalDetailsPage.ClickDurationMinusButton();
            string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
            string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

            Assert.AreNotEqual(firstTotalToRepayValue, totalToRepayAtPersonalDetails);

            var acceptedPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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

            var date = DateTime.ParseExact(dealDonePage.GetRepaymentDate(), "d MMMM yyyy", null);

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
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;
            personalDetailsPage.HowMuch = "195";
            personalDetailsPage.HowLong = "5";
            string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
            string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

            Assert.AreNotEqual(firstTotalToRepayValue, totalToRepayAtPersonalDetails);

            var acceptedPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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

            var date = DateTime.ParseExact(dealDonePage.GetRepaymentDate(), "d MMMM yyyy", null);

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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-175")]
        public void ChangeLoanAmountAndDurationOnPersonalDetailsViaSlidersMotion()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;

            personalDetailsPage.MoveAmountSlider = 20;
            personalDetailsPage.MoveDurationSlider = 20;

            string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
            string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

            Assert.AreNotEqual(firstTotalToRepayValue, totalToRepayAtPersonalDetails);

            var acceptedPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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

            var date = DateTime.ParseExact(dealDonePage.GetRepaymentDate(), "d MMMM yyyy", null);

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
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            switch (Config.AUT)
            {
                case AUT.Za:
                    var accountDetailsPage = journey.ApplyForLoan(200, 10)
                                       .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                       .FillAddressDetails().CurrentPage as AccountDetailsPage;
                    accountDetailsPage.AccountDetailsSection.Password = "Passw0rd";
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = "qweqweqwe";
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "123124";//to lost focus
                    Thread.Sleep(500);
                    Assert.IsTrue(accountDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
                    break;
                case AUT.Ca:
                    var addressDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .FillAddressDetails().CurrentPage as AddressDetailsPage;
                    addressDetailsPage.AccountDetailsSection.Password = "Passw0rd";
                    addressDetailsPage.AccountDetailsSection.PasswordConfirm = "qweqweqwe";
                    addressDetailsPage.AccountDetailsSection.SecretQuestion = "12312"; //to lost focus
                    Thread.Sleep(500);
                    Assert.IsTrue(addressDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
                    break;

            }

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-190")]
        public void L0JourneyDataOnAcceptedPageShouldBeCorrect()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            string totalAmountOnPersonalDetails = personalDetailsPage.GetTotalAmount + ".00";
            string totalFeesOnPersonalDetails = personalDetailsPage.GetTotalFees;
            string totalToRepayOnPersonalDetails = personalDetailsPage.GetTotalToRepay;
            string repaymentDateOnPersonalDetails = personalDetailsPage.GetRepaymentDate;

            var acceptedPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .WaitForAcceptedPage()
                .CurrentPage as AcceptedPage;

            Assert.AreEqual(totalToRepayOnPersonalDetails, acceptedPage.GetTotalToRepay);
            Assert.AreEqual(repaymentDateOnPersonalDetails, acceptedPage.GetRepaymentDate);

            switch (Config.AUT)
            {
                case AUT.Ca:
                    string[] date = acceptedPage.GetPaymentDueDate.Replace(",", "").Split(' ');
                    string day = date[2][0] == '0' ? date[2].Remove(0, 1) : date[2];
                    string paymentDate = date[0] + " " + day + " " + date[1].Remove(3) + " " + date[3];

                    Assert.AreEqual(totalAmountOnPersonalDetails, acceptedPage.GetPrincipalAmountBorrowed);
                    Assert.AreEqual(totalAmountOnPersonalDetails, acceptedPage.GetPrincipalAmountToBeTransfered);
                    Assert.AreEqual(totalFeesOnPersonalDetails, acceptedPage.GetTotalCostOfCredit);
                    Assert.AreEqual(totalToRepayOnPersonalDetails, acceptedPage.GetTotalAmountDueUnderTheAgreement);
                    Assert.AreEqual(repaymentDateOnPersonalDetails, paymentDate);
                    break;
                case AUT.Za:
                    var dateTime = DateTime.ParseExact(acceptedPage.GetPaymentDueDate, "dddd, d MMMM yyyy", null);
                    string actualRepaymentDate;
                    switch (dateTime.Day % 10)
                    {
                        case 1:
                            actualRepaymentDate = (dateTime.Day > 10 && dateTime.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", dateTime)
                                                        : String.Format("{0:dddd d\\s\\t MMM yyyy}", dateTime);
                            break;
                        case 2:
                            actualRepaymentDate = (dateTime.Day > 10 && dateTime.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", dateTime)
                                                        : String.Format("{0:dddd d\\n\\d MMM yyyy}", dateTime);
                            break;
                        case 3:
                            actualRepaymentDate = (dateTime.Day > 10 && dateTime.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", dateTime)
                                                        : String.Format("{0:dddd d\\r\\d MMM yyyy}", dateTime);
                            break;
                        default:
                            actualRepaymentDate = String.Format("{0:dddd d\\t\\h MMM yyyy}", dateTime);
                            break;

                    }

                    Assert.AreEqual(totalAmountOnPersonalDetails, acceptedPage.GetLoanAmount);
                    Assert.AreEqual(totalToRepayOnPersonalDetails, acceptedPage.GetTotalToPayOnPaymentDate);
                    Assert.AreEqual(repaymentDateOnPersonalDetails, actualRepaymentDate);
                    break;
            }


        }

        [Test, AUT(AUT.Za), JIRA("ZA-2108")]
        public void L0VerifyWongaLzeroZaModuleSignatureInsertedInPage()
        {
            // Checks for the presence of "<!-- Output from wonga_lzero_za/<$_GET['q']> -->" in page source.
            // This test complements the normal ZA L0 tests since the L0 journey should be functionally the
            // same as before the refactor.

            // Create a journey:
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            // Go to the first page:
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-details -->"));

            // Go to the second page:
            var addressDetailsPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask)).CurrentPage as AddressDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(addressDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-address -->"));

            // Go to the third page:
            var accountDetailsPage = journey.FillAddressDetails().CurrentPage as AccountDetailsPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(accountDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-account -->"));

            // Go to the fourth page:
            var personalBankAccountPage = journey.FillAccountDetails().CurrentPage as PersonalBankAccountPage;

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-bank -->"));
        }

        [Test, AUT(AUT.Uk), Pending("Example of full Uk L0 journey")]
        public void UKL0JourneyTest()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journey.ApplyForLoan(200, 10)
                .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage()
                .GoToMySummaryPage()
                .CurrentPage as MySummaryPage;

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-170")]
        public void CustomerOnHowItWorksPageShouldBeAbleUseSlidersProperly()
        {
            var howItWorks = Client.HowItWorks();
            var personalDetailsPage = howItWorks.ApplyForLoan(200, 10);
            Assert.IsTrue(personalDetailsPage is PersonalDetailsPage);
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("QA-181")]
        public void L0JourneyCustomerOnCurrentAddressPageDoesNotEnterSomeRequiredFieldsWarningMessageDisplayed()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var addressDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .CurrentPage as AddressDetailsPage;


            switch (Config.AUT)
            {
                #region case Za
                case AUT.Za:
                    addressDetailsPage.HouseNumber = "25";
                    addressDetailsPage.Street = "high road";
                    addressDetailsPage.Town = "Kuku";
                    addressDetailsPage.County = "Province";
                    addressDetailsPage.AddressPeriod = "2 to 3 years";
                    Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
                    addressDetailsPage.PostCode = Get.GetPostcode();
                    addressDetailsPage.HouseNumber = "";
                    Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    addressDetailsPage.HouseNumber = "25";
                    addressDetailsPage.Street = "";
                    Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
                    addressDetailsPage.Street = "high road";
                    addressDetailsPage.Town = "";
                    Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
                    addressDetailsPage.Town = "Kuku";
                    addressDetailsPage.County = "";
                    Assert.IsTrue(addressDetailsPage.IsCountyWarningOccurred());
                    addressDetailsPage.County = "Province";
                    addressDetailsPage.AddressPeriod = "--- Please select ---";
                    Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
                    break;
                #endregion
                #region case Ca
                case AUT.Ca:
                    addressDetailsPage.Street = "Edward";
                    addressDetailsPage.Town = "Hearst";
                    addressDetailsPage.PostCode = "V4F3A9";
                    addressDetailsPage.AddressPeriod = "2 to 3 years";
                    Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    addressDetailsPage.HouseNumber = "1403";
                    addressDetailsPage.Street = "";
                    Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
                    addressDetailsPage.Street = "Edward";
                    addressDetailsPage.Town = "";
                    Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
                    addressDetailsPage.Town = "Hearst";
                    addressDetailsPage.PostCode = "";
                    Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
                    addressDetailsPage.PostCode = "V4F3A9";
                    addressDetailsPage.AddressPeriod = "--- Please select ---";
                    Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
                    break;
                #endregion
                #region case Uk
                case AUT.Uk:
                    addressDetailsPage.PostCodeLookup = "SW6 6PN";
                    addressDetailsPage.LookupByPostCode();
                    addressDetailsPage.GetAddressesDropDown();
                    Do.Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
                    Do.Until(() => addressDetailsPage.HouseNumber = "93");
                    Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
                    addressDetailsPage.AddressPeriod = "3 to 4 years";
                    addressDetailsPage.HouseNumber = "";
                    Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    addressDetailsPage.HouseNumber = "93";
                    addressDetailsPage.Street = "";
                    Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
                    addressDetailsPage.Street = "Harbord Street";
                    addressDetailsPage.Town = "";
                    Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
                    addressDetailsPage.Town = "LONDON";
                    addressDetailsPage.PostcodeInForm = "";
                    Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
                    break;
                #endregion
            }

            // Assert.Throws<AssertionFailureException>(() => { var processingPage = addressDetailsPage.Next(); });
        }
    }
}
