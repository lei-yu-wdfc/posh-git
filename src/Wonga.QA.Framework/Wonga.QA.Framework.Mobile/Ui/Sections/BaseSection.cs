using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
{
    public abstract class BaseSection
    {
        public BasePageMobile Page;
        public IWebElement Section;

        protected BaseSection(String selector, BasePageMobile page)
        {
            Page = page;
            Section = Page.Content.FindElement(By.CssSelector(selector));
            //Section = Page.Content.FindElements(By.TagName("fieldset")).Single(fieldset => fieldset.FindElements(By.TagName("legend")).Any(element => (Regex.Match(element.Text, legend)).Success));
        }
    }
}
