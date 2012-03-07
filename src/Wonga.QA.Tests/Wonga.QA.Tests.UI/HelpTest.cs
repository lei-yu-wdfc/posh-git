﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Tests.Ui
{
    public class HelpTest : UiTest
    {

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-164")]
        public void SelectingAHelpQuestionTakesMeToFAQPageWithCorrectQuestionSelected()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            List<String> listQuestions = page.Help.GetListQuestions();
            foreach (string question in listQuestions)
            {
                page.Help.HelpTriggerClick();
                FAQPage faqPage = page.Help.SelectQuestion(question);
                Assert.AreEqual(question, faqPage.Faq.GetSelectedQuestionText());
                Assert.AreEqual(question, faqPage.Faq.GetAnswerQuestionText());
                page = Client.Home();

            }
        }
    }
}