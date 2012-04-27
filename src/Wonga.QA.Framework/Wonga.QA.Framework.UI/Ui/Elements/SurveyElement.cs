using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class SurveyElement : BaseElement
    {
        private IWebElement _surveyBox;

        public SurveyElement(BasePage page) : base(page)
        {
           
        }

        public bool IsVisible
        {
            get { return  Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SurveyElement.SurveyBox)).Displayed; }
        }
    }

}
