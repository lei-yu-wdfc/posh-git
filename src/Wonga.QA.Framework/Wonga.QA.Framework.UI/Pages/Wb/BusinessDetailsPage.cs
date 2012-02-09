using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
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
            _form = Content.FindElement(By.Id("lzero-business-form"));

            _next = _form.FindElement(By.Name("next"));

            _bizName = _form.FindElement(By.Name("biz_name"));
            _bizNumber = _form.FindElement(By.Name("biz_number"));
        }

        public AdditionalDirectorsPage Next()
        {
            _next.Click();
            return new AdditionalDirectorsPage(Client);
        }
    }
}
