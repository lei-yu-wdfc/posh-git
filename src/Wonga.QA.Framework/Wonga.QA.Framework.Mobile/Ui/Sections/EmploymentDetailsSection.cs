using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Elements;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
{
    public class EmploymentDetailsSection : BaseSection
    {
        private readonly IWebElement _employmentStatus;
        private readonly IWebElement _employerIndustry;
        private readonly IWebElement _employerName;
        private readonly IWebElement _employmentPosition;
        private readonly IWebElement _timeWithEmployerYears;
        private readonly IWebElement _timeWithEmployerMonths;
        private readonly IWebElement _selfEmployedmonthlyIncome;
        private readonly IWebElement _monthlyIncome;
        private readonly IWebElement _selfEmployedDateOfNextDueIncome;
        private readonly IWebElement _nextPayday;
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
            set
            {
                Do.Until(() => _incomeFrequency.Displayed);
                _incomeFrequency.SelectOption(value);
            }
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
            set
            {
                Do.Until(() => _monthlyIncome.Displayed);
                _monthlyIncome.SendValue(value);
            }
        }

        public string SelfEmployedMonthlyIncome
        {
            set
            {
                _selfEmployedmonthlyIncome.SendValue(value);
            }
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
                var date = value.Split('/'); //TODO: make mobiscroll select supplied date
                _nextPayday.Click();
                var mobiscroll = Do.Until(() => new DayMonthYearMobiScrollElement(Page.Client));
                mobiscroll.SelectNextPayDate();
            }
        }

        public string SelfNextPayDate
        {
            set
            {
                var date = value.Split('/'); //TODO: make mobiscroll select supplied date
                _selfEmployedDateOfNextDueIncome.Click();
                var mobiscroll = Do.Until(() => new DayMonthYearMobiScrollElement(Page.Client));
                mobiscroll.SelectNextPayDate();
            }
        }

        public EmploymentDetailsSection(BasePageMobile page) : base(UiMapMobile.Get.EmploymentDetailsSection.Fieldset, page)
        {
            _employmentStatus = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmploymentStatus));
            _employerIndustry = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmployerIndustry));
            _employerName = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmployerName));
            _timeWithEmployerYears = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.TimeWithEmployerYears));
            _timeWithEmployerMonths = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.TimeWithEmployerMonths));
            _monthlyIncome = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.MonthlyIncome));
            _selfEmployedmonthlyIncome = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.SelfEmployedMonthlyIncome));
            
            _salaryPaidToBank = Section.FindElements(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.SalaryPaidToBank));
            _incomeFrequency = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.IncomeFrequency));
            switch (Config.AUT)
            {
                case (AUT.Za):
                    _employmentPosition = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmploymentPosition));
                    _workPhone = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.WorkPhone));
                    //Frontend changed to be one field Pay Date
                    _nextPayday = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.NextPayDay));
                    _selfEmployedDateOfNextDueIncome = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.SelfEmployedDateOfNextDueIncome));
                    break;
                case (AUT.Ca):
                    _employmentPosition = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmploymentPosition));
                    _selfEmployedDateOfNextDueIncome = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.SelfEmployedDateOfNextDueIncome));
                    break;
                case (AUT.Uk):
                    _employmentPosition = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmploymentPosition));
                    _nextPayday = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.NextPayDay));
                   
                    _workPhone = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.WorkPhone));
                    break;
                case (AUT.Pl):
                    _workPhone = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.WorkPhone));
                    _universityType =
                        Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.UniversityType));
                    _universityCity = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.UniversityCity));
                    _yearsInUniversity = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.YearsInUniversity));
                    _universityName =
                        Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.UniversityName));
                    break;
                default:
                    _employmentPosition = Section.FindElement(By.CssSelector(UiMapMobile.Get.EmploymentDetailsSection.EmploymentPosition));
                    break;

            }
        }

        public bool AllUniversitiesExists(List<string> list)
        {
            int i= 0;
             bool temp = true;
            while (temp && i<list.Count)
            {
                temp = _universityName.CanSelectOption(list[i]);
                i++;
            }
            return temp;

        }
        
        public bool CanEnterLettersToWorkPhoneField(string text)
        {
            return _workPhone.VerifyTextEntering(text);
        }
    }
}
