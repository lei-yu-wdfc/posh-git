using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class AcceptedPage : BasePage, IDecisionPage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _acceptBusinessLoan;
        private readonly IWebElement _acceptGuarantorLoan;
        private readonly IWebElement _submit;

        public AcceptedPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.Id(Elements.Get.WbAcceptedPage.FormId));
            _acceptBusinessLoan = _form.FindElement(By.Id(Elements.Get.WbAcceptedPage.AcceptBusinessLoan));
            _acceptGuarantorLoan = _form.FindElement(By.Id(Elements.Get.WbAcceptedPage.AcceptGuarantorLoan));
            _submit = _form.FindElement(By.Name(Elements.Get.WbAcceptedPage.SubmitButton));
        }

        public void SignTermsMainApplicant()
        {
            var signTermsLink = _acceptBusinessLoan.FindElement(By.LinkText(Elements.Get.WbAcceptedPage.AcceptLinkText));
            signTermsLink.Click();
        }

        public void SignTermsGuarantor()
        {
            var signGuarantorTermsLink = _acceptGuarantorLoan.FindElement(By.LinkText(Elements.Get.WbAcceptedPage.AcceptLinkText));
            signGuarantorTermsLink.Click();
        }

        public IApplyPage Submit()
        {
            _submit.Click();
            return new Common.DealDonePage(Client);
        }

    }
}
