using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public abstract class BaseElement
    {
        public BasePage Page;
        public IWebElement MenuContent;

        protected BaseElement(BasePage page)
        {
            Page = page;
            if (!Config.Ui.Browser.Equals(Config.UiConfig.BrowserType.FirefoxMobile))
            {
                MenuContent = Do.Until(() => Page.Client.Driver.FindElement(By.ClassName("menu")));
            }
        }
    }
}
