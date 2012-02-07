using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class AdditionalDirectorsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _done;
        private readonly IWebElement _addAnother;

        public AdditionalDirectorsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-directors-form"));
            _done = _form.FindElement(By.Name("done"));
            _addAnother = _form.FindElement(By.Name("add"));
        }

        public MainBusinessBankAccountPage Done()
        {
            _done.Click();
            return new MainBusinessBankAccountPage(Client);
        }

        public AddAditionalDirectorsPage AddAditionalDirector()
        {
            _addAnother.Click();
            return new AddAditionalDirectorsPage(Client);
        }
    }
}
