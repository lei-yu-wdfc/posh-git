using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.UiTests.Web.Region.Ca.MyAccounts;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    public class L0AcceptedLoan : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

        [Test, AUT(AUT.Za, AUT.Ca, AUT.Uk), Owner(Owner.MihailPodobivsky), Category(TestCategories.SmokeTest)]
        public void AcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home()).WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var mySummary = journey.Teleport<MySummary>() as MySummaryPage;
        }
    }
}
