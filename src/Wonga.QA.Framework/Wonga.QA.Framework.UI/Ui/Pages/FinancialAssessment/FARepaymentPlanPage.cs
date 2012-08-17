using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FARepaymentPlanPage : BasePage
    {
        private IWebElement _firstRepaymentDate;
        private IWebElement _paymentFrequency;
        private IWebElement _repaymentAmount;

        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FARepaymentPlanPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _firstRepaymentDate = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.FirstRepaymentDate));
            _paymentFrequency = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.PaymentFrequency));
            _repaymentAmount = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.RepaymentAmount));
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.ButtonNext));
        }

        public string FirstRepaymentDate
        {
            get { return _firstRepaymentDate.GetValue(); }
            set { _firstRepaymentDate.SendValue(value); }
        }

        public string PaymentFrequency
        {
            get { return _paymentFrequency.GetValue(); }
            set { _paymentFrequency.SendKeys(value); }
        }

        public string RepaymentAmount
        {
            get { return _repaymentAmount.GetValue(); }
            set { _repaymentAmount.SendValue(value); }
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FADebtsPage(Client);
        }

        public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FARepaymentPlanPage(Client, validator);
            }
            return new FAWaitPage(Client);
        }
    }
}
