using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class PreviousAddresDetailsSection : BaseSection
    {
        private IWebElement _flatNumber;
        private IWebElement _postCode;
        private IWebElement _province;
        private IWebElement _city;
        private IWebElement _street;



        public String PostCode
        {
            set
            {
                _postCode =
                    Section.FirstOrDefaultElement(By.CssSelector(UiMap.Get.PreviousAddresDetailsSection.PostCode));
                _postCode.SendValue(value);
            }
        }
        public String FlatNumber
        {
            set
            {
                _flatNumber =
                   Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PreviousAddresDetailsSection.FlatNumber));
                _flatNumber.SendValue(value);
            }
        }
        public String Province
        {
            set
            {
                _province =
                        Section.FirstOrDefaultElement(By.CssSelector(UiMap.Get.PreviousAddresDetailsSection.Province));
                _province.SelectOption(value);
            }
        }
        public String Town
        {
            set
            {
                _city =
                        Section.FirstOrDefaultElement(By.CssSelector(UiMap.Get.PreviousAddresDetailsSection.Town));
                _city.SendValue(value);
            }
        }
        public String Street
        {
            set
            {
                _street = Section.FirstOrDefaultElement(By.CssSelector(UiMap.Get.PreviousAddresDetailsSection.Street));
                _street.SendValue(value);
            }
        }


        public PreviousAddresDetailsSection(BasePage page)
            : base(UiMap.Get.PreviousAddresDetailsSection.Fieldset, page)
        {
            
               
        }


    }
}
