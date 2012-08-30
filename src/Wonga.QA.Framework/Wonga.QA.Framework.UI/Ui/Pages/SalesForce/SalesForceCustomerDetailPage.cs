using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.SalesForce
{
    public class SalesForceCustomerDetailPage : BaseSfPage
    {
        private IWebElement _loanStatus;
        private IWebElement _loanLink;

        public SalesForceCustomerDetailPage(UiClient client)
            : base(client)
        {    }

        public String LoanStatus
        {
            get
            {
                _loanStatus = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceCustomerDetailPage.LoanStatus));
                return _loanStatus.GetValue();
            }
        }

        public SalesForceLoanDetailPage ViewLoan()
        {
            _loanLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceCustomerDetailPage.LoanLink));
            _loanLink.Click();
            return new SalesForceLoanDetailPage(Client);
        }
    }
}