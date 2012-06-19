using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.UI.Elements;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
{
    public class FAQElement : BaseElement
    {
        private readonly IWebElement _selectQuestion;
        private readonly IWebElement _answerQuestion;

        public FAQElement(FAQPage page)
            : base(page)
        {
            _selectQuestion = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.FAQElement.SelectQuestion));
            _answerQuestion = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.FAQElement.AnswerQuestion));
        }

        public string GetSelectedQuestionText()
        {
            return _selectQuestion.Text.Trim(' ');
        }

        public string GetAnswerQuestionText()
        {
            return _answerQuestion.Text.Trim(' ');
        }
    }
}
