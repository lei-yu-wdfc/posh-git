using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class PersonalDebitCardPage : BasePage
    {
        private readonly IWebElement _next;
        private readonly IWebElement _form;

        public Sections.DebitCardSection DebitCardSection { get; set; }
        public Sections.MobilePinVerificationSection MobilePinVerification { get; set; }

        public PersonalDebitCardPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMap.Get.PersonalDebitCardDetailsPage.FormId));
            _next = _form.FindElement(By.CssSelector(UiMap.Get.PersonalDebitCardDetailsPage.NextButton));
            MobilePinVerification = new Sections.MobilePinVerificationSection(this);
            DebitCardSection = new Sections.DebitCardSection(this);
        }

        public BasePage Next()
        {
            _next.Click();
            switch(Config.AUT)
            {
                case(AUT.Wb):
                    return new Wb.BusinessDetailsPage(Client);
                case(AUT.Uk):
                    return new ProcessingPage(Client);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
