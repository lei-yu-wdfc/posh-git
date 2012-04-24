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
    public class SmallExtensionSlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _extendDuration;
        private readonly IWebElement _extendDate;
        private readonly IWebElement _grandTotalAmount;
        private readonly IWebElement _newFees;

        public SmallExtensionSlidersElement(BasePage page) : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.SmallExtensionSlidersElement.FormId));
            _extendDuration = _form.FindElement(By.CssSelector(Ui.Get.SmallExtensionSlidersElement.ExtensionLoanDuration));
            _extendDate = _form.FindElement(By.CssSelector(Ui.Get.SmallExtensionSlidersElement.RepaymentDate));
            _grandTotalAmount = _form.FindElement(By.CssSelector(Ui.Get.SmallExtensionSlidersElement.TotalToRepay));
            _newFees = _form.FindElement(By.CssSelector(Ui.Get.SmallExtensionSlidersElement.ExtensionInterestFees));
        }

        public String HowLong
        {
            get { return _extendDuration.GetValue(); }
            set { _extendDuration.SendValue(value); Thread.Sleep(2000); }
        }

        public String UntilWhen
        {
            get { return _extendDate.Text; }
        }
        public String GetGrandTotal
        {
            get { return _grandTotalAmount.Text; }
        }
        public String GetNewFees
        {
            get { return _newFees.Text; }
        }
    }
}
