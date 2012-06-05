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
    public class ProvinceSection : BaseSection
    {
        private IWebElement _province;

        public string Province { set{_province.SelectOption(value);} }

        public ProvinceSection(BasePage basePage) : base(UiMap.Get.ProvinceSection.Fieldset, basePage)
        
        {switch (Config.AUT)
 	 	
            {
                case AUT.Ca:
                case AUT.Wb:
                case AUT.Uk:
                case AUT.Za:
                    _province = Section.FindElement(By.CssSelector(UiMap.Get.ProvinceSection.Province));
                    break;
 	 	    }
        }

        public bool ClosePopup()
        {
            Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ProvinceSection.PopupCloseLink)).Click();
            return true;
        }
    }
}
