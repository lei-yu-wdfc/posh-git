using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class JargonBusterPage : BasePage
    {
        private IWebElement _alphabet;
        private List<IWebElement> _alphabetList;

        public JargonBusterPage(UiClient client)
            : base(client)
        {
            _alphabet = Client.Driver.FindElement(By.CssSelector(Ui.Get.JargonBusterPage.Alphabet));
        }

        public List<IWebElement> GetAlphabetLinks()
        {
            return _alphabetList = new List<IWebElement>(_alphabet.FindElements(By.TagName("a")).Where(e => e.GetAttribute("href") != null));
        }
    }
}
