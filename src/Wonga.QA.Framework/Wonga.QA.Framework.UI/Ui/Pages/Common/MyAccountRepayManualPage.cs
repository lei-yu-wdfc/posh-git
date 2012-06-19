using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MyAccountRepayManualPage : BasePage
    {

        private readonly IWebElement _repaymentDate;
        private readonly IWebElement _amountDueOnDueDate;
        private readonly IWebElement _repaymentAmount;
        private readonly IWebElement _remainderToRepay;
        private readonly IWebElement _bankAccountMasked;

        public MyAccountRepayManualPage(UiClient client)
            : base(client)
        {
            _repaymentDate = Content.FindElement(By.CssSelector(UiMap.Get.MyAccountRepayManualPage.RepaymentDate));
            _amountDueOnDueDate =
                Content.FindElement(By.CssSelector(UiMap.Get.MyAccountRepayManualPage.AmountDueOnDueDate));
            _repaymentAmount = Content.FindElement(By.CssSelector(UiMap.Get.MyAccountRepayManualPage.RepaymentAmount));
            _remainderToRepay = Content.FindElement(By.CssSelector(UiMap.Get.MyAccountRepayManualPage.RemainderToRepay));
            _bankAccountMasked =
                Content.FindElement(By.CssSelector(UiMap.Get.MyAccountRepayManualPage.BankAccountMasked));
        }
    }
}

