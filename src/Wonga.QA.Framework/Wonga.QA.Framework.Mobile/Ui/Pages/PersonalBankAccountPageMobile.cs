using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Mappings.Pages;
using Wonga.QA.Framework.Mobile.Ui.Sections;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class PersonalBankAccountPageMobile : BasePageMobile
    { 
        private readonly IWebElement _form;
        private readonly IWebElement _next;

        public BankAccountSection BankAccountSection { get; set; }
        public MobilePinVerificationSection PinVerificationSection { get; set; }

        public PersonalBankAccountPageMobile(MobileUiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.PersonalBankAccountPageMobile.FormId));
            _next = _form.FindElement(By.CssSelector(UiMapMobile.Get.PersonalBankAccountPageMobile.NextButton));
            BankAccountSection = new BankAccountSection(this);
            switch(Config.AUT)
            {
                case(AUT.Za):
                //case (AUT.Ca):
                    PinVerificationSection = new MobilePinVerificationSection(this);
                    break;
            }
        }

        public BasePageMobile Next()
        {
            _next.Click();
            switch(Config.AUT)
            {
                //case(AUT.Wb):
                case (AUT.Uk):
                    return new PersonalDebitCardPageMobile(Client);
                case(AUT.Za):
                //case (AUT.Ca):
                    return new ProcessingPageMobile(Client);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
