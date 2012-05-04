using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Elements
{
    public class ContactElement : BaseElement
    {
        private IWebElement _contactTitle;

        public ContactElement(BasePage page)
            : base(page)
        {
        }

        public bool IsContactPopupPresent()
        {
            try
            {
                _contactTitle = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ContactElement.ContactTitle));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
