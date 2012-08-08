using System;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.UiTests.Web.Region.Za.MyAccounts
{
    [Parallelizable(TestScope.All), AUT(AUT.Za)]
    public class HelpTest : UiTest
    {
        [Test, Pending("ZA-1600, ZA-2474")] //ZA-1600 bug check
        public void CouncilForDebtCollectorsLinkCheck()
        {
            var faq = Client.Faq();
            Assert.IsTrue(faq.IsLinkCorrect(UiMap.Get.FAQPage.CouncilForDebtCollectorsLink, ContentMap.Get.FAQPageLinks.CouncilForDebtCollectorsLink));
        }
    }
}
