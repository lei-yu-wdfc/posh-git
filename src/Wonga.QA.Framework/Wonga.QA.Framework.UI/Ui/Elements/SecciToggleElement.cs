using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.Ui.Elements
{
    public class SecciToggleElement : BaseElement
    {
        private readonly IWebElement _toggleLink;
        /*private readonly IWebElement _loanAmount;
        private readonly IWebElement _grandTotalAmount;
        private readonly IWebElement _totalFees;
        private readonly IWebElement _subTotalAmount;
         * */

        public SecciToggleElement(BasePage page)
            : base(page)
        {
            _toggleLink = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SecciToggleElement.Link));
        }

        public void SecciToggleButtonClick()
        {
            _toggleLink.Click();
            Thread.Sleep(3000);
        }

        public string GetSecciToggleButtonText()
        {
            return _toggleLink.Text;
        }
    }
}