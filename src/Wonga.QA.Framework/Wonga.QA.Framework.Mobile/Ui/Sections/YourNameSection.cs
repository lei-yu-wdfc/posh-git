using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
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

        public YourNameSection(BasePageMobile page)
            : base(UiMapMobile.Get.YourNameSection.Fieldset, page)
        {
           
            _firstName = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourNameSection.FirstName));
            _middleName = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourNameSection.MiddleName));
            _lastName = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourNameSection.LastName));
            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Wb:
                case AUT.Uk:
                case AUT.Za:
                    _title = Section.FindElement(By.CssSelector(UiMapMobile.Get.YourNameSection.Title));

                    break;
            }
        }


    }
}
