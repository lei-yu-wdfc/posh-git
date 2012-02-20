using System;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Wb
{
    public class BusinessDetailsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _next;
        private readonly IWebElement _bizName;
        private readonly IWebElement _bizNumber;

        public String BusinessName { set { _bizName.SendValue(value); } }
        public String BusinessNumber { set { _bizNumber.SendValue(value); } }

        public BusinessDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Elements.Get.WbBusinessDetailsPage.FormId));
            _next = _form.FindElement(By.CssSelector(Elements.Get.WbBusinessDetailsPage.NextButton));
            _bizName = _form.FindElement(By.CssSelector(Elements.Get.WbBusinessDetailsPage.BusinessName));
            _bizNumber = _form.FindElement(By.CssSelector(Elements.Get.WbBusinessDetailsPage.BusinessNumber));
        }

        public Wb.AdditionalDirectorsPage Next()
        {
            _next.Click();
            return new Wb.AdditionalDirectorsPage(Client);
        }
    }
}
