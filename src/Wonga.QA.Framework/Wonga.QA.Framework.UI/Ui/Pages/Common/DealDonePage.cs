using System;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class DealDonePage : BasePage, IApplyPage
    {
        private IWebElement _continueButton;
        private IWebElement _repayDate;
        private IWebElement _repayAmount;

        public DealDonePage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(UiMap.Get.DealDonePage.HeaderText));
            _continueButton = Content.FirstOrDefaultElement(By.CssSelector(UiMap.Get.DealDonePage.ContinueButtonLink)) ??
                              Content.FirstOrDefaultElement(By.CssSelector(UiMap.Get.DealDonePage.ContinueButton));
        }

        public String GetRepaymentDate()
        {
            _repayDate = Content.FindElement(By.CssSelector(UiMap.Get.DealDonePage.RepayDate));
           return _repayDate.Text;
        }

        public String GetRapaymentAmount()
        {
            _repayAmount = Content.FindElement(By.CssSelector(UiMap.Get.DealDonePage.RepayAmount));
             return _repayAmount.Text; 
        }

        public IApplyPage ContinueToMyAccount()
        {
            Thread.Sleep(2000);
            if (Config.AUT != AUT.Uk)
                _continueButton.Click();
            else
                Client.Driver.FindElement(By.CssSelector(UiMap.Get.DealDonePage.GoToMyAccount)).Click();
                
            return Do.Until(() => new MySummaryPage(Client));
        }

        public String GetDealDonePageText
        {
            get { return Content.Text; }
        }
    }
}
