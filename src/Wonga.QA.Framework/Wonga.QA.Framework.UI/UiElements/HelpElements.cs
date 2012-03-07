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



namespace Wonga.QA.Framework.UI.UiElements
{
    public class HelpElements : BaseElement
    {
        private readonly IWebElement _helpTrigger;
        private readonly IWebElement _listQuestions;


        public HelpElements(BasePage page)
            : base(page)
        {
            _helpTrigger = Page.Client.Driver.FindElement(By.Id(Elements.Get.HelpSection.HelpTrigger));
            _listQuestions = Page.Client.Driver.FindElement(By.CssSelector(Elements.Get.HelpSection.HelplistQuestions));
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
    }
}