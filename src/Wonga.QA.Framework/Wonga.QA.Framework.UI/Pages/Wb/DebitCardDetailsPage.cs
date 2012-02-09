using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class DebitCardDetailsPage : BasePage
    {
        private readonly IWebElement _next;
        private readonly IWebElement _form;

        private readonly IWebElement _cardType;
        private readonly IWebElement _cardNumber;
        private readonly IWebElement _cardName;
        private readonly IWebElement _cardExpiryDateMonth;
        private readonly IWebElement _cardExpiryDateYear;
        private readonly IWebElement _cardStartDateMonth;
        private readonly IWebElement _cardStartDateYear;
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
                _cardExpiryDateMonth.SelectOption(date[0]);
                _cardExpiryDateYear.SelectOption(date[1]);
            }
        }
        public String StartDate
        {
            set
            {
                var date = value.Split('/');
                _cardStartDateMonth.SelectOption(date[0]);
                _cardStartDateYear.SelectOption(date[1]);
            }
        }

        public Wonga.QA.Framework.UI.Elements.Sections.MobilePinVerificationSection MobilePinVerification { get; set; }

        public DebitCardDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-card-form"));
            _next = _form.FindElement(By.Name("next"));
            _cardType = _form.FindElement(By.Name("card_type"));
            _cardNumber = _form.FindElement(By.Name("card_number"));
            _cardName = _form.FindElement(By.Name("card_name"));
            _cardExpiryDateMonth = _form.FindElement(By.Name("card_expiry_date[month]"));
            _cardExpiryDateYear = _form.FindElement(By.Name("card_expiry_date[year]"));
            _cardStartDateMonth = _form.FindElement(By.Name("card_start_date[month]"));
            _cardStartDateYear = _form.FindElement(By.Name("card_start_date[year]"));
            _cardSecurity = _form.FindElement(By.Name("card_security"));
            MobilePinVerification = new Wonga.QA.Framework.UI.Elements.Sections.MobilePinVerificationSection(this);
        }

        public BusinessDetailsPage Next()
        {
            _next.Click();
            return new BusinessDetailsPage(Client);
        }
    }
}
