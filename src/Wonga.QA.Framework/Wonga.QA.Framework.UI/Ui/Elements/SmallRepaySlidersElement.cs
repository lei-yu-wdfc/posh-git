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
    public class SmallRepaySlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _repayAmount;
        private readonly IWebElement _remainderAmount;
        //private readonly IWebElement _amountMinusButton;
        //private readonly IWebElement _amountPlusButton;

        public SmallRepaySlidersElement(BasePage page) : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SmallRepaySlidersElement.FormId));
            _repayAmount = _form.FindElement(By.CssSelector(UiMap.Get.SmallRepaySlidersElement.RepayAmount));
            _remainderAmount = _form.FindElement(By.CssSelector(UiMap.Get.SmallRepaySlidersElement.RemainderAmount));
        }

        public String HowMuch
        {
            get { return _repayAmount.GetValue(); }
            set { _repayAmount.SendValue(value); Thread.Sleep(2000); }
        }

        public String GetRemainderTotal
        {
            get { return _remainderAmount.Text; }
        }
    }
}
