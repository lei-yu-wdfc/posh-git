using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.UI.Elements;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.UI.Elements
{
    public class HelpElement : BaseElement
    {
        private readonly IWebElement _helpTrigger;
        private readonly IWebElement _listQuestions;
        private readonly IWebElement _troubleshooting;
        private readonly IWebElement _jargonBuster;
        private IWebElement _contactUs;

        public HelpElement(BasePageMobile page)
            : base(page)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                case AUT.Pl:
                    _troubleshooting =
                    Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.HelpElement.TroubleshootingQuestions));
                    _jargonBuster = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.HelpElement.JargonBuster));
                    break;
            }
            _helpTrigger = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.HelpElement.HelpTrigger));
            _listQuestions = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.HelpElement.HelplistQuestions));

        }

        public void HelpTriggerClick()
        {
            _helpTrigger.Click();
        }

        public List<string> GetListQuestions()
        {
            List<string> questions = _listQuestions.FindElements(By.TagName("option")).Select(option => option.Text.Trim(' ')).ToList();
            questions.Remove(ContentMapMobile.Get.HelpElement.SelectQuestion);
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
            questions.Remove(ContentMapMobile.Get.HelpElement.SelectQuestion);
            return questions;
        }
        public FAQPage SelectTroubleshootingQuestion(string question)
        {
            _troubleshooting.SelectOption(question);
            return new FAQPage(Page.Client);
        }

        //public JargonBusterPage JargonBusterClick()
        //{
        //    _jargonBuster.Click();
        //    return new JargonBusterPage(Page.Client);
        //}

        public void ContactUsClick()
        {
            _contactUs = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.HelpElement.ContactUs));
            _contactUs.Click();
        }
    }
}
