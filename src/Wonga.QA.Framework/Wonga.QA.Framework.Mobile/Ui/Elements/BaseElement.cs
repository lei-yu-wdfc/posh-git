using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
{
    public abstract class BaseElement
    {
        public BasePageMobile Page;
        public IWebElement MenuContent;

        protected BaseElement(BasePageMobile page)
        {
            Page = page;
            if (!Config.Ui.Browser.Equals(Config.UiConfig.BrowserType.FirefoxMobile))
            {
                MenuContent = Do.Until(() => Page.Client.Driver.FindElement(By.ClassName("menu")));
            }
        }
    }
}
