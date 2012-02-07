using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Pages;

namespace Wonga.QA.Framework.UI.Elements.Sections
{
    public class YourDetailsSection : BaseSection
    {
        private readonly ReadOnlyCollection<IWebElement> _gender;
        private readonly IWebElement _idNumber;
        private readonly IWebElement _dependants;
        private readonly IWebElement _dateOfBirthDay;
        private readonly IWebElement _dateOfBirthMonth;
        private readonly IWebElement _dateOfBirthYear;
        private readonly IWebElement _homeStatus;
        private readonly IWebElement _maritalStatus;

        public String Number { set { _idNumber.SendValue(value); } }
        public String Gender { set { _gender.SelectLabel(value); } }
        public String DateOfBirth
        {
            set
            {
                var date = value.Split('/');
                _dateOfBirthDay.SelectOption(date[0]);
                _dateOfBirthMonth.SelectOption(date[1]);
                _dateOfBirthYear.SelectOption(date[2]);
            }
        }
        public String HomeStatus { set { _homeStatus.SelectOption(value); } }
        public String MaritalStatus { set { _maritalStatus.SelectOption(value); } }
        public String NumberOfDependants { set { _dependants.SelectOption(value); } }

        public YourDetailsSection(BasePage page)
            : base("Personal Details|Your details", page)
        {
            switch (Config.AUT)
            {
                case (AUT.Za):
                    _idNumber = Section.FindElement(By.Name("id_number"));
                    _dependants = Section.FindElement(By.Name("dependants"));
                    break;
                case (AUT.Ca):
                    _idNumber = Section.FindElement(By.Name("id_number_ca"));
                    break;
                case (AUT.Wb):
                    _dependants = Section.FindElement(By.Name("dependants"));
                    break;
            }

            _gender = Section.FindElements(By.Name("gender"));
            _dateOfBirthDay = Section.FindElement(By.Name("date_of_birth[day]"));
            _dateOfBirthMonth = Section.FindElement(By.Name("date_of_birth[month]"));
            _dateOfBirthYear = Section.FindElement(By.Name("date_of_birth[year]"));
            _homeStatus = Section.FindElement(By.Name("home_status"));
            _maritalStatus = Section.FindElement(By.Name("marital_status"));
        }
    }
}
