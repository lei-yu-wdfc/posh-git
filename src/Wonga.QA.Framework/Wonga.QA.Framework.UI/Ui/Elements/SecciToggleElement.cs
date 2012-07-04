using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Framework.UI.Elements
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