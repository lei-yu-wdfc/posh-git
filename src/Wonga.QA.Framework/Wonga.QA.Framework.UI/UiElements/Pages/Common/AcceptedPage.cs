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
        private readonly IWebElement _submit;

        public AcceptedPage(UiClient client) : base(client)
        {
            Thread.Sleep(3000);
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
            }
            
            _submit = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.SubmitButton));
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

        public IApplyPage Submit()
        {
            _submit.Click();
            return new Common.DealDonePage(Client);
        }

    }
}
