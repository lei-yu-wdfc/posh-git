using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
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


        public TopupSlidersElement(BasePageMobile page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.FormId));
            _loanAmount = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.LoanAmount));
            _amountMinusButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.AmountMinusButton));
            _amountPlusButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.AmountPlusButton));
            _totalAmount = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.TotalAmount));
            _totalFees = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.TotalFees));
            _totalToRepay = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.TotalToRepay));
        }

        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set
            {
                _loanAmount.EraseAll();
                _loanAmount.SendValue(value);
                _loanAmount.LostFocus();
            }
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

        public TopupRequestPage Apply()
        {
            _submit = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.SubmitButton));
            _submit.Click();
            return new TopupRequestPage(Page.Client);
        }
        public bool IsSubmitButtonPresent()
        {
            try
            {
                _submit = _form.FindElement(By.CssSelector(UiMapMobile.Get.TopupSlidersElement.SubmitButton));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
