using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FAIncomePage : BasePage
    {
        private IWebElement _salaryAfterTax;
        private IWebElement _partnerSalaryAfterTax;
        private IWebElement _jobseekerAllowance;
        private IWebElement _incomeSupport;
        private IWebElement _workingTaxCredit;
        private IWebElement _childTaxCredit;
        private IWebElement _statePension;
        private IWebElement _privateOrWorkPension;
        private IWebElement _pensionCredit;
        private IWebElement _other;
        private IWebElement _maintenenceOrChildSupport;
        private IWebElement _incomeFromBoardersOrLodgers;
        private IWebElement _studentLoansOrGrants;
        private IWebElement _otherIncome;
        private readonly IWebElement _totalIncome;
        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;
        private IWebElement _salaryAfterTaxError;
        private IWebElement _partnerSalaryAfterTaxError;
        private IWebElement _jobseekerAllowanceError;
        private IWebElement _incomeSupportError;
        private IWebElement _workingTaxCreditError;
        private IWebElement _childTaxCreditError;
        private IWebElement _statePensionError;
        private IWebElement _privateOrWorkPensionError;
        private IWebElement _pensionCreditError;
        private IWebElement _otherError;
        private IWebElement _maintenenceOrChildSupportError;
        private IWebElement _incomeFromBoardersOrLodgersError;
        private IWebElement _studentLoansOrGrantsError;
        private IWebElement _otherIncomeError;

        public FAIncomePage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _salaryAfterTax = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.SalaryAfterTax));
            _partnerSalaryAfterTax = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PartnerSalaryAfterTax));
            _jobseekerAllowance = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.JobseekerAllowance));
            _incomeSupport = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.IncomeSupport));
            _workingTaxCredit = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.WorkingTaxCredit));
            _childTaxCredit = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ChildTaxCredit));
            _statePension = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.StatePension));
            _privateOrWorkPension = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PrivateOrWorkPension));
            _pensionCredit = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PensionCredit));
            _other = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.Other));
            _maintenenceOrChildSupport = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.MaintenenceOrChildSupport));
            _incomeFromBoardersOrLodgers = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.IncomeFromBoardersOrLodgers));
            _studentLoansOrGrants = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.StudentLoansOrGrants));
            _otherIncome = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.OtherIncome));
            _totalIncome = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.TotalIncome));
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ButtonNext));
        }

        public string SalaryAfterTax
        {
            get { return _salaryAfterTax.GetValue(); }
            set { _salaryAfterTax.SendValue(value); }
        }

        public string PartnerSalaryAfterTax
        {
            get { return _partnerSalaryAfterTax.GetValue(); }
            set { _partnerSalaryAfterTax.SendValue(value); }
        }

        public string JobseekerAllowance
        {
            get { return _jobseekerAllowance.GetValue(); }
            set { _jobseekerAllowance.SendValue(value); }
        }

        public string IncomeSupport
        {
            get { return _incomeSupport.GetValue(); }
            set { _incomeSupport.SendValue(value); }
        }

        public string WorkingTaxCredit
        {
            get { return _workingTaxCredit.GetValue(); }
            set { _workingTaxCredit.SendValue(value); }
        }

        public string ChildTaxCredit
        {
            get { return _childTaxCredit.GetValue(); }
            set { _childTaxCredit.SendValue(value); }
        }

        public string StatePension
        {
            get { return _statePension.GetValue(); }
            set { _statePension.SendValue(value); }
        }

        public string PrivateOrWorkPension
        {
            get { return _privateOrWorkPension.GetValue(); }
            set { _privateOrWorkPension.SendValue(value); }
        }

        public string PensionCredit
        {
            get { return _pensionCredit.GetValue(); }
            set { _pensionCredit.SendValue(value); }
        }

        public string Other
        {
            get { return _other.GetValue(); }
            set { _other.SendValue(value); }
        }

        public string MaintenenceOrChildSupport
        {
            get { return _maintenenceOrChildSupport.GetValue(); }
            set { _maintenenceOrChildSupport.SendValue(value); }
        }

        public string IncomeFromBoardersOrLodgers
        {
            get { return _incomeFromBoardersOrLodgers.GetValue(); }
            set { _incomeFromBoardersOrLodgers.SendValue(value); }
        }

        public string StudentLoansOrGrants
        {
            get { return _studentLoansOrGrants.GetValue(); }
            set { _studentLoansOrGrants.SendValue(value); }
        }

        public string OtherIncome
        {
            get { return _otherIncome.GetValue(); }
            set { _otherIncome.SendValue(value); }
        }

        public void ClickOnOtherIncome()
        {
            _otherIncome.Click();
        }

        public string TotalIncome
        {
            get { return _totalIncome.GetValue(); }
            set { _totalIncome.SendValue(value); }
        }

        public void ClickOnTotalIncome()
        {
            _totalIncome.Click();
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FAAboutYouPage(Client);
        }

        public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FAIncomePage(Client, validator);
            }
            return new FAExpenditurePage(Client);
        }

        public bool SalaryAfterTaxErrorPresent()
        {
            try
            {
                _salaryAfterTaxError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.SalaryAfterTaxError)));
                return true;
            }
            catch { return false; }
        }

        public bool PartnerSalaryAfterTaxErrorPresent()
        {
            try
            {
                _partnerSalaryAfterTaxError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PartnerSalaryAfterTaxError)));
                return true;
            }
            catch { return false; }
        }

        public bool JobseekerAllowanceErrorPresent()
        {
            try
            {
                _jobseekerAllowanceError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.JobseekerAllowanceError)));
                return true;
            }
            catch { return false; }
        }

        public bool IncomeSupportErrorPresent()
        {
            try
            {
                _incomeSupportError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.IncomeSupportError)));
                return true;
            }
            catch { return false; }
        }

        public bool WorkingTaxCreditErrorPresent()
        {
            try
            {
                _workingTaxCreditError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.WorkingTaxCreditError)));
                return true;
            }
            catch { return false; }
        }

        public bool ChildTaxCreditErrorPresent()
        {
            try
            {
                _childTaxCreditError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ChildTaxCreditError)));
                return true;
            }
            catch { return false; }
        }

        public bool StatePensionErrorPresent()
        {
            try
            {
                _statePensionError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.StatePensionError)));
                return true;
            }
            catch { return false; }
        }

        public bool PrivateOrWorkPensionErrorPresent()
        {
            try
            {
                _privateOrWorkPensionError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PrivateOrWorkPensionError)));
                return true;
            }
            catch { return false; }
        }

        public bool PensionCreditErrorPresent()
        {
            try
            {
                _pensionCreditError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PensionCreditError)));
                return true;
            }
            catch { return false; }
        }

        public bool OtherErrorPresent()
        {
            try
            {
                _otherError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.OtherError)));
                return true;
            }
            catch { return false; }
        }

        public bool MaintenenceOrChildSupportErrorPresent()
        {
            try
            {
                _maintenenceOrChildSupportError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.MaintenenceOrChildSupportError)));
                return true;
            }
            catch { return false; }
        }

        public bool IncomeFromBoardersOrLodgersErrorPresent()
        {
            try
            {
                _incomeFromBoardersOrLodgersError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.IncomeFromBoardersOrLodgersError)));
                return true;
            }
            catch { return false; }
        }

        public bool StudentLoansOrGrantsErrorPresent()
        {
            try
            {
                _studentLoansOrGrantsError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.StudentLoansOrGrantsError)));
                return true;
            }
            catch { return false; }
        }

        public bool OtherIncomeErrorPresent()
        {
            try
            {
                _otherIncomeError = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.OtherIncomeError)));
                return true;
            }
            catch { return false; }
        }
    }
}
