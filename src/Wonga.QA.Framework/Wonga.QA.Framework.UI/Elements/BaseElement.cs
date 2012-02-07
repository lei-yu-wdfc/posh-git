using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Pages;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.UI.Elements
{
    public abstract class BaseElement
    {
        public BasePage Page;
        public IWebElement MenuContent;

        protected BaseElement(BasePage page)
        {
            Page = page;
            MenuContent = Do.Until(() => Page.Client.Driver.FindElement(By.ClassName("menu")));
        }
    }
}
