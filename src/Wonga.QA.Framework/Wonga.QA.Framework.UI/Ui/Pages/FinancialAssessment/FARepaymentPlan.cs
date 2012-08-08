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
        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FARepaymentPlanPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentRepaymentPlanPage.ButtonNext));
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FADebtsPage(Client);
        }
    }
}
