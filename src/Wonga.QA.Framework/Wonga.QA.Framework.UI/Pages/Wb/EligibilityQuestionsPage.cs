using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.Pages.Wb
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
        private readonly IWebElement _next;

        public Boolean CheckResident { set { _resident.Toggle(value); } }
        public Boolean CheckDirector { set { _director.Toggle(value); } }
        public Boolean CheckActiveCompany { set { _activeCompany.Toggle(value); } }
        public Boolean CheckTurnover { set { _turnover.Toggle(value); } }
        public Boolean CheckVat { set { _vat.Toggle(value); } }
        public Boolean CheckOnlineAccess { set { _onlineAccess.Toggle(value); } }
        public Boolean CheckGuarantee { set { _guarantee.Toggle(value); } }

        public EligibilityQuestionsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-questions-form"));

            _director = _form.FindElement(By.Name("director"));
            _resident = _form.FindElement(By.Name("resident"));
            _activeCompany = _form.FindElement(By.Name("active_company"));
            _turnover = _form.FindElement(By.Name("turnover"));
            _vat = _form.FindElement(By.Name("vat"));
            _onlineAccess = _form.FindElement(By.Name("online_access"));
            _guarantee = _form.FindElement(By.Name("guarantee"));
            _next = _form.FindElement(By.Name("next"));
        }

        public PersonalDetailsPage Submit()
        {
            _next.Click();
            return new PersonalDetailsPage(Client);
        }
    }
}
