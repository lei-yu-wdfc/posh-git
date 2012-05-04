using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class YourNameSection : BaseSection
    {
        private readonly IWebElement _title;
        private readonly IWebElement _firstName;
        private readonly IWebElement _middleName;
        private readonly IWebElement _lastName;

        public String Title { set { _title.SelectOption(value); } }
        public String FirstName { set { _firstName.SendValue(value); } }
        public String MiddleName { set { _middleName.SendValue(value); } }
        public String LastName { set { _lastName.SendValue(value); } }

        public YourNameSection(BasePage page) : base(UiMap.Get.YourNameSection.Fieldset, page)
        {
            _title = Section.FindElement(By.CssSelector(UiMap.Get.YourNameSection.Title));
            _firstName = Section.FindElement(By.CssSelector(UiMap.Get.YourNameSection.FirstName));
            _middleName = Section.FindElement(By.CssSelector(UiMap.Get.YourNameSection.MiddleName));
            _lastName = Section.FindElement(By.CssSelector(UiMap.Get.YourNameSection.LastName));
        }


    }
}
