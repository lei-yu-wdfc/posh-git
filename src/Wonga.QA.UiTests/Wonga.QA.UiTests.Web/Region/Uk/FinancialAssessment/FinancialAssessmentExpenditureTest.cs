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

        private void GetEsentualExpedentureSectionData(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            fields.Add("Rent", faexpenditurepage.Rent);
            fields.Add("GroundRentAndServiceCharges", faexpenditurepage.GroundRentAndServiceCharges);
            fields.Add("Mortgage", faexpenditurepage.Mortgage);
            fields.Add("MonthlySecuredLoanPayments", faexpenditurepage.MonthlySecuredLoanPayments);
            fields.Add("BuildingAndContentsInsurance", faexpenditurepage.BuildingAndContentsInsurance);
            fields.Add("PensionAndLifeInsurance", faexpenditurepage.PensionAndLifeInsurance);
            fields.Add("CouncilTaxOrRates", faexpenditurepage.CouncilTaxOrRates);
            fields.Add("Gas", faexpenditurepage.Gas);
            fields.Add("Electricity", faexpenditurepage.Electricity);
            fields.Add("Water", faexpenditurepage.Water);
            fields.Add("OtherUtilities", faexpenditurepage.OtherUtilities);
            fields.Add("TvLisense", faexpenditurepage.TvLisense);
            fields.Add("CourtFines", faexpenditurepage.CourtFines);
            fields.Add("MaintenenceOrChildSupportPayments", faexpenditurepage.MaintenenceOrChildSupportPayments);
            fields.Add("HirePurchaseOrConditionalSale", faexpenditurepage.HirePurchaseOrConditionalSale);
            fields.Add("ChildcareCosts", faexpenditurepage.ChildcareCosts);
            fields.Add("AdultcareCosts", faexpenditurepage.AdultcareCosts);
            fields.Add("OtherEssentialExpediture", faexpenditurepage.OtherEssentialExpediture);
        }

        private void GetPhoneSectionData(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            fields.Add("HomePhone", faexpenditurepage.HomePhone);
            fields.Add("MobilePhone", faexpenditurepage.MobilePhone);
            fields.Add("OtherPhone", faexpenditurepage.OtherPhone);
        }

        private void GetHouseKeepingSectionData(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            fields.Add("Food", faexpenditurepage.Food);
            fields.Add("Cleaning", faexpenditurepage.Cleaning);
            fields.Add("Toiletries", faexpenditurepage.Toiletries);
            fields.Add("NewspapersAndMagazines", faexpenditurepage.NewspapersAndMagazines);
            fields.Add("CigarettesAndTobacco", faexpenditurepage.CigarettesAndTobacco);
            fields.Add("Alcohol", faexpenditurepage.Alcohol);
            fields.Add("LaundryAndDryCleaning", faexpenditurepage.LaundryAndDryCleaning);
            fields.Add("ClothingAndFootwear", faexpenditurepage.ClothingAndFootwear);
            fields.Add("BabyItems", faexpenditurepage.BabyItems);
            fields.Add("PetItems", faexpenditurepage.PetItems);
            fields.Add("OtherHousekeeping", faexpenditurepage.OtherHousekeeping);
        }

        private void GetTravelSectionData(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            fields.Add("PublicTransport", faexpenditurepage.PublicTransport);
            fields.Add("Texis", faexpenditurepage.Texis);
            fields.Add("CarInsurance", faexpenditurepage.CarInsurance);
            fields.Add("VehicleTax", faexpenditurepage.VehicleTax);
            fields.Add("Fuel", faexpenditurepage.Fuel);
            fields.Add("MOTAndCarMeintenece", faexpenditurepage.MOTAndCarMeintenece);
            fields.Add("Breakdown", faexpenditurepage.Breakdown);
            fields.Add("ParkingChargesOrTolls", faexpenditurepage.ParkingChargesOrTolls);
            fields.Add("OtherTravelcosts", faexpenditurepage.OtherTravelcosts);
        }

        private Dictionary<String, String> SetNewValue(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            Dictionary<String, String> newFields = new Dictionary<String, String>();
            foreach (var field in fields)
            {
                String currentValue = Get.RandomInt(10).ToString();
                PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, field.Key, currentValue);
                newFields.Add(field.Key, field.Value);
                newFields[field.Key] = currentValue;
            }
            return newFields;
        }

        private void CheckDataValidation(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            FieldTypeString[] typesArr = new FieldTypeString[] { FieldTypeString.Numbers };
            foreach (var field in fields)
            {
                ValidationHelper<FieldTypeString, FAExpenditurePage>.CheckValidation(faexpenditurepage, field.Key, typesArr.ToList(), FieldTypeList.ExcludeArray, Validation);
                PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, field.Key, field.Value);
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

        private void CheckDataAfterGoBack(FAExpenditurePage faexpenditurepage, Dictionary<String, String> fields)
        {
            var fadebtspage = faexpenditurepage.NextClick() as FADebtsPage;
            Assert.Contains(fadebtspage.Url, "/debts", "Pass to debts page, with right data in fields");

            faexpenditurepage = fadebtspage.PreviousClick() as FAExpenditurePage;

            foreach (var field in fields)
            {
                Object fieldValue = PropertiesHelper<FAExpenditurePage>.GetFieldValue(faexpenditurepage, field.Key);
                Assert.AreEqual(fieldValue, field.Value, field.Key + " check after go back");
            }
        }

        //1
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataValidationForEsentualExpedenture()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetEsentualExpedentureSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataValidationForPhone()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetPhoneSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataValidationForHouseKeeping()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetHouseKeepingSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataValidationForTravel()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetTravelSectionData(faexpenditurepage, fields);
            CheckDataValidation(faexpenditurepage, fields);
        }

        //2
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataAfterGoBackForEsentualExpedenture()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetEsentualExpedentureSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataAfterGoBackForPhone()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetPhoneSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataAfterGoBackForHouseKeeping()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetHouseKeepingSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckDataAfterGoBackForTravel()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetTravelSectionData(faexpenditurepage, fields);
            fields = SetNewValue(faexpenditurepage, fields);
            CheckDataAfterGoBack(faexpenditurepage, fields);
        }

        //3
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment")]
        public void CheckBlankDataAutoProceed()
        {
            FAExpenditurePage faexpenditurepage = ExedenturePageTeleport();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            GetEsentualExpedentureSectionData(faexpenditurepage, fields);
            GetPhoneSectionData(faexpenditurepage, fields);
            GetHouseKeepingSectionData(faexpenditurepage, fields);
            GetTravelSectionData(faexpenditurepage, fields);

            foreach (var field in fields)
            {
                PropertiesHelper<FAExpenditurePage>.SetFieldValue(faexpenditurepage, field.Key, String.Empty);
            }

            var fadebtspage = faexpenditurepage.NextClick() as FADebtsPage;
            Assert.Contains(fadebtspage.Url, "/debts", "Pass to debts page, with blank fields");
        }
    }
}
