using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class ChangeMyAddressElement : BaseElement
    {
        public String Flat
        { 
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.Flat)).SendValue(value);
            }
        }

        public String HouseNumber
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.HouseNumber)).SendValue(value);
            }
        }

        public String Street
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.Street)).SendValue(value);
            }
        }

        public String District
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.District)).SendValue(value);
            }
        }

        public String Town
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.Town)).SendValue(value);
            }
        }

        public String Postcode
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.Postcode)).SendValue(value);
            }
        }

        public String County
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.County)).SendValue(value);
            }
        }

        public String AddressPeriod
        {
            set
            {
                Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.AddressPeriod)).SelectOption(value);
            }
        }
        
        public ChangeMyAddressElement(BasePage page)
            : base(page)
        {
        }

        public bool IsChangeMyAddressTitleDisplayed()
        {
            var title = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.Title));
            return title.Displayed;
        }
        public bool IsPostcodeWarningOccurred()
        {
            try
            {
                var postCodeErrorForm =
                           Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ChangeMyAddressElement.PostcodeErrorForm));
                string postCodeErrorFormClass = postCodeErrorForm.GetAttribute("class");

                if (postCodeErrorFormClass.Equals("invalid"))
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return false;

        }
    }
}
