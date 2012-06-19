using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
{
    public class MobilePinVerificationSection : BaseSection
    {
        private readonly IWebElement _pin;
        private readonly IWebElement _resendPin;
        private IWebElement _resendPinMessege;

        public MobilePinVerificationSection(BasePageMobile page)
            : base(UiMapMobile.Get.MobilePinVerificationSection.Fieldset, page)
        {
            _pin = Section.FindElement(By.CssSelector(UiMapMobile.Get.MobilePinVerificationSection.Pin));
            _resendPin = Section.FindElement(By.CssSelector(UiMapMobile.Get.MobilePinVerificationSection.ResendPin));
        }
        public String Pin { set { _pin.SendValue(value); } }

        public void ResendPinClick()
        {
            _resendPin.Click();
        }

        public void CloseResendPinPopup()
        {
            IWebElement popup =
                Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MobilePinVerificationSection.ResendPinPopupClose)));
            popup.Click();
        }

        public bool ResendPinClickAndCheck()
        {
            _resendPin.Click();
            try
            {
                _resendPinMessege = Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MobilePinVerificationSection.ResendPinMessage)));
                if (_resendPinMessege.Text.Equals(ContentMapMobile.Get.MobilePinVerificationSection.ResendPinMessage1) || _resendPinMessege.Text.Equals(ContentMapMobile.Get.MobilePinVerificationSection.ResendPinMessage2) || _resendPinMessege.Text.Equals(ContentMapMobile.Get.MobilePinVerificationSection.ResendPinMessage3))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Text wrong");
                    Console.WriteLine("Current text: " + _resendPinMessege.Text);
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
