﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;



namespace Wonga.QA.Framework.UI.Elements
{
    public class HelpElement : BaseElement
    {
        private readonly IWebElement _helpTrigger;
        private readonly IWebElement _listQuestions;
        private readonly IWebElement _troubleshooting;


        public HelpElement(BasePage page)
            : base(page)
        {
            _helpTrigger = Page.Client.Driver.FindElement(By.Id(Ui.Get.HelpElement.HelpTrigger));
            _listQuestions = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.HelpElement.HelplistQuestions));
            _troubleshooting =
                Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.HelpElement.TroubleshootingQuestions));
        }

        public void HelpTriggerClick()
        {
            _helpTrigger.Click();
        }

        public List<string> GetListQuestions()
        {
            List<string> questions = _listQuestions.FindElements(By.TagName("option")).Select(option => option.Text.Trim(' ')).ToList();
            questions.Remove("Please select a question...");
            return questions;
        }
        public FAQPage SelectQuestion(string question)
        {
            _listQuestions.SelectOption(question);
            return new FAQPage(Page.Client);
        }

        public List<string> GetTroubleshootingQuestions()
        {
            List<string> questions = _troubleshooting.FindElements(By.TagName("option")).Select(option => option.Text.Trim(' ')).ToList();
            questions.Remove("Please select a question...");
            return questions;
        }
        public FAQPage SelectTroubleshootingQuestion(string question)
        {
            _troubleshooting.SelectOption(question);
            return new FAQPage(Page.Client);
        }
    }
}
