using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class AddAditionalDirectorsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _done;
        private readonly IWebElement _addAnother;

        private readonly IWebElement _title;
        private readonly IWebElement _firstName;
        private readonly IWebElement _lastName;
        private readonly IWebElement _email;
        private readonly IWebElement _emailConfirm;

        public new String Title
        {
            set { _title.SelectOption(value); }
            
        }
        public String FirstName
        {
            set { _firstName.SendValue(value); }
        }
        public String LastName
        {
            set { _lastName.SendValue(value); }
        }
        public String EmailAddress
        {
            set { _email.SendValue(value); }
        }
        public String ConfirmEmailAddress
        {
            set { _emailConfirm.SendValue(value); }
        }

        public AddAditionalDirectorsPage(UiClient client) : base(client)
        {
            _form = Content.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.FormId));
            _done = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.DoneButton));
            _addAnother = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.AddAnotherButton));

            _title = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.Title));
            _firstName = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.FirstName));
            _lastName = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.LastName));
            _email = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.EmailAddress));
            _emailConfirm = _form.FindElement(By.CssSelector(UiMap.Get.AddAditionalDirectorsPage.ConfirmEmailAddress));
            
        }

        public BusinessBankAccountPage Done()
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
