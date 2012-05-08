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
            : base(UiMap.Get.MobilePinVerificationSection.Fieldset, page)
        {
            _pin = Section.FindElement(By.CssSelector(UiMap.Get.MobilePinVerificationSection.Pin));
            _resendPin = Section.FindElement(By.CssSelector(UiMap.Get.MobilePinVerificationSection.ResendPin));
        }
        public String Pin { set { _pin.SendValue(value); } }
        public bool ResendPinClickAndCheck()
        {
            _resendPin.Click();
            try
            {
                _resendPinMessege = Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.MobilePinVerificationSection.ResendPinMessage)));
                if (_resendPinMessege.Text.Equals("Your pin has now been resent to your phone. Your should receive it within minutes.") || _resendPinMessege.Text.Equals("We have resent your PIN."))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Text wrong");
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Can't find pupop");
                return false;
            }

        }
    }
}
