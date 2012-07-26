using System;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    public class HelpTest : UiTest
    {

		[Test, AUT(AUT.Za, AUT.Ca, AUT.Wb), JIRA("QA-164, QA-254"), Pending("ZA-2705")] //Category(TestCategories.SmokeTest)
        public void SelectingAHelpQuestionTakesMeToFAQPageWithCorrectQuestionSelected()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            List<String> listQuestions = page.Help.GetListQuestions();
            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Za:
                    foreach (string question in listQuestions)
                    {
                        page.Help.HelpTriggerClick();
                        FAQPage faqPage = page.Help.SelectQuestion(question);
                        Assert.AreEqual(question, faqPage.Faq.GetSelectedQuestionText());
                        Assert.AreEqual(question, faqPage.Faq.GetAnswerQuestionText());
                        page = Client.Home();
                    }
                    break;
                case AUT.Wb:
                    var listQuestionsWb = new List<string> { "How does Wonga Business work?", "What do I need to apply for a Wonga Business loan?", "How much cash can my business borrow?", "How much does a Wonga loan cost?", "Does Wonga require a credit check?", "What if the business or I have bad credit?", "What personal information does Wonga need?", "Can my business get a loan if it has no assets?", "Does my business need a bank account?", "Do I need a bank account?", "Do I need to give a reason why the business wants to borrow the money on the application?", "Is Wonga a member of any financial bodies?", "Is Wonga right for my business?", "What if the business is already in debt?", "How long will it take for the business to get the loan?", "How long before I have to repay a loan in full?", "How do I repay a loan?", "What happens if the business does not repay the loan?", "How do bank holidays and weekends affect the service?", "What if I have a complaint?", "The business still has not received the cash", "I can’t login", "I can’t remember my password", "I haven’t received my email during application", "I haven't received my PIN via text message", "I can’t find my house or business address", "My business loan application has been ‘referred’ – what’s happening?", "My business has had loans before but I’ve just been rejected" };
                    Assert.AreEqual(listQuestionsWb.Count, listQuestions.Count);
                    listQuestions.Sort();
                    listQuestionsWb.Sort();
                    for (int i = 0; i < listQuestions.Count; i++)
                    {
                        Console.WriteLine(listQuestionsWb[i] + "\n" + listQuestions[i] + "\n");
                        Assert.AreEqual(listQuestionsWb[i], listQuestions[i]);
                    }
                    break;
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-165"), Category(TestCategories.SmokeTest)]
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

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-166"), Pending("ZA-1065")] //SmokeTest
        public void JargonBusterLinkShouldNavigateThroughPageByClickingDifferentLettersFromAlphabet()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            var jargonBasterPage = page.Help.JargonBusterClick();
            var alphabetLinks = jargonBasterPage.GetAlphabetLinks();
            foreach (var element in alphabetLinks)
            {
                element.Click();
                Assert.IsTrue(jargonBasterPage.Url.Contains("#" + element.Text.ToLower()));
            }
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-167"), Category(TestCategories.SmokeTest)]
        public void ClickOnContactUsCauseContactInformationDisplayedOnPage()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            Thread.Sleep(1000); //wait for ajax load help submenu
            page.Help.ContactUsClick();
            Thread.Sleep(5000); //wait for ajax load the popup
            Assert.IsTrue(page.Contact.IsContactPopupPresent());

        }

        [Test, AUT(AUT.Za), Pending("ZA-1600, ZA-2474")] //ZA-1600 bug check
        public void CouncilForDebtCollectorsLinkCheck()
        {
            var faq = Client.Faq();
            Assert.IsTrue(faq.IsLinkCorrect(UiMap.Get.FAQPage.CouncilForDebtCollectorsLink, ContentMap.Get.FAQPageLinks.CouncilForDebtCollectorsLink));
        }

        [Test, AUT(AUT.Wb), JIRA("QA-291")]
        public void ContactPageMustContainOnlyDetailsForWongaBuisness()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            page.Help.ContactUsClick();
            List<string> wbEmails = page.Contact.GetLinksTextFromPopup();
            foreach (string email in wbEmails)
            {
                Assert.IsTrue(email.Contains("@wongabusiness.com"));
            }
        }
    }
}
