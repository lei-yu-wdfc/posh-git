using OpenQA.Selenium;
using Wonga.QA.MigrationTests.Selenium.V2.Pages;

namespace Wonga.QA.MigrationTests.Selenium.V2.Elements
{
    public class V2SlidersElement : V2BaseElement
    {
        private readonly IWebElement _form;
        //private readonly IWebElement _amountSlider;
        //private readonly IWebElement _durationSlider;
        //private readonly IWebElement _loanAmount;
        //private readonly IWebElement _loanDuration;
        //private IWebElement _submit;

        public V2SlidersElement(V2BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.Id("ctl00_bodyControl"));

        }
    }
}
