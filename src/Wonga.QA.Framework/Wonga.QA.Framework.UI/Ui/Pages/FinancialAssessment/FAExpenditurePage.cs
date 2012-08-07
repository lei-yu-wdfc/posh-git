using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FAExpenditurePage : BasePage
    {
        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FAExpenditurePage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentExpenditurePage.ButtonNext));
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FAIncomePage(Client);
        }

        public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FAExpenditurePage(Client, validator);
            }
            return new FADebtsPage(Client);
        }
    }
}
