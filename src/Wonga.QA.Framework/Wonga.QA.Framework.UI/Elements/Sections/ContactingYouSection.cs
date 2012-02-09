using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Pages;

namespace Wonga.QA.Framework.UI.Elements.Sections
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
                }

            }
        }
        public String EmailAddress { set { _email.SendValue(value); } }
        public String ConfirmEmailAddress { set { _emailConfirm.SendValue(value); } }

        public ContactingYouSection(BasePage page)
            : base("Contacting you", page)
        {
            _email = Section.FindElement(By.Name("email"));
            _emailConfirm = Section.FindElement(By.Name("email_confirm"));

            switch (Config.AUT)
            {
                case (AUT.Za):
                    _homePhone = Section.FindElement(By.Name("home_phone_za"));
                    _mobilePhone = Section.FindElement(By.Name("mobile_phone_za"));
                    break;
                case (AUT.Ca):
                    _homePhone = Section.FindElement(By.Name("home_phone_ca[part1]"));
                    _homePhoneP2 = Section.FindElement(By.Name("home_phone_ca[part2]"));
                    _homePhoneP3 = Section.FindElement(By.Name("home_phone_ca[part3]"));
                    _mobilePhone = Section.FindElement(By.Name("mobile_phone_ca[part1]"));
                    _mobilePhoneP2 = Section.FindElement(By.Name("mobile_phone_ca[part2]"));
                    _mobilePhoneP3 = Section.FindElement(By.Name("mobile_phone_ca[part3]"));
                    break;
                case (AUT.Wb):
                    _homePhone = Section.FindElement(By.Name("home_phone"));
                    _mobilePhone = Section.FindElement(By.Name("mobile_phone"));
                    break;
            }
        }
    }
}
