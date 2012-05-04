using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class HomePageMobile : BasePage, IApplyPage
    {
        public SlidersElement Sliders { get; set; }

        public TabsElementMobile Tabs { get; set; }

        public HomePageMobile(UiClient client)
            : base(client)
        {
            Sliders = new SlidersElement(this);
            Tabs = new TabsElementMobile(this);
        }
    }
}
