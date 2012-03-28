using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Tests.Ui
{
    public class RedirectedToSelectedSiteTest : UiTest
    {
        [Test, AUT(AUT.Ca), JIRA("QA-240"), Pending("FE bug, button in top of page are broken")]
        public void RedirectedToSelectedSite()
        {
            const string ukUrl = "www.wonga.com";
            const string zaUrl = "www.wonga.co.za";
            //const string caUrl = "www.wonga.co.ca";
            switch (Config.AUT)
            {
                case (AUT.Ca):
                    NavigateToRegionAndTest(AUT.Za, zaUrl);
                    NavigateToRegionAndTest(AUT.Uk, ukUrl);
                    break;
            }
        }

        private void NavigateToRegionAndTest(AUT aut, string domain)
        {
            var page = Client.Home();
            page.InternationalElements.InternationalTriggerClick();
            switch (aut)
            {
                case (AUT.Za):
                    page.InternationalElements.InternationalPanelZaClick();
                    break;
                case (AUT.Uk):
                    page.InternationalElements.InternationalPanelUkClick();
                    break;
            }
            Assert.IsTrue(page.Url.Contains(domain));
        }
    }
}
