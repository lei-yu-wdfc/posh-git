using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FADebtsPage : BasePage
    {
        private readonly IWebElement _currentBalance;
        private readonly IWebElement _rentPayments;
        private readonly IWebElement _mortgage;
        private readonly IWebElement _otherSecuredLoans;
        private readonly IWebElement _councilTax;
        private readonly IWebElement _maintenceOrChildSupport;
        private readonly IWebElement _gas;
        private readonly IWebElement _electricity;
        private readonly IWebElement _hirePurchaseOrConditionalSale;
        private readonly IWebElement _other;
        private readonly IWebElement _nonPriorityDebtsCreditor0;
        private readonly IWebElement _nonPriorityDebtsAmount0;
        private readonly IWebElement _nonPriorityDebtsCreditor1;
        private readonly IWebElement _nonPriorityDebtsAmount1;
        private readonly IWebElement _nonPriorityDebtsCreditor2;
        private readonly IWebElement _nonPriorityDebtsAmount2;
        private readonly IWebElement _nonPriorityDebtsCreditor3;
        private readonly IWebElement _nonPriorityDebtsAmount3;
        private readonly IWebElement _nonPriorityDebtsCreditor4;
        private readonly IWebElement _nonPriorityDebtsAmount4;
        private readonly IWebElement _nonPriorityDebtsCreditor5;
        private readonly IWebElement _nonPriorityDebtsAmount5;
        private readonly IWebElement _nonPriorityDebtsCreditor6;
        private readonly IWebElement _nonPriorityDebtsAmount6;
        private readonly IWebElement _nonPriorityDebtsCreditor7;
        private readonly IWebElement _nonPriorityDebtsAmount7;
        private readonly IWebElement _nonPriorityDebtsCreditor8;
        private readonly IWebElement _nonPriorityDebtsAmount8;
        private readonly IWebElement _nonPriorityDebtsCreditor9;
        private readonly IWebElement _nonPriorityDebtsAmount9;

        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FADebtsPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _currentBalance = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.CurrentBalance));
            _rentPayments = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.RentPayments));
            _mortgage = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.Mortgage));
            _otherSecuredLoans = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.OtherSecuredLoans));
            _councilTax = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.CouncilTax));
            _maintenceOrChildSupport = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.MaintenceOrChildSupport));
            _gas = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.Gas));
            _electricity = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.Electricity));
            _hirePurchaseOrConditionalSale = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.HirePurchaseOrConditionalSale));
            _other = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.Other));
            _nonPriorityDebtsCreditor0 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor0));
            _nonPriorityDebtsAmount0 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount0));
            _nonPriorityDebtsCreditor1 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor1));
            _nonPriorityDebtsAmount1 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount1));
            _nonPriorityDebtsCreditor2 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor2));
            _nonPriorityDebtsAmount2 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount2));
            _nonPriorityDebtsCreditor3 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor3));
            _nonPriorityDebtsAmount3 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount3));
            _nonPriorityDebtsCreditor4 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor4));
            _nonPriorityDebtsAmount4 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount4));
            _nonPriorityDebtsCreditor5 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor5));
            _nonPriorityDebtsAmount5 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount5));
            _nonPriorityDebtsCreditor6 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor6));
            _nonPriorityDebtsAmount6 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount6));
            _nonPriorityDebtsCreditor7 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor7));
            _nonPriorityDebtsAmount7 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount7));
            _nonPriorityDebtsCreditor8 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor8));
            _nonPriorityDebtsAmount8 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount8));
            _nonPriorityDebtsCreditor9 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsCreditor9));
            _nonPriorityDebtsAmount9 = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount9));
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.ButtonNext));
        }

        public string RentPayments
        {
            get { return _rentPayments.GetValue(); }
            set { _rentPayments.SendValue(value); }
        }

        public string Mortgage
        {
            get { return _mortgage.GetValue(); }
            set { _mortgage.SendValue(value); }
        }

        public string OtherSecuredLoans
        {
            get { return _otherSecuredLoans.GetValue(); }
            set { _otherSecuredLoans.SendValue(value); }
        }

        public string CouncilTax
        {
            get { return _councilTax.GetValue(); }
            set { _councilTax.SendValue(value); }
        }

        public string MaintenanceOrChildSupport
        {
            get { return _maintenceOrChildSupport.GetValue(); }
            set { _maintenceOrChildSupport.SendValue(value); }
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

        public string HirePurchaseOrConditionalSale
        {
            get { return _hirePurchaseOrConditionalSale.GetValue(); }
            set { _hirePurchaseOrConditionalSale.SendValue(value); }
        }

        public string Other
        {
            get { return _other.GetValue(); }
            set { _other.SendValue(value); }
        }

        public void ClickNonPriorityDebtsAmount0()
        {
            _nonPriorityDebtsAmount0.Click();
        }

        public void ClickOther()
        {
            _other.Click();
        }

        public string NonPriorityDebtsCreditor0
        {
            get { return _nonPriorityDebtsCreditor0.GetValue(); }
            set { _nonPriorityDebtsCreditor0.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor1
        {
            get { return _nonPriorityDebtsCreditor1.GetValue(); }
            set { _nonPriorityDebtsCreditor1.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor2
        {
            get { return _nonPriorityDebtsCreditor2.GetValue(); }
            set { _nonPriorityDebtsCreditor2.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor3
        {
            get { return _nonPriorityDebtsCreditor3.GetValue(); }
            set { _nonPriorityDebtsCreditor3.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor4
        {
            get { return _nonPriorityDebtsCreditor4.GetValue(); }
            set { _nonPriorityDebtsCreditor4.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor5
        {
            get { return _nonPriorityDebtsCreditor5.GetValue(); }
            set { _nonPriorityDebtsCreditor5.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor6
        {
            get { return _nonPriorityDebtsCreditor6.GetValue(); }
            set { _nonPriorityDebtsCreditor6.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor7
        {
            get { return _nonPriorityDebtsCreditor7.GetValue(); }
            set { _nonPriorityDebtsCreditor7.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor8
        {
            get { return _nonPriorityDebtsCreditor8.GetValue(); }
            set { _nonPriorityDebtsCreditor8.SendValue(value); }
        }

        public string NonPriorityDebtsCreditor9
        {
            get { return _nonPriorityDebtsCreditor9.GetValue(); }
            set { _nonPriorityDebtsCreditor9.SendValue(value); }
        }

        public string NonPriorityDebtsAmount0
        {
            get { return _nonPriorityDebtsAmount0.GetValue(); }
            set { _nonPriorityDebtsAmount0.SendValue(value); }
        }

        public string NonPriorityDebtsAmount1
        {
            get { return _nonPriorityDebtsAmount1.GetValue(); }
            set { _nonPriorityDebtsAmount1.SendValue(value); }
        }

        public string NonPriorityDebtsAmount2
        {
            get { return _nonPriorityDebtsAmount2.GetValue(); }
            set { _nonPriorityDebtsAmount2.SendValue(value); }
        }

        public string NonPriorityDebtsAmount3
        {
            get { return _nonPriorityDebtsAmount3.GetValue(); }
            set { _nonPriorityDebtsAmount3.SendValue(value); }
        }

        public string NonPriorityDebtsAmount4
        {
            get { return _nonPriorityDebtsAmount4.GetValue(); }
            set { _nonPriorityDebtsAmount4.SendValue(value); }
        }

        public string NonPriorityDebtsAmount5
        {
            get { return _nonPriorityDebtsAmount5.GetValue(); }
            set { _nonPriorityDebtsAmount5.SendValue(value); }
        }

        public string NonPriorityDebtsAmount6
        {
            get { return _nonPriorityDebtsAmount6.GetValue(); }
            set { _nonPriorityDebtsAmount6.SendValue(value); }
        }

        public string NonPriorityDebtsAmount7
        {
            get { return _nonPriorityDebtsAmount7.GetValue(); }
            set { _nonPriorityDebtsAmount7.SendValue(value); }
        }

        public string NonPriorityDebtsAmount8
        {
            get { return _nonPriorityDebtsAmount0.GetValue(); }
            set { _nonPriorityDebtsAmount8.SendValue(value); }
        }

        public string NonPriorityDebtsAmount9
        {
            get { return _nonPriorityDebtsAmount9.GetValue(); }
            set { _nonPriorityDebtsAmount9.SendValue(value); }
        }

        public string GetPrepopulatedCurrentBalance()
        {
            return _currentBalance.GetValue();
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FAExpenditurePage(Client);
        }

        public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FADebtsPage(Client, validator);
            }
            return new FARepaymentPlanPage(Client);
        }

        private bool FindErrorOnPage(string selector)
        {
            try
            {
                if (Do.Until(() => Client.Driver.FindElement(By.CssSelector(selector))).Text == "")
                {
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        public bool RentPaymentsErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.RentPaymentsError);
        }

        public bool MortgageErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.MortgageError);
        }

        public bool OtherSecuredLoansErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.OtherSecuredLoansError);
        }

        public bool CouncilTaxErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.CouncilTaxError);
        }

        public bool MaintenceOrChildSupportErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.MaintenceOrChildSupportError);
        }

        public bool GasErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.GasError);
        }

        public bool ElectricityErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.ElectricityError);
        }

        public bool HirePurchaseOrConditionalSaleErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.HirePurchaseOrConditionalSaleError);
        }

        public bool OtherErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.OtherError);
        }

        public bool NonPriorityDebtsAmount0ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount0Error);
        }

        public bool NonPriorityDebtsAmount1ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount1Error);
        }

        public bool NonPriorityDebtsAmount2ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount2Error);
        }

        public bool NonPriorityDebtsAmount3ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount3Error);
        }

        public bool NonPriorityDebtsAmount4ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount4Error);
        }

        public bool NonPriorityDebtsAmount5ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount5Error);
        }

        public bool NonPriorityDebtsAmount6ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount6Error);
        }

        public bool NonPriorityDebtsAmount7ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount7Error);
        }

        public bool NonPriorityDebtsAmount8ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount8Error);
        }

        public bool NonPriorityDebtsAmount9ErrorPresent()
        {
            return FindErrorOnPage(UiMap.Get.FinancialAssessmentDebtsPage.NonPriorityDebtsAmount9Error);
        }
    }
}
