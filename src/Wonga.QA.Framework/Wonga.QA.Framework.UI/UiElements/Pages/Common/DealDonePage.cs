using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public DealDonePage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(Elements.Get.DealDonePage.HeaderText));
            _continueButton = Content.FindElement(By.CssSelector(Elements.Get.DealDonePage.ContinueButtonLinkText));
        }

        public IApplyPage ContinueToMyAccount()
        {
            throw new NotImplementedException();

            //switch (Config.AUT)
            //{
            //    case AUT.Za:
            //        throw new NotImplementedException();
            //    case AUT.Ca:
            //        throw new NotImplementedException();
            //    case AUT.Wb:
            //        throw new NotImplementedException();
            //}
            
        }
    }
}
