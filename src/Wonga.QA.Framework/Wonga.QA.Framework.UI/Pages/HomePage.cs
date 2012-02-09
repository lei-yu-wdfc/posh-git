using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;

namespace Wonga.QA.Framework.UI.Pages
{
    public class HomePage : BasePage
    {
        public IWebElement MenuContent;
        public SlidersElement Sliders { get; set; }
        //public TabsElement Tabs { get; set; }

        public HomePage(UiClient client)
            : base(client)
        {
            Sliders = new SlidersElement(this);
            //Tabs = new TabsElement(this);
        }
    }
}
