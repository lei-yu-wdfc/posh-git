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
    [Parallelizable(TestScope.All)]
    public class RedirectedToSelectedSiteTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-240, QA-168"), Category(TestCategories.SmokeTest)]
        public void RedirectedToSelectedSite()
        {
            const string ukUrl = "www.wonga.com";
            const string zaUrl = "www.wonga.co.za";
            const string caUrl = "www.wonga.ca";
            switch (Config.AUT)
            {
                case (AUT.Ca):
                    NavigateToRegionAndTest(AUT.Za, zaUrl);
                    NavigateToRegionAndTest(AUT.Uk, ukUrl);
                    break;
                case (AUT.Za):
                    NavigateToRegionAndTest(AUT.Uk, ukUrl);
                    NavigateToRegionAndTest(AUT.Ca, caUrl);
                    break;
            }
        }

        private void NavigateToRegionAndTest(AUT aut, string domain)
        {
            var page = Client.Home();
            string url = page.Url;
            page.InternationalElements.InternationalTriggerClick();
            switch (aut)
            {
                case (AUT.Za):
                    page.InternationalElements.InternationalPanelZaClick();
                    break;
                case (AUT.Uk):
                    page.InternationalElements.InternationalPanelUkClick();
                    break;
                case (AUT.Ca):
                    page.InternationalElements.InternationalPanelCaClick();
                    break;
            }
            Do.With.Interval(2).While(() => url == page.Url + "/#");
            Console.WriteLine(page.Url + "\n" + domain + "\n");
            Assert.IsTrue(page.Url.Contains(domain));
        }
    }
}
