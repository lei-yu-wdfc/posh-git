using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class ProvinceSection : BaseSection
    {
        private IWebElement _province;

        public string Province { set{_province.SelectOption(value);} }

        public ProvinceSection(BasePage basePage) : base(Elements.Get.ProvinceSection.Fieldset, basePage)
        {
            _province = Section.FindElement(By.CssSelector(Elements.Get.ProvinceSection.Province));
        }

        public bool ClosePopup()
        {
            Page.Client.Driver.FindElement(By.CssSelector(Elements.Get.ProvinceSection.PopupCloseLink)).Click();
            return true;
        }
    }
}
