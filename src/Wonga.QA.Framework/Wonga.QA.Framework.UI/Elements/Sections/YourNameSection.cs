using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Pages;

namespace Wonga.QA.Framework.UI.Elements.Sections
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

        public YourNameSection(BasePage page)
            : base("Personal Details|Your name", page)
        {
            _title = Section.FindElement(By.Name("title"));
            _firstName = Section.FindElement(By.Name("first_name"));
            _middleName = Section.FindElement(By.Name("middle_name"));
            _lastName = Section.FindElement(By.Name("last_name"));
        }


    }
}
