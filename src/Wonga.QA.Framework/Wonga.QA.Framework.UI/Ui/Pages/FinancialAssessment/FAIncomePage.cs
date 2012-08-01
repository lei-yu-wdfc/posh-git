using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
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
        //private readonly IWebElement _totalIncome;
        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FAIncomePage(UiClient client)
            : base(client)
        {
            // _totalIncome = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.TotalIncome));
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ButtonNext));
        }

        public bool SetSalaryAfterTax(string salaryAfterTax)
        {
            _salaryAfterTax = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.SalaryAfterTax)));

            _salaryAfterTax.Clear();
            _salaryAfterTax.SendKeys(salaryAfterTax);
            return true;
        }

        public bool SetPartnerSalaryAfterTax(string partnerSalaryAfterTax)
        {
            _partnerSalaryAfterTax = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PartnerSalaryAfterTax)));

            _partnerSalaryAfterTax.Clear();
            _partnerSalaryAfterTax.SendKeys(partnerSalaryAfterTax);
            return true;
        }

        public bool SetJobseekerAllowance(string jobseekerAllowance)
        {
            _jobseekerAllowance = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.JobseekerAllowance)));

            _jobseekerAllowance.Clear();
            _jobseekerAllowance.SendKeys(jobseekerAllowance);
            return true;
        }

        public bool SetIncomeSupport(string incomeSupport)
        {
            _incomeSupport = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.IncomeSupport)));

            _incomeSupport.Clear();
            _incomeSupport.SendKeys(incomeSupport);
            return true;
        }

        public bool SetWorkingTaxCredit(string workingTaxCredit)
        {
            _workingTaxCredit = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.WorkingTaxCredit)));

            _workingTaxCredit.Clear();
            _workingTaxCredit.SendKeys(workingTaxCredit);
            return true;
        }

        public bool SetChildTaxCredit(string childTaxCredit)
        {
            _childTaxCredit = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.ChildTaxCredit)));

            _childTaxCredit.Clear();
            _childTaxCredit.SendKeys(childTaxCredit);
            return true;
        }

        public bool SetStatePension(string statePension)
        {
            _statePension = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.StatePension)));

            _statePension.Clear();
            _statePension.SendKeys(statePension);
            return true;
        }

        public bool SetPrivateOrWorkPension(string privateOrWorkPension)
        {
            _privateOrWorkPension = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PrivateOrWorkPension)));

            _privateOrWorkPension.Clear();
            _privateOrWorkPension.SendKeys(privateOrWorkPension);
            return true;
        }

        public bool SetPensionCredit(string pensionCredit)
        {
            _pensionCredit = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.PensionCredit)));

            _pensionCredit.Clear();
            _pensionCredit.SendKeys(pensionCredit);
            return true;
        }

        public bool SetOther(string other)
        {
            _other = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.Other)));

            _other.Clear();
            _other.SendKeys(other);
            return true;
        }

        public bool SetMaintenenceOrChildSupport(string maintenenceOrChildSupport)
        {
            _maintenenceOrChildSupport = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.MaintenenceOrChildSupport)));

            _maintenenceOrChildSupport.Clear();
            _maintenenceOrChildSupport.SendKeys(maintenenceOrChildSupport);
            return true;
        }

        public bool SetIncomeFromBoardersOrLodgers(string incomeFromBoardersOrLodgers)
        {
            _incomeFromBoardersOrLodgers = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.IncomeFromBoardersOrLodgers)));

            _incomeFromBoardersOrLodgers.Clear();
            _incomeFromBoardersOrLodgers.SendKeys(incomeFromBoardersOrLodgers);
            return true;
        }

        public bool SetStudentLoansOrGrants(string studentLoansOrGrants)
        {
            _studentLoansOrGrants = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.StudentLoansOrGrants)));

            _studentLoansOrGrants.Clear();
            _studentLoansOrGrants.SendKeys(studentLoansOrGrants);
            return true;
        }

        public bool SetOtherIncome(string otherIncome)
        {
            _otherIncome = Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentIncomePage.OtherIncome)));

            _otherIncome.Clear();
            _otherIncome.SendKeys(otherIncome);
            return true;
        }

        public FAAboutYouPage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FAAboutYouPage(Client);
        }

        public FAExpenditurePage NextClick()
        {
            _buttonNext.Click();
            return new FAExpenditurePage(Client);
        }
    }
}
