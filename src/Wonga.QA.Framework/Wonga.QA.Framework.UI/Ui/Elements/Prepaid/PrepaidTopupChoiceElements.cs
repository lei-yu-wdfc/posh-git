using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Framework.UI.Elements
{
    public class PrepaidTopupChoiceElements : BaseElement
    {

        private readonly IWebElement _debit;
        private readonly IWebElement _salary;

        public PrepaidTopupChoiceElements(BasePage page) : base(page)
        {
            _debit = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidTopupChoiceElements.Debit));
            _salary = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidTopupChoiceElements.Salary));
        }
    }
}
