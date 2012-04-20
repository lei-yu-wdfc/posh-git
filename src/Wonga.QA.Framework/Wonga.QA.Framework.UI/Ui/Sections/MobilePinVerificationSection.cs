using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class MobilePinVerificationSection : BaseSection
    {
        private readonly IWebElement _pin;
        private readonly IWebElement _resendPin;
        private IWebElement _resendPinMessege;

        public MobilePinVerificationSection(BasePage page)
            : base(Ui.Get.MobilePinVerificationSection.Fieldset, page)
        {
            _pin = Section.FindElement(By.CssSelector(Ui.Get.MobilePinVerificationSection.Pin));
            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Za:
                    _resendPin = Section.FindElement(By.CssSelector(Ui.Get.MobilePinVerificationSection.ResendPin));
                    break;
            }
        }
        public String Pin { set { _pin.SendValue(value); } }
        public bool ResendPinClickAndCheck()
        {
            _resendPin.Click();
            try
            {

                Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.MobilePinVerificationSection.ResendPinMessage)));
                _resendPinMessege = Page.Client.Driver.FindElement(By.CssSelector(Ui.Get.MobilePinVerificationSection.ResendPinMessage));
                if (_resendPinMessege.Text.Equals("Your pin has now been resent to your phone. Your should receive it within minutes."))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
