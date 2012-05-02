

using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class ExtensionSecciDocumentSection : BaseSection
    {
        private readonly ReadOnlyCollection<IWebElement> _gender;
        private readonly IWebElement _secciHeader;
        private readonly IWebElement _secciPrint;
        private readonly IWebElement _secci;

        //helper methods?

        public ExtensionSecciDocumentSection doPrint()
        {
            _secciPrint.Click();
            //print event check?
        }

        public String GetSecci()
        {
            return _secci.GetValue();
        }

        public ExtensionSecciDocumentSection(BasePage page)
            : base(UiMap.Get.ExtensionSecciDocumentSection.Fieldset, page)
        {
          _secciHeader = Section.FindElement(By.CssSelector(UiMap.Get.ExtensionSecciDocumentSection.SecciHeader));
          _secciPrint = Section.FindElement(By.CssSelector(UiMap.Get.ExtensionSecciDocumentSection.SecciPrint));
          _secci = Section.FindElement(By.CssSelector(UiMap.Get.ExtensionSecciDocumentSection.SecciContent));

        }
    }
}