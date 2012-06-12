﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class L0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-180"), Pending("Wierd problem")]
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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-189"), Category(TestCategories.Smoke)]
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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-177"), Category(TestCategories.Smoke)] //AUT.Ca removed because of sliders changing
        public void ChangeLoanAmountAndDurationOnPersonalDetailsViaPlusMinusOptions()
        {
            //CA is out due to new wonga sliders being implemented on homepage only 
            //soon it will be on "my account" and in other regions

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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-176"), Category(TestCategories.Smoke)] //AUT.Ca removed because of sliders changing
        public void ChangeLoanAmountAndDurationOnPersonalDetailsViaTypingToTheFields()
        {
            //CA is out due to new wonga sliders being implemented on homepage only 
            //soon it will be on "my account" and in other regions

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;
            personalDetailsPage.HowMuch = "195";
            personalDetailsPage.HowLong = "5";
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.PersonalDetailsPage.LoanAmount)).LostFocus();
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

        [Test, AUT(AUT.Pl), JIRA("PL-220")]
        public void FiilAllFieldsAtPersonalDetailsPagePlannedForPolishApplication()
        {

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10)
                                    .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask)).CurrentPage as PersonalDetailsPage;

        }

        [Test, AUT(AUT.Pl), JIRA("PL-222")]
        public void WhenFillingFieldsWrongGetWarningMessages()
        {

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = "1234qwere";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourNameSection.FirstName, UiMap.Get.YourNameSection.FirstNameErrorForm));
            personalDetailsPage.YourName.MiddleName = "1234qwere";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourNameSection.MiddleName, UiMap.Get.YourNameSection.MiddleNameErrorForm));
            personalDetailsPage.YourName.LastName = "1234asd";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourNameSection.LastName, UiMap.Get.YourNameSection.LastNameErrorForm));
            personalDetailsPage.ContactingYou.EmailAddress = "qwert123";
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = "qwert123";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "123qwe";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.ContactingYouSection.Email, UiMap.Get.ContactingYouSection.EmailErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.ContactingYouSection.EmailConfirm, UiMap.Get.ContactingYouSection.EmailConfirmErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.ContactingYouSection.MobilePhone, UiMap.Get.ContactingYouSection.MobilePhoneErrorForm));
            personalDetailsPage.BikVerification = true;
            personalDetailsPage.BikVerification = false;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.PrivacyPolicy = false;
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.PersonalDetailsPage.CheckBikVerification, UiMap.Get.PersonalDetailsPage.BikVerificationErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.PersonalDetailsPage.CheckBikVerification, UiMap.Get.PersonalDetailsPage.PrivatePolicyErrorForm));
            personalDetailsPage.YourDetails.PeselNumber = "12232";
            personalDetailsPage.YourDetails.Number = "qwe123";
            personalDetailsPage.YourDetails.MotherMaidenName = "qwe123";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.PeselNumber, UiMap.Get.YourDetailsSection.PeselWarningForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.IdNumber, UiMap.Get.YourDetailsSection.IdNumberWarningForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.MotherMaidenName, UiMap.Get.YourDetailsSection.MotherMaidenNameWarningForm));
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Umowa o prace na czas okreslony";
            personalDetailsPage.EmploymentDetails.WorkPhone = "07712345678123";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.WorkPhone, UiMap.Get.EmploymentDetailsSection.WorkPhoneErrorForm));
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "";
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.MonthlyIncome, UiMap.Get.EmploymentDetailsSection.MonthlyIncomeErrorForm));
            Assert.IsFalse(personalDetailsPage.ContactingYou.CanEnterLettersToMobilePhoneField("123qwqdw111"));
            Assert.IsTrue(personalDetailsPage.ContactingYou.CanEnterLettersToMobilePhoneField("123111"));
        }


        [Test, AUT(AUT.Pl), JIRA("PL-220")]
        public void VerifyTypesOfAllFieldsAtPersonalDetailsPageplannedForPolishApplication()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourNameSection.FirstName, "aaa"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourNameSection.MiddleName));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourNameSection.LastName, "aaa"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourDetailsSection.PeselNumber, "1111"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourDetailsSection.IdNumber, "1111"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourDetailsSection.MotherMaidenName));

            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.YourDetailsSection.EducationLevel, "Podstawowe"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.YourDetailsSection.MaritalStatus, "Wolny"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.YourDetailsSection.Dependants, "2"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.YourDetailsSection.VehicleOwner, "Tak"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.YourDetailsSection.AllegroLogin, "1111"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.EmploymentStatus, "Umowa o prace na czas okreslony"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.MonthlyIncome, "10000"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.EmployerName));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.EmployerIndustry, "Rolnictwo"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.TimeWithEmployerYears, "2"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.TimeWithEmployerMonths, "2"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.WorkPhone, "11111"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.IncomeFrequency, "raz na tydzien"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.NextPaydayDateDay, "2"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.NextPaydayDateMonth, "Jun"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.NextPaydayDateYear, "2012"));
            Assert.IsTrue(Selenium.IsChoiseItems(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.SalaryPaidToBank, "Yes"));
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Student";
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.UniversityType, "panstwowa"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.UniversityCity, "Warszawa"));
            Assert.IsTrue(Selenium.IsDropdownList(personalDetailsPage, UiMap.Get.EmploymentDetailsSection.YearsInUniversity, "2"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.ContactingYouSection.Email, "11111@11.com"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.ContactingYouSection.EmailConfirm, "11111@11.com"));
            Assert.IsTrue(Selenium.IsTextBox(personalDetailsPage, UiMap.Get.ContactingYouSection.MobilePhone, "07712345678"));
            Assert.IsTrue(Selenium.IsCheckBox(personalDetailsPage, UiMap.Get.PersonalDetailsPage.CheckPrivacyPolicy));
            Assert.IsTrue(Selenium.IsCheckBox(personalDetailsPage, UiMap.Get.PersonalDetailsPage.CheckMarketingAcceptance));
            Assert.IsTrue(Selenium.IsCheckBox(personalDetailsPage, UiMap.Get.PersonalDetailsPage.CheckBikVerification));
        }

        [Test, AUT(AUT.Pl), JIRA("PL-222")]
        public void WanringOccuredNearEmptyFieldsAfterSubmitClick()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSubmit();
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourNameSection.FirstName, UiMap.Get.YourNameSection.FirstNameErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourNameSection.FirstName, UiMap.Get.YourNameSection.FirstNameErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourNameSection.LastName, UiMap.Get.YourNameSection.LastNameErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.ContactingYouSection.Email, UiMap.Get.ContactingYouSection.EmailErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.ContactingYouSection.EmailConfirm, UiMap.Get.ContactingYouSection.EmailConfirmErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.ContactingYouSection.MobilePhone, UiMap.Get.ContactingYouSection.MobilePhoneErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.PersonalDetailsPage.CheckBikVerification, UiMap.Get.PersonalDetailsPage.BikVerificationErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.PersonalDetailsPage.CheckPrivacyPolicy, UiMap.Get.PersonalDetailsPage.PrivatePolicyErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.PeselNumber, UiMap.Get.YourDetailsSection.PeselWarningForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.IdNumber, UiMap.Get.YourDetailsSection.IdNumberWarningForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.MotherMaidenName, UiMap.Get.YourDetailsSection.MotherMaidenNameWarningForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.EducationLevel, UiMap.Get.YourDetailsSection.EducationLevelErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.MaritalStatus, UiMap.Get.YourDetailsSection.MartialStatusErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.Dependants, UiMap.Get.YourDetailsSection.DependantsErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.YourDetailsSection.VehicleOwner, UiMap.Get.YourDetailsSection.VehicleOwnerErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.EmploymentStatus, UiMap.Get.EmploymentDetailsSection.EmploymentStatusErrorForm));

            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Umowa o prace na czas okreslony";
            personalDetailsPage.ClickSubmit();

            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.MonthlyIncome, UiMap.Get.EmploymentDetailsSection.MonthlyIncomeErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.EmployerName, UiMap.Get.EmploymentDetailsSection.EmployerNameErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.EmployerIndustry, UiMap.Get.EmploymentDetailsSection.EmployerIndustryErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.TimeWithEmployerYears, UiMap.Get.EmploymentDetailsSection.TimeWithEmployerYearsErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.TimeWithEmployerMonths, UiMap.Get.EmploymentDetailsSection.TimeWiyhEmployerMonthsErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.WorkPhone, UiMap.Get.EmploymentDetailsSection.WorkPhoneErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.IncomeFrequency, UiMap.Get.EmploymentDetailsSection.IncomeFrequencyErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.NextPaydayDateDay, UiMap.Get.EmploymentDetailsSection.NextPayDateErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.SalaryPaidToBank, UiMap.Get.EmploymentDetailsSection.SalaryPaidToBankErrorForm));
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Student";
            personalDetailsPage.ClickSubmit();

            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.UniversityType, UiMap.Get.EmploymentDetailsSection.UniversityTypeErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.UniversityCity, UiMap.Get.EmploymentDetailsSection.UniversityCityErrorForm));
            Assert.IsTrue(personalDetailsPage.IsWarningOccurred(UiMap.Get.EmploymentDetailsSection.YearsInUniversity, UiMap.Get.EmploymentDetailsSection.YearsInUniversityErrorForm));

        }

        [Test, AUT(AUT.Pl), JIRA("PL-222")]
        public void WhenFillingFieldsRightGetSuccessTicks()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;

            personalDetailsPage.YourName.FirstName = "John";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourNameSection.FirstName, UiMap.Get.YourNameSection.FirstNameErrorForm));
            personalDetailsPage.YourName.MiddleName = "Amadeus";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourNameSection.MiddleName, UiMap.Get.YourNameSection.MiddleNameErrorForm));
            personalDetailsPage.YourName.LastName = "Kowalski";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourNameSection.LastName, UiMap.Get.YourNameSection.LastNameErrorForm));
            personalDetailsPage.YourDetails.PeselNumber = "78081130217";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.PeselNumber, UiMap.Get.YourDetailsSection.PeselWarningForm));
            personalDetailsPage.YourDetails.Number = "AHP765835";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.IdNumber, UiMap.Get.YourDetailsSection.IdNumberWarningForm));
            personalDetailsPage.YourDetails.MotherMaidenName = "Mazur";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.MotherMaidenName, UiMap.Get.YourDetailsSection.MotherMaidenNameWarningForm));
            personalDetailsPage.YourDetails.EducationLevel = "Gimnazjalne";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.EducationLevel, UiMap.Get.YourDetailsSection.EducationLevelErrorForm));
            personalDetailsPage.YourDetails.MaritalStatus = "Wolny";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.MaritalStatus, UiMap.Get.YourDetailsSection.MartialStatusErrorForm));
            personalDetailsPage.YourDetails.NumberOfDependants = "2";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.Dependants, UiMap.Get.YourDetailsSection.DependantsErrorForm));
            personalDetailsPage.YourDetails.VehicleOwner = "Tak";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.VehicleOwner, UiMap.Get.YourDetailsSection.VehicleOwnerErrorForm));
            personalDetailsPage.YourDetails.AllegroLogin = "1223Qwe";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.YourDetailsSection.AllegroLogin, UiMap.Get.YourDetailsSection.AllegroLoginErrorForm));
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Student";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.EmploymentStatus, UiMap.Get.EmploymentDetailsSection.EmploymentStatusErrorForm));
            personalDetailsPage.EmploymentDetails.MonthlyIncome = "1000";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.MonthlyIncome, UiMap.Get.EmploymentDetailsSection.MonthlyIncomeErrorForm));
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "raz na tydzien";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.IncomeFrequency,
                                                                   UiMap.Get.EmploymentDetailsSection.
                                                                       IncomeFrequencyErrorForm));
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.NextPaydayDateDay, UiMap.Get.EmploymentDetailsSection.NextPayDateErrorForm));
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.SalaryPaidToBank, UiMap.Get.EmploymentDetailsSection.SalaryPaidToBankErrorForm));
            personalDetailsPage.EmploymentDetails.UniversityType = "panstwowa";
            Assert.IsTrue(Do.Until(() => personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.UniversityType, UiMap.Get.EmploymentDetailsSection.UniversityTypeErrorForm)));
            personalDetailsPage.EmploymentDetails.UniversityCity = "Opole";
            Assert.IsTrue(Do.Until(() => personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.UniversityCity, UiMap.Get.EmploymentDetailsSection.UniversityCityErrorForm)));
            personalDetailsPage.EmploymentDetails.YearsInUniversity = "2";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.YearsInUniversity, UiMap.Get.EmploymentDetailsSection.YearsInUniversityErrorForm));
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Umowa o prace na czas nieokreslony";
            personalDetailsPage.EmploymentDetails.EmployerName = "Zukovich";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.EmployerName, UiMap.Get.EmploymentDetailsSection.EmployerNameErrorForm));
            personalDetailsPage.EmploymentDetails.EmployerIndustry = "Rolnictwo";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.EmployerIndustry, UiMap.Get.EmploymentDetailsSection.EmployerIndustryErrorForm));
            personalDetailsPage.EmploymentDetails.TimeWithEmployerMonths = "1";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.TimeWithEmployerMonths, UiMap.Get.EmploymentDetailsSection.TimeWiyhEmployerMonthsErrorForm));
            personalDetailsPage.EmploymentDetails.TimeWithEmployerYears = "1";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.TimeWithEmployerYears, UiMap.Get.EmploymentDetailsSection.TimeWithEmployerYearsErrorForm));
            personalDetailsPage.EmploymentDetails.WorkPhone = "07712345678";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.WorkPhone, UiMap.Get.EmploymentDetailsSection.WorkPhoneErrorForm));
            personalDetailsPage.ContactingYou.EmailAddress = "asd@asd.com";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.ContactingYouSection.Email, UiMap.Get.ContactingYouSection.EmailErrorForm));
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = "asd@asd.com";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.ContactingYouSection.EmailConfirm, UiMap.Get.ContactingYouSection.EmailConfirmErrorForm));
            personalDetailsPage.ContactingYou.CellPhoneNumber = "07712345678";
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.ContactingYouSection.MobilePhone, UiMap.Get.ContactingYouSection.MobilePhoneErrorForm));
            personalDetailsPage.PrivacyPolicy = true;
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.PersonalDetailsPage.CheckPrivacyPolicy, UiMap.Get.PersonalDetailsPage.PrivatePolicyErrorForm));
            personalDetailsPage.BikVerification = true;
            Assert.IsTrue(personalDetailsPage.IsSuccessTickOccured(UiMap.Get.PersonalDetailsPage.CheckBikVerification, UiMap.Get.PersonalDetailsPage.BikVerificationErrorForm));

        }


        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-175"), Pending("Wierd selenium problem")]
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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-183"), Category(TestCategories.Smoke)]
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
                    accountDetailsPage.AccountDetailsSection.PasswordConfirm = "Passw0rds";
                    accountDetailsPage.AccountDetailsSection.SecretQuestion = "123124";//to lost focus
                    Thread.Sleep(500);
                    Assert.IsTrue(accountDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
                    break;
                case AUT.Ca:
                    var addressDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .FillAddressDetails().CurrentPage as AddressDetailsPage;
                    addressDetailsPage.AccountDetailsSection.Password = "Passw0rd";
                    addressDetailsPage.AccountDetailsSection.PasswordConfirm = "Passw0rds";
                    addressDetailsPage.AccountDetailsSection.SecretQuestion = "12312"; //to lost focus
                    Thread.Sleep(500);
                    Assert.IsTrue(addressDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
                    break;

            }

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-190"), Category(TestCategories.Smoke)]
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
                    string paymentDate = date[0] + " " + day + " " + date[1].Remove(3) + " " + date[3]; // Note: Temp fix, need better solutions

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

        [Test, AUT(AUT.Za), JIRA("ZA-2108"), Pending("Broken")]
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

        [Test, AUT(AUT.Uk)]
        public void L0Journey()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journey.ApplyForLoan(200, 10)
                .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage().CurrentPage as DealDonePage;
        }

        [Test, AUT(AUT.Za), JIRA("QA-170")] //Removed from smoke because of the problem with sliders update
        public void CustomerOnHowItWorksPageShouldBeAbleUseSlidersProperly()
        {
            //CA is out due to new wonga sliders being implemented on homepage only 
            //soon it will be on "my account" and in other regions

            var howItWorks = Client.HowItWorks();
            var personalDetailsPage = howItWorks.ApplyForLoan(200, 10);
            Assert.IsTrue(personalDetailsPage is PersonalDetailsPage);
        }

        [Test, AUT(AUT.Wb), JIRA("QA-251")]
        public void WbFrontendLoadsCorrectly()
        {
            var homePage = Client.Home();
            homePage.AssertThatIsWbHomePage();
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("QA-181"), Pending("ZA-2512")]
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
        }

        [Test, AUT(AUT.Wb), JIRA("QA-181")]
        public void L0JourneyCustomerOnCurrentAddressPageDoesNotEnterSomeRequiredFieldsWarningMessageDisplayedWb()
        {
            var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
            var addressDetailsPage = journeyWb.ApplyForLoan(5500, 30)
                .AnswerEligibilityQuestions()
                .FillPersonalDetails("TESTNoCheck").CurrentPage as AddressDetailsPage;
            addressDetailsPage.PostCode = "SW6 6PN";
            addressDetailsPage.LookupByPostCode();
            addressDetailsPage.GetAddressesDropDown();
            Do.Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
            Do.Until(() => addressDetailsPage.AddressPeriod = "2 to 3 years");
            addressDetailsPage.HouseNumber = "";
            Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
            addressDetailsPage.HouseNumber = "1";
            addressDetailsPage.Street = "";
            Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
            addressDetailsPage.Street = "Harbord Street";
            addressDetailsPage.Town = "";
            Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
            addressDetailsPage.Town = "LONDON";
            addressDetailsPage.AddressPeriod = "--- Please select ---";
            Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
            addressDetailsPage.PostcodeInForm = "";
            Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-191")] //Removed from smoke because of selenium problem with new sliders
        public void CustomerClicksAcceptButtonChosenLoanAmountShouldDepositedIntoAccountCheckDatabase()
        {
            DateTime date;
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            MySummaryPage mySummary;
            switch (Config.AUT)
            {
                case AUT.Ca:
                    date = DateTime.Now.AddDays(DateHelper.GetNumberOfDaysUntilStartOfLoanForCa() + 20);
                    mySummary = journey.ApplyForLoan(200, 20)
                                          .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                          .FillAddressDetails()
                                          .FillAccountDetails().FillBankDetails()
                                          .WaitForAcceptedPage()
                                          .FillAcceptedPage()
                                          .GoToMySummaryPage().CurrentPage as MySummaryPage;
                    var customerCa = Do.Until(() => Drive.Data.Comms.Db.CustomerDetails.FindBy(Forename: journey.FirstName, Surname: journey.LastName));
                    Console.WriteLine(customerCa.Email.ToString());
                    Console.WriteLine(customerCa.AccountId.ToString());
                    var applicationCa = Do.Until(() => Drive.Data.Payments.Db.Applications.FindBy(AccountId: customerCa.AccountId));
                    Console.WriteLine(applicationCa.AccountId.ToString());
                    var fixedTermApplicationCa = Do.Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(applicationCa.ApplicationId));
                    Assert.AreEqual("200.00", fixedTermApplicationCa.LoanAmount.ToString());
                    Assert.AreEqual(String.Format("{0:dddd MMMM yyyy}", date), String.Format("{0:dddd MMMM yyyy}", fixedTermApplicationCa.PromiseDate));
                    break;
                case AUT.Za:
                    date = DateTime.Now.AddDays(20);
                    mySummary = journey.ApplyForLoan(200, 20)
                                          .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                          .FillAddressDetails()
                                          .FillAccountDetails().FillBankDetails()
                                          .WaitForAcceptedPage()
                                          .FillAcceptedPage()
                                          .GoToMySummaryPage().CurrentPage as MySummaryPage;
                    var customerZa = Do.Until(() => Drive.Data.Comms.Db.CustomerDetails.FindBy(Forename: journey.FirstName, Surname: journey.LastName));
                    Console.WriteLine(customerZa.Email.ToString());
                    Console.WriteLine(customerZa.AccountId.ToString());
                    var applicationZa = Do.Until(() => Drive.Data.Payments.Db.Applications.FindBy(AccountId: customerZa.AccountId));
                    Console.WriteLine(applicationZa.AccountId.ToString());
                    var fixedTermApplicationZa = Do.Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(applicationZa.ApplicationId));
                    Assert.AreEqual("200.00", fixedTermApplicationZa.LoanAmount.ToString());
                    Assert.AreEqual(String.Format("{0:d MMMM yyyy}", date), String.Format("{0:d MMMM yyyy}", fixedTermApplicationZa.PromiseDate));
                    break;
            }
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-186"), Category(TestCategories.Smoke)]
        public void InvalidFormatPasswordShouldCauseWarningMessageAndValidPasswordShouldDissmissWarning()
        {
            switch (Config.AUT)
            {
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetL0Journey(Client.Home());
                    var myAccountCa = journeyCa.ApplyForLoan(200, 10)
                                        .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                        .FillAddressDetails().CurrentPage as AddressDetailsPage;
                    myAccountCa.AccountDetailsSection.Password = "sdfsdfs";
                    Thread.Sleep(1000);
                    Assert.IsTrue(myAccountCa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
                    myAccountCa.AccountDetailsSection.Password = "Sdfdfs123";
                    Assert.IsFalse(myAccountCa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
                    break;
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
                    var myAccountZa = journeyZa.ApplyForLoan(200, 10)
                                        .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                        .FillAddressDetails().CurrentPage as AccountDetailsPage;
                    myAccountZa.AccountDetailsSection.Password = "sdfsdfs";
                    Thread.Sleep(1000);
                    Assert.IsTrue(myAccountZa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
                    myAccountZa.AccountDetailsSection.Password = "Sdfdfs123";
                    Assert.IsFalse(myAccountZa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
                    break;
                case AUT.Wb:
                    var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
                    var accountDetailsPage = journeyWb.ApplyForLoan(5500, 30)
                        .AnswerEligibilityQuestions()
                        .FillPersonalDetails("TESTNoCheck")
                        .FillAddressDetails("2 to 3 years").CurrentPage as AccountDetailsPage;
                    accountDetailsPage.AccountDetailsSection.Password = "sdfsdfs";
                    Thread.Sleep(1000);
                    Assert.IsTrue(accountDetailsPage.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
                    accountDetailsPage.AccountDetailsSection.Password = "Sdfdfs123";
                    Assert.IsFalse(accountDetailsPage.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
                    break;
            }
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-188")] //Removed from smoke because of selenium problem with new sliders + Popup broken on Za, AUT removed
        public void CustomerOnBankDetailsPageClicksOnResendPinLinkMessageShouldDisplayedAndPinShouldResent()
        {
            string telephone = "077009" + Get.RandomLong(1000, 9999).ToString();
            string ukMobileTelephone = Get.GetMobilePhone();
            switch (Config.AUT)
            {
                #region Ca
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetL0Journey(Client.Home());
                    var personalDetailsPageCa = journeyCa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
                    var emailCa = Get.RandomEmail();
                    personalDetailsPageCa.ProvinceSection.Province = "British Columbia";
                    Do.Until(() => personalDetailsPageCa.ProvinceSection.ClosePopup());
                    personalDetailsPageCa.YourName.FirstName = Get.RandomString(3, 10);
                    personalDetailsPageCa.YourName.MiddleName = Get.GetMiddleName();
                    personalDetailsPageCa.YourName.LastName = Get.RandomString(3, 10);
                    personalDetailsPageCa.YourName.Title = "Mr";
                    personalDetailsPageCa.YourDetails.Number = "123213126";
                    personalDetailsPageCa.YourDetails.DateOfBirth = "1/Jan/1980";
                    personalDetailsPageCa.YourDetails.Gender = "Male";
                    personalDetailsPageCa.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPageCa.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageCa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPageCa.EmploymentDetails.MonthlyIncome = "1000";
                    personalDetailsPageCa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
                    personalDetailsPageCa.EmploymentDetails.EmployerIndustry = "Finance";
                    personalDetailsPageCa.EmploymentDetails.EmploymentPosition = "Professional (finance, accounting, legal, HR)";
                    personalDetailsPageCa.EmploymentDetails.TimeWithEmployerYears = "1";
                    personalDetailsPageCa.EmploymentDetails.TimeWithEmployerMonths = "0";
                    personalDetailsPageCa.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPageCa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
                    personalDetailsPageCa.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPageCa.ContactingYou.CellPhoneNumber = telephone;
                    personalDetailsPageCa.ContactingYou.EmailAddress = emailCa;
                    personalDetailsPageCa.ContactingYou.ConfirmEmailAddress = emailCa;
                    personalDetailsPageCa.PrivacyPolicy = true;
                    personalDetailsPageCa.CanContact = true;
                    journeyCa.CurrentPage = personalDetailsPageCa.Submit() as AddressDetailsPage;
                    var myBankAccountCa = journeyCa.FillAddressDetails().FillAccountDetails().CurrentPage as PersonalBankAccountPage;
                    Assert.IsTrue(myBankAccountCa.PinVerificationSection.ResendPinClickAndCheck());
                    var smsCa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(telephone.Replace("077", "177")));
                    foreach (var sms in smsCa)
                    {
                        Console.WriteLine(sms.MessageText + "/" + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.ca."));
                    }
                    Assert.AreEqual(2, smsCa.Count());
                    break;
                #endregion
                #region Za
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
                    var personalDetailsPageZa = journeyZa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
                    var emailZa = Get.RandomEmail();
                    personalDetailsPageZa.YourName.FirstName = Get.RandomString(3, 10);
                    personalDetailsPageZa.YourName.LastName = Get.RandomString(3, 10);
                    personalDetailsPageZa.YourName.Title = "Mr";
                    personalDetailsPageZa.YourDetails.Number = Get.GetNationalNumber(new DateTime(1957, 3, 10), true);
                    personalDetailsPageZa.YourDetails.DateOfBirth = "10/Mar/1957";
                    personalDetailsPageZa.YourDetails.Gender = "Female";
                    personalDetailsPageZa.YourDetails.HomeStatus = "Owner Occupier";
                    personalDetailsPageZa.YourDetails.HomeLanguage = "English";
                    personalDetailsPageZa.YourDetails.NumberOfDependants = "0";
                    personalDetailsPageZa.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageZa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPageZa.EmploymentDetails.MonthlyIncome = "3000";
                    personalDetailsPageZa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
                    personalDetailsPageZa.EmploymentDetails.EmployerIndustry = "Accountancy";
                    personalDetailsPageZa.EmploymentDetails.EmploymentPosition = "Administration";
                    personalDetailsPageZa.EmploymentDetails.TimeWithEmployerYears = "9";
                    personalDetailsPageZa.EmploymentDetails.TimeWithEmployerMonths = "5";
                    personalDetailsPageZa.EmploymentDetails.WorkPhone = "0123456789";
                    personalDetailsPageZa.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPageZa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
                    personalDetailsPageZa.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPageZa.ContactingYou.CellPhoneNumber = telephone;
                    personalDetailsPageZa.ContactingYou.EmailAddress = emailZa;
                    personalDetailsPageZa.ContactingYou.ConfirmEmailAddress = emailZa;
                    personalDetailsPageZa.PrivacyPolicy = true;
                    personalDetailsPageZa.CanContact = "Yes";
                    personalDetailsPageZa.MarriedInCommunityProperty =
                        "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
                    journeyZa.CurrentPage = personalDetailsPageZa.Submit() as AddressDetailsPage;
                    var myBankAccountZa = journeyZa.FillAddressDetails().FillAccountDetails().CurrentPage as PersonalBankAccountPage;
                    Assert.IsTrue(myBankAccountZa.PinVerificationSection.ResendPinClickAndCheck());
                    var smsZa = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(telephone.Replace("077", "2777")));
                    foreach (var sms in smsZa)
                    {
                        Console.WriteLine(sms.MessageText + "/" + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.com."));
                    }
                    Assert.AreEqual(2, smsZa.Count());
                    break;
                #endregion
                #region Wb
                case AUT.Wb:
                    var emailWb = Get.RandomEmail();
                    var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
                    var personalDetailsPageWb = journeyWb.ApplyForLoan(5500, 30)
                    .AnswerEligibilityQuestions().CurrentPage as PersonalDetailsPage;
                    personalDetailsPageWb.YourName.FirstName = Get.RandomString(3, 10);
                    personalDetailsPageWb.YourName.MiddleName = Get.RandomString(3, 10);
                    personalDetailsPageWb.YourName.LastName = Get.RandomString(3, 10);
                    personalDetailsPageWb.YourName.Title = "Mr";

                    personalDetailsPageWb.YourDetails.Gender = "Female";
                    personalDetailsPageWb.YourDetails.DateOfBirth = "1/Jan/1990";
                    personalDetailsPageWb.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPageWb.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageWb.YourDetails.NumberOfDependants = "0";

                    personalDetailsPageWb.ContactingYou.HomePhoneNumber = "02071111234";
                    personalDetailsPageWb.ContactingYou.CellPhoneNumber = ukMobileTelephone;
                    personalDetailsPageWb.ContactingYou.EmailAddress = emailWb;
                    personalDetailsPageWb.ContactingYou.ConfirmEmailAddress = emailWb;

                    personalDetailsPageWb.CanContact = "No";
                    personalDetailsPageWb.PrivacyPolicy = true;
                    journeyWb.CurrentPage = personalDetailsPageWb.Submit() as AddressDetailsPage;
                    var debitCardPage = journeyWb.FillAddressDetails("Between 4 months and 2 years")
                      .EnterAdditionalAddressDetails()
                      .FillAccountDetails()
                      .FillBankDetails().CurrentPage as PersonalDebitCardPage;
                    Assert.IsTrue(debitCardPage.MobilePinVerification.ResendPinClickAndCheck());
                    Console.WriteLine(ukMobileTelephone);
                    string ukTelephoneWithInternationalCode = ukMobileTelephone.Replace("077", "4477");
                    var smsWb = Do.Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(ukTelephoneWithInternationalCode));
                    foreach (var sms in smsWb)
                    {
                        Console.WriteLine(sms.MessageText + "/" + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at WongaBusiness.com."));
                    }
                    //Assert.AreEqual(2, smsWb.Count()); Assertion originally was assert equal to 2. Need to investigate if this is correct. comment By Ben Ifie 10.28 29/05/12
                    Assert.AreEqual(1, smsWb.Count());
                    break;
                #endregion
            }
        }

        [Test, AUT(AUT.Wb), JIRA("QA-258"), Category(TestCategories.Smoke)]
        public void TheWongaBusinessPolicyHaveNoReferenceToZaCaUk()
        {
            string ca = "wonga.ca";
            string za = "wonga.co.za";
            string uk = "wonga.com";
            var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
            var personaltDetailsPage = journey.ApplyForLoan(5500, 30)
                                         .AnswerEligibilityQuestions().CurrentPage as PersonalDetailsPage;
            personaltDetailsPage.PrivacyPolicyClick();
            List<string> hrefs = personaltDetailsPage.GetHrefsOfLinksOnPrivacyPopup();
            foreach (var href in hrefs)
            {
                Console.WriteLine(href);
                Assert.IsFalse(href.Contains(ca));
                Assert.IsFalse(href.Contains(za));
                Assert.IsFalse(href.Contains(uk));
            }
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-184"), Category(TestCategories.Smoke)]
        public void CustomerEntersPasswordThatEqualToTheEmailAddressWarningMessageShouldDisplayed()
        {
            var email = Get.RandomEmail();
            switch (Config.AUT)
            {
                #region Wb
                case AUT.Wb:
                    var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
                    var personalDetailsPageWb = journeyWb.ApplyForLoan(5500, 30)
                    .AnswerEligibilityQuestions().CurrentPage as PersonalDetailsPage;
                    personalDetailsPageWb.YourName.FirstName = Get.RandomString(3, 10);
                    personalDetailsPageWb.YourName.MiddleName = Get.RandomString(3, 10);
                    personalDetailsPageWb.YourName.LastName = Get.RandomString(3, 10);
                    personalDetailsPageWb.YourName.Title = "Mr";

                    personalDetailsPageWb.YourDetails.Gender = "Female";
                    personalDetailsPageWb.YourDetails.DateOfBirth = "1/Jan/1990";
                    personalDetailsPageWb.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPageWb.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageWb.YourDetails.NumberOfDependants = "0";

                    personalDetailsPageWb.ContactingYou.HomePhoneNumber = "02071111234";
                    personalDetailsPageWb.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
                    personalDetailsPageWb.ContactingYou.EmailAddress = email;
                    personalDetailsPageWb.ContactingYou.ConfirmEmailAddress = email;

                    personalDetailsPageWb.CanContact = "No";
                    personalDetailsPageWb.PrivacyPolicy = true;
                    journeyWb.CurrentPage = personalDetailsPageWb.Submit() as AddressDetailsPage;
                    var accountDetailsPageWb = journeyWb.FillAddressDetails("2 to 3 years").CurrentPage as AccountDetailsPage;
                    accountDetailsPageWb.AccountDetailsSection.Password = "bla";
                    accountDetailsPageWb.AccountDetailsSection.Password = email;
                    Do.Until(accountDetailsPageWb.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
                    accountDetailsPageWb.AccountDetailsSection.Password = "Passw0rd";
                    Do.While(accountDetailsPageWb.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
                    break;
                #endregion
                #region Ca
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetL0Journey(Client.Home());
                    var personalDetailsPageCa = journeyCa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
                    personalDetailsPageCa.ProvinceSection.Province = "British Columbia";
                    Do.Until(() => personalDetailsPageCa.ProvinceSection.ClosePopup());
                    personalDetailsPageCa.YourName.FirstName = Get.RandomString(3, 10);
                    personalDetailsPageCa.YourName.MiddleName = Get.GetMiddleName();
                    personalDetailsPageCa.YourName.LastName = Get.RandomString(3, 10);
                    personalDetailsPageCa.YourName.Title = "Mr";
                    personalDetailsPageCa.YourDetails.Number = "123213126";
                    personalDetailsPageCa.YourDetails.DateOfBirth = "1/Jan/1980";
                    personalDetailsPageCa.YourDetails.Gender = "Male";
                    personalDetailsPageCa.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPageCa.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageCa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPageCa.EmploymentDetails.MonthlyIncome = "1000";
                    personalDetailsPageCa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
                    personalDetailsPageCa.EmploymentDetails.EmployerIndustry = "Finance";
                    personalDetailsPageCa.EmploymentDetails.EmploymentPosition = "Professional (finance, accounting, legal, HR)";
                    personalDetailsPageCa.EmploymentDetails.TimeWithEmployerYears = "1";
                    personalDetailsPageCa.EmploymentDetails.TimeWithEmployerMonths = "0";
                    personalDetailsPageCa.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPageCa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
                    personalDetailsPageCa.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPageCa.ContactingYou.CellPhoneNumber = "07700900001";
                    personalDetailsPageCa.ContactingYou.EmailAddress = email;
                    personalDetailsPageCa.ContactingYou.ConfirmEmailAddress = email;
                    personalDetailsPageCa.PrivacyPolicy = true;
                    personalDetailsPageCa.CanContact = true;
                    journeyCa.CurrentPage = personalDetailsPageCa.Submit() as AddressDetailsPage;
                    var accountDetailsPageCa = journeyCa.FillAddressDetails().CurrentPage as AddressDetailsPage;
                    accountDetailsPageCa.AccountDetailsSection.Password = email;
                    Do.Until(accountDetailsPageCa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
                    accountDetailsPageCa.AccountDetailsSection.Password = "Passw0rd";
                    Do.While(accountDetailsPageCa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
                    break;
                #endregion
                #region Za
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
                    var personalDetailsPageZa = journeyZa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
                    personalDetailsPageZa.YourName.FirstName = Get.RandomString(3, 10);
                    personalDetailsPageZa.YourName.LastName = Get.RandomString(3, 10);
                    personalDetailsPageZa.YourName.Title = "Mr";
                    personalDetailsPageZa.YourDetails.Number = Get.GetNationalNumber(new DateTime(1957, 3, 10), true);
                    personalDetailsPageZa.YourDetails.DateOfBirth = "10/Mar/1957";
                    personalDetailsPageZa.YourDetails.Gender = "Female";
                    personalDetailsPageZa.YourDetails.HomeStatus = "Owner Occupier";
                    personalDetailsPageZa.YourDetails.HomeLanguage = "English";
                    personalDetailsPageZa.YourDetails.NumberOfDependants = "0";
                    personalDetailsPageZa.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageZa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPageZa.EmploymentDetails.MonthlyIncome = "3000";
                    personalDetailsPageZa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
                    personalDetailsPageZa.EmploymentDetails.EmployerIndustry = "Accountancy";
                    personalDetailsPageZa.EmploymentDetails.EmploymentPosition = "Administration";
                    personalDetailsPageZa.EmploymentDetails.TimeWithEmployerYears = "9";
                    personalDetailsPageZa.EmploymentDetails.TimeWithEmployerMonths = "5";
                    personalDetailsPageZa.EmploymentDetails.WorkPhone = "0123456789";
                    personalDetailsPageZa.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPageZa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
                    personalDetailsPageZa.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPageZa.ContactingYou.CellPhoneNumber = "0770090001";
                    personalDetailsPageZa.ContactingYou.EmailAddress = email;
                    personalDetailsPageZa.ContactingYou.ConfirmEmailAddress = email;
                    personalDetailsPageZa.PrivacyPolicy = true;
                    personalDetailsPageZa.CanContact = "Yes";
                    personalDetailsPageZa.MarriedInCommunityProperty =
                        "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
                    journeyZa.CurrentPage = personalDetailsPageZa.Submit() as AddressDetailsPage;
                    var accountDetailsPageZa = journeyZa.FillAddressDetails().CurrentPage as AccountDetailsPage;
                    accountDetailsPageZa.AccountDetailsSection.Password = email;
                    Do.Until(accountDetailsPageZa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
                    accountDetailsPageZa.AccountDetailsSection.Password = "Passw0rd";
                    Do.While(accountDetailsPageZa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
                    break;
                #endregion
            }
        }

        [Test, AUT(AUT.Wb), JIRA("QA-256")]
        public void EnsureCustomerCanAddGuarantorsToL0()
        {
            var firstName = Get.RandomString(3, 15);
            var lastName = Get.RandomString(3, 15);
            var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
            var additionalDirectorsPage = journey.ApplyForLoan(5500, 30)
             .AnswerEligibilityQuestions()
             .FillPersonalDetails("TESTNoCheck")
             .FillAddressDetails("More than 4 years")
             .FillAccountDetails()
             .FillBankDetails()
             .FillCardDetails()
             .EnterBusinessDetails().CurrentPage as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = firstName;
            addAdditionalDirectorPage.LastName = lastName;
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            string directors = additionalDirectorsPage.GetDirectors();
            Assert.IsTrue(directors.Contains(firstName + " " + lastName));
        }

        [Test, AUT(AUT.Wb), JIRA("QA-256")]
        public void EnsureAllGurantorsReceiveTheUnsignedGuarantorEmail()
        {
            var email = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(5500, 30)
             .AnswerEligibilityQuestions().CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = Get.GetName();
            personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
            personalDetailsPage.YourName.LastName = Get.RandomString(10);
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1990";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.ContactingYou.HomePhoneNumber = "02071111234";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "07700900001";
            personalDetailsPage.ContactingYou.EmailAddress = email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPage.CanContact = "No";
            personalDetailsPage.PrivacyPolicy = true;
            journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            var additionalDirectorsPage = journey.FillAddressDetails("More than 4 years")
             .FillAccountDetails()
             .FillBankDetails()
             .FillCardDetails()
             .EnterBusinessDetails().CurrentPage as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = Get.RandomString(3, 15);
            addAdditionalDirectorPage.LastName = Get.RandomString(3, 15);
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;
            journey.CurrentPage = addAdditionalDirectorPage.Done() as BusinessBankAccountPage;
            var homePage = journey.EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForApplyTermsPage()
               .ApplyTerms()
               .FillAcceptedPage()
               .GoHomePage();

            var mail = Do.Until(() => Drive.Data.QaData.Db.Emails.FindByEmailAddress(email));
            var mailTemplate = Do.Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail.EmailId, Key: "Html_body"));
            Assert.IsNotNull(mailTemplate);
            Assert.IsTrue(mailTemplate.value.Contains("Good news"));

            var mail2 = Do.Until(() => Drive.Data.QaData.Db.Emails.FindByEmailAddress(additionalDirectorEmail));
            var mailTemplate2 = Do.Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail2.EmailId, Key: "Html_body"));
            Assert.IsNotNull(mailTemplate2);
            Assert.IsTrue(mailTemplate2.value.Contains("Good news"));
        }

        [Test, AUT(AUT.Wb), JIRA("QA-256")]
        public void EnsureWhenL0LandsOnMyAccountsThatTheProgressOfLoanIsAllThatDisplayedAndNotLoanDetails()
        {
            var email = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
            var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(5500, 30)
             .AnswerEligibilityQuestions().CurrentPage as PersonalDetailsPage;
            personalDetailsPage.YourName.FirstName = Get.GetName();
            personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
            personalDetailsPage.YourName.LastName = Get.RandomString(10);
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1990";
            personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.ContactingYou.HomePhoneNumber = "02071111234";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "07700900000";
            personalDetailsPage.ContactingYou.EmailAddress = email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPage.CanContact = "No";
            personalDetailsPage.PrivacyPolicy = true;
            journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            var additionalDirectorsPage = journey.FillAddressDetails("More than 4 years")
             .FillAccountDetails()
             .FillBankDetails()
             .FillCardDetails()
             .EnterBusinessDetails().CurrentPage as AdditionalDirectorsPage;
            var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
            addAdditionalDirectorPage.Title = "Mr";
            addAdditionalDirectorPage.FirstName = Get.RandomString(3, 15);
            addAdditionalDirectorPage.LastName = Get.RandomString(3, 15);
            addAdditionalDirectorPage.EmailAddress = additionalDirectorEmail;
            addAdditionalDirectorPage.ConfirmEmailAddress = additionalDirectorEmail;
            journey.CurrentPage = addAdditionalDirectorPage.Done() as BusinessBankAccountPage;
            var homePage = journey.EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForApplyTermsPage()
               .ApplyTerms()
               .FillAcceptedPage()
               .GoHomePage();
            var myPayments = Client.Payments();
            var mySummary = myPayments.Navigation.MySummaryButtonClick();
            Assert.IsTrue(mySummary.GetMyAccountStatus().Contains(ContentMap.Get.MySummaryPage.AccountStatusMessage));
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-174")]
        public void L0JourneyCustomerUsesCombinationOfFirstNameLastNameAndEmailThatIsInDbRedirectedToLoginPage()
        {
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();

            switch (Config.AUT)
            {
                #region Ca
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetL0Journey(Client.Home());
                    var personalDetailsPageCa = journeyCa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
                    personalDetailsPageCa.ProvinceSection.Province = "British Columbia";
                    Do.Until(() => personalDetailsPageCa.ProvinceSection.ClosePopup());
                    personalDetailsPageCa.YourName.FirstName = name;
                    personalDetailsPageCa.YourName.LastName = surname;
                    personalDetailsPageCa.YourName.Title = "Mr";
                    personalDetailsPageCa.YourDetails.Number = "123213126";
                    personalDetailsPageCa.YourDetails.DateOfBirth = "1/Jan/1980";
                    personalDetailsPageCa.YourDetails.Gender = "Female";
                    personalDetailsPageCa.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPageCa.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageCa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPageCa.EmploymentDetails.MonthlyIncome = "1000";
                    personalDetailsPageCa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
                    personalDetailsPageCa.EmploymentDetails.EmployerIndustry = "Finance";
                    personalDetailsPageCa.EmploymentDetails.EmploymentPosition = "Professional (finance, accounting, legal, HR)";
                    personalDetailsPageCa.EmploymentDetails.TimeWithEmployerYears = "1";
                    personalDetailsPageCa.EmploymentDetails.TimeWithEmployerMonths = "0";
                    personalDetailsPageCa.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPageCa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
                    personalDetailsPageCa.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPageCa.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
                    personalDetailsPageCa.ContactingYou.EmailAddress = customer.Email;
                    personalDetailsPageCa.ContactingYou.ConfirmEmailAddress = customer.Email;
                    personalDetailsPageCa.PrivacyPolicy = true;
                    personalDetailsPageCa.CanContact = true;
                    personalDetailsPageCa.ClickSubmit();
                    var loginPageCa = new LoginPage(Client);
                    Assert.IsTrue(loginPageCa.Url.Contains("/login"));
                    break;
                #endregion
                #region Za
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
                    var personalDetailsPageZa = journeyZa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
                    personalDetailsPageZa.YourName.FirstName = name;
                    personalDetailsPageZa.YourName.LastName = surname;
                    personalDetailsPageZa.YourName.Title = "Mr";
                    personalDetailsPageZa.YourDetails.Number = Get.GetNationalNumber(new DateTime(1957, 3, 10), true);
                    personalDetailsPageZa.YourDetails.DateOfBirth = "10/Mar/1957";
                    personalDetailsPageZa.YourDetails.Gender = "Female";
                    personalDetailsPageZa.YourDetails.HomeStatus = "Owner Occupier";
                    personalDetailsPageZa.YourDetails.HomeLanguage = "English";
                    personalDetailsPageZa.YourDetails.NumberOfDependants = "0";
                    personalDetailsPageZa.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageZa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
                    personalDetailsPageZa.EmploymentDetails.MonthlyIncome = "3000";
                    personalDetailsPageZa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
                    personalDetailsPageZa.EmploymentDetails.EmployerIndustry = "Accountancy";
                    personalDetailsPageZa.EmploymentDetails.EmploymentPosition = "Administration";
                    personalDetailsPageZa.EmploymentDetails.TimeWithEmployerYears = "9";
                    personalDetailsPageZa.EmploymentDetails.TimeWithEmployerMonths = "5";
                    personalDetailsPageZa.EmploymentDetails.WorkPhone = "0123456789";
                    personalDetailsPageZa.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPageZa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
                    personalDetailsPageZa.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPageZa.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
                    personalDetailsPageZa.ContactingYou.EmailAddress = customer.Email;
                    personalDetailsPageZa.ContactingYou.ConfirmEmailAddress = customer.Email;
                    personalDetailsPageZa.PrivacyPolicy = true;
                    personalDetailsPageZa.CanContact = "Yes";
                    personalDetailsPageZa.MarriedInCommunityProperty =
                        "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
                    personalDetailsPageZa.ClickSubmit();
                    var loginPageZa = new LoginPage(Client);
                    Assert.IsTrue(loginPageZa.Url.Contains("/login"));
                    break;
                #endregion
                #region Wb
                case AUT.Wb:
                    var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
                    var personalDetailsPageWb = journeyWb.ApplyForLoan(5500, 30)
                    .AnswerEligibilityQuestions().CurrentPage as PersonalDetailsPage;
                    personalDetailsPageWb.YourName.FirstName = name;
                    personalDetailsPageWb.YourName.LastName = surname;
                    personalDetailsPageWb.YourName.Title = "Mr";
                    personalDetailsPageWb.YourDetails.Gender = "Female";
                    personalDetailsPageWb.YourDetails.DateOfBirth = "1/Jan/1990";
                    personalDetailsPageWb.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPageWb.YourDetails.MaritalStatus = "Single";
                    personalDetailsPageWb.YourDetails.NumberOfDependants = "0";
                    personalDetailsPageWb.ContactingYou.HomePhoneNumber = "02071111234";
                    personalDetailsPageWb.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
                    personalDetailsPageWb.ContactingYou.EmailAddress = customer.Email;
                    personalDetailsPageWb.ContactingYou.ConfirmEmailAddress = customer.Email;
                    personalDetailsPageWb.CanContact = "No";
                    personalDetailsPageWb.PrivacyPolicy = true;
                    personalDetailsPageWb.ClickSubmit();
                    var loginPageWb = new LoginPage(Client);
                    Assert.IsTrue(loginPageWb.Url.Contains("/login"));
                    break;
                #endregion
            }

        }

        [Test, AUT(AUT.Za), JIRA("QA-179"), Category(TestCategories.Smoke)]
        public void L0JourneyCustomerIdNumberShouldBeAlignedWithDOBAndGender()
        {
            var email = Get.RandomEmail();
            var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPageZa = journeyZa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPageZa.YourName.FirstName = Get.RandomString(3, 10);
            personalDetailsPageZa.YourName.LastName = Get.RandomString(3, 10);
            personalDetailsPageZa.YourName.Title = "Mr";
            personalDetailsPageZa.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPageZa.YourDetails.HomeLanguage = "English";
            personalDetailsPageZa.YourDetails.NumberOfDependants = "0";
            personalDetailsPageZa.YourDetails.MaritalStatus = "Single";
            personalDetailsPageZa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPageZa.EmploymentDetails.MonthlyIncome = "3000";
            personalDetailsPageZa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
            personalDetailsPageZa.EmploymentDetails.EmployerIndustry = "Accountancy";
            personalDetailsPageZa.EmploymentDetails.EmploymentPosition = "Administration";
            personalDetailsPageZa.EmploymentDetails.TimeWithEmployerYears = "9";
            personalDetailsPageZa.EmploymentDetails.TimeWithEmployerMonths = "5";
            personalDetailsPageZa.EmploymentDetails.WorkPhone = "0123456789";
            personalDetailsPageZa.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPageZa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPageZa.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPageZa.ContactingYou.CellPhoneNumber = "0770090000";
            personalDetailsPageZa.ContactingYou.EmailAddress = email;
            personalDetailsPageZa.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPageZa.PrivacyPolicy = true;
            personalDetailsPageZa.CanContact = "Yes";
            personalDetailsPageZa.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            personalDetailsPageZa.YourDetails.Number = Get.GetNationalNumber(new DateTime(1957, 3, 10), true);
            personalDetailsPageZa.YourDetails.Gender = "Male";
            personalDetailsPageZa.YourDetails.DateOfBirth = "9/Mar/1957";
            personalDetailsPageZa.YourDetails.Gender = "Female";
            personalDetailsPageZa.YourDetails.DateOfBirth = "10/Mar/1957";
            journeyZa.CurrentPage = personalDetailsPageZa.Submit() as AddressDetailsPage;
        }

        [Test, AUT(AUT.Za), JIRA("QA-275"), Pending("ZA-1952, Za-2489")]
        public void PasswordThatEqualToTheEmailWithUpperLastSimbolAddressWarningMessageShouldDisplayed()
        {
            var email = Get.RandomEmail();
            var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPageZa = journeyZa.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPageZa.YourName.FirstName = Get.RandomString(3, 10);
            personalDetailsPageZa.YourName.LastName = Get.RandomString(3, 10);
            personalDetailsPageZa.YourName.Title = "Mr";
            personalDetailsPageZa.YourDetails.Number = Get.GetNationalNumber(new DateTime(1957, 3, 10), true);
            personalDetailsPageZa.YourDetails.DateOfBirth = "10/Mar/1957";
            personalDetailsPageZa.YourDetails.Gender = "Female";
            personalDetailsPageZa.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPageZa.YourDetails.HomeLanguage = "English";
            personalDetailsPageZa.YourDetails.NumberOfDependants = "0";
            personalDetailsPageZa.YourDetails.MaritalStatus = "Single";
            personalDetailsPageZa.EmploymentDetails.EmploymentStatus = "Employed Full Time";
            personalDetailsPageZa.EmploymentDetails.MonthlyIncome = "3000";
            personalDetailsPageZa.EmploymentDetails.EmployerName = Get.EnumToString(RiskMask.TESTEmployedMask);
            personalDetailsPageZa.EmploymentDetails.EmployerIndustry = "Accountancy";
            personalDetailsPageZa.EmploymentDetails.EmploymentPosition = "Administration";
            personalDetailsPageZa.EmploymentDetails.TimeWithEmployerYears = "9";
            personalDetailsPageZa.EmploymentDetails.TimeWithEmployerMonths = "5";
            personalDetailsPageZa.EmploymentDetails.WorkPhone = "0123456789";
            personalDetailsPageZa.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPageZa.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPageZa.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPageZa.ContactingYou.CellPhoneNumber = "0770090000";
            personalDetailsPageZa.ContactingYou.EmailAddress = email;
            personalDetailsPageZa.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPageZa.PrivacyPolicy = true;
            personalDetailsPageZa.CanContact = "Yes";
            personalDetailsPageZa.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            journeyZa.CurrentPage = personalDetailsPageZa.Submit() as AddressDetailsPage;
            var accountDetailsPageZa = journeyZa.FillAddressDetails().CurrentPage as AccountDetailsPage;
            Console.WriteLine(email.Remove(email.Length - 1, 1) + "M");
            accountDetailsPageZa.AccountDetailsSection.Password = email.Remove(email.Length - 1, 1) + "M";
            accountDetailsPageZa.AccountDetailsSection.PasswordConfirm = email.Remove(email.Length - 1, 1) + "M";
            accountDetailsPageZa.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountDetailsPageZa.AccountDetailsSection.SecretAnswer = "Secret answer";
            try
            {
                accountDetailsPageZa = accountDetailsPageZa.NextClick();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(ContentMap.Get.ProblemProcessingDetailsMessage));
                IWebElement section = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.Fieldset));
                IWebElement password = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.Password));
                IWebElement passwordConfirm = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.PasswordConfirm));
                IWebElement secretQuestion = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.SecretQuestion));
                IWebElement secretAnswer = section.FindElement(By.CssSelector(UiMap.Get.AccountDetailsSection.SecretAnswer));
                IWebElement next = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AccountDetailsPage.NextButton));
                password.SendValue("Passw0rd");
                passwordConfirm.SendValue("Passw0rd");
                secretQuestion.SendValue("Secret question'-.");
                secretAnswer.SendValue("Secret answer");
                next.Click();
                try
                {
                    var page = new HomePage(Client);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains(ContentMap.Get.ProblemProcessingDetailsMessage));
                }
            }
        }

        [Test, AUT(AUT.Za), Category(TestCategories.Smoke), JIRA("QA-277")]
        public void L0JourneyInvalidPostcodeShouldCauseWarningMessageValidPostcodeShouldDimissWarning()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .CurrentPage as AddressDetailsPage;
            addressPage.PostCode = "12.5";
            addressPage.HouseNumber = "25";
            addressPage.Street = "high road";
            addressPage.Town = "Kuku";
            addressPage.County = "Province";
            addressPage.AddressPeriod = "2 to 3 years";
            try
            {
                addressPage = addressPage.NextClick();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(ContentMap.Get.AddressDeatailsPage.PostcodeError));
                IWebElement form = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AddressDetailsPage.FormId));
                IWebElement postCode = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.Postcode));
                IWebElement houseNumber = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.HouseNumber));
                IWebElement addressPeriod = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.AddressPeriod));
                IWebElement next = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.NextButton));
                IWebElement county = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.County));
                IWebElement street = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.Street));
                IWebElement town = form.FirstOrDefaultElement(By.CssSelector(UiMap.Get.AddressDetailsPage.Town));
                postCode.SendValue("1234");
                houseNumber.SendValue("25");
                street.SendValue("high road");
                town.SendValue("Kuku");
                county.SendValue("Province");
                addressPeriod.SelectOption("2 to 3 years");
                next.Click();
                try
                {
                    var page = new AccountDetailsPage(Client);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains(ContentMap.Get.AddressDeatailsPage.PostcodeError));
                }
            }
        }

        [Test, AUT(AUT.Za), Category(TestCategories.Smoke), JIRA("QA-276")]
        public void CustomerUsesExistingIdNumberShouldBeAbleToProceed()
        {
            var customer = Do.Until(() => Drive.Data.Comms.Db.CustomerDetails.FindAllByGender(2).FirstOrDefault());
            Console.WriteLine(customer.NationalNumber.ToString() + "  /  " + customer.DateOfBirth.ToString().Replace(" 00:00:00", ""));

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            journey.DateOfBirth = customer.DateOfBirth;
            journey.NationalId = customer.NationalNumber.ToString();
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;
        }

        [Test, AUT(AUT.Ca), JIRA("QA-280"), Pending("There is no <<Your previous addres>> section whan I select eny addres periods.")]
        public void L0CustomerEntersInappropriatePostcodeToPreviousAddressSectionShouldNotGoFurther()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask)).CurrentPage as AddressDetailsPage;
            addressPage.HouseNumber = "1403";
            addressPage.Street = "Edward";
            addressPage.Town = "Hearst";
            addressPage.PostCode = "V4F3A9";
            addressPage.PostOfficeBox = "C12345";
            addressPage.AddressPeriod = "Less than 4 months";

            addressPage.PreviousAddresDetails.FlatNumber = "4";
            addressPage.PreviousAddresDetails.Street = "Edward";
            addressPage.PreviousAddresDetails.Town = "Hearst";
            addressPage.PreviousAddresDetails.Province = "Alberta";
            addressPage.PreviousAddresDetails.PostCode = "Q0K0K4";
            addressPage.Next();
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-172"), Pending("CA code appearing in ZA - Michael Nowicki to fix")]
        public void L0JourneyCustomerMakeALoanCheckOneLastStepPageValidDataDisplayed()
        {
            int _amountMax;
            int _termMax;

            ApiResponse _response;

            string totalToRepay, repaymentDate, promisesTotalToRepay, promisesDay, loanAmount, promisesLoanAmount;
            int amountOfLoan, termsOfLoan;

            ApiRequest request;
            switch (Config.AUT)
            {
                case AUT.Uk:
                    request = new GetFixedTermLoanOfferUkQuery();
                    break;
                case AUT.Za:
                    request = new GetFixedTermLoanOfferZaQuery();
                    break;
                case AUT.Ca:
                    request = new GetFixedTermLoanOfferCaQuery();
                    break;
                case AUT.Wb:
                    request = new GetBusinessFixedInstallmentLoanOfferWbUkQuery();
                    break;
                default:
                    throw new NotImplementedException();
            }

            _response = Drive.Api.Queries.Post(request);
            _amountMax = (int)Decimal.Parse(_response.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            _termMax = Int32.Parse(_response.Values["TermMax"].Single(), CultureInfo.InvariantCulture);

            amountOfLoan = _amountMax;
            termsOfLoan = _termMax;

            PersonalDetailsPage personalDetailsPage = null;
            var email = Get.RandomEmail();

            AcceptedPage acceptedPage;
            MySummaryPage summaryPage;

            switch (Config.AUT)
            {
                case AUT.Wb:
                    const String middleNameMask = "TESTNoCheck";
                    var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
                    var applyTermsPage = journeyWb.ApplyForLoan(amountOfLoan, termsOfLoan)
                                             .AnswerEligibilityQuestions()
                                             .FillPersonalDetails(middleNameMask)
                                             .FillAddressDetails("More than 4 years")
                                             .FillAccountDetails()
                                             .FillBankDetails()
                                             .FillCardDetails()
                                             .EnterBusinessDetails()
                                             .DeclineAddAdditionalDirector()
                                             .EnterBusinessBankAccountDetails()
                                             .EnterBusinessDebitCardDetails()
                                             .WaitForApplyTermsPage()
                                             .CurrentPage as ApplyTermsPage;

                    loanAmount = applyTermsPage.GetLoanAmount().Replace(",", "") + ".00.";
                    var terms = applyTermsPage.GetTermsOfLoan();

                    acceptedPage = journeyWb.ApplyTerms()
                                       .CurrentPage as AcceptedPage;

                    Assert.IsNotNull(acceptedPage);

                    var promisesTermsOfLoan =
                        acceptedPage.GetTermsOfLoan.Replace("This Agreement will be of ", "").Replace(
                            " weeks duration.", "");
                    promisesLoanAmount = acceptedPage.GetLoanAmount.Replace("TheLoanAmountwillbe", "");

                    var lastPage = journeyWb.FillAcceptedPage()
                                       .GoHomePage()
                                       .CurrentPage as HomePage;
                    Assert.IsNotNull(lastPage);

                    Assert.AreEqual(terms, promisesTermsOfLoan);
                    Assert.AreEqual(loanAmount, promisesLoanAmount);
                    break;

                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetL0Journey(Client.Home());
                    personalDetailsPage =
                        journeyCa.ApplyForLoan(amountOfLoan, termsOfLoan).CurrentPage as PersonalDetailsPage;

                    loanAmount = personalDetailsPage.GetTotalAmount.Remove(0, 1) + ".00";
                    totalToRepay = personalDetailsPage.GetTotalToRepay;
                    repaymentDate = personalDetailsPage.GetRepaymentDate;

                    acceptedPage = journeyCa.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                       .FillAddressDetails()
                                       .FillAccountDetails()
                                       .FillBankDetails()
                                       .WaitForAcceptedPage()
                                       .CurrentPage as AcceptedPage;
                    Assert.IsNotNull(acceptedPage);

                    promisesDay = acceptedPage.GetRepaymentDate;
                    promisesTotalToRepay = acceptedPage.GetTotalToRepay;
                    promisesLoanAmount = acceptedPage.GetLoanAmount.Remove(0, 1);

                    Assert.AreEqual(loanAmount, promisesLoanAmount);
                    Assert.AreEqual(repaymentDate, promisesDay);
                    Assert.AreEqual(totalToRepay, promisesTotalToRepay);

                    summaryPage = journeyCa.FillAcceptedPage()
                                      .GoToMySummaryPage()
                                      .CurrentPage as MySummaryPage;

                    Assert.IsNotNull(summaryPage);
                    break;

                case AUT.Za:
                    var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
                    personalDetailsPage =
                        journeyZa.ApplyForLoan(amountOfLoan, termsOfLoan).CurrentPage as PersonalDetailsPage;

                    loanAmount = personalDetailsPage.GetTotalAmount.Remove(0, 1) + ".00";
                    totalToRepay = personalDetailsPage.GetTotalToRepay;
                    repaymentDate = personalDetailsPage.GetRepaymentDate;

                    acceptedPage = journeyZa.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                       .FillAddressDetails()
                                       .FillAccountDetails()
                                       .FillBankDetails()
                                       .WaitForAcceptedPage()
                                       .CurrentPage as AcceptedPage;
                    Assert.IsNotNull(acceptedPage);

                    promisesDay = acceptedPage.GetRepaymentDate;
                    promisesTotalToRepay = acceptedPage.GetTotalToRepay;
                    promisesLoanAmount = acceptedPage.GetLoanAmount.Remove(0, 1);

                    Assert.AreEqual(loanAmount, promisesLoanAmount);
                    Assert.AreEqual(repaymentDate, promisesDay);
                    Assert.AreEqual(totalToRepay, promisesTotalToRepay);

                    summaryPage = journeyZa.FillAcceptedPage()
                                      .GoToMySummaryPage()
                                      .CurrentPage as MySummaryPage;

                    Assert.IsNotNull(summaryPage);
                    break;
            }
        }
        [Test, AUT(AUT.Wb), JIRA("QA-287"), Category(TestCategories.Smoke)]
        public void WbL0JourneyShouldNotBeAbleToProceedWithoutAcceptingAllEligibilityQuestions()
        {
            int getRandomNumber = Get.RandomInt(0, 7);
            bool[] checkBox = new bool[8] { true, true, true, true, true, true, true, true };
            checkBox[getRandomNumber] = false;

            var journeyWb = JourneyFactory.GetL0JourneyWB(Client.Home());
            var eligibilityQuestionsPage = journeyWb.ApplyForLoan(100, 20)
                                                   .CurrentPage as EligibilityQuestionsPage;

            eligibilityQuestionsPage.CheckActiveCompany = checkBox[0];
            eligibilityQuestionsPage.CheckDirector = checkBox[1];
            eligibilityQuestionsPage.CheckGuarantee = checkBox[2];
            eligibilityQuestionsPage.CheckResident = checkBox[4];
            eligibilityQuestionsPage.CheckDebitCard = checkBox[7];

            var URLbefore = Client.Driver.Url;
            eligibilityQuestionsPage.ClickNextButton();
            Thread.Sleep(2000);
            var URLafter = Client.Driver.Url;

            Assert.AreEqual(URLbefore, URLafter);
            //Assert.IsTrue(e.Message.Contains("was Box must be ticked to proceed"));
        }

        [Test, AUT(AUT.Za), Pending("Test is yet to be complete. Author: Ben Ifie")]
        public void L0DropOff()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .WaitForAcceptedPage()
                                 .IgnoreAcceptingLoanAndReturnToHomePageAndLogin()
                                 .CurrentPage as MySummaryPage;

        }

        [Test, AUT(AUT.Uk), JIRA("UK-969", "UKWEB-250"), MultipleAsserts, Pending("Test is in development. Also waiting for functionality implementation.")]
        public void L0PreAgreementPartonAccountSetupPageTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            journeyL0.ApplyForLoan(200, 10)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails();

            var accountSetupPage = new AccountDetailsPage(this.Client);

            Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
            Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
            Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

            //string baseWindowHdl = Client.Driver.CurrentWindowHandle;

            //Check SECCI popup window
            accountSetupPage.ClickSecciLink();
            // TBD: check header and values and close the pop-up
            //Assert.Contains(accountSetupPage.SecciPopupWindowContent(), "150");
            // end of TBD: check header and values and close the pop-up
            accountSetupPage.ClosePopupWindow();

            Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
            accountSetupPage.ClosePopupWindow();

            Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
            accountSetupPage.ClosePopupWindow();

            // Manually check that loan agreement and SECCI emails are sent
            Console.WriteLine("Manually check that that loan agreement and SECCI emails are sent for user={0}", email);
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-204")]
        public void WhenUserAcceptsTheAgreementThenHeGotEmail()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                  .New()
                  .WithEmailAddress(email)
                  .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();

            var mail = Do.Until(() => Drive.Data.QaData.Db.Email.FindAllByEmailAddress(email)).FirstOrDefault();
            Console.WriteLine(mail.EmailId);
            var mailTemplate = Do.Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail.EmailId, Key: "Loan_Agreement"));
            Console.WriteLine(mailTemplate.Value.ToString());
            Assert.IsNotNull(mailTemplate);
            Assert.IsTrue(mailTemplate.value.ToString().Contains("You promise to pay and will make one repayment of"));
        }

        [Test, AUT(AUT.Za), JIRA("QA-247")]
        [Row(100, 37)]
        [Row(100, 31)]
        [Row(131, 34)]
        [Row(153, 37)]
        public void VerifyThatInduplumNeverBrokenAndTotalToRepayIsSmalestThenTwoLoanAmount(int _loanAmount, int _duration)
        {
            int controlSum = _loanAmount * 2;
            double totalToRepay;

            var HomePage = Client.Home();

            HomePage.Sliders.HowMuch = _loanAmount.ToString();
            HomePage.Sliders.HowLong = _duration.ToString();

            totalToRepay = Convert.ToDouble(HomePage.Sliders.GetTotalToRepay.Remove(0, 1));
            Assert.IsTrue(totalToRepay <= controlSum);

            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetails = journey.ApplyForLoan(_loanAmount, _duration).CurrentPage as PersonalDetailsPage;

            totalToRepay = Convert.ToDouble(personalDetails.GetTotalToRepay.Remove(0, 1));
            Assert.IsTrue(totalToRepay <= controlSum);

            var SummaryPage = journey.FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                    .FillAddressDetails()
                                    .FillAccountDetails()
                                    .FillBankDetails()
                                    .WaitForAcceptedPage()
                                    .CurrentPage as AcceptedPage;
            totalToRepay = Convert.ToDouble(SummaryPage.GetTotalToRepay.Remove(0, 1));
            Assert.IsTrue(totalToRepay <= controlSum);
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-303")]
        public void L0ShouldPossibleToCompleteAnL0WithSelfEmployedStatus()
        {
            string Email = Get.RandomEmail();
            DateTime DateOfBirth = new DateTime(1957, 10, 30);
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;

            switch (Config.AUT)
            {
                #region case Za
                case AUT.Za:
                    string NationalId = Get.GetNationalNumber(DateOfBirth, true);
                    personalDetailsPage.YourName.FirstName = journey.FirstName;
                    personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
                    personalDetailsPage.YourName.LastName = journey.LastName;
                    personalDetailsPage.YourName.Title = "Mr";
                    personalDetailsPage.YourDetails.Number = NationalId.ToString();
                    personalDetailsPage.YourDetails.DateOfBirth = DateOfBirth.ToString("d/MMM/yyyy");
                    personalDetailsPage.YourDetails.Gender = "Female";
                    personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
                    personalDetailsPage.YourDetails.HomeLanguage = "English";
                    personalDetailsPage.YourDetails.NumberOfDependants = "0";
                    personalDetailsPage.YourDetails.MaritalStatus = "Single";
                    personalDetailsPage.EmploymentDetails.EmploymentStatus = "Self Employed";
                    personalDetailsPage.EmploymentDetails.SelfEmployedMonthlyIncome = "3000";
                    personalDetailsPage.EmploymentDetails.WorkPhone = "0123456789";
                    personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPage.EmploymentDetails.IncomeFrequency = "Weekly";
                    personalDetailsPage.EmploymentDetails.SelfNextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
                    personalDetailsPage.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
                    personalDetailsPage.ContactingYou.EmailAddress = Email;
                    personalDetailsPage.ContactingYou.ConfirmEmailAddress = Email;
                    personalDetailsPage.PrivacyPolicy = true;
                    personalDetailsPage.CanContact = "Yes";
                    personalDetailsPage.MarriedInCommunityProperty =
                        "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";

                    journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
                    var processingPageZa = journey.FillAddressDetails()
                              .FillAccountDetails()
                              .FillBankDetails()
                              .CurrentPage as ProcessingPage;
                    var acceptedPageZa = processingPageZa.WaitFor<AcceptedPage>() as AcceptedPage;
                    acceptedPageZa.SignAgreementConfirm();
                    acceptedPageZa.SignDirectDebitConfirm();
                    var dealDoneZa = acceptedPageZa.Submit();
                    break;
                #endregion
                #region case Ca
                case AUT.Ca:
                    personalDetailsPage.ProvinceSection.Province = "British Columbia";
                    Do.Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

                    personalDetailsPage.YourName.FirstName = journey.FirstName;
                    personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
                    personalDetailsPage.YourName.LastName = journey.LastName;
                    personalDetailsPage.YourName.Title = "Mr";
                    personalDetailsPage.YourDetails.Number = "123213126";
                    personalDetailsPage.YourDetails.DateOfBirth = "1/Jan/1980";
                    personalDetailsPage.YourDetails.Gender = "Male";
                    personalDetailsPage.YourDetails.HomeStatus = "Tenant Furnished";
                    personalDetailsPage.YourDetails.MaritalStatus = "Single";
                    personalDetailsPage.EmploymentDetails.EmploymentStatus = "Self Employed";
                    personalDetailsPage.EmploymentDetails.SelfEmployedMonthlyIncome = "1000";
                    personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
                    personalDetailsPage.EmploymentDetails.SelfNextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("dd MMM yyyy");
                    personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
                    personalDetailsPage.ContactingYou.CellPhoneNumber = "9876543210";
                    personalDetailsPage.ContactingYou.EmailAddress = Email;
                    personalDetailsPage.ContactingYou.ConfirmEmailAddress = Email;
                    personalDetailsPage.PrivacyPolicy = true;
                    personalDetailsPage.CanContact = true;
                    journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
                    var processingPageCa = journey.FillAddressDetails()
                              .FillAccountDetails()
                              .FillBankDetails()
                              .CurrentPage as ProcessingPage;
                    var acceptedPage = processingPageCa.WaitFor<AcceptedPage>() as AcceptedPage;
                    acceptedPage.SignConfirmCaL0(DateTime.Now.ToString("d MMM yyyy"), journey.FirstName, journey.LastName);
                    var dealDone = acceptedPage.Submit();
                    break;
                #endregion


            }


        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-302")]
        public void CustomerOnBankDetailsPageClicksOnResendPinLinkAndGoFarther()
        {
            // string telephone = "077009" + Get.RandomLong(1000, 9999).ToString();
            switch (Config.AUT)
            {
                #region Ca
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetL0Journey(Client.Home());
                    var myBankAccountCa = journeyCa.ApplyForLoan(200, 10)
                        .FillPersonalDetails()
                        .FillAddressDetails().FillAccountDetails().CurrentPage as PersonalBankAccountPage;
                    myBankAccountCa.PinVerificationSection.ResendPinClick();
                    Thread.Sleep(2000);
                    myBankAccountCa.PinVerificationSection.CloseResendPinPopup();
                    var pageCa = journeyCa.FillBankDetails()
                        .CurrentPage as ProcessingPage;
                    break;
                #endregion
                #region Za
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
                    var myBankAccountZa = journeyZa.ApplyForLoan(200, 10)
                        .FillPersonalDetails()
                        .FillAddressDetails().FillAccountDetails().CurrentPage as PersonalBankAccountPage;
                    myBankAccountZa.PinVerificationSection.ResendPinClick();
                    Thread.Sleep(2000);
                    myBankAccountZa.PinVerificationSection.CloseResendPinPopup();
                    var pageZa = journeyZa.FillBankDetails()
                        .CurrentPage as ProcessingPage;
                    break;
                #endregion
            }
        }

        [Test, AUT(AUT.Za), JIRA("QA-308")]
        public void ShouldPossibleToCompleteAnL0WithRetiredStatus()
        {
            string Email = Get.RandomEmail();
            DateTime DateOfBirth = new DateTime(1957, 10, 30);
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            string employerName = Get.EnumToString(RiskMask.TESTEmployedMask);

            string NationalId = Get.GetNationalNumber(DateOfBirth, true);
            personalDetailsPage.YourName.FirstName = journey.FirstName;
            personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
            personalDetailsPage.YourName.LastName = journey.LastName;
            personalDetailsPage.YourName.Title = "Mr";
            personalDetailsPage.YourDetails.Number = NationalId.ToString();
            personalDetailsPage.YourDetails.DateOfBirth = DateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = "Female";
            personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPage.YourDetails.HomeLanguage = "English";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Retired";
            personalDetailsPage.EmploymentDetails.SelfEmployedMonthlyIncome = "3000";
            personalDetailsPage.ContactingYou.HomePhoneNumber = "0123456789";
            personalDetailsPage.ContactingYou.CellPhoneNumber = "0123456789";
            personalDetailsPage.EmploymentDetails.NextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = Email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = Email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            personalDetailsPage.MarriedInCommunityProperty =
                "I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";

            journey.CurrentPage = personalDetailsPage.Submit() as AddressDetailsPage;
            var processingPageZa = journey.FillAddressDetails()
                      .FillAccountDetails()
                      .FillBankDetails()
                      .CurrentPage as ProcessingPage;
            var acceptedPageZa = processingPageZa.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPageZa.SignAgreementConfirm();
            acceptedPageZa.SignDirectDebitConfirm();
            var dealDoneZa = acceptedPageZa.Submit();

        }

    }
}
