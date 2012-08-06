using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Ui.Validators;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FADebtsPage : BasePage
    {
         private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;

        public FADebtsPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _buttonPrevious = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentDebtsPage.ButtonNext));
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FAExpenditurePage(Client);
        }

    /*    public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FADebtsPage(Client, validator);
            }
            return new FAAceptedTest(Client);
        }*/
    }
}
