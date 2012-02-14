using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class HomePage : BasePage
    {
        //public IWebElement MenuContent;
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
