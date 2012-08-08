using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Pl.LnL0
{
    [Parallelizable(TestScope.All), AUT(AUT.Pl)]
    public class L0JourneyTests : UiTest
    {

        [Test, JIRA("PL-220")]
        public void FiilAllFieldsAtPersonalDetailsPagePlannedForPolishApplication()
        {

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .FillAndStop();
            var personalDetailsPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

        }

        [Test, JIRA("PL-222")]
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


        [Test, JIRA("PL-220")]
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

        [Test, JIRA("PL-222")]
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

        [Test, JIRA("PL-222")]
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
    }
}
