using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class EligibilityQuestionsPage : BasePage, IApplyPage
    {
        private readonly IWebElement _form;

        private readonly IWebElement _resident;
        private readonly IWebElement _director;
        private readonly IWebElement _activeCompany;
        private readonly IWebElement _turnover;
        private readonly IWebElement _vat;
        private readonly IWebElement _onlineAccess;
        private readonly IWebElement _guarantee;
        private readonly IWebElement _debitCard;
        private readonly IWebElement _next;

        public Boolean CheckResident { set { _resident.Toggle(value); } }
        public Boolean CheckDirector { set { _director.Toggle(value); } }
        public Boolean CheckActiveCompany { set { _activeCompany.Toggle(value); } }
        public Boolean CheckTurnover { set { _turnover.Toggle(value); } }
        public Boolean CheckVat { set { _vat.Toggle(value); } }
        public Boolean CheckOnlineAccess { set { _onlineAccess.Toggle(value); } }
        public Boolean CheckGuarantee { set { _guarantee.Toggle(value); } }
        public Boolean CheckDebitCard{set{_debitCard.Toggle(value);}}

        public EligibilityQuestionsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.FormId));

            _director = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckDirector));
            _resident = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckResident));
            _activeCompany = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckActiveCompany));
            _turnover = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckTurnover));
            _vat = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckVat));
            _onlineAccess = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckOnlineAccess));
            _guarantee = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckGuarantee));
            _debitCard = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.CheckDebitCard));
            _next = _form.FindElement(By.CssSelector(Elements.Get.WbEligibilityQuestionsPage.NextButton));
        }

        public PersonalDetailsPage Submit()
        {
            _next.Click();
            return new PersonalDetailsPage(Client);
        }
    }
}
