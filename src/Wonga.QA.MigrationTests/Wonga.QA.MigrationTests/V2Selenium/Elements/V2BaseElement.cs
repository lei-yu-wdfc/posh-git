using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.MigrationTests.V2Selenium.Pages;

namespace Wonga.QA.MigrationTests.V2Selenium.Elements
{
    public class V2BaseElement
    {
        public V2BasePage Page;

        protected V2BaseElement(V2BasePage page)
        {
            Page = page;
        }
    }
}
