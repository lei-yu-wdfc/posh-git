using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Ui.Pages.Helpers;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using System.Reflection;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Ui.Enums;
using System.Threading;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssessmentExpenditureTest : UiTest
    {
        private Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            return customer;
        }

        private FAExpenditurePage ExedenturePageTeleport()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            FAExpenditurePage faexpenditurepage =
                journeyLn.FillAndStop().Teleport<FAExpenditurePage>() as FAExpenditurePage;
            return faexpenditurepage;
        }

        private void GetEsentualExpedentureSectionData(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            fields.Add(new DataValidation("Rent", faexpenditurepage.Rent, FieldType.String, FieldTypeList.IncludeArray, new Int32[] { (Int32)FieldTypeString.SpecialSymbols }.ToList()));
            fields.Add(new DataValidation("GroundRentAndServiceCharges", faexpenditurepage.GroundRentAndServiceCharges, FieldType.String));
            fields.Add(new DataValidation("Mortgage", faexpenditurepage.Mortgage, FieldType.String));
            fields.Add(new DataValidation("MonthlySecuredLoanPayments", faexpenditurepage.MonthlySecuredLoanPayments, FieldType.String));
            fields.Add(new DataValidation("BuildingAndContentsInsurance", faexpenditurepage.BuildingAndContentsInsurance, FieldType.String));
            fields.Add(new DataValidation("PensionAndLifeInsurance", faexpenditurepage.PensionAndLifeInsurance, FieldType.String));
            fields.Add(new DataValidation("CouncilTaxOrRates", faexpenditurepage.CouncilTaxOrRates, FieldType.String));
            fields.Add(new DataValidation("Gas", faexpenditurepage.Gas, FieldType.String));
            fields.Add(new DataValidation("Electricity", faexpenditurepage.Electricity, FieldType.String));
            fields.Add(new DataValidation("Water", faexpenditurepage.Water, FieldType.String));
            fields.Add(new DataValidation("OtherUtilities", faexpenditurepage.OtherUtilities, FieldType.String));
            fields.Add(new DataValidation("TvLisense", faexpenditurepage.TvLisense, FieldType.String));
            fields.Add(new DataValidation("CourtFines", faexpenditurepage.CourtFines, FieldType.String));
            fields.Add(new DataValidation("MaintenenceOrChildSupportPayments", faexpenditurepage.MaintenenceOrChildSupportPayments, FieldType.String));
            fields.Add(new DataValidation("HirePurchaseOrConditionalSale", faexpenditurepage.HirePurchaseOrConditionalSale, FieldType.String));
            fields.Add(new DataValidation("ChildcareCosts", faexpenditurepage.ChildcareCosts, FieldType.String));
            fields.Add(new DataValidation("AdultcareCosts", faexpenditurepage.AdultcareCosts, FieldType.String));
            fields.Add(new DataValidation("OtherEssentialExpediture", faexpenditurepage.OtherEssentialExpediture, FieldType.String));
        }

        private void GetPhoneSectionData(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            fields.Add(new DataValidation("HomePhone", faexpenditurepage.HomePhone, FieldType.String));
            fields.Add(new DataValidation("MobilePhone", faexpenditurepage.MobilePhone, FieldType.String));
            fields.Add(new DataValidation("OtherPhone", faexpenditurepage.OtherPhone, FieldType.String, FieldTypeList.IncludeArray, new Int32[] { (Int32)FieldTypeString.SpecialSymbols }.ToList()));
        }

        private void GetHouseKeepingSectionData(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            fields.Add(new DataValidation("Food", faexpenditurepage.Food, FieldType.String));
            fields.Add(new DataValidation("Cleaning", faexpenditurepage.Cleaning, FieldType.String));
            fields.Add(new DataValidation("Toiletries", faexpenditurepage.Toiletries, FieldType.String));
            fields.Add(new DataValidation("NewspapersAndMagazines", faexpenditurepage.NewspapersAndMagazines, FieldType.String));
            fields.Add(new DataValidation("CigarettesAndTobacco", faexpenditurepage.CigarettesAndTobacco, FieldType.String));
            fields.Add(new DataValidation("Alcohol", faexpenditurepage.Alcohol, FieldType.String));
            fields.Add(new DataValidation("LaundryAndDryCleaning", faexpenditurepage.LaundryAndDryCleaning, FieldType.String));
            fields.Add(new DataValidation("ClothingAndFootwear", faexpenditurepage.ClothingAndFootwear, FieldType.String));
            fields.Add(new DataValidation("BabyItems", faexpenditurepage.BabyItems, FieldType.String));
            fields.Add(new DataValidation("PetItems", faexpenditurepage.PetItems, FieldType.String));
            fields.Add(new DataValidation("OtherHousekeeping", faexpenditurepage.OtherHousekeeping, FieldType.String));
        }

        private void GetTravelSectionData(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            fields.Add(new DataValidation("PublicTransport", faexpenditurepage.PublicTransport, FieldType.String));
            fields.Add(new DataValidation("Texis", faexpenditurepage.Texis, FieldType.String));
            fields.Add(new DataValidation("CarInsurance", faexpenditurepage.CarInsurance, FieldType.String));
            fields.Add(new DataValidation("VehicleTax", faexpenditurepage.VehicleTax, FieldType.String));
            fields.Add(new DataValidation("Fuel", faexpenditurepage.Fuel, FieldType.String));
            fields.Add(new DataValidation("MOTAndCarMeintenece", faexpenditurepage.MOTAndCarMeintenece, FieldType.String));
            fields.Add(new DataValidation("Breakdown", faexpenditurepage.Breakdown, FieldType.String));
            fields.Add(new DataValidation("ParkingChargesOrTolls", faexpenditurepage.ParkingChargesOrTolls, FieldType.String));
            fields.Add(new DataValidation("OtherTravelcosts", faexpenditurepage.OtherTravelcosts, FieldType.String));
        }

        private List<DataValidation> SetNewValue(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            List<DataValidation> newFields = new List<DataValidation>();
            foreach (var field in fields)
            {
                String currentValue = Get.RandomInt(10).ToString();
                PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, field.FieldName, currentValue);
                if (field.IsExtended)
                    newFields.Add(new DataValidation(field.FieldName, currentValue, field.FieldType, field.FieldTypeList, field.RulesArray));
                else
                    newFields.Add(new DataValidation(field.FieldName, currentValue, field.FieldType));
            }
            return newFields;
        }

        private void CheckDataValidation(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            Dictionary<FieldType, Int32[]> dictionary = new Dictionary<FieldType, Int32[]>();
            dictionary.Add(FieldType.String, new Int32[] { (Int32)FieldTypeString.Numbers });

            foreach (var field in fields)
            {
                if (!field.IsExtended)
                    ValidationHelper<FAExpenditurePage>.CheckValidation(faexpenditurepage, field.FieldName, dictionary[field.FieldType].ToList(), FieldTypeList.ExcludeArray, Validation, null, field.FieldType);
                else
                    ValidationHelper<FAExpenditurePage>.CheckValidation(faexpenditurepage, field.FieldName, field.RulesArray, field.FieldTypeList, Validation, null, field.FieldType);
                PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, field.FieldName, field.FieldValue);
            }
        }


        private void Validation(FAExpenditurePage faexpenditurepage, String fieldName, String value)
        {
            BasePage basepage;
            PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, fieldName, value);
            try { basepage = faexpenditurepage.NextClick(error: true); }
            catch { basepage = new FADebtsPage(Client); }
            Assert.Contains(faexpenditurepage.Url, "/expenditure", String.Format("Pass to expedenture page, with invalid {0} field", fieldName));
            if (faexpenditurepage.Url.Contains("/debts")) { faexpenditurepage = (basepage as FADebtsPage).PreviousClick() as FAExpenditurePage; }
        }

        private void CheckDataAfterGoBack(FAExpenditurePage faexpenditurepage, List<DataValidation> fields)
        {
            var fadebtspage = faexpenditurepage.NextClick() as FADebtsPage;
            Assert.Contains(fadebtspage.Url, "/debts", "Pass to debts page, with right data in fields");

            faexpenditurepage = fadebtspage.PreviousClick() as FAExpenditurePage;

            foreach (var field in fields)
            {
                Object fieldValue = PropertiesHelper<FAExpenditurePage>.GetFieldValue(faexpenditurepage, field.FieldName);
                Assert.AreEqual(fieldValue, field.FieldValue, field.FieldName + " check after go back");
            }
        }

        //1
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataValidationForEsentualExpedenture()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetEsentualExpedentureSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataValidationForPhone()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetPhoneSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataValidationForHouseKeeping()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetHouseKeepingSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataValidationForTravel()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetTravelSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        //2
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataAfterGoBackForEsentualExpedenture()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetEsentualExpedentureSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataAfterGoBackForPhone()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetPhoneSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataAfterGoBackForHouseKeeping()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetHouseKeepingSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckDataAfterGoBackForTravel()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetTravelSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        //3
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckBlankDataAutoProceed()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            List<DataValidation> fields = new List<DataValidation>();
            GetEsentualExpedentureSectionData(faexpenditurepage, fields);
            GetPhoneSectionData(faexpenditurepage, fields);
            GetHouseKeepingSectionData(faexpenditurepage, fields);
            GetTravelSectionData(faexpenditurepage, fields);

            foreach (var field in fields)
            {
                PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, field.FieldName, String.Empty);
            }

            var fadebtspage = faexpenditurepage.NextClick() as FADebtsPage;
            Assert.Contains(fadebtspage.Url, "/debts", "Pass to debts page, with blank fields");
        }
    }
}
