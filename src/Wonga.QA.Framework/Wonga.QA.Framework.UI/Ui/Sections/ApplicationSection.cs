using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class ApplicationSection : BaseSection
    {
        private readonly IWebElement _securityCode;
        private readonly IWebElement _minCash;

        private IWebElement _businessLoanApplicationDetails;
        private IWebElement _additionalDirectorsOrPartners;

        public String SetSecurityCode
        {
            set { _securityCode.SendValue(value); }
        }
        public String SetMinCash
        {
            set { _minCash.SendValue(value); }
        }
        public String SetPin
        {
            set
            {
                var pin =
                    Do.Until(() => Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.EditPin)));
                Do.Until(()=>pin.Displayed);
                pin.SendValue(value);
            }
        }
        public ApplicationSection(BasePage page)
            : base(UiMap.Get.ApplicationSection.FormId, page)
        {
            switch (Config.AUT)
            {
                case AUT.Uk:
                    _securityCode = Section.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.SecurityCode));
                    _minCash = Section.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.MinCash));
                    break;
                case AUT.Wb:
                    _businessLoanApplicationDetails = Section.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.BusinessLoanApplicationDetails));
                    _additionalDirectorsOrPartners = Section.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.AdditionalDirectorsOrPartners));
                    Assert.That(_businessLoanApplicationDetails.Text, Is.EqualTo(ContentMap.Get.ApplicationSection.BusinessLoanApplicationDetails));
                    Assert.That(_additionalDirectorsOrPartners.Text, Is.EqualTo(ContentMap.Get.ApplicationSection.AdditionalDirectorsOrPartners));
                    break;
            }
        }

        public void ClickChangeMobileButton()
        {
            Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.ApplicationSection.ChangeMobileButton)).Click();
        }
    }
}
