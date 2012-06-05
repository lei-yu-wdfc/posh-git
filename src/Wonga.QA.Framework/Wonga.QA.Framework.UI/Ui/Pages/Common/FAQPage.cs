using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class FAQPage : BasePage
    {
        public FAQElement Faq { get; set; }

        public FAQPage(UiClient client)
            : base(client)
        {
            Faq = new FAQElement(this);
        }

        public bool IsLinkCorrect(String linkSelector, String href)
        {
            var link = Do.Until(()=>Client.Driver.FindElement(By.CssSelector(linkSelector)));
            return href.Equals(link.GetAttribute("href"));
        }
    }
}
