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
        private readonly IWebElement _nextPaydayDay;
        private readonly IWebElement _nextPaydayMonth;
        private readonly IWebElement _nextPaydayYear;

        public string EmploymentStatus
        {
            set { _employmentStatus.SelectOption(value); }
        }

        public string NextPayDate
        {
            set
            {
                switch (Config.AUT)
                {
                    case AUT.Za:
                    case AUT.Ca:
                        _nextPaydayDate.SendValue(value);
                        break;
                    case AUT.Uk:
                        var date = value.Split(' ');
                        _nextPaydayDay.SelectOption(date[0]);
                        _nextPaydayMonth.SelectOption(date[1]);
                        _nextPaydayYear.SelectOption(date[2]);
                        break;

                }
            }
        }

        public string IncomeFrequency
        {
            set { _incomeFrequency.SelectOption(value); }
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
            set { _monthlyIncome.SendValue(value); }
        }

        public string EmployerName
        {
            set { _employerName.SendValue(value); }
        }

        public string EmployerIndustry
        {
            set { _employerIndustry.SelectOption(value); }
        }

        public string EmploymentPosition
        {
            set { _employmentPosition.SelectOption(value); }
        }

        public string TimeWithEmployerMonths
        {
            set { _timeWithEmployerMonths.SelectOption(value); }
        }

        public string TimeWithEmployerYears
        {
            set { _timeWithEmployerYears.SelectOption(value); }
        }

        public string WorkPhone
        {
            set { _workPhone.SendValue(value); }
        }


        public EmploymentDetailsSection(BasePage page)
            : base(Ui.Get.EmploymentDetailsSection.Fieldset, page)
        {
            _employmentStatus = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.EmploymentStatus));
            _employerIndustry = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.EmployerIndustry));
            _employerName = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.EmployerName));
            _employmentPosition = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.EmploymentPosition));
            _timeWithEmployerYears = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.TimeWithEmployerYears));
            _timeWithEmployerMonths = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.TimeWithEmployerMonths));
            _monthlyIncome = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.MonthlyIncome));
            switch (Config.AUT)
            {
                case (AUT.Za):
                    {
                        _nextPaydayDate = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.NextPaydayDate));
                        _salaryPaidToBank = Section.FindElements(By.CssSelector(Ui.Get.EmploymentDetailsSection.SalaryPaidToBank));
                        _incomeFrequency = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.IncomeFrequency));
                        _workPhone = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.WorkPhone));
                        break;
                    }
                case AUT.Ca:
                    {
                        _nextPaydayDate = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.NextPaydayDate));
                        _salaryPaidToBank = Section.FindElements(By.CssSelector(Ui.Get.EmploymentDetailsSection.SalaryPaidToBank));
                        _incomeFrequency = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.IncomeFrequency));
                        break;
                    }
                case AUT.Uk:
                    {
                        _nextPaydayDay = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.NextPaydayDay));
                        _nextPaydayMonth =
                            Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.NextPaydayMonth));
                        _nextPaydayYear = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.NextPaydayYear));
                        _salaryPaidToBank = Section.FindElements(By.CssSelector(Ui.Get.EmploymentDetailsSection.SalaryPaidToBank));
                        _incomeFrequency = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.IncomeFrequency));
                        _workPhone = Section.FindElement(By.CssSelector(Ui.Get.EmploymentDetailsSection.WorkPhone));
                        break;
                    }

            }
        }
    }
}
