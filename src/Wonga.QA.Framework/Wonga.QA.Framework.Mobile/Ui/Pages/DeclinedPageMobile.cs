using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.UI.Elements;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class DeclinedPageMobile : BasePageMobile, IDecisionPage
    {
        public TabsElement TabsElement { get; set; }
        public LoginElement LoginElement { get; set; }
        public HelpElement HelpElement { get; set; }
        public InternationalElement InternationalElement { get; set; }

        public DeclinedPageMobile(MobileUiClient client)
            : base(client)
        {
            switch (Config.AUT)
            {
              case AUT.Za:
              //case AUT.Ca:
                  Assert.That(Headers, Has.Item(ContentMapMobile.Get.DeclinedPageMobile.HeaderText));
                  break;
              default:
                    throw new NotImplementedException();
            }
        }

        public bool LookForHeaderLinks()
        {
            return Do.Until(IsHeaderLinksExist);
        }

        private bool IsHeaderLinksExist()
        {
            try
            {
                TabsElement = new TabsElement(this);
                LoginElement = new LoginElement(this);
                HelpElement = new HelpElement(this);
                InternationalElement = new InternationalElement(this);
            }
            catch(NoSuchElementException)
            {
                return false;
            }
            catch(Exception e)
            {
                throw e;
            }
            return true;
        }

        public bool DeclineAdviceExists()
        {
            var tokenResult = !string.IsNullOrWhiteSpace(Content.FindElement(By.CssSelector(UiMapMobile.Get.DeclinedPage.DeclineAdvice)).Text);
            return tokenResult;
        }
    }
}
