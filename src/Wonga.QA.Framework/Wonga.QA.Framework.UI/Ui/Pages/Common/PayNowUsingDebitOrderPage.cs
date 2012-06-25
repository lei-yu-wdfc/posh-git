using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Ui.Pages.Common
{
    public class PayNowUsingDebitOrderPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _editRepaymentDate;
        private readonly IWebElement _amountOnDueDate;
        private readonly IWebElement _editRepaymentAmount;
        private readonly IWebElement _remainderToPay;
        private readonly IWebElement _editBankAccountMasked;
        private readonly IWebElement _captcha;
        private readonly IWebElement _editCaptchaField;
        private readonly IWebElement _cancelButton;
        private readonly IWebElement _submit;
        
        public PayNowUsingDebitOrderPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.Form));
            _editRepaymentDate = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.EditRepaymentAmount));
            _amountOnDueDate = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.AmountOnDueDate));
            _editRepaymentAmount = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.EditRepaymentAmount));
            _remainderToPay = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.RemainderToPay));
            _editBankAccountMasked = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.EditBankAccountMasked));
            _captcha = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.Captcha));
            _editCaptchaField = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.EditCaptchaField));
            _cancelButton = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.CancelButton));
            _submit = _form.FindElement(By.CssSelector(UiMap.Get.PayNowUsingDebitOrderPage.Submit));
        }

    }
}
