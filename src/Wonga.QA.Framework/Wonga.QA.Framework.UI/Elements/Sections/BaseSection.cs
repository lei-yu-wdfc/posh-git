using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Pages;

namespace Wonga.QA.Framework.UI.Elements.Sections
{
    public abstract class BaseSection
    {
        public BasePage Page;
        public IWebElement Section;

        protected BaseSection(String legend, BasePage page)
        {
            Page = page;
            Section = Page.Content.FindElements(By.TagName("fieldset")).Single(fieldset => fieldset.FindElements(By.TagName("legend")).Any(element => (Regex.Match(element.Text, legend)).Success));
        }
    }
}
