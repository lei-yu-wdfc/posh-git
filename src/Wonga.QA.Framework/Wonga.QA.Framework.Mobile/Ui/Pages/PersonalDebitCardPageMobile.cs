using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class PersonalDebitCardPageMobile : BasePageMobile
    {
        private readonly IWebElement _next;
        private readonly IWebElement _form;

        public Sections.DebitCardSection DebitCardSection { get; set; }
        public Sections.MobilePinVerificationSection MobilePinVerification { get; set; }

        public PersonalDebitCardPageMobile(MobileUiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.PersonalDebitCardPageMobile.FormId));
            _next = _form.FindElement(By.CssSelector(UiMapMobile.Get.PersonalDebitCardPageMobile.NextButton));
            MobilePinVerification = new Sections.MobilePinVerificationSection(this);
            DebitCardSection = new Sections.DebitCardSection(this);
        }

        public BasePageMobile Next()
        {
            _next.Click();
            switch(Config.AUT)
            {
                //case(AUT.Wb):
                    //return new Wb.BusinessDetailsPage(Client);
                case(AUT.Uk):
                    return new ProcessingPageMobile(Client);
                default:
                    throw new NotImplementedException();
            }
        }
        
    }
}
