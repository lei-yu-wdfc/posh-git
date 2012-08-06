using System;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Framework.UI.Elements
{
    public class SmallTopupSlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _loanAmount;
        private readonly IWebElement _grandTotalAmount;
        private readonly IWebElement _totalFees;
        private readonly IWebElement _subTotalAmount;
        private readonly IWebElement _topUpAmount;
        //private readonly IWebElement _amountMinusButton;
        //private readonly IWebElement _amountPlusButton;

        public SmallTopupSlidersElement(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SmallTopupSlidersElement.FormId));
            _loanAmount = _form.FindElement(By.CssSelector(UiMap.Get.SmallTopupSlidersElement.TopupLoanAmount));
            _grandTotalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SmallTopupSlidersElement.TotalToRepay));
            _totalFees = _form.FindElement(By.CssSelector(UiMap.Get.SmallTopupSlidersElement.TopupFees));
            _subTotalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SmallTopupSlidersElement.TopupToRepay));
            _topUpAmount = _form.FindElement(By.CssSelector(UiMap.Get.SmallTopupSlidersElement.TopupAmount));
            //_loanAmount = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.LoanAmount));
            //_totalAmount = _form.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageNewCreditRequest));
            //_totalFees = _form.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageNewInterestAndFees));
            //_totalToRepay = _form.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageNewGrandTotal));
            
        }

        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set { _loanAmount.SendValue(value); Thread.Sleep(2000); }
        }

        public String GetGrandTotal
        {
            get { return _grandTotalAmount.Text; }
        }
        public String GetTotalFees
        {
            get { return _totalFees.Text; }
        }
        public String GetSubTotal
        {
            get { return _subTotalAmount.Text; }
        }

        public String GetTopUpAmount
        {
            get { return _topUpAmount.Text; }
        }

    }
}
