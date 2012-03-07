using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;

namespace Wonga.QA.Framework.UI
{
    public static class Selenium
    {
        public static IWebDriver Driver(this IWebElement element)
        {
            return ((RemoteWebElement)element).WrappedDriver; 
        }

        public static void SelectLabel(this ReadOnlyCollection<IWebElement> elements, String value)
        {
            
            elements.SelectLabel(label => label.Trim() == value);
        }

        public static void SelectLabel(this ReadOnlyCollection<IWebElement> elements, Func<String, Boolean> func)
        {
            List<IWebElement> labels = elements.SelectMany(element => element.Driver().FindElements(By.TagName("label")).Where(label => label.GetAttribute("for") == element.GetAttribute("id"))).ToList();
            labels.First(label => func(label.Text)).Click();
        }

        public static void SelectValue(this ReadOnlyCollection<IWebElement> elements, string value)
        {
            List<IWebElement> labels = elements.Where(element => element.GetValue() == value).ToList();
            labels.ForEach(x => x.Click());
        }

        public static void SelectOption(this IWebElement element, String value)
        {
            element.SelectOption(option => option.Trim() == value);
        }

        public static IWebElement FirstOrDefaultElement(this IWebElement element, By by)
        {
            return element.FindElements(by).FirstOrDefault();
        }

        public static void SelectOption(this IWebElement element, Func<String, Boolean> func)
        {
            ReadOnlyCollection<IWebElement> options = element.FindElements(By.TagName("option"));
            try
            {
                options.Single(option => func(option.Text)).Click();
            }
            catch
            {
                TestLog.DebugTrace.WriteLine(String.Join(", ", options.Select(option => option.Text)));
                throw;
            }
        }

        public static void SendValue(this IWebElement element, String value)
        {
            element.Clear();
            element.SendKeys(value);
        }

        public static String GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static void Toggle(this IWebElement element, Boolean value)
        {
            if (element.Selected != value)
                element.Click();
        }

        //Does not work in Firefox and IE only Chrome atm.
        //public static void MouseOver(this IWebElement element)
        //{
        //    Actions actions = new Actions(element.Driver());
        //    actions.MoveToElement(element);
        //}
    }
}
