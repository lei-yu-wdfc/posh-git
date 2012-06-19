using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Elements;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class FAQPage : BasePageMobile
    {
        public FAQElement Faq { get; set; }

        public FAQPage(MobileUiClient client)
            : base(client)
        {
            Faq = new FAQElement(this);
        }

        public bool IsLinkCorrect(String linkSelector, String href)
        {
            var link = Do.Until(() => Client.Driver.FindElement(By.CssSelector(linkSelector)));
            return href.Equals(link.GetAttribute("href"));
        }
    }
}
