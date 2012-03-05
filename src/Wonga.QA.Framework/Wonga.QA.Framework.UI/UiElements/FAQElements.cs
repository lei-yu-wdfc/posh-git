using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements
{
    public class FAQElements : BaseElement
    {
        private readonly IWebElement _selectQuestion;
        private readonly IWebElement _answerQuestion;

        public FAQElements(FAQPage page)
            : base(page)
        {
            _selectQuestion = Page.Client.Driver.FindElement(By.XPath(Elements.Get.FaqSection.SelectQuestion));
            _answerQuestion = Page.Client.Driver.FindElement(By.XPath(Elements.Get.FaqSection.AnswerQuestion));
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


