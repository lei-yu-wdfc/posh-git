using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class DealDonePage : BasePageMobile, IApplyPage
    {
        private IWebElement _continueButton;
        private IWebElement _repayDate;
        private IWebElement _repayAmount;

        public DealDonePage(MobileUiClient client)
            : base(client)
        {
            Assert.That(Headers, Has.Item(UiMapMobile.Get.DealDonePage.HeaderText));
            _continueButton = Content.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.DealDonePage.ContinueButtonLink)) ??
                              Content.FirstOrDefaultElement(By.CssSelector(UiMapMobile.Get.DealDonePage.ContinueButton));
        }

        public String GetRepaymentDate()
        {
            _repayDate = Content.FindElement(By.CssSelector(UiMapMobile.Get.DealDonePage.RepayDate));
            return _repayDate.Text;
        }

        public String GetRapaymentAmount()
        {
            _repayAmount = Content.FindElement(By.CssSelector(UiMapMobile.Get.DealDonePage.RepayAmount));
            return _repayAmount.Text;
        }

        public IApplyPage ContinueToMyAccount()
        {
            Thread.Sleep(2000);
            if (Config.AUT != AUT.Uk)
                _continueButton.Click();
            else
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.DealDonePage.GoToMyAccount)).Click();

            return Do.Until(() => new MySummaryPageMobile(Client));
        }

        public String GetDealDonePageText
        {
            get { return Content.Text; }
        }
    }
}
