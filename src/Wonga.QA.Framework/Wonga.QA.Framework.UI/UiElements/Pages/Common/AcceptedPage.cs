using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class AcceptedPage : BasePage, IDecisionPage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _acceptBusinessLoanLink;
        private readonly IWebElement _acceptGuarantorLoanLink;
        private readonly IWebElement _agreementConfirm;
        private readonly IWebElement _directDebitConfirm;
        private readonly IWebElement _initials;
        private readonly IWebElement _initials2;
        private readonly IWebElement _initials3;
        private readonly IWebElement _signature;
        private readonly IWebElement _dateOfAgreement;
        private readonly IWebElement _continueTermsButton;
        private readonly IWebElement _continueDirectDebitButton;

        public String Initials1 { set{_initials.SendValue(value);} }
        public String Initials2 { set { _initials2.SendValue(value); } }
        public String Initials3 { set { _initials3.SendValue(value); } }
        public String Signature { set{_signature.SendValue(value);} }
        public String DateOfAgreement { set{_signature.SendValue(value);} }
       
        public AcceptedPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Elements.Get.AcceptedPage.FormId));
            switch(Config.AUT)
            {
                case(AUT.Wb):
                    _acceptBusinessLoanLink = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.AcceptBusinessLoan));
                    _acceptGuarantorLoanLink = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.AcceptGuarantorLoan));
                    break;
                case(AUT.Za):
                    _agreementConfirm = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.AgreementConfirm));
                    _directDebitConfirm = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.DirectDebitConfirm));
                    break;
                case (AUT.Ca):
                    _agreementConfirm = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.AgreementConfirm));
                    _directDebitConfirm = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.DirectDebitConfirm));
                    _initials = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.Initials1));
                    _initials2 = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.Initials2));
                    _initials3 = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.Initials3));
                    _signature = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.Signature));
                    _dateOfAgreement = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.DateOfAgreement));
                    _continueTermsButton = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.ContinueTermsButton));
                    _continueDirectDebitButton = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.ContinueDirectDebitButton));
                    break;
            }
        }


        public void SignAgreementConfirm()
        {
            _agreementConfirm.Click();
        }

        public void SignDirectDebitConfirm()
        {
            _directDebitConfirm.Click();
        }

        public void SignTermsMainApplicant()
        {
            _acceptBusinessLoanLink.Click();
        }

        public void SignTermsGuarantor()
        {
            _acceptGuarantorLoanLink.Click();
        }

        public void SignConfirmCA(string date, string firstName, string lastName)
        {
            string initials = string.Format("{0}{1}", firstName[0], lastName[0]);
            string signature = string.Format("{0} {1}", firstName, lastName);
            _initials.SendKeys(initials);
            _initials2.SendKeys(initials);
            _initials3.SendKeys(initials);
            _signature.SendKeys(signature);
            _dateOfAgreement.SendKeys(date);
            _signature.Click();
            _continueTermsButton.Click();
            _agreementConfirm.Click();
            _continueDirectDebitButton.Click();
            _directDebitConfirm.Click();
        }

        public IApplyPage Submit()
        {
            _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.SubmitButton)).Click();
            return new DealDonePage(Client);
        }

    }
}
