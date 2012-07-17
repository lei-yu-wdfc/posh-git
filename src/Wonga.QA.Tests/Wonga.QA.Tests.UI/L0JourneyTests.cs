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
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Ca;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI.Mappings;
﻿using Wonga.QA.Framework.UI.Ui.Validators;
﻿using Wonga.QA.Framework.UI.UiElements.Pages;
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
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var addressPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;
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

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-189"), Category(TestCategories.SmokeTest)]
		public void L0JourneyInvalidPINShouldCauseWarningMessageOnNextPage()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.WithPin("9999")
				.FillAndStop();
			var bankDetailsPage = journey.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
			Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage.Next(); });

		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-177"), Category(TestCategories.SmokeTest)] //AUT.Ca removed because of sliders changing
		public void ChangeLoanAmountAndDurationOnPersonalDetailsViaPlusMinusOptions()
		{
			//CA is out due to new wonga sliders being implemented on homepage only 
			//soon it will be on "my account" and in other regions

			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
			personalDetailsPage.ClickSliderToggler();
			var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;
			personalDetailsPage.ClickAmountPlusButton();
			personalDetailsPage.ClickDurationMinusButton();
			string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
			string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

			Assert.AreNotEqual(firstTotalToRepayValue, totalToRepayAtPersonalDetails);

			var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

			string actualTotalToRepay = acceptedPage.GetTotalToRepay;
			string actualRepaymentDate = acceptedPage.GetRepaymentDate;

			Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
			Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);
			var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;

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
					var mySummaryPage = journey.Teleport<MySummaryPage>() as MySummaryPage;

					actualTotalToRepay = mySummaryPage.GetTotalToRepay;

					Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
					//TODO add the dates comparison
					break;
				//TODO case AUT.Za:
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-176"), Category(TestCategories.SmokeTest)] //AUT.Ca removed because of sliders changing
		public void ChangeLoanAmountAndDurationOnPersonalDetailsViaTypingToTheFields()
		{
			//CA is out due to new wonga sliders being implemented on homepage only 
			//soon it will be on "my account" and in other regions

			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
			personalDetailsPage.ClickSliderToggler();
			var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;
			personalDetailsPage.HowMuch = "195";
			personalDetailsPage.HowLong = "5";
			Client.Driver.FindElement(By.CssSelector(UiMap.Get.PersonalDetailsPage.LoanAmount)).LostFocus();
			string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
			string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

			Assert.AreNotEqual(firstTotalToRepayValue, totalToRepayAtPersonalDetails);

			var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

			string actualTotalToRepay = acceptedPage.GetTotalToRepay;
			string actualRepaymentDate = acceptedPage.GetRepaymentDate;

			Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
			Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);
			var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;

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
					var mySummaryPage = journey.Teleport<MySummaryPage>() as MySummaryPage;

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

			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.FillAndStop();
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

		}

		[Test, AUT(AUT.Pl), JIRA("PL-222")]
		public void WhenFillingFieldsWrongGetWarningMessages()
		{

			var journey = JourneyFactory.GetL0Journey(Client.Home());
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
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
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
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
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
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
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

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
			Assert.IsTrue(Do.With.Message("Problem with employe details section on PersonalDetail page").Until(() => personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.UniversityType, UiMap.Get.EmploymentDetailsSection.UniversityTypeErrorForm)));
			personalDetailsPage.EmploymentDetails.UniversityCity = "Opole";
			Assert.IsTrue(Do.With.Message("Problem with employe details section on PersonalDetail page").Until(() => personalDetailsPage.IsSuccessTickOccured(UiMap.Get.EmploymentDetailsSection.UniversityCity, UiMap.Get.EmploymentDetailsSection.UniversityCityErrorForm)));
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
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
			personalDetailsPage.ClickSliderToggler();
			var firstTotalToRepayValue = personalDetailsPage.GetTotalToRepay;

			personalDetailsPage.MoveAmountSlider = 20;
			personalDetailsPage.MoveDurationSlider = 20;

			string totalToRepayAtPersonalDetails = personalDetailsPage.GetTotalToRepay;
			string repaymentDateAtPersonalDetails = personalDetailsPage.GetRepaymentDate;

			Assert.AreNotEqual(firstTotalToRepayValue, totalToRepayAtPersonalDetails);

			var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

			string actualTotalToRepay = acceptedPage.GetTotalToRepay;
			string actualRepaymentDate = acceptedPage.GetRepaymentDate;

			Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
			Assert.AreEqual(repaymentDateAtPersonalDetails, actualRepaymentDate);
			var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;

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
					var mySummaryPage = journey.Teleport<MySummaryPage>() as MySummaryPage;

					actualTotalToRepay = mySummaryPage.GetTotalToRepay;

					Assert.AreEqual(totalToRepayAtPersonalDetails, actualTotalToRepay);
					//TODO add the dates comparison
					break;
				//TODO case AUT.Za:
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-183"), Category(TestCategories.SmokeTest)]
		public void EnterDifferentPasswordsAtAccountDetailsPageShouldCauseWarningMessage()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			switch (Config.AUT)
			{
				case AUT.Za:
					var accountDetailsPage = journey.Teleport<AccountDetailsPage>() as AccountDetailsPage;
					accountDetailsPage.AccountDetailsSection.Password = "Passw0rd";
					accountDetailsPage.AccountDetailsSection.PasswordConfirm = "Passw0rds";
					accountDetailsPage.AccountDetailsSection.SecretQuestion = "123124";//to lost focus
					Thread.Sleep(500);
					Assert.IsTrue(accountDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
					break;
				case AUT.Ca:
					var addressDetailsPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;
					addressDetailsPage.AccountDetailsSection.Password = "Passw0rd";
					addressDetailsPage.AccountDetailsSection.PasswordConfirm = "Passw0rds";
					addressDetailsPage.AccountDetailsSection.SecretQuestion = "12312"; //to lost focus
					Thread.Sleep(500);
					Assert.IsTrue(addressDetailsPage.AccountDetailsSection.IsPasswordMismatchWarningOccured());
					break;

			}

		}

		[Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-190"), Category(TestCategories.SmokeTest)]
		public void L0JourneyDataOnAcceptedPageShouldBeCorrect()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
			string totalAmountOnPersonalDetails = personalDetailsPage.GetTotalAmount + ".00";
			string totalFeesOnPersonalDetails = personalDetailsPage.GetTotalFees;
			string totalToRepayOnPersonalDetails = personalDetailsPage.GetTotalToRepay;
			string repaymentDateOnPersonalDetails = personalDetailsPage.GetRepaymentDate;

			var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

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
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));

			// Go to the first page:
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

			// Check that the page contains the wonga_doubleclick module v1.0 signature:
			Assert.IsTrue(personalDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-details -->"));

			// Go to the second page:
			var addressDetailsPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;

			// Check that the page contains the wonga_doubleclick module v1.0 signature:
			Assert.IsTrue(addressDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-address -->"));

			// Go to the third page:
			var accountDetailsPage = journey.Teleport<AccountDetailsPage>() as AccountDetailsPage;

			// Check that the page contains the wonga_doubleclick module v1.0 signature:
			Assert.IsTrue(accountDetailsPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-account -->"));

			// Go to the fourth page:
			var personalBankAccountPage = journey.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;

			// Check that the page contains the wonga_doubleclick module v1.0 signature:
			Assert.IsTrue(personalBankAccountPage.Client.Source().Contains("<!-- Output from wonga_lzero_za/apply-bank -->"));
		}

		[Test, AUT(AUT.Uk)]
		public void L0Journey()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var dealDone = journey.Teleport<DealDonePage>() as DealDonePage;
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

		[Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("QA-181")]
		public void L0JourneyCustomerOnCurrentAddressPageDoesNotEnterSomeRequiredFieldsWarningMessageDisplayed()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var addressDetailsPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;
		    Validator validator = new  ValidatorBuilder().Default(Client).WithoutErrorsCheck().Build();

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
                    AddressDetailsPage addresspage2 = new AddressDetailsPage(Client, validator);
					addresspage2.PostCode = Get.GetPostcode();
                    addresspage2.HouseNumber = "";
                    Assert.IsTrue(addresspage2.IsHouseNumberWarningOccurred());
                    AddressDetailsPage addresspage3 = new AddressDetailsPage(Client, validator);
                    addresspage3.HouseNumber = "25";
                    addresspage3.Street = "";
                    Assert.IsTrue(addresspage3.IsStreetWarningOccurred());
                    AddressDetailsPage addresspage4 = new AddressDetailsPage(Client, validator);
                    addresspage4.Street = "high road";
                    addresspage4.Town = "";
                    Assert.IsTrue(addresspage4.IsTownWarningOccurred());
                    AddressDetailsPage addresspage5 = new AddressDetailsPage(Client, validator);
                    addresspage5.Town = "Kuku";
                    addresspage5.County = "";
                    Assert.IsTrue(addresspage5.IsCountyWarningOccurred());
                    AddressDetailsPage addresspage6 = new AddressDetailsPage(Client, validator);
                    addresspage6.County = "Province";
                    addresspage6.AddressPeriod = "--- Please select ---";
                    Assert.IsTrue(addresspage6.IsAddressPeriodWarningOccurred());
					break;
				#endregion
				#region case Ca
				case AUT.Ca:
					addressDetailsPage.Street = "Edward";
					addressDetailsPage.Town = "Hearst";
					addressDetailsPage.PostCode = "V4F3A9";
					addressDetailsPage.AddressPeriod = "2 to 3 years";
					Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    AddressDetailsPage addresspageca2 = new AddressDetailsPage(Client, validator);
                    addresspageca2.HouseNumber = "1403";
                    addresspageca2.Street = "";
                    Assert.IsTrue(addresspageca2.IsStreetWarningOccurred());
                    AddressDetailsPage addresspageca3 = new AddressDetailsPage(Client, validator);
                    addresspageca3.Street = "Edward";
                    addresspageca3.Town = "";
                    Assert.IsTrue(addresspageca3.IsTownWarningOccurred());
                    AddressDetailsPage addresspageca4 = new AddressDetailsPage(Client, validator);
                    addresspageca4.Town = "Hearst";
                    addresspageca4.PostCode = "";
                    Assert.IsTrue(addresspageca4.IsPostcodeWarningOccurred());
                    AddressDetailsPage addresspageca5 = new AddressDetailsPage(Client, validator);
                    addresspageca5.PostCode = "V4F3A9";
                    addresspageca5.AddressPeriod = "--- Please select ---";
                    Assert.IsTrue(addresspageca5.IsAddressPeriodWarningOccurred());
					break;
				#endregion
				#region case Uk
				case AUT.Uk:
					addressDetailsPage.PostCodeLookup = "SW6 6PN";
					addressDetailsPage.LookupByPostCode();
					addressDetailsPage.GetAddressesDropDown();
					Do.With.Message("There is no Adress field on AddresDetails Page").Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
					Do.With.Message("There is no house number field on AddresDetails Page").Until(() => addressDetailsPage.HouseNumber = "93");
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
			var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
				.WithMiddleName("TESTNoCheck");
			var addressDetailsPage = journeyWb.Teleport<AddressDetailsPage>() as AddressDetailsPage;
			addressDetailsPage.PostCode = "SW6 6PN";
			addressDetailsPage.LookupByPostCode();
			addressDetailsPage.GetAddressesDropDown();
			Do.With.Message("There is no Adress field on AddresDetails Page").Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
			Do.With.Message("There is no addres period field on AddresDetails Page").Until(() => addressDetailsPage.AddressPeriod = "2 to 3 years");
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
			string firstName = Get.RandomString(3, 10);
			string lastName = Get.RandomString(3, 10);
			DateTime date;
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.WithFirstName(firstName)
				.WithLastName(lastName);
			MySummaryPage mySummary;
			switch (Config.AUT)
			{
				case AUT.Ca:
					date = DateTime.Now.AddDays(DateHelper.GetNumberOfDaysUntilStartOfLoanForCa() + 20);
					mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;
					var customerCa = Do.With.Message("There is no sought-for customer in DB").Until(() => Drive.Data.Comms.Db.CustomerDetails.FindBy(Forename: firstName, Surname: lastName));
					Console.WriteLine(customerCa.Email.ToString());
					Console.WriteLine(customerCa.AccountId.ToString());
					var applicationCa = Do.With.Message("Theer is no sought-for application in DB").Until(() => Drive.Data.Payments.Db.Applications.FindBy(AccountId: customerCa.AccountId));
					Console.WriteLine(applicationCa.AccountId.ToString());
					var fixedTermApplicationCa = Do.With.Message("There is no fixedTermApplication entry in DB").Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(applicationCa.ApplicationId));
					Assert.AreEqual("200.00", fixedTermApplicationCa.LoanAmount.ToString());
					Assert.AreEqual(String.Format("{0:dddd MMMM yyyy}", date), String.Format("{0:dddd MMMM yyyy}", fixedTermApplicationCa.PromiseDate));
					break;
				case AUT.Za:
					date = DateTime.Now.AddDays(20);
					mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;
					var customerZa = Do.With.Message("There is no sought-for customer in DB").Until(() => Drive.Data.Comms.Db.CustomerDetails.FindBy(Forename: firstName, Surname: lastName));
					Console.WriteLine(customerZa.Email.ToString());
					Console.WriteLine(customerZa.AccountId.ToString());
					var applicationZa = Do.With.Message("Theer is no sought-for application in DB").Until(() => Drive.Data.Payments.Db.Applications.FindBy(AccountId: customerZa.AccountId));
					Console.WriteLine(applicationZa.AccountId.ToString());
					var fixedTermApplicationZa = Do.With.Message("There is no fixedTermApplication entry in DB").Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(applicationZa.ApplicationId));
					Assert.AreEqual("200.00", fixedTermApplicationZa.LoanAmount.ToString());
					Assert.AreEqual(String.Format("{0:d MMMM yyyy}", date), String.Format("{0:d MMMM yyyy}", fixedTermApplicationZa.PromiseDate));
					break;
			}
		}

		[Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-186"), Category(TestCategories.SmokeTest)]
		public void InvalidFormatPasswordShouldCauseWarningMessageAndValidPasswordShouldDissmissWarning()
		{
			switch (Config.AUT)
			{
				case AUT.Ca:
					var journeyCa = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
					var myAccountCa = journeyCa.Teleport<AddressDetailsPage>() as AddressDetailsPage;
					myAccountCa.AccountDetailsSection.Password = "sdfsdfs";
					Thread.Sleep(1000);
					Assert.IsTrue(myAccountCa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
					myAccountCa.AccountDetailsSection.Password = "Sdfdfs123";
					Assert.IsFalse(myAccountCa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
					break;
				case AUT.Za:
					var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
					var myAccountZa = journeyZa.Teleport<AccountDetailsPage>() as AccountDetailsPage;
					myAccountZa.AccountDetailsSection.Password = "sdfsdfs";
					Thread.Sleep(1000);
					Assert.IsTrue(myAccountZa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
					myAccountZa.AccountDetailsSection.Password = "Sdfdfs123";
					Assert.IsFalse(myAccountZa.AccountDetailsSection.IsPasswordInvalidFormatWarningOccured());
					break;
				case AUT.Wb:
					var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
						.WithMiddleName("TESTNoCheck")
						.WithAddresPeriod("2 to 3 years");
					var accountDetailsPage = journeyWb.Teleport<AccountDetailsPage>() as AccountDetailsPage;
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
					var journeyCa = JourneyFactory.GetL0Journey(Client.Home())
						.WithMobilePhone(telephone);
					var myBankAccountCa = journeyCa.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
					Assert.IsTrue(myBankAccountCa.PinVerificationSection.ResendPinClickAndCheck());
					var smsCa = Do.With.Message("There is no sought-for sms in DB").Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(telephone.Replace("077", "177")));
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
					var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
						.WithMobilePhone(telephone);
					var myBankAccountZa = journeyZa.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
					Assert.IsTrue(myBankAccountZa.PinVerificationSection.ResendPinClickAndCheck());
					var smsZa = Do.With.Message("There is no sought-for sms in DB").Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(telephone.Replace("077", "2777")));
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
					var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
						.WithMobilePhone(ukMobileTelephone)
						.WithAddresPeriod("Between 4 months and 2 years");
					var debitCardPage = journeyWb.Teleport<PersonalDebitCardPage>() as PersonalDebitCardPage;
					Assert.IsTrue(debitCardPage.MobilePinVerification.ResendPinClickAndCheck());
					Console.WriteLine(ukMobileTelephone);
					string ukTelephoneWithInternationalCode = ukMobileTelephone.Replace("077", "4477");
					var smsWb = Do.With.Message("There is no sought-for sms in DB").Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(ukTelephoneWithInternationalCode));
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

		[Test, AUT(AUT.Wb), JIRA("QA-258"), Category(TestCategories.SmokeTest)]
		public void TheWongaBusinessPolicyHaveNoReferenceToZaCaUk()
		{
			string ca = "wonga.ca";
			string za = "wonga.co.za";
			string uk = "wonga.com";
			var journey = JourneyFactory.GetL0Journey(Client.Home());
			var personaltDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
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

		[Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-184"), Category(TestCategories.SmokeTest)]
		public void CustomerEntersPasswordThatEqualToTheEmailAddressWarningMessageShouldDisplayed()
		{
			var email = Get.RandomEmail();
			switch (Config.AUT)
			{
				#region Wb
				case AUT.Wb:
					var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmail(email)
						.WithAddresPeriod("2 to 3 years");
					var accountDetailsPageWb = journeyWb.Teleport<AccountDetailsPage>() as AccountDetailsPage;
					//accountDetailsPageWb.AccountDetailsSection.Password = "bla"; // wierd string
					accountDetailsPageWb.AccountDetailsSection.Password = email;
					Do.With.Message("Password that equals email is not warning occured").Until(accountDetailsPageWb.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
					accountDetailsPageWb.AccountDetailsSection.Password = "Passw0rd";
					Do.With.Message("Password that not equals email is warning occured").While(accountDetailsPageWb.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
					break;
				#endregion
				#region Ca
				case AUT.Ca:
					var journeyCa = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmail(email);
					var accountDetailsPageCa = journeyCa.Teleport<AddressDetailsPage>() as AddressDetailsPage;
					accountDetailsPageCa.AccountDetailsSection.Password = email;
					Do.With.Message("Password that equals email is not warning occured").Until(accountDetailsPageCa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
					accountDetailsPageCa.AccountDetailsSection.Password = "Passw0rd";
					Do.With.Message("Password that not equals email is warning occured").While(accountDetailsPageCa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
					break;
				#endregion
				#region Za
				case AUT.Za:
					var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmail(email);
					var accountDetailsPageZa = journeyZa.Teleport<AccountDetailsPage>() as AccountDetailsPage;
					accountDetailsPageZa.AccountDetailsSection.Password = email;
					Do.With.Message("Password that equals email is not warning occured").Until(accountDetailsPageZa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
					accountDetailsPageZa.AccountDetailsSection.Password = "Passw0rd";
					Do.With.Message("Password that not equals email is warning occured").While(accountDetailsPageZa.AccountDetailsSection.IsPasswordEqualsEmailWarningOccured);
					break;
				#endregion
			}
		}

		[Test, AUT(AUT.Wb), JIRA("QA-256")]
		public void EnsureCustomerCanAddGuarantorsToL0()
		{
			var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
			var firstName = Get.RandomString(3, 15);
			var lastName = Get.RandomString(3, 15);
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithMiddleName("TESTNoCheck")
				.WithAddresPeriod("More than 4 years")
				.WithAdditionalDirrector()
				.WithAdditionalDirectorName(firstName)
				.WithAdditionalDirectorSurName(lastName)
				.WithAdditionalDirectorEmail(additionalDirectorEmail)
				.FillAndStop();
			var additionalDirectorsPage = journey.Teleport<AdditionalDirectorsPage>() as AdditionalDirectorsPage;
			var addAdditionalDirectorPage = additionalDirectorsPage.AddAditionalDirector();
			string directors = additionalDirectorsPage.GetDirectors();
			Assert.IsTrue(directors.Contains(firstName + " " + lastName));
		}

		[Test, AUT(AUT.Wb), JIRA("QA-256")]
		public void EnsureAllGurantorsReceiveTheUnsignedGuarantorEmail()
		{
			var email = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
			var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithMiddleName("TESTNoCheck")
				.WithEmail(email)
				.WithAddresPeriod("More than 4 years")
				.WithAdditionalDirrector()
				.WithAdditionalDirectorEmail(additionalDirectorEmail);
			var homePage = journey.Teleport<HomePage>() as HomePage;

			var mail = Do.With.Message("There is no sought-for email in DB").Until(() => Drive.Data.QaData.Db.Emails.FindByEmailAddress(email));
			var mailTemplate = Do.With.Message("There is no sought-for email token in DB").Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail.EmailId, Key: "Html_body"));
			Assert.IsNotNull(mailTemplate);
			Assert.IsTrue(mailTemplate.value.Contains("Good news"));

			var mail2 = Do.With.Message("There is no sought-for email in DB").Until(() => Drive.Data.QaData.Db.Emails.FindByEmailAddress(additionalDirectorEmail));
			var mailTemplate2 = Do.With.Message("There is no sought-for email token in DB").Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail2.EmailId, Key: "Html_body"));
			Assert.IsNotNull(mailTemplate2);
			Assert.IsTrue(mailTemplate2.value.Contains("Good news"));
		}

		[Test, AUT(AUT.Wb), JIRA("QA-256")]
		public void EnsureWhenL0LandsOnMyAccountsThatTheProgressOfLoanIsAllThatDisplayedAndNotLoanDetails()
		{
			var email = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
			var additionalDirectorEmail = String.Format("qa.wonga.com+{0}@gmail.com", Guid.NewGuid());
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithMiddleName("TESTNoCheck")
				.WithEmail(email)
				.WithAddresPeriod("More than 4 years")
				.WithAdditionalDirrector()
				.WithAdditionalDirectorEmail(additionalDirectorEmail);
			var homePage = journey.Teleport<HomePage>() as HomePage;
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
					var journeyCa = JourneyFactory.GetL0Journey(Client.Home())
						.WithFirstName(name)
						.WithLastName(surname)
						.WithEmail(email)
						.FillAndStop();
					var personalDetailsPageCa = journeyCa.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
					personalDetailsPageCa.ClickSubmit();
					var loginPageCa = new LoginPage(Client);
					Assert.IsTrue(loginPageCa.Url.Contains("/login"));
					break;
				#endregion
				#region Za
				case AUT.Za:
					var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
						.WithFirstName(name)
						.WithLastName(surname)
						.WithEmail(email)
						.FillAndStop();
					var personalDetailsPageZa = journeyZa.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
					personalDetailsPageZa.ClickSubmit();
					var loginPageZa = new LoginPage(Client);
					Assert.IsTrue(loginPageZa.Url.Contains("/login"));
					break;
				#endregion
				#region Wb
				case AUT.Wb:
					var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
						.WithFirstName(name)
						.WithLastName(surname)
						.WithEmail(email)
						.FillAndStop();
					var personalDetailsPageWb = journeyWb.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
					personalDetailsPageWb.ClickSubmit();
					var loginPageWb = new LoginPage(Client);
					Assert.IsTrue(loginPageWb.Url.Contains("/login"));
					break;
				#endregion
			}

		}

		[Test, AUT(AUT.Za), JIRA("QA-179"), Category(TestCategories.SmokeTest)]
		public void L0JourneyCustomerIdNumberShouldBeAlignedWithDOBAndGender()
		{
			var email = Get.RandomEmail();
			var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
			var personalDetailsPageZa = journeyZa.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
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
			var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmail(email)
				.WithPassword(email.Remove(email.Length - 1, 1) + "M")
				.FillAndStop();
			var accountDetailsPageZa = journeyZa.Teleport<AccountDetailsPage>() as AccountDetailsPage;
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

		[Test, AUT(AUT.Za), Category(TestCategories.SmokeTest), JIRA("QA-277")]
		public void L0JourneyInvalidPostcodeShouldCauseWarningMessageValidPostcodeShouldDimissWarning()
		{
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.WithPosteCode("12.5")
				.FillAndStop();
			var addressPage = journey.Teleport<AddressDetailsPage>() as AddressDetailsPage;
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

		[Test, AUT(AUT.Za), Category(TestCategories.SmokeTest), JIRA("QA-276")]
		public void CustomerUsesExistingIdNumberShouldBeAbleToProceed()
		{
			var customer = Do.With.Message("There is no customer in DB").Until(() => Drive.Data.Comms.Db.CustomerDetails.FindAllByGender(2).FirstOrDefault());
			Console.WriteLine(customer.NationalNumber.ToString() + "  /  " + customer.DateOfBirth.ToString().Replace(" 00:00:00", ""));
			
			
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.WithNationalId(customer.NationalNumber.ToString())
				.WithDateOfBirth(customer.DateOfBirth);
			var processingPage = journey.Teleport<ProcessingPage>() as ProcessingPage;
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
					var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
						.WithAmount(amountOfLoan).WithDuration(termsOfLoan)
						.WithMiddleName(middleNameMask)
						.WithAddresPeriod("More than 4 years");
					var applyTermsPage = journeyWb.Teleport<ApplyTermsPage>() as ApplyTermsPage;

					loanAmount = applyTermsPage.GetLoanAmount().Replace(",", "") + ".00.";
					var terms = applyTermsPage.GetTermsOfLoan();

					acceptedPage = journeyWb.Teleport<AcceptedPage>() as AcceptedPage;

					Assert.IsNotNull(acceptedPage);

					var promisesTermsOfLoan =
						acceptedPage.GetTermsOfLoan.Replace("This Agreement will be of ", "").Replace(
							" weeks duration.", "");
					promisesLoanAmount = acceptedPage.GetLoanAmount.Replace("TheLoanAmountwillbe", "");

					var lastPage = journeyWb.Teleport<HomePage>() as HomePage;
					Assert.IsNotNull(lastPage);

					Assert.AreEqual(terms, promisesTermsOfLoan);
					Assert.AreEqual(loanAmount, promisesLoanAmount);
					break;

				case AUT.Ca:
					var journeyCa = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
						.WithAmount(amountOfLoan).WithDuration(termsOfLoan);
					personalDetailsPage = journeyCa.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

					loanAmount = personalDetailsPage.GetTotalAmount.Remove(0, 1) + ".00";
					totalToRepay = personalDetailsPage.GetTotalToRepay;
					repaymentDate = personalDetailsPage.GetRepaymentDate;

					acceptedPage = journeyCa.Teleport<AcceptedPage>() as AcceptedPage;
					Assert.IsNotNull(acceptedPage);

					promisesDay = acceptedPage.GetRepaymentDate;
					promisesTotalToRepay = acceptedPage.GetTotalToRepay;
					promisesLoanAmount = acceptedPage.GetLoanAmount.Remove(0, 1);

					Assert.AreEqual(loanAmount, promisesLoanAmount);
					Assert.AreEqual(repaymentDate, promisesDay);
					Assert.AreEqual(totalToRepay, promisesTotalToRepay);

					summaryPage = journeyCa.Teleport<MySummaryPage>() as MySummaryPage;

					Assert.IsNotNull(summaryPage);
					break;

				case AUT.Za:
					var journeyZa = JourneyFactory.GetL0Journey(Client.Home())
						.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
						.WithAmount(amountOfLoan).WithDuration(termsOfLoan);
					personalDetailsPage =
						journeyZa.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

					loanAmount = personalDetailsPage.GetTotalAmount.Remove(0, 1) + ".00";
					totalToRepay = personalDetailsPage.GetTotalToRepay;
					repaymentDate = personalDetailsPage.GetRepaymentDate;

					acceptedPage = journeyZa.Teleport<AcceptedPage>() as AcceptedPage;
					Assert.IsNotNull(acceptedPage);

					promisesDay = acceptedPage.GetRepaymentDate;
					promisesTotalToRepay = acceptedPage.GetTotalToRepay;
					promisesLoanAmount = acceptedPage.GetLoanAmount.Remove(0, 1);

					Assert.AreEqual(loanAmount, promisesLoanAmount);
					Assert.AreEqual(repaymentDate, promisesDay);
					Assert.AreEqual(totalToRepay, promisesTotalToRepay);

					summaryPage = journeyZa.Teleport<MySummaryPage>() as MySummaryPage;

					Assert.IsNotNull(summaryPage);
					break;
			}
		}
		[Test, AUT(AUT.Wb), JIRA("QA-287"), Category(TestCategories.SmokeTest)]
		public void WbL0JourneyShouldNotBeAbleToProceedWithoutAcceptingAllEligibilityQuestions()
		{
			int getRandomNumber = Get.RandomInt(0, 4);
			bool[] checkBox = new bool[5] { true, true, true, true, true };
			checkBox[getRandomNumber] = false;

			var journeyWb = JourneyFactory.GetL0Journey(Client.Home())
				.WithEligibilityQuestions(checkBox[0], checkBox[1], checkBox[2], checkBox[3], checkBox[4])
				.FillAndStop();
			var eligibilityQuestionsPage = journeyWb.Teleport<EligibilityQuestionsPage>() as EligibilityQuestionsPage;

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
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
			var mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;

		}

		[Test, AUT(AUT.Uk), JIRA("UK-969", "UKWEB-250"), MultipleAsserts]
		public void L0PreAgreementPartonAccountSetupPageTest()
		{
			var loginPage = Client.Login();
			string email = Get.RandomEmail();
			int loanAmount = 200;
			Console.WriteLine("email={0}", email);

			// L0 journey
			var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
				.WithAmount(loanAmount)
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.WithEmail(email);
			var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

			Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
			Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
			Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

			Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
			accountSetupPage.ClosePopupWindow();

			Thread.Sleep(1000);

			Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
			accountSetupPage.ClosePopupWindow();

			Thread.Sleep(1000);

			accountSetupPage.ClickSecciLink();
			Assert.Contains(accountSetupPage.SecciPopupWindowContent(), loanAmount.ToString("#"));
			accountSetupPage.ClosePopupWindow();
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

			var mail = Do.With.Message("There is no sought-for email in DB").Until(() => Drive.Data.QaData.Db.Email.FindAllByEmailAddress(email)).FirstOrDefault();
			Console.WriteLine(mail.EmailId);
			var mailTemplate = Do.With.Message("There is no sought-for email token in DB").Until(() => Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: mail.EmailId, Key: "Loan_Agreement"));
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

			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
				.WithAmount(_loanAmount).WithDuration(_loanAmount);
			var personalDetails = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

			totalToRepay = Convert.ToDouble(personalDetails.GetTotalToRepay.Remove(0, 1));
			Assert.IsTrue(totalToRepay <= controlSum);

			var SummaryPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
			totalToRepay = Convert.ToDouble(SummaryPage.GetTotalToRepay.Remove(0, 1));
			Assert.IsTrue(totalToRepay <= controlSum);
		}

		[Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-303")]
		public void L0ShouldPossibleToCompleteAnL0WithSelfEmployedStatus()
		{
			string Email = Get.RandomEmail();
			string firstName = Get.RandomString(3, 10);
			string lastName = Get.RandomString(3, 10);
			DateTime DateOfBirth = new DateTime(1957, 10, 30);
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithFirstName(firstName).WithLastName(lastName);
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

			switch (Config.AUT)
			{
				#region case Za
				case AUT.Za:
					string NationalId = Get.GetNationalNumber(DateOfBirth, true);
					personalDetailsPage.YourName.FirstName = firstName;
					personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
					personalDetailsPage.YourName.LastName = lastName;
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
					var dealDoneZa = journey.Teleport<DealDonePage>() as DealDonePage;
					break;
				#endregion
				#region case Ca
				case AUT.Ca:
					personalDetailsPage.ProvinceSection.Province = "British Columbia";
					Do.With.Message("Problem with closing popup on PersonalDetails Page").Until(() => personalDetailsPage.ProvinceSection.ClosePopup());

					personalDetailsPage.YourName.FirstName = firstName;
					personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
					personalDetailsPage.YourName.LastName = lastName;
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
					var dealDoneCa = journey.Teleport<DealDonePage>() as DealDonePage;
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
					var myBankAccountCa = journeyCa.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
					myBankAccountCa.PinVerificationSection.ResendPinClick();
					Thread.Sleep(2000);
					myBankAccountCa.PinVerificationSection.CloseResendPinPopup();
					var pageCa = journeyCa.Teleport<ProcessingPage>() as ProcessingPage;
					break;
				#endregion
				#region Za
				case AUT.Za:
					var journeyZa = JourneyFactory.GetL0Journey(Client.Home());
					var myBankAccountZa = journeyZa.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
					myBankAccountZa.PinVerificationSection.ResendPinClick();
					Thread.Sleep(2000);
					myBankAccountZa.PinVerificationSection.CloseResendPinPopup();
					var pageZa = journeyZa.Teleport<ProcessingPage>() as ProcessingPage;
					break;
				#endregion
			}
		}

		[Test, AUT(AUT.Za), JIRA("QA-308")]
		public void ShouldPossibleToCompleteAnL0WithRetiredStatus()
		{
			string firstName = Get.RandomString(3, 10);
			string lastName = Get.RandomString(3, 10);
			string Email = Get.RandomEmail();
			DateTime DateOfBirth = new DateTime(1957, 10, 30);
			var journey = JourneyFactory.GetL0Journey(Client.Home())
				.WithFirstName(firstName).WithLastName(lastName);
			var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;
			string employerName = Get.EnumToString(RiskMask.TESTEmployedMask);

			string NationalId = Get.GetNationalNumber(DateOfBirth, true);
			personalDetailsPage.YourName.FirstName = firstName;
			personalDetailsPage.YourName.MiddleName = "TESTNoCheck";
			personalDetailsPage.YourName.LastName = lastName;
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
			var processingPageZa = journey.Teleport<MySummaryPage>() as MySummaryPage;
		}

	}
}