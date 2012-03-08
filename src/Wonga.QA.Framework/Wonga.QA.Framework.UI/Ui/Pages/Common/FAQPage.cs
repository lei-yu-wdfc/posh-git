using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
