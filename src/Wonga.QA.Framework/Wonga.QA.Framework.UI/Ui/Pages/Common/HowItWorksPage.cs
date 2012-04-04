using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Ui.Pages.Common
{
    class HowItWorksPage : BasePage
    {
        public SlidersElement Sliders { get; set; }

        public HowItWorksPage(UiClient client)
            : base(client)
        {
            Sliders = new SlidersElement(this);
        }
    }
}
