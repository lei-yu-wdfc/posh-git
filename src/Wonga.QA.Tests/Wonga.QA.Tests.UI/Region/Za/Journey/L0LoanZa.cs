using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class L0LoanZa : UiTest
    {
        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Za)]
        public void ZaDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Za), JIRA("QA-278"), Pending("ZA-2302")]
        public void ZaDeclinedPageContainsHeaderLinks()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
            declinedPage.LookForHeaderLinks();
        }
       
    }
}
