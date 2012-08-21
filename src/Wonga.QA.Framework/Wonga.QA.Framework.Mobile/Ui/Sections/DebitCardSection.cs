using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Elements;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Sections
{
    public class DebitCardSection : BaseSection
    {

        private readonly IWebElement _cardType;
        private readonly IWebElement _cardNumber;
        private readonly IWebElement _cardName;
        private readonly IWebElement _cardExpiry;
        //private readonly IWebElement _cardExpiryDateMonth;
        //private readonly IWebElement _cardExpiryDateYear;
        //private readonly IWebElement _cardStartDateMonth;
        //private readonly IWebElement _cardStartDateYear;
        private readonly IWebElement _cardSecurity;

        public String CardType { set { _cardType.SelectOption(value); } }
        public String CardNumber { set { _cardNumber.SendValue(value); } }
        public String CardName { set { _cardName.SendValue(value); } }
        public String CardSecurity { set { _cardSecurity.SendValue(value); } }
        public String ExpiryDate
        {
            set
            {
                var date = value.Split('/');
                _cardExpiry.Click();
                var monthYearScrollElement = Do.Until(() => new MonthYearMobiScrollElement(Page.Client));
                monthYearScrollElement.SelectExpiryDate();
                //_cardExpiryDateMonth.SelectOption(date[0]);
                //_cardExpiryDateYear.SelectOption(date[1]);
            }
        }
        //public String StartDate
        //{
        //    set
        //    {
        //        var date = value.Split('/');
        //        _cardStartDateMonth.SelectOption(date[0]);
        //        _cardStartDateYear.SelectOption(date[1]);
        //    }
        //}

        public DebitCardSection(BasePageMobile page)
            : base(UiMapMobile.Get.DebitCardSection.Fieldset, page)
        {
            _cardType = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardType));
            _cardNumber = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardNumber));
            _cardName = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardName));
            _cardExpiry = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardExpiry));
            //_cardExpiryDateMonth = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardExpiryDateMonth)); //mobiscroll
            //_cardExpiryDateYear = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardExpiryDateYear)); //mobiscroll
            //_cardStartDateMonth = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardStartDateMonth)); //mobiscroll
            //_cardStartDateYear = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardStartDateYear)); //mobiscroll
            _cardSecurity = Section.FindElement(By.CssSelector(UiMapMobile.Get.DebitCardSection.CardSecurityNumber));
        }
    }
}
