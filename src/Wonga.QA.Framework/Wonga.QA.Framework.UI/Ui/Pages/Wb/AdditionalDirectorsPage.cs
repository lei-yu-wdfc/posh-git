using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class AdditionalDirectorsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _done;
        private readonly IWebElement _addAnother;

        public AdditionalDirectorsPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Ui.Get.AdditionalDirectorsPage.FormId));
            _done = _form.FindElement(By.CssSelector(Ui.Get.AdditionalDirectorsPage.DoneButton));
            _addAnother = _form.FindElement(By.CssSelector(Ui.Get.AdditionalDirectorsPage.AddAnotherDirector));
        }

        public BusinessBankAccountPage Next()
        {
            _done.Click();
            return new BusinessBankAccountPage(Client);
        }

        public AddAditionalDirectorsPage AddAditionalDirector()
        {
            _addAnother.Click();
            return new AddAditionalDirectorsPage(Client);
        }
    }
}
