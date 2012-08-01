using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.Mappings.Pages.FinancialAssessment;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FinancialAssessmentPage : BasePage
    {
        private readonly IWebElement _username;
        private readonly IWebElement _assesmentsEmail;
        private readonly IWebElement _buttonGetStarted;
        private bool _isNotAvailablePage;

        public FinancialAssessmentPage(UiClient client)
            : base(client)
        {
            _username = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentPage.PrepopulatedName));
            try
            {
                _assesmentsEmail =
                    Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentPage.AssesmentsTeamEmail));
                _buttonGetStarted =
                    Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentPage.GetStartedButton));
                _isNotAvailablePage = false;
            }
            catch
            {
                _isNotAvailablePage = true;
            }
        }

        public bool IsNotAvailablePage()
        {
            return _isNotAvailablePage;
        }

        public BasePage GetStartedClick()
        {
            if (_isNotAvailablePage)
            {
                throw new Exception("You loged in like customer without loan, and try start V3 Online Income & Expenditure process");
            }
            _buttonGetStarted.Click();
            return new FAAboutYouPage(Client);
        }

        public string GetPrepopulatedName()
        {
            return _username.Text;
        }

        public string GetAssesmentsEmail()
        {
            if (_isNotAvailablePage)
            {
                return "You loged in like customer without loan, and you see Not Available Page";
            }
            return _assesmentsEmail.Text;
        }
    }
}
