using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Mappings.Pages.PayLater;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class SubmitionPage : BasePage
    {
        private readonly IWebElement _footerInfo;
        private readonly IWebElement _availabelCreditCookie;
        //private readonly IWebElement _availabelCredit;
        private readonly IWebElement _infoCirculButton;
        private readonly IWebElement _textApproved;
        private readonly IWebElement _repaymentDetails;
        private readonly IWebElement _submitButton;

        public SubmitionPage(UiClient client)
            : base(client)
        {
            _footerInfo = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.FooterInfo));
            _availabelCreditCookie = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.AvailabelCreditCookie));
            //_availabelCredit = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.AvailabelCredit));
            _infoCirculButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.InfoCirculButton));
            _textApproved = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.TextApproved));
            _repaymentDetails = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.RepaymentDetails));
            _submitButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.SubmitionPage.SubmitButton));
        }


        public Dictionary<string, string> InspectElementsSubmition()
        {
            var elements = new Dictionary<string, string>
                               {
                                   {"FooterInfo", _footerInfo.Text},
                                   {"AvailabelCreditCookie", _availabelCreditCookie.Text},
                                   {"InfoCirculButton", _infoCirculButton.Text},
                                   {"TextApproved", _textApproved.Text},
                                   {"RepaymentDetails", _repaymentDetails.Text},
                               };

            return elements;
        }
        
        public PayLaterThanksForm RedirectToThanks()
        {
            _submitButton.Click();

            return new PayLaterThanksForm(Client);
        }
    }
}
