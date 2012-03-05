using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class FAQPage : BasePage
    {
        public FAQElements Faq { get; set; }

        public FAQPage(UiClient client)
            : base(client)
        {
            Faq = new FAQElements(this);

        }
    }
}
