using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.SalesForce
{
    public class SalesForceLoanPage : BaseSfPage
    {
        private readonly IWebElement _loanStatus;

        public SalesForceLoanPage(UiClient client)
            : base(client)
        {
            _loanStatus = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SalesForceLoanPage.LoanStatus));
        }

        public String LoanStatus
        {
            get { return _loanStatus.GetValue(); }
        }
    }
}