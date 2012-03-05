using OpenQA.Selenium;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class HomePage : BasePage
    {
        //public IWebElement MenuContent;
        public SlidersElement Sliders { get; set; }
        public HelpElements Help { get; set; }
        public InternationalElements InternationalElements { get; set; }
        //public TabsElement Tabs { get; set; }

        public HomePage(UiClient client)
            : base(client)
        {
            Sliders = new SlidersElement(this);
            switch (Config.AUT)
            {
                case(AUT.Ca):
                case (AUT.Za):
                    Help = new HelpElements(this);
                    InternationalElements = new InternationalElements(this);
                break;
            }
            //Tabs = new TabsElement(this);
            
        }
    }
}
