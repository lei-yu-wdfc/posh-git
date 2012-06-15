using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
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
        private readonly IWebElement _homeLanguage;
        private readonly IWebElement _maritalStatus;
        private readonly IWebElement _peselNumber;
        private readonly IWebElement _motherMaidenName;
        private readonly IWebElement _educationLevel;
        private readonly IWebElement _vehicleOwner;
        private readonly IWebElement _allegroLogin;

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
        public String HomeLanguage { set { _homeLanguage.SelectOption(value); } }
        public String MaritalStatus { set { _maritalStatus.SelectOption(value); } }
        public String NumberOfDependants { set { _dependants.SelectOption(value); } }
        public String PeselNumber { set { _peselNumber.SendValue(value); } }
        public String MotherMaidenName { set { _motherMaidenName.SendValue(value); } }
        public String EducationLevel { set { _educationLevel.SelectOption(value); } }
        public String VehicleOwner { set { _vehicleOwner.SelectOption(value); } }
        public String AllegroLogin { set { _allegroLogin.SendValue(value); } }

        public YourDetailsSection(BasePageMobile page)
            : base(UiMapMobile.Get.YourDetailsSection.Fieldset, page)
        {
            switch (Config.AUT)
            {
                case (AUT.Za):
                    _idNumber = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.IdNumber));
                    _dependants = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Dependants));
                    _homeLanguage = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.HomeLanguage));
                    break;
                case (AUT.Ca):
                    _idNumber = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.IdNumber));
                    break;
                case (AUT.Wb):
                    _dependants = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Dependants));
                    break;
                case (AUT.Uk):
                    _dependants = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Dependants));
                    break;
                case (AUT.Pl):
                    _idNumber = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.IdNumber));
                    _dependants = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Dependants));
                    _peselNumber = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.PeselNumber));
                    _motherMaidenName = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.MotherMaidenName));

                    _educationLevel = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.EducationLevel));
                    _vehicleOwner = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.VehicleOwner));
                    _allegroLogin = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.AllegroLogin));
                    break;
            }

            _gender = Section.FindElements(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Gender));
            _maritalStatus = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.MaritalStatus));
            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Wb:
                case AUT.Uk:
                case AUT.Za:
                    _homeStatus = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.HomeStatus));
                    _dateOfBirthDay = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.DateOfBirthDay));
                    _dateOfBirthMonth = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.DateOfBirthMonth));
                    _dateOfBirthYear = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourDetailsSection.DateOfBirthYear));
                    break;
            }
        }

        public bool IsGenderDoesntMutchIdNumber()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    var messages = Do.Until(() => Section.FindElements(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Warning)));
                    Console.WriteLine(messages[2].Text);

                    if (messages[2].Text.Equals(ContentMapMobile.Get.YourDetailsSection.IdNumberWarning))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public bool IsDOBDoesntMutchIdNumber()
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    var messages = Do.Until(() => Section.FindElements(By.CssSelector(UiMapMobile.Get.YourDetailsSection.Warning)));
                    Console.WriteLine(messages[3].Text);
                    if (messages[3].Text.Equals(ContentMapMobile.Get.YourDetailsSection.IdNumberWarning))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
