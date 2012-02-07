using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class AcceptedPage : BasePage, IDecisionPage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _acceptBusinessLoan;
        private readonly IWebElement _acceptGuarantorLoan;

        public AcceptedPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("wonga-loan-approve-form"));
            _acceptBusinessLoan = _form.FindElement(By.Id("terms-accept"));
            _acceptGuarantorLoan = _form.FindElement(By.Id("guarantor-accept"));
        }

        public void SignTermsMainApplicant()
        {
            var signTermsLink = _acceptBusinessLoan.FindElement(By.LinkText("I accept the loan terms and conditions"));
            signTermsLink.Click();
        }

        public void SignTermsGuarantor()
        {
            var signGuarantorTermsLink =
                _acceptGuarantorLoan.FindElement(By.LinkText("I accept the guarantor terms and conditions"));
            signGuarantorTermsLink.Click();
        }

    }
}
