using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

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
