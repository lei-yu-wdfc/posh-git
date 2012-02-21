using System;
using System.Collections.Generic;
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
        private readonly IWebElement _nextPaydayDay;
        private readonly IWebElement _nextPaydayMonth;
        private readonly IWebElement _nextPaydayYear;
        private readonly IWebElement _workPhone;
        private readonly IWebElement _salaryPaidToBank;


        public EmploymentDetailsSection(BasePage page) : base(Elements.Get.EmploymentDetailsSection.Legend, page)
        {
            _employmentStatus = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.EmploymentStatus));
            _employerIndustry = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.EmployerIndustry));
            _employerName = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.EmployerName));
            _employmentPosition = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.EmploymentPosition));
            _timeWithEmployerYears = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.TimeWithEmployerYears));
            _timeWithEmployerMonths = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.TimeWithEmployerMonths));
            _monthlyIncome = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.MonthlyIncome));
            _nextPaydayDay = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.NextPaydayDay));
            _nextPaydayMonth = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.NextPaydayMonth));
            _nextPaydayYear = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.NextPaydayYear));
            _workPhone = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.WorkPhone));
            _salaryPaidToBank = Section.FindElement(By.Name(Elements.Get.EmploymentDetailsSection.SalaryPaidToBank));

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
