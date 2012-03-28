using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-164"), Pending("FE bug")]
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

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-165"), Pending("FE bug, button in top of page are broken")]
        public void SelectingATroubleshootingQuestionTakesMeToPageWithCorrectQuestionSelected()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            List<String> troubleshootingQuestions = page.Help.GetTroubleshootingQuestions();
            foreach (string question in troubleshootingQuestions)
            {
                page.Help.HelpTriggerClick();
                FAQPage faqPage = page.Help.SelectTroubleshootingQuestion(question);
                Assert.AreEqual(question, faqPage.Faq.GetSelectedQuestionText());
                Assert.AreEqual(question, faqPage.Faq.GetAnswerQuestionText());
                page = Client.Home();

            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-166"), Pending("Bug on JargonBuster page. It has the same href for U and V.")]
        public void JargonBusterLinkShouldNavigateThroughPageByClickingDifferentLettersFromAlphabet()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            var jargonBasterPage =  page.Help.JargonBusterClick();
            var alphabetLinks =  jargonBasterPage.GetAlphabetLinks();
            foreach (var element in alphabetLinks)
            {
                    element.Click();
                    Assert.IsTrue(jargonBasterPage.Url.Contains("#"+element.Text.ToLower()));
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-167"), Pending("FE bug, button in top of page are broken")]
        public void ClickOnContactUsCauseContactInformationDisplayedOnPage()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            page.Help.ContactUsClick();
            Thread.Sleep(5000); //wait for ajax load the popup
            Assert.IsTrue(page.Contact.IsContactPopupPresent()); 
            
        }
    }
}
