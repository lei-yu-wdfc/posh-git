using OpenQA.Selenium;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class AcceptedPage : BasePage, IDecisionPage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _acceptBusinessLoanLink;
        private readonly IWebElement _acceptGuarantorLoanLink;
        private readonly IWebElement _submit;

        public AcceptedPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Elements.Get.AcceptedPage.FormId));
            _acceptBusinessLoanLink = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.AcceptBusinessLoan));
            _acceptGuarantorLoanLink = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.AcceptGuarantorLoan));
            _submit = _form.FindElement(By.CssSelector(Elements.Get.AcceptedPage.SubmitButton));
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
