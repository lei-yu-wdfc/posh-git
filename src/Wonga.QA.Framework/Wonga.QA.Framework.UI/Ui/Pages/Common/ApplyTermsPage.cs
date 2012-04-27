using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ApplyTermsPage : BasePage, IDecisionPage
    {

        private readonly IWebElement _next;
        private readonly IWebElement _editLoanDurationField;
        
        public ApplyTermsPage(UiClient client) : base(client)
        {
            _next = Content.FindElement(By.CssSelector(UiMap.Get.ApplyTermsPage.NextButton));
            _editLoanDurationField = Content.FindElement(By.CssSelector(UiMap.Get.ApplyTermsPage.EditLoanDurationField));
        }

        public void EditDurationOfLoan(String value)
        {
            _editLoanDurationField.SendValue(value);
        }

        public AcceptedPage Next()
        {
            _next.Click();
            return new AcceptedPage(Client);
        }
    }
}
