using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public abstract class BaseSection
    {
        public BasePage Page;
        public IWebElement Section;

        protected BaseSection(String legend, BasePage page)
        {
            Page = page;
            Section = Page.Content.FindElement(By.CssSelector(legend));
            //Section = Page.Content.FindElements(By.TagName("fieldset")).Single(fieldset => fieldset.FindElements(By.TagName("legend")).Any(element => (Regex.Match(element.Text, legend)).Success));
        }
    }
}
