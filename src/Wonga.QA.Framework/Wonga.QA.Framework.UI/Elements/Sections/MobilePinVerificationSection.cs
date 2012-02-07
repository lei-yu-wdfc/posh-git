using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Pages;

namespace Wonga.QA.Framework.UI.Elements.Sections
{
    public class MobilePinVerificationSection : BaseSection
    {
        private readonly IWebElement _pin;

        public MobilePinVerificationSection(BasePage page)
            : base("Mobile PIN verification", page)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    _pin = Section.FindElement(By.Name("pin_za"));
                    break;
                case AUT.Ca:
                    _pin = Section.FindElement(By.Name("pin_ca"));
                    break;
                case AUT.Wb:
                    _pin = Section.FindElement(By.Name("pin"));
                    break;
            }
        }
        public String Pin { set { _pin.SendValue(value); } }
    }
}
