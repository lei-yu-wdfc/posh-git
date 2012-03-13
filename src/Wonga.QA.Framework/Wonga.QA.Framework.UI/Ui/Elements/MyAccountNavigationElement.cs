using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Elements
{
    public class MyAccountNavigationElement : BaseElement
    {
        private readonly IWebElement _myPaymentDetailsButton;

        public MyAccountNavigationElement(BasePage page)
            : base(page)
        {
            _myPaymentDetailsButton =
                Page.Client.Driver.FindElement(By.XPath(Ui.Get.MyAccountNavigationSection.MyPaymentsDetails));
        }
        public MyPaymentsPage MyPaymentDetailsButtonClick()
        {
            _myPaymentDetailsButton.Click();
            return new MyPaymentsPage(Page.Client);
        }
    }
}
