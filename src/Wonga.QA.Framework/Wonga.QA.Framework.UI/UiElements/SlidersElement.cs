using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements
{
    public class SlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _loanAmount;
        private readonly IWebElement _loanDuration;
        private readonly IWebElement _submit;

        public SlidersElement(BasePage page) : base(page)
        {
            _form = Page.Content.FindElement(By.Id(Elements.Get.SliderElement.FormId));
            _loanAmount = _form.FindElement(By.Name(Elements.Get.SliderElement.LoanAmount));
            _loanDuration = _form.FindElement(By.Name(Elements.Get.SliderElement.LoanDuration));
            _submit = _form.FindElement(By.Name(Elements.Get.SliderElement.SubmitButton));
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
