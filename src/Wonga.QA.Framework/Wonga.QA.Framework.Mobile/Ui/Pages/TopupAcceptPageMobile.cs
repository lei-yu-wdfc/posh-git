using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class TopupAcceptPageMobile : BasePageMobile
    {
        private readonly IWebElement _title;
        private readonly IWebElement _secci;
        private readonly IWebElement _acceptButton;
        
        public TopupAcceptPageMobile(MobileUiClient client) : base(client)
        {
            _title = Content.FindElement(By.CssSelector(UiMapMobile.Get.TopupAcceptPageMobile.Title));
            if (!_title.Text.Equals("One Last Step"))
                throw new Exception("Title not as expected");
            _secci = Content.FindElement(By.CssSelector(UiMapMobile.Get.TopupAcceptPageMobile.Secci));
            _acceptButton = _secci.FindElement(By.CssSelector(UiMapMobile.Get.TopupAcceptPageMobile.AcceptButton));
        }

        public TopUpDealDonePage Accept()
        {
            _acceptButton.Click();
            return new TopUpDealDonePage(Client);
        }
    }
}
