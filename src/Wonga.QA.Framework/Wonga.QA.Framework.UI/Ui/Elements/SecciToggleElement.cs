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
    public class SecciToggleElement : BaseElement
    {
        private readonly IWebElement _toggleLink;
        /*private readonly IWebElement _loanAmount;
        private readonly IWebElement _grandTotalAmount;
        private readonly IWebElement _totalFees;
        private readonly IWebElement _subTotalAmount;
         * */
        //private readonly IWebElement _amountMinusButton;
        //private readonly IWebElement _amountPlusButton;

        public SecciToggleElement(BasePage page)
            : base(page)
        {
            _toggleLink = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SecciToggleElement.ToggleLink));
            /*_loanAmount = _form.FindElement(By.CssSelector(UiMap.Get.SecciToggleElement.TopupLoanAmount));
            _grandTotalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SecciToggleElement.TotalToRepay));
            _totalFees = _form.FindElement(By.CssSelector(UiMap.Get.SecciToggleElement.TopupFees));
            _subTotalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SecciToggleElement.TopupToRepay));
             * */
        }

        public void MyPaymentDetailsButtonClick()
        {
            _toggleLink.Click();
            //return new MyPaymentsPage(Page.Client)
        }
    }
}