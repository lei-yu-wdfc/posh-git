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
        private readonly IWebElement _editLoan;

        private IWebElement _editLoanDuration;
        
        
        public ApplyTermsPage(UiClient client) : base(client)
        {
            _next = Content.FindElement(By.CssSelector(Ui.Get.ApplyTermsPage.NextButton));
            _editLoan = Content.FindElement(By.CssSelector(Ui.Get.ApplyTermsPage.EditLoan));
        }

        public void EditDurationOfLoan(String value)
        {
            _editLoan.Click();
            _editLoanDuration = Content.FindElement(By.CssSelector(Ui.Get.ApplyTermsPage.EditLoanDuration));
            _editLoanDuration.SendValue(value);
        }

        public AcceptedPage Next()
        {
            _next.Click();
            return new AcceptedPage(Client);
        }
    }
}
