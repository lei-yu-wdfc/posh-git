using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Elements;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class MySummaryPageMobile : BasePageMobile, IApplyPage
    {
        private readonly IWebElement _myPaymentDetailsButton;
        private readonly IWebElement _mySummaryButton;
        private readonly IWebElement _myPersonalDetailsButton;
        private readonly IWebElement _warningBox;

        public IWebElement ViewLoanDetails;

        public SlidersElement SlidersElement { get; set; }
        public TopupSlidersElement TopupSlidersElement { get; set; }

        public MySummaryPageMobile(MobileUiClient client) : base(client)
        {
            _mySummaryButton = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.MySummaryButton));
            _myPersonalDetailsButton =
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.MyPersonalDetailsButton));
            _myPaymentDetailsButton =
                Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.MyPaymentDetailsButton));
            _warningBox = Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MySummaryPage.WarningBox));
        }

        public ApplyPageMobile ApplyForLoan(string howlong, string howmuch)
        {
            SlidersElement = new SlidersElement(this);
            SlidersElement.HowMuch = howlong;
            SlidersElement.HowLong = howmuch;
            SlidersElement.Submit.Click();
            return new ApplyPageMobile(Client);
        }

        public MyPersonalDetailsPageMobile GoToMyPersonalDetailsPage()
        {
            _myPersonalDetailsButton.Click();
            return new MyPersonalDetailsPageMobile(Client);
        }

        public MySummaryPageMobile ViewMyLoanDetails()
        {
            ViewLoanDetails = Client.Driver.FindElement(By.CssSelector(".ViewLoanDetails"));
            ViewLoanDetails.Click();
            var myLoanDetailsPopUp = Do.Until(() => new MyLoanDetailsPopUpElement(this));
            myLoanDetailsPopUp.Close();
            return new MySummaryPageMobile(Client);
        }

        public TopupRequestPage TopUpLoan(string amount)
        {
            TopupSlidersElement = new TopupSlidersElement(this);
            TopupSlidersElement.HowMuch = amount;
            TopupSlidersElement.Apply();
            return new TopupRequestPage(Client);
        }
    }
}
