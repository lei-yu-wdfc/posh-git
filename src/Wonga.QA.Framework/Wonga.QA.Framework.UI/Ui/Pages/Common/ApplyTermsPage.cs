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
        private IWebElement _form;
        private readonly IWebElement _next;
        private readonly IWebElement _editLoanDurationField;
        private IWebElement _loanAmount;
        private IWebElement _termsOfLoan;
        
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
            _next.Submit();
            return new AcceptedPage(Client);
        }
        public string GetLoanAmount()
        {
               _form = Content.FindElement(By.CssSelector(UiMap.Get.ApplyTermsPage.FormId));
               _loanAmount = _form.FindElement(By.CssSelector(UiMap.Get.ApplyTermsPage.LoanAmount));
            return _loanAmount.Text;
        }   
        public string GetTermsOfLoan()
        {
            _form = Content.FindElement(By.CssSelector(UiMap.Get.ApplyTermsPage.FormId));
            _termsOfLoan = _form.FindElement(By.CssSelector(UiMap.Get.ApplyTermsPage.TermsOfLoan));
            return _termsOfLoan.Text;
        }
    }
}
