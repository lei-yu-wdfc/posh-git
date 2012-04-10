using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Framework.UI.Elements
{
    public class SmallTopupSliders : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _loanAmount;
        private readonly IWebElement _totalAmount;
        private readonly IWebElement _totalFees;
        private readonly IWebElement _totalToRepay;
        private readonly IWebElement _amountMinusButton;
        private readonly IWebElement _amountPlusButton;

        public SmallTopupSliders(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.SlidersElement.FormId));
            _loanAmount = _form.FindElement(By.CssSelector(Ui.Get.SlidersElement.LoanAmount));
            _totalAmount = _form.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewCreditRequest));
            _totalFees = _form.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewInterestAndFees));
            _totalToRepay = _form.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewGrandTotal));

        }

        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set { _loanAmount.SendValue(value); Thread.Sleep(2000); }
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
    }
}
