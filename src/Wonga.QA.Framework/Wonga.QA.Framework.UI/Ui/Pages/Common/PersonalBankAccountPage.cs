using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Sections;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class PersonalBankAccountPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;

        public BankAccountSection BankAccountSection { get; set; }
        public MobilePinVerificationSection PinVerificationSection { get; set; }

        public PersonalBankAccountPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Ui.Get.PersonalBankAccountPage.FormId));
            _next = _form.FindElement(By.CssSelector(Ui.Get.PersonalBankAccountPage.NextButton));
            BankAccountSection = new BankAccountSection(this);
            switch(Config.AUT)
            {
                case(AUT.Za):
                case (AUT.Ca):
                    PinVerificationSection = new MobilePinVerificationSection(this);
                    break;
            }
        }

        public BasePage Next()
        {
            _next.Click();
            switch(Config.AUT)
            {
                case(AUT.Wb):
                    return new PersonalDebitCardPage(Client);
                case(AUT.Za):
                case (AUT.Ca):
                    return new ProcessingPage(Client);
                case AUT.Uk:
                    return new PersonalDebitCardPage(Client);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
