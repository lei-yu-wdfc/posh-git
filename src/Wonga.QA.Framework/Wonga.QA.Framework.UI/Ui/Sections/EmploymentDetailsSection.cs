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
        private readonly IWebElement _nextPaydayDateDay;
        private readonly IWebElement _nextPaydayDateMonth;
        private readonly IWebElement _nextPaydayDateYear;
        private readonly IWebElement _workPhone;
        private readonly IWebElement _incomeFrequency;
        private readonly ReadOnlyCollection<IWebElement> _salaryPaidToBank;
        private readonly IWebElement _universityType;
        private readonly IWebElement _universityCity;
        private readonly IWebElement _universityName;
        private readonly IWebElement _yearsInUniversity;
        
        public string  UniversityName
        {
            set {_universityName.SelectOption(value); }
        }
       public string UniversityType
        {
            set {_universityType.SelectOption(value); }
        }

        public string UniversityCity
        {
            set { _universityCity.SelectOption(value); }
        }
 	 	
        public string YearsInUniversity
        {
            set { _yearsInUniversity.SelectOption(value); }
        }

        public string EmploymentStatus
        {
            set{_employmentStatus.SelectOption(value);}
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

        public string NextPayDate
        {
            set
            {
                switch (Config.AUT)
                {
                    case AUT.Uk:
                    case AUT.Za:
                    case AUT.Pl:
                        var date = value.Split('/');
                        _nextPaydayDateDay.SelectOption(date[0]);
                        _nextPaydayDateMonth.SelectOption(date[1]);
                        _nextPaydayDateYear.SelectOption(date[2]);
                        break;
                    default:
                        _nextPaydayDate.SendValue(value);
                        break;
                }   
               
            }
        }

        public EmploymentDetailsSection(BasePage page) : base(UiMap.Get.EmploymentDetailsSection.Fieldset, page)
        {
            _employmentStatus = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.EmploymentStatus));
            _employerIndustry = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.EmployerIndustry));
            _employerName = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.EmployerName));
            _employmentPosition = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.EmploymentPosition));
            _timeWithEmployerYears = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.TimeWithEmployerYears));
            _timeWithEmployerMonths = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.TimeWithEmployerMonths));
            _monthlyIncome = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.MonthlyIncome));
            _salaryPaidToBank = Section.FindElements(By.CssSelector(UiMap.Get.EmploymentDetailsSection.SalaryPaidToBank));
            _incomeFrequency = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.IncomeFrequency));
            switch (Config.AUT)
            {
                case (AUT.Za):
                    _workPhone = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.WorkPhone));
                    _nextPaydayDateDay = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateDay));
                    _nextPaydayDateMonth = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateMonth));
                    _nextPaydayDateYear = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateYear));
                    break;
                case (AUT.Ca):
                    _nextPaydayDate = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDate));
                    break;
                case (AUT.Uk):
                    _nextPaydayDateDay = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateDay));
                    _nextPaydayDateMonth = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateMonth));
                    _nextPaydayDateYear = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateYear));
                    _workPhone = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.WorkPhone));
                    break;
                    case (AUT.Pl):
                    _nextPaydayDateDay = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateDay));
                    _nextPaydayDateMonth = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateMonth));
                    _nextPaydayDateYear = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.NextPaydayDateYear));
                    _workPhone = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.WorkPhone));
                    _universityType =
                        Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.UniversityType));
                    _universityCity = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.UniversityCity));
                    _yearsInUniversity = Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.YearsInUniversity));
                    _universityName =
                        Section.FindElement(By.CssSelector(UiMap.Get.EmploymentDetailsSection.UniversityName));
                    break;

            }
        }
        private bool CanEnterLettersToWorkPhoneField(string text)
        {
            return _workPhone.VerifyTextEntering(text);
        }
    }
}
