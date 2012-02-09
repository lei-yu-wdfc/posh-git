using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Pages;
using Wonga.QA.Framework.UI.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.Elements
{
    public class SlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _loanAmount;
        private readonly IWebElement _loanDuration;
        private readonly IWebElement _submit;
        //private IWebElement _loanSummary;

        public SlidersElement(BasePage page)
            : base(page)
        {
            _form = Page.Content.FindElement(By.Id("wonga-sliders-form"));
            _loanAmount = _form.FindElement(By.Name("loan_amount"));
            _loanDuration = _form.FindElement(By.Name("loan_duration"));
            _submit = _form.FindElement(By.Name("op"));
            //_loanSummary = _form.FindElement(By.Id("wonga-slider-loan-summary"));
        }

        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set { _loanAmount.SendValue(value); }
        }
        public String HowLong
        {
            get { return _loanDuration.GetValue(); }
            set { _loanDuration.SendValue(value); }
        }

        public IApplyPage Apply()
        {
            _submit.Click();
            if (Config.AUT == AUT.Wb)
                return new Pages.Wb.EligibilityQuestionsPage(Page.Client);
            return null;
        }
    }
}
