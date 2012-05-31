using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Elements
{
    public class ContactElement : BaseElement
    {
        private IWebElement _contactTitle;
        private IWebElement _popup;
        private ReadOnlyCollection<IWebElement> _hrefs;

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

        public bool CheckWbEmailsOnPopup()
        {
            var local = true;
            _popup = Do.Until(()=>Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ContactElement.FormId)));
            _hrefs =  _popup.FindElements(By.CssSelector("a"));
            foreach (var href in _hrefs)
            {
                if (!href.Text.Contains("@wongabusiness.com"))
                {
                    Console.WriteLine(href.Text);
                    local = false;
                }
            }
            return local;
        }
    }
}
