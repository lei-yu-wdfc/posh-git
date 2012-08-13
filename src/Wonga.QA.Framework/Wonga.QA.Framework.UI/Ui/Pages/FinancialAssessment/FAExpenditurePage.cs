using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FAExpenditurePage : BasePage
    {
        private IWebElement _rent;
        private IWebElement _groundRentAndServiceChanges;
        private IWebElement _mortgage;
        private IWebElement _otherSecuredLoans;
        private IWebElement _buildingInsurence;
        private IWebElement _pensionAndLifeInsurence;
        private IWebElement _councilTax;
        private IWebElement _gas;
        private IWebElement _electricity;
        private IWebElement _water;
        private IWebElement _otherUtilites;
        private IWebElement _tVLecense;
        private IWebElement _courtfines;
        private IWebElement _maintence;
        private IWebElement _hirePurchase;
        private IWebElement _childCare;
        private IWebElement _adultCare;
        private IWebElement _otherExp;
        private IWebElement _homePhone;
        private IWebElement _mobilePhone;
        private IWebElement _otherPhone;
        private IWebElement _foodAndMilk;
        private IWebElement _cleaningAndToilet;
        private IWebElement _toiletries;
        private IWebElement _newspapersAndMagazines;
        private IWebElement _cigaretesTabaccoSweets;
        private IWebElement _alcohol;
        private IWebElement _laundaryAndDryCleaning;
        private IWebElement _clothingAndFootwear;
        private IWebElement _babyItems;
        private IWebElement _petItems;
        private IWebElement _otherHousekeeping;
        private IWebElement _publicTransport;
        private IWebElement _otherTaxis;
        private IWebElement _carInsurence;
        private IWebElement _vehicleTaxi;
        private IWebElement _fuel;
        private IWebElement _carMaintance;
        private IWebElement _breakdown;
        private IWebElement _parkingCharges;
        private IWebElement _otherTravelCosts;
        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FAExpenditurePage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _rent = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Rent));
            _groundRentAndServiceChanges = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.GroundRentAndServiceCharges));
            _mortgage = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Mortgage));
            _otherSecuredLoans = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.MonthlySecuredLoanPayments));
            _buildingInsurence = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.BuildingAndContentsInsurance));
            _pensionAndLifeInsurence = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.PensionAndLifeInsurance));
            _councilTax = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.CouncilTaxOrRates));
            _gas = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Gas));
            _electricity = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Electricity));
            _water = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Water));
            _otherUtilites = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.OtherUtilities));
            _tVLecense = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.TVLicense));
            _courtfines = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.CourtFines));
            _maintence = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.MaintenenceOrChildSupportPayments));
            _hirePurchase = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.HirePurchaseOrConditionalSale));
            _childCare = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ChildcareCosts));
            _adultCare = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.AdultcareCosts));
            _otherExp = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.OtherEssentialExpediture));
            _homePhone = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.HomePhone));
            _mobilePhone = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.MobilePhone));
            _otherPhone = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.OtherPhone));
            _foodAndMilk = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Food));
            _cleaningAndToilet = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.LaundryAndDryCleaning));
            _toiletries = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Toiletries));
            _newspapersAndMagazines = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.NewspapersAndMagazines));
            _cigaretesTabaccoSweets = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.CigarettesAndTobacco));
            _alcohol = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Alcohol));
            _laundaryAndDryCleaning = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.LaundryAndDryCleaning));
            _clothingAndFootwear = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ClothingAndFootwear));
            _babyItems = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.BabyItems));
            _petItems = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.PetItems));
            _otherHousekeeping = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.OtherHousekeeping));
            _publicTransport = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.PublicTransport));
            _otherTaxis = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Texis));
            _carInsurence = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.CarInsurance));
            _vehicleTaxi = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.VehicleTax));
            _fuel = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Fuel));
            _carMaintance = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.MOTAndCarMeintenece));
            _breakdown = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.Breakdown));
            _parkingCharges = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ParkingChargesOrTolls));
            _otherTravelCosts = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.OtherTravelcosts));

            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ButtonNext));
        }

        public string Rent
        {
            get { return _rent.GetValue(); }
            set { _rent.SendValue(value); }
        }

        public string GroundRentAndServiceCharges
        {
            get { return _groundRentAndServiceChanges.GetValue(); }
            set { _groundRentAndServiceChanges.SendValue(value); }
        }

        public string Mortgage
        {
            get { return _mortgage.GetValue(); }
            set { _mortgage.SendValue(value); }
        }

        public string MonthlySecuredLoanPayments
        {
            get { return _otherSecuredLoans.GetValue(); }
            set { _otherSecuredLoans.SendValue(value); }
        }

        public string BuildingAndContentsInsurance
        {
            get { return _buildingInsurence.GetValue(); }
            set { _buildingInsurence.SendValue(value); }
        }

        public string PensionAndLifeInsurance
        {
            get { return _pensionAndLifeInsurence.GetValue(); }
            set { _pensionAndLifeInsurence.SendValue(value); }
        }

        public string CouncilTaxOrRates
        {
            get { return _councilTax.GetValue(); }
            set { _councilTax.SendValue(value); }
        }

        public string Gas
        {
            get { return _gas.GetValue(); }
            set { _gas.SendValue(value); }
        }

        public string Electricity
        {
            get { return _electricity.GetValue(); }
            set { _electricity.SendValue(value); }
        }

        public string Water
        {
            get { return _water.GetValue(); }
            set { _water.SendValue(value); }
        }

        public string OtherUtilities
        {
            get { return _otherUtilites.GetValue(); }
            set { _otherUtilites.SendValue(value); }
        }

        public string TvLisense
        {
            get { return _tVLecense.GetValue(); }
            set { _tVLecense.SendValue(value); }
        }

        public string CourtFines
        {
            get { return _courtfines.GetValue(); }
            set { _courtfines.SendValue(value); }
        }
        public string MaintenenceOrChildSupportPayments
        {
            get { return _maintence.GetValue(); }
            set { _maintence.SendValue(value); }
        }

        public string HirePurchaseOrConditionalSale
        {
            get { return _hirePurchase.GetValue(); }
            set { _hirePurchase.SendValue(value); }
        }

        public string ChildcareCosts
        {
            get { return _childCare.GetValue(); }
            set { _childCare.SendValue(value); }
        }

        public string AdultcareCosts
        {
            get { return _adultCare.GetValue(); }
            set { _adultCare.SendValue(value); }
        }
        public string OtherEssentialExpediture
        {
            get { return _otherExp.GetValue(); }
            set { _otherExp.SendValue(value); }
        }

        public string HomePhone
        {
            get { return _homePhone.GetValue(); }
            set { _homePhone.SendValue(value); }
        }

        public string MobilePhone
        {
            get { return _mobilePhone.GetValue(); }
            set { _mobilePhone.SendValue(value); }
        }

        public string OtherPhone
        {
            get { return _otherPhone.GetValue(); }
            set { _otherPhone.SendValue(value); }
        }
        public string Food
        {
            get { return _foodAndMilk.GetValue(); }
            set { _foodAndMilk.SendValue(value); }
        }

        public string Cleaning
        {
            get { return _cleaningAndToilet.GetValue(); }
            set { _cleaningAndToilet.SendValue(value); }
        }

        public string Toiletries
        {
            get { return _toiletries.GetValue(); }
            set { _toiletries.SendValue(value); }
        }

        public string NewspapersAndMagazines
        {
            get { return _newspapersAndMagazines.GetValue(); }
            set { _newspapersAndMagazines.SendValue(value); }
        }
        public string CigarettesAndTobacco
        {
            get { return _cigaretesTabaccoSweets.GetValue(); }
            set { _cigaretesTabaccoSweets.SendValue(value); }
        }

        public string Alcohol
        {
            get { return _alcohol.GetValue(); }
            set { _alcohol.SendValue(value); }
        }

        public string LaundryAndDryCleaning
        {
            get { return _laundaryAndDryCleaning.GetValue(); }
            set { _laundaryAndDryCleaning.SendValue(value); }
        }

        public string ClothingAndFootwear
        {
            get { return _clothingAndFootwear.GetValue(); }
            set { _clothingAndFootwear.SendValue(value); }
        }

        public string BabyItems
        {
            get { return _babyItems.GetValue(); }
            set { _babyItems.SendValue(value); }
        }

        public string PetItems
        {
            get { return _petItems.GetValue(); }
            set { _petItems.SendValue(value); }
        }

        public string OtherHousekeeping
        {
            get { return _otherHousekeeping.GetValue(); }
            set { _otherHousekeeping.SendValue(value); }
        }

        public string PublicTransport
        {
            get { return _publicTransport.GetValue(); }
            set { _publicTransport.SendValue(value); }
        }

        public string Texis
        {
            get { return _otherTaxis.GetValue(); }
            set { _otherTaxis.SendValue(value); }
        }

        public string CarInsurance
        {
            get { return _carInsurence.GetValue(); }
            set { _carInsurence.SendValue(value); }
        }
        public string VehicleTax
        {
            get { return _vehicleTaxi.GetValue(); }
            set { _vehicleTaxi.SendValue(value); }
        }
        public string Fuel
        {
            get { return _fuel.GetValue(); }
            set { _fuel.SendValue(value); }
        }
        public string MOTAndCarMeintenece
        {
            get { return _carMaintance.GetValue(); }
            set { _carMaintance.SendValue(value); }
        }

        public string Breakdown
        {
            get { return _breakdown.GetValue(); }
            set { _breakdown.SendValue(value); }
        }

        public string ParkingChargesOrTolls
        {
            get { return _parkingCharges.GetValue(); }
            set { _parkingCharges.SendValue(value); }
        }

        public string OtherTravelcosts
        {
            get { return _otherTravelCosts.GetValue(); }
            set { _otherTravelCosts.SendValue(value); }
        }


        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FAIncomePage(Client);
        }

        public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FAExpenditurePage(Client, validator);
            }
            return new FADebtsPage(Client);
        }
    }
}
