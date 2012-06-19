using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using Wonga.QA.Framework.UI.UiElements.Pages;

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

         public static Boolean VerifyTextEntering(this IWebElement element, string text)
 	 	
        {
           element.SendValue(text);
           return element.GetValue().Equals(text);
        }
 	 	

        public static void SelectLabel(this ReadOnlyCollection<IWebElement> elements, Func<String, Boolean> func)
        {
            List<IWebElement> labels = elements.SelectMany(element => element.Driver().FindElements(By.TagName("label")).Where(label => label.GetAttribute("for") == element.GetAttribute("id"))).ToList();
            labels.First(label => func(label.Text)).Click();
        }

         public static Boolean CanSelectLabel(ReadOnlyCollection<IWebElement> elements, string label)
        {
            try
            {
                elements.SelectLabel(label);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

         public static Boolean CanSelectOption(this IWebElement element, string text)
        {
             try
            {
                element.SelectOption(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
         public static Boolean CanSendValue(this IWebElement element, string text) 	 	
        {
            try
            {
                element.SendValue(text);
                return true;
            }
            catch(Exception)
           {
                return false;
           }
        }

        public static Boolean CanSendValue(this IWebElement element)
 	 	
        {
            try
            {
                element.SendValue("abc123");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
         public static Boolean CanToggle(this IWebElement element)
        {
             try
            {
                element.Toggle(true);
                return true;
            }
 	 	    catch (Exception)
            {
                return false;
            }
 	 	
        }
        
        public static bool DragAndDropToOffset(this IWebElement element, int xOffset, int yOffset)
        {
            Actions actions = new Actions(element.Driver());
            actions.DragAndDropToOffset(element, xOffset, yOffset).Perform();
            return true;
        }


        public static void EraseAll(this IWebElement element)
        {
            element.Click();
            element.SendKeys(Keys.End);
            int numberOfCharsToErase = element.GetValue().Length;

            do
            {
                element.SendKeys(Keys.Backspace);
                numberOfCharsToErase--;
            } while (numberOfCharsToErase != 0);
        }

        public static void EraseFromEnd(this IWebElement element, int numberOfCharsToErase)
        {
            element.Click();
            element.SendKeys(Keys.End);
            for(int i =0; i<numberOfCharsToErase;i++)
            {
                element.SendKeys(Keys.Backspace);
            }
        }
        public static void EraseFromStart(this IWebElement element, int numberOfCharsToErase)
        {
            element.Click();
            for (int i = 0; i < numberOfCharsToErase; i++)
            {
                element.SendKeys(Keys.Delete);
            }
        }

        public static void LostFocus(this IWebElement element)
        {
            element.SendKeys(Keys.Tab);
        }


        //Does not work in Firefox and IE only Chrome atm.
        //public static void MouseOver(this IWebElement element)
        //{
        //    Actions actions = new Actions(element.Driver());
        //    actions.MoveToElement(element);
        //}

        public static IWebElement FindEitherElement(this IWebElement parent, params By[] selectors)
        {
            return selectors.SelectMany(parent.FindElements).Single();
        }

        public static Boolean IsChoiseItems (BasePage page, string selector, string label)
       {
           ReadOnlyCollection<IWebElement> elements = page.Client.Driver.FindElements(By.CssSelector(selector));
 	 	   return CanSelectLabel(elements, label);
       }
        public static Boolean IsDropdownList (BasePage page, string selector, string text)
        {
            IWebElement element = page.Client.Driver.FindElement(By.CssSelector(selector));
            return !CanSendValue(element, text) && CanSelectOption(element,text);
        }

        public static Boolean IsTextBox(BasePage page, string selector, string text)
        {
            IWebElement element = page.Client.Driver.FindElement(By.CssSelector(selector));
            return CanSendValue(element, text) && !CanSelectOption(element, text);
        }

        public static Boolean IsTextBox(BasePage page, string selector)
        {
            IWebElement element = page.Client.Driver.FindElement(By.CssSelector(selector));
            return CanSendValue(element, "aaa") && !CanSelectOption(element, "aaa");
        }

        public static Boolean IsCheckBox(BasePage page, string selector)
        {
            IWebElement element = page.Client.Driver.FindElement(By.CssSelector(selector));
            return element.CanToggle();
        }
    }
}
