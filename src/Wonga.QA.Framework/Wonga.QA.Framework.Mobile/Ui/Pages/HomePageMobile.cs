using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Mobile.UI.Elements;
using Wonga.QA.Framework.Mobile.Ui.Elements;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class HomePageMobile : BasePageMobile, IApplyPage
    {
        public SlidersElement Sliders { get; set; }

        public TabsElementMobile Tabs { get; set; }

        public HomePageMobile(MobileUiClient client)
            : base(client)
        {
            Sliders = new SlidersElement(this);
            Tabs = new TabsElementMobile(this);
        }
    }
}
