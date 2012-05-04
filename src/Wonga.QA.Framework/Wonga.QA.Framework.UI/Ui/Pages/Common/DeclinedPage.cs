using System;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class DeclinedPage : BasePage,IDecisionPage
    {
        public TabsElement TabsElement { get; set; }
        public LoginElement LoginElement { get; set; }
        public HelpElement HelpElement { get; set; }
        public InternationalElement InternationalElement { get; set; }

        public DeclinedPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(UiMap.Get.DeclinedPage.HeaderText));
        }

        public bool LookForHeaderLinks()
        {
            return Do.Until(IsHeaderLinksExist);
        }

        private bool IsHeaderLinksExist()
        {
            try
            {
                TabsElement = new TabsElement(this);
                LoginElement = new LoginElement(this);
                HelpElement = new HelpElement(this);
                InternationalElement = new InternationalElement(this);
            }
            catch(NoSuchElementException)
            {
                return false;
            }
            catch(Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}
