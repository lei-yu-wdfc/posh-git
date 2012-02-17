using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Sections
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

        public YourDetailsSection(BasePage page) : base(Elements.Get.YourDetailsElement.Legend, page)
        {
            switch (Config.AUT)
            {
                case (AUT.Za):
                    _idNumber = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.IdNumber));
                    _dependants = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.Dependants));
                    break;
                case (AUT.Ca):
                    _idNumber = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.IdNumber));
                    break;
                case (AUT.Wb):
                    _dependants = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.Dependants));
                    break;
                case(AUT.Uk):
                    _dependants = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.Dependants));
                    break;
            }

            _gender = Section.FindElements(By.Name(Elements.Get.YourDetailsElement.Gender));
            _dateOfBirthDay = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.DateOfBirthDay));
            _dateOfBirthMonth = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.DateOfBirthMonth));
            _dateOfBirthYear = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.DateOfBirthYear));
            _homeStatus = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.HomeStatus));
            _maritalStatus = Section.FindElement(By.Name(Elements.Get.YourDetailsElement.MaritalStatus));
        }
    }
}
