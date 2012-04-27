using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class AdditionalDirectorsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _done;
        private IWebElement _addAnother;

        public AdditionalDirectorsPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMap.Get.AdditionalDirectorsPage.FormId));
            _done = _form.FindElement(By.CssSelector(UiMap.Get.AdditionalDirectorsPage.DoneButton));
        }

        public BusinessBankAccountPage Next()
        {
            _done.Click();
            return new BusinessBankAccountPage(Client);
        }

        public AddAditionalDirectorsPage AddAditionalDirector()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.AdditionalDirectorsPage.AddAnotherDirector)).Click();
            return new AddAditionalDirectorsPage(Client);
        }

        public string GetDirectors()
        {
            return Client.Driver.FindElement(By.CssSelector(UiMap.Get.AdditionalDirectorsPage.Directors)).Text;
        }
    }
}
