using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Elements
{
    public class MyAccountNavigationElement : BaseElement
    {
        private readonly IWebElement _myPaymentDetailsButton;
        private readonly IWebElement _mySummaryButton;
        private readonly IWebElement _myPersonalDetailsButton;

        public MyAccountNavigationElement(BasePage page)
            : base(page)
        {
            _mySummaryButton =
                        Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyAccountNavigationSection.MySummary));
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                    _myPaymentDetailsButton =
                        Page.Client.Driver.FindElement(
                            By.CssSelector(UiMap.Get.MyAccountNavigationSection.MyPaymentsDetails));
                    _myPersonalDetailsButton =
                        Page.Client.Driver.FindElement(
                            By.CssSelector(UiMap.Get.MyAccountNavigationSection.MyPersonalDetails));
                    break;
                case AUT.Wb:
                    _myPersonalDetailsButton =
                        Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyAccountNavigationSection.YourDetails));
                    _myPaymentDetailsButton =
                        Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyAccountNavigationSection.AccountDetails));
                    break;


            }
        }
        public MyPaymentsPage MyPaymentDetailsButtonClick()
        {
            _myPaymentDetailsButton.Click();
            return new MyPaymentsPage(Page.Client);
        }
        public MyPersonalDetailsPage MyPersonalDetailsButtonClick()
        {
            _myPersonalDetailsButton.Click();
            return new MyPersonalDetailsPage(Page.Client);
        }
        public MySummaryPage MySummaryButtonClick()
        {
            _mySummaryButton.Click();
            return new MySummaryPage(Page.Client);
        }

    }
}
