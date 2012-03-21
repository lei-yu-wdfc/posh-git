using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class SurveyElement : BaseElement
    {
        private IWebElement _surveyBox;

        public SurveyElement(BasePage page) : base(page)
        {
            _surveyBox = page.Client.Driver.FindElement(By.XPath("//div[@id='wonga_survey_box']"));
        }

        public bool IsVisible
        {
            get { return _surveyBox.Displayed; }
        }
    }

}
