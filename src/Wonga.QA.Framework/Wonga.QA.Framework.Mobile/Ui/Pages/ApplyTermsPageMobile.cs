using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class ApplyTermsPageMobile : BasePageMobile, IDecisionPage
    {
        private IWebElement _form;
        private readonly IWebElement _next;
        private readonly IWebElement _editLoanDurationField;
        private IWebElement _loanAmount;
        private IWebElement _termsOfLoan;

        public ApplyTermsPageMobile(MobileUiClient client)
            : base(client)
        {
            _next = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyTermsPage.NextButton));
            _editLoanDurationField = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyTermsPage.EditLoanDurationField));
        }

        public void EditDurationOfLoan(String value)
        {
            _editLoanDurationField.SendValue(value);
        }

        public AcceptedPageMobile Next()
        {
            _next.Submit();
            return new AcceptedPageMobile(Client);
        }
        public string GetLoanAmount()
        {
            _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyTermsPage.FormId));
            _loanAmount = _form.FindElement(By.CssSelector(UiMapMobile.Get.ApplyTermsPage.LoanAmount));
            return _loanAmount.Text;
        }   
        public string GetTermsOfLoan()
        {
            _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.ApplyTermsPage.FormId));
            _termsOfLoan = _form.FindElement(By.CssSelector(UiMapMobile.Get.ApplyTermsPage.TermsOfLoan));
            return _termsOfLoan.Text;
        }
    }
}
