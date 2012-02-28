using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class EmploymentDetailsSection : BaseSection
    {
        private readonly IWebElement _employmentStatus;
        private readonly IWebElement _employerIndustry;
        private readonly IWebElement _employerName;
        private readonly IWebElement _employmentPosition;
        private readonly IWebElement _timeWithEmployerYears;
        private readonly IWebElement _timeWithEmployerMonths;
        private readonly IWebElement _monthlyIncome;
        private readonly IWebElement _nextPaydayDate;
        private readonly IWebElement _workPhone;
        private readonly IWebElement _incomeFrequency;
        private readonly ReadOnlyCollection<IWebElement> _salaryPaidToBank;

        public string EmploymentStatus
        {
            set{_employmentStatus.SelectOption(value);}
        }

        public string NextPayDate
        {
            set { _nextPaydayDate.SendValue(value); }
        }

        public string IncomeFrequency
        {
            set{_incomeFrequency.SelectOption(value);}
        }

        public bool SalaryPaidToBank
        {
            set
            {
                if (value)
                    _salaryPaidToBank.SelectLabel("Yes");
                else
                    _salaryPaidToBank.SelectLabel("No");
            }
        }

        public string MonthlyIncome
        {
            set{ _monthlyIncome.SendValue(value);}
        }

        public string EmployerName
        {
            set{_employerName.SendValue(value);}
        }

        public string EmployerIndustry
        {
            set{_employerIndustry.SelectOption(value);}
        }

        public string EmploymentPosition
        {
            set{_employmentPosition.SelectOption(value);}
        }

        public string TimeWithEmployerMonths
        {
            set{_timeWithEmployerMonths.SelectOption(value);}
        }

        public string TimeWithEmployerYears
        {
            set { _timeWithEmployerYears.SelectOption(value); }
        }

        public string WorkPhone
        {
            set{_workPhone.SendValue(value);}
        }

        public string NextPayDayDate
        {
            set{_nextPaydayDate.SendValue(value);}
        }

        public EmploymentDetailsSection(BasePage page) : base(Elements.Get.EmploymentDetailsSection.Fieldset, page)
        {
            _employmentStatus = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.EmploymentStatus));
            _employerIndustry = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.EmployerIndustry));
            _employerName = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.EmployerName));
            _employmentPosition = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.EmploymentPosition));
            _timeWithEmployerYears = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.TimeWithEmployerYears));
            _timeWithEmployerMonths = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.TimeWithEmployerMonths));
            _monthlyIncome = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.MonthlyIncome));
            _nextPaydayDate = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.NextPaydayDate));
            _workPhone = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.WorkPhone));
            _salaryPaidToBank = Section.FindElements(By.CssSelector(Elements.Get.EmploymentDetailsSection.SalaryPaidToBank));
            _incomeFrequency = Section.FindElement(By.CssSelector(Elements.Get.EmploymentDetailsSection.IncomeFrequency));
            //switch (Config.AUT)
            //{
            //    case (AUT.Uk):
            //        {

            //            break;
            //        }

            //}
        }
    }
}
