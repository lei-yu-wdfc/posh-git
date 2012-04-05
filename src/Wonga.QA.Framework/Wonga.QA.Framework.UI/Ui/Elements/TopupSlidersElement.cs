using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Framework.UI.Elements
{
    public class TopupSlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _loanAmount;
        private IWebElement _submit;
        private readonly IWebElement _totalAmount;
        private readonly IWebElement _totalFees;
        private readonly IWebElement _totalToRepay;
        private readonly IWebElement _amountMinusButton;
        private readonly IWebElement _amountPlusButton;


        public TopupSlidersElement(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.FormId));
            _loanAmount = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.LoanAmount));
            _amountMinusButton = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.AmountMinusButton));
            _amountPlusButton = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.AmountPlusButton));
            _totalAmount = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.TotalAmount));
            _totalFees = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.TotalFees));
            _totalToRepay = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.TotalToRepay));
        }

        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set { _loanAmount.SendValue(value); }
        }

        public String GetTotalAmount
        {
            get { return _totalAmount.Text; }
        }
        public String GetTotalFees
        {
            get { return _totalFees.Text; }
        }
        public String GetTotalToRepay
        {
            get { return _totalToRepay.Text; }
        }

        public void ClickAmountMinusButton()
        {
            _amountMinusButton.Click();
        }
        public void ClickAmountPlusButton()
        {
            _amountPlusButton.Click();
        }


        public TopupAgreementPage Apply()
        {
            _submit = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.SubmitButton));
            _submit.Click();

            //return null;
            return new TopupAgreementPage(Page.Client);
        }
        public bool IsSubmitButtonPresent()
        {
            try
            {
                _submit = _form.FindElement(By.CssSelector(Ui.Get.TopupSlidersElement.SubmitButton));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
