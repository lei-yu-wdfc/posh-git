using System;
using System.Threading;
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
        //private readonly IWebElement _amountMinusButton;
        //private readonly IWebElement _amountPlusButton;

        public SmallTopupSlidersElement(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.SmallTopupSlidersElement.FormId));
            _loanAmount = _form.FindElement(By.CssSelector(Ui.Get.SmallTopupSlidersElement.TopupLoanAmount));
            _grandTotalAmount = _form.FindElement(By.CssSelector(Ui.Get.SmallTopupSlidersElement.TotalToRepay));
            _totalFees = _form.FindElement(By.CssSelector(Ui.Get.SmallTopupSlidersElement.TopupFees));
            _subTotalAmount = _form.FindElement(By.CssSelector(Ui.Get.SmallTopupSlidersElement.TopupToRepay));
            //_loanAmount = _form.FindElement(By.CssSelector(Ui.Get.SlidersElement.LoanAmount));
            //_totalAmount = _form.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewCreditRequest));
            //_totalFees = _form.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewInterestAndFees));
            //_totalToRepay = _form.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewGrandTotal));
            
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
    }
}
