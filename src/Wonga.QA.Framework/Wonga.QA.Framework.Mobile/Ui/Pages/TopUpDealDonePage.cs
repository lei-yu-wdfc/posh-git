using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class TopUpDealDonePage : BasePageMobile
    {
        private readonly IWebElement _title;
        private readonly IWebElement _backToMyAccount;

        
        public TopUpDealDonePage(MobileUiClient client) : base(client)
        {
            _title = Content.FindElement(By.CssSelector(UiMapMobile.Get.TopUpDealDonePage.Title));
            if (!_title.Text.Equals("Success! Your request for some extra credit has been approved."))
                throw new Exception("Title not as expected");
            _backToMyAccount = Content.FindElement(By.CssSelector(UiMapMobile.Get.TopUpDealDonePage.BackToMyAccount));
        }
    }
}
