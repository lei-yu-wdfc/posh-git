using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.MigrationTests.V2Selenium.Pages
{
    public abstract class V2BasePage
    {
        public UiClient Client;

        protected V2BasePage(UiClient client)
        {
            Client = client;
        }
    }
}
