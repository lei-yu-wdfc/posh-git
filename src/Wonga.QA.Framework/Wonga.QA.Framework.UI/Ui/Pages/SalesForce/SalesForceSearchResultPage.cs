using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.SalesForce
{
    public class SalesForceSearchResultPage : BaseSfPage
    {
        private IWebElement _customer;

        public SalesForceSearchResultPage(UiClient client)
            : base(client)
        {
        }

        public bool IsCustomerFind()
        {
            try
            {
                _customer = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceSearchResultPage.Customer));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SalesForceCustomerDetailPage GoToCustomerDetailsPage()
        {
            _customer = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceSearchResultPage.Customer));
            _customer.Click();
            return new SalesForceCustomerDetailPage(Client);
        }
    }
}