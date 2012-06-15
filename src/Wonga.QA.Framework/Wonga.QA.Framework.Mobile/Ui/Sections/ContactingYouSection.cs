using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Framework.Mobile.Mappings.Sections;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
{
    public class ContactingYouSection : BaseSection
    {
        private readonly IWebElement _homePhone;
        private readonly IWebElement _homePhoneP2;
        private readonly IWebElement _homePhoneP3;
        private readonly IWebElement _mobilePhone;
        private readonly IWebElement _mobilePhoneP2;
        private readonly IWebElement _mobilePhoneP3;
        private readonly IWebElement _email;
        private readonly IWebElement _emailConfirm;

        public String HomePhoneNumber
        {
            set
            {
                switch (Config.AUT)
                {
                    case (AUT.Za):
                        _homePhone.SendValue(value);
                        break;
                    case (AUT.Ca):
                        _homePhone.SendValue(value.Substring(0, 3));
                        _homePhoneP2.SendValue(value.Substring(3, 3));
                        _homePhoneP3.SendValue(value.Substring(6, 4));
                        break;
                    case (AUT.Wb):
                        _homePhone.SendValue(value);
                        break;
                }
            }
        }
        public String CellPhoneNumber
        {
            set
            {
                switch (Config.AUT)
                {
                    case (AUT.Za):
                        _mobilePhone.SendValue(value);
                        break;
                    case (AUT.Ca):
                        _mobilePhone.SendValue(value.Substring(0, 3));
                        _mobilePhoneP2.SendValue(value.Substring(3, 3));
                        _mobilePhoneP3.SendValue(value.Substring(6, 4));
                        break;
                    case (AUT.Wb):
                        _mobilePhone.SendValue(value);
                        break;
                    case (AUT.Uk):
                    case (AUT.Pl):
                        _mobilePhone.SendValue(value);
                        break;
                }

            }
        }
        public String EmailAddress { set { _email.SendValue(value); } }
        public String ConfirmEmailAddress { set { _emailConfirm.SendValue(value); } }

        public ContactingYouSection(BasePageMobile page) : base(UiMapMobile.Get.ContactingYouSection.Fieldset, page)
        {
            switch (Config.AUT)
            {
                case (AUT.Za):
                case (AUT.Wb):
                case (AUT.Ca):
                case (AUT.Uk): 
                _homePhone = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.HomePhone));
                    break;
            }
            _email = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.Email));
            _emailConfirm = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.EmailConfirm));
           
            _mobilePhone = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.MobilePhone));

            switch (Config.AUT)
            {
                case (AUT.Ca):
                    _homePhoneP2 = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.HomePhoneP2));
                    _homePhoneP3 = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.HomePhoneP3));
                    _mobilePhoneP2 = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.MobilePhoneP2));
                    _mobilePhoneP3 = Section.FindElement(By.CssSelector(UiMapMobile.Get.ContactingYouSection.MobilePhoneP3));
                    break;
            }
            
        }

        public bool CanEnterLettersToMobilePhoneField(string text)
        {
            return _mobilePhone.VerifyTextEntering(text);
        }
    }
}
